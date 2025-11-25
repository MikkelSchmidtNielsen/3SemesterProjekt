using Common.ResultInterfaces;
using Domain.Models;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateTokenCommandHandler
    {
        public IResult<string> Handle(User user);
    }
}
