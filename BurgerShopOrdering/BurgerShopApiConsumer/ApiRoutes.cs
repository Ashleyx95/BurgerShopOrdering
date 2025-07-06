using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer
{
    public class ApiRoutes
    {
        public const string Base = "https://8d6dgmc8-7070.euw.devtunnels.ms/api";
        public const string Products = Base + "/products/";
        public const string Categories = Base + "/categories/";
        public const string Orders = Base + "/orders/";
        public const string Accounts = Base + "/accounts/";
    }
}
