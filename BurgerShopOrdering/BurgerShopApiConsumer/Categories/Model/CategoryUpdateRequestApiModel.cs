﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgerShopApiConsumer.Categories.Models
{
    public class CategoryUpdateRequestApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
