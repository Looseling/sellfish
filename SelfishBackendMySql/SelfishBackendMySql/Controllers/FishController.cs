using AutoMapper;
using AzureContext;
using DataAccess.Interfaces;
using DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SelfishBackendMySql.DTO;
using System;                                                                                     
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfishBackend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FishController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFishRepository _repository;
        private readonly sellfish_dbContext _context;

        public FishController(IFishRepository repostiry,
                              IWebHostEnvironment environment,
                              IMapper mapper,
                              sellfish_dbContext context)
        {
            _repository = repostiry;
            _environment = environment;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var fishs = _mapper.Map<List<FishDTO>>(_repository.GetManyFish());
         
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(fishs);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var fish = _mapper.Map<FishDTO>(_repository.GetSingleFish(Id));

            if (fish == null)
            {
                return NotFound();
            }
            
            return Ok(fish);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FishDTO fishToCreate)
        { 
            if (fishToCreate == null)
                return BadRequest(ModelState);

            Fish fish = null;

            try
            {
                fish = _repository.GetManyFish().
                Where(c => c.FishName.Trim().ToUpper() == fishToCreate.FishName.Trim().ToUpper()).FirstOrDefault();
            }
            catch (Exception)
            {
            }
            
            if (fish != null)
            {
                ModelState.AddModelError("", "Fish already exist in Db");
                return StatusCode(422, ModelState); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Fish>(fishToCreate);

            if (!_repository.AddFish(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving entity");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created");
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {

            var fishToDelete = _context.Fish.FirstOrDefault(o => o.Id == Id);
            if (fishToDelete == null)
            {
                return NotFound("Object to delete is not in DB");
            }

            _context.Fish.Remove(fishToDelete);
            _context.SaveChanges();

            return StatusCode(200);
        }

        [HttpPut]
        public IActionResult Put(int Id, Fish fish)
        {
            var fishToChange = _context.Fish.FirstOrDefault(o => o.Id == Id);

            if (fishToChange == null)
            {
                return NotFound("object to update is not in DB");
            }
            _context.SaveChanges();

            return StatusCode(200);
        }


        //[NonAction]
        //public IActionResult Upload(IFormFile file)
        //{
        //    try
        //    {
        //        var fullPath = GetFilepath();
        //        if (file.Length > 0)
        //        {
        //            var fileName = file.FileName;
        //            using (var stream = new FileStream(Path.Combine(fullPath,fileName), FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            var dbPath = $"\\Uploads\\Fish\\{fileName}";
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex} ");
        //    }
        //}


        [NonAction]

        private string GetFilepath()
        {
            return this._environment.WebRootPath + "\\Uploads\\Fish\\";
        }













    }
}
