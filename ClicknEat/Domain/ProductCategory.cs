﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClicknEat.Domain
{
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public List<Product> Products { get; set; }
    }
}
