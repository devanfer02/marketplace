﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Stock { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

        // relations
        [ForeignKey(name: "UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
