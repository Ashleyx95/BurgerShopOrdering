using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Users.Model
{
    public class UserLoginRequestApiModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
