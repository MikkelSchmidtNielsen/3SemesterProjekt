using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Location { get; private set; }
        public string? Description { get; private set; }
        public bool IsAvailable { get; private set; }

        [Timestamp]
		public byte[] RowVersions { get; private set; }

		public Resource(CreateResourceFactoryDto dto)
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
            else if (Type == null)
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
