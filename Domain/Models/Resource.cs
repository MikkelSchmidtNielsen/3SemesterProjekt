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
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public double ResourceBasePrice { get; set; }
        public Booking Booking { get; }

        public Resource(int resourceId, string resourceName, string resourceType, double resourceBasePrice)
        {
            Id = resourceId;
            ResourceName = resourceName;
            ResourceType = resourceType;
            ResourceBasePrice = resourceBasePrice;

            ValidateInformation();
        }

        public bool ValidateInformation()
        {
            if (Id == 0)
            {
                throw new Exception("Resource ID is 0.");
            }
            else if (ResourceName == null)
            {
                throw new Exception("Resource name is null.");
            }
            else if(ResourceType == null)
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
