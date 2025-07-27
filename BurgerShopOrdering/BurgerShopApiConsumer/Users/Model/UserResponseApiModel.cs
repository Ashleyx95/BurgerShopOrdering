using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Users.Model
{
    public class UserResponseApiModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsClient { get; set; }
    }
}
