using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopOrdering.core.Services.Models
{
    public class BaseResult
    {
        public bool Success => Errors.Count == 0;
        public List<string> Errors { get; set; } = [];
    }
}
