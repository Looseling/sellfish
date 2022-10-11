using AzureContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IFishRepository
    {
        public ICollection<Fish> GetManyFish();

        public Fish GetSingleFish(int Id);

        public bool AddFish(Fish fish);

        public bool Save();
    }
}
