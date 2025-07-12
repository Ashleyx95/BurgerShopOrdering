using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Orders.Model
{
    public enum OrderStatus
    {
        Besteld = 0,
        Bereiden = 1,
        Klaar = 2,
        Afgehaald = 3
    }
}
