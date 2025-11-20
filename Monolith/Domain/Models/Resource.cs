using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Resource
    {
        public int Id { get; init; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public decimal BasePrice { get; private set; }

        private Resource() { }
        public Resource(int id, string name, string type, decimal basePrice)
        {
            Id = id;
            Name = name;
            Type = type;
            BasePrice = basePrice;

            ValidateInformation();
        }

        public bool ValidateInformation()
        {
            if (Id == 0)
            {
                throw new Exception("Resource ID is 0.");
            }
            else if (Name == null)
            {
                throw new Exception("Resource name is null.");
            }
            else if(Type == null)
            {
                throw new Exception("Resource type is null.");
            }
            else
        {
                return true;
            }
        }
    }
}
