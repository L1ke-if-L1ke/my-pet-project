using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApp1.ServiceDefaults.User
{
    public record UserRegistrationDate
    {
        public DateOnly Date { get; }
        public UserRegistrationDate(DateOnly date)
        {
            Date = date;
        }
    }
}
