using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ressource
    {
        public int ResourceID { get; init; }
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public double ResourceBasePrice { get; set; }

        public Ressource(int resourceId, string resourceName, string resourceType, double resourceBasePrice)
        {
            ResourceID = resourceId;
            ResourceName = resourceName;
            ResourceType = resourceType;
            ResourceBasePrice = resourceBasePrice;

            ValidateInformation();
        }

        public bool ValidateInformation()
        {
            if (ResourceID == 0 || ResourceID == null)
            {
                throw new Exception("Resource ID is either 0 or null.");
            }
            else if (ResourceName == null)
            {
                throw new Exception("Resource name is null.");
            }
            else if(ResourceType == null)
            {
                throw new Exception("Resource type is null.");
            }
            else if(ResourceBasePrice == null)
            {
                throw new Exception("Resource price is null.");
            }
            else
            {
                return true;
            }
        }
    }
}
