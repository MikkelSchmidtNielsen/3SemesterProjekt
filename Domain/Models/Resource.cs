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
        public List<Booking>? Bookings { get; }
        private Resource() {}

        public Resource(int resourceId, string resourceName, string resourceType, decimal resourceBasePrice)
        {
            Id = resourceId;
            Name = resourceName;
            Type = resourceType;
            BasePrice = resourceBasePrice;

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
