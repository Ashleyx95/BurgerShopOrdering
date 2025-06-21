using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Order>? Orders { get; set; }

        public ApplicationUser(string firstName, string lastName, string email, bool emailConfirmed = false)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            NormalizedEmail = email.ToUpper();
            UserName = email;
            NormalizedUserName = email.ToUpper();
            EmailConfirmed = emailConfirmed;
        }
    }
}
