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
        public string Name { get; private set; }
        public string Type { get; private set; }
        public decimal BasePrice { get; private set; }
        public int Location { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public List<Booking>? Bookings { get; }

        public Resource(int resourceId, string resourceName, string resourceType, decimal resourceBasePrice, int location, string? description)
        {
            Id = resourceId;
            Name = resourceName;
            Type = resourceType;
            BasePrice = resourceBasePrice;
            Location = location;
            Description = description;
            IsAvailable = true;

            ValidateInformation();
        }

        private Resource() { }
        public Resource(int id, string name, string type, decimal basePrice)
        {
            Id = id;
            Name = name;
            Type = type;
            BasePrice = basePrice;

            ValidateInformation();

        }
        public Resource(string resourceName, string resourceType, decimal resourceBasePrice, int location, string? description)
        {
            Name = resourceName;
            Type = resourceType;
            BasePrice = resourceBasePrice;
            Location = location;
            Description = description;
            IsAvailable = true;

            ValidateInformation();

        }

        public Resource(CreateResourceDto dto)
        {
            Name = dto.Name;
            Type = dto.Type;
            BasePrice = dto.BasePrice;
            Location = dto.Location;
            Description = dto.Description;
            IsAvailable = true;

            ValidateInformation();
        }
        private Resource()
        {

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
