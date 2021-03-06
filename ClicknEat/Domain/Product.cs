﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClicknEat.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? RestaurantId = Guid.Empty;

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        public string ImagePath { get; set; }

        public ProductCategory ProductCategory { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public Restaurant Restaurant { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public Product()
        {
            OrderDetails = new Collection<OrderDetail>();
            ShoppingCartItems = new Collection<ShoppingCartItem>();
        }

    }
}
