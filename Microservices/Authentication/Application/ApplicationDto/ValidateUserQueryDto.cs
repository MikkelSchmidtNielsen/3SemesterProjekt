using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationDto
{
    public class ValidateUserQueryDto
    {
        public string Email { get; set; }
        public int Otp {  get; set; }
    }
}
