using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuMapper.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Ingredients { get; set; }
        public string Status { get; set; }

        // --- NEW FIELD FOR THE IMAGE ---
        public string ImageUrl { get; set; }

        public ICollection<MenuItemAllergen> MenuItemAllergens { get; set; } = new List<MenuItemAllergen>();
    }
}