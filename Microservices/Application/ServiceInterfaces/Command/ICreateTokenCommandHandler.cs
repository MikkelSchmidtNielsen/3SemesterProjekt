using Common.ResultInterfaces;
using Domain;

namespace Application.ServiceInterfaces.Command
{
    public interface ICreateTokenCommandHandler
    {
        public IResult<string> Handle(User user);
    }
}
