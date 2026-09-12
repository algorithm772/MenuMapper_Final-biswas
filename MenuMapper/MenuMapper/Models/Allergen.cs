using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuMapper.Models
{
    public class Allergen
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation property to the bridge table
        public ICollection<MenuItemAllergen> MenuItemAllergens { get; set; } = new List<MenuItemAllergen>();
    }
}