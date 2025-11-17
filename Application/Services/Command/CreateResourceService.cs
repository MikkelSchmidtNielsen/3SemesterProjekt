using Application.Factories;
using Application.ServiceInterfaces.Command;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Domain.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Command
{
    public class CreateResourceService : ICreateResourceService
    {
        private readonly ResourceFactory _factory;

        public async Task<IResult<Resource>> CreateResourceAsync(CreateResourceDto dto)
        {
            var checkIfNameAlreadyExists = await _factory.CreateResourceAsync(dto);

            if (checkIfNameAlreadyExists.IsError())
            {
                return Result<Resource>.Error(originalType: null, new Exception("Der findes allerede en ressource med dette navn."));
            }
            else
            {
                return Result<Resource>.Success(originalType: null);
            }
        }
    }
}
