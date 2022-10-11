using AutoMapper;
using AzureContext;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SelfishBackendMySql.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfishBackendMySql.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IUserRepository _userRepository;
        public readonly sellfish_dbContext _context;
        public UserController(sellfish_dbContext context, IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_context.Users.ToArray());

            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{Id}")]

        public IActionResult Get(int Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);

            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userToCreate,[FromHeader] int CartId)
        {
            if (userToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var isDuplicate = _context.Users.Any(o => o.Email.Trim().ToUpper() == userToCreate.Email.Trim().ToUpper());

            if (isDuplicate)
            {
                ModelState.AddModelError("", "Such User exist in db");
                return StatusCode(403, ModelState);    
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userMap = _mapper.Map<User>(userToCreate);

            if (!_userRepository.AddUser(userMap))
            {
                return StatusCode(500, "something went wrong while saving entity");
            }

            return Ok("Succesfully Created");
        }
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var userDelete = _context.Users.FirstOrDefault(o => o.Id == Id);

            if (userDelete == null)
            {
                return BadRequest();
            }

            _context.Users.Remove(userDelete);
            _context.SaveChanges();
         
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int Id, User user)
        {
            var userToChange = _context.Users.FirstOrDefault(o => o.Id == Id);

            if (userToChange == null)
            {
                return NotFound("object to update is not in DB");
            }
            _context.SaveChanges();

            return StatusCode(200);
        }





    }
}
