namespace MenuMapper.Models
{
    public class MenuItemAllergen
    {
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int AllergenId { get; set; }
        public Allergen Allergen { get; set; }
    }
}