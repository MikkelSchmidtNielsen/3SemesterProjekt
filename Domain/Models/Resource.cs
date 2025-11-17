using Domain.ModelsDto;
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
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }

        public Resource(int resourceId, string resourceName, string resourceType, decimal resourceBasePrice, string? description)
        {
            Id = resourceId;
            Name = resourceName;
            Type = resourceType;
            BasePrice = resourceBasePrice;
            Description = description;

            ValidateInformation();

        }

        public Resource(CreateResourceDto dto)
        {
            Name = dto.Name;
            Type = dto.Type;
            BasePrice = dto.BasePrice;
            Description = dto.Description;

            ValidateInformation();
        }

        public bool ValidateInformation()
        {
            if (Name == null)
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
