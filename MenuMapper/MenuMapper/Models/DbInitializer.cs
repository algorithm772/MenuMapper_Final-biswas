using System.Collections.Generic;
using System.Linq;

namespace MenuMapper.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.MenuItems.Any())
            {
                return;
            }

            // 1. Create the Global 14 + Mushrooms
            var dairy = new Allergen { Name = "Dairy" };
            var peanuts = new Allergen { Name = "Peanuts" };
            var treeNuts = new Allergen { Name = "Tree Nuts" };
            var gluten = new Allergen { Name = "Gluten" };
            var egg = new Allergen { Name = "Egg" };
            var soy = new Allergen { Name = "Soy" };
            var fish = new Allergen { Name = "Fish" };
            var shellfish = new Allergen { Name = "Shellfish" };
            var molluscs = new Allergen { Name = "Molluscs" };
            var sesame = new Allergen { Name = "Sesame" };
            var mustard = new Allergen { Name = "Mustard" };
            var celery = new Allergen { Name = "Celery" };
            var lupin = new Allergen { Name = "Lupin" };
            var sulfites = new Allergen { Name = "Sulfites" };
            var mushrooms = new Allergen { Name = "Mushrooms" };

            context.Allergens.AddRange(
                dairy, peanuts, treeNuts, gluten, egg, soy, fish,
                shellfish, molluscs, sesame, mustard, celery, lupin, sulfites, mushrooms
            );

            // 2. Create the Menu Items (Now with ImageUrl)
            var cheeseburger = new MenuItem
            {
                Name = "Classic Cheeseburger",
                Price = 299.00m,
                Category = "Main Course",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=800&q=80",
                Ingredients = "Beef patty, Cheddar Cheese, Brioche Bun",
                Status = "Complete"
            };

            var padThai = new MenuItem
            {
                Name = "Shrimp Pad Thai",
                Price = 349.50m,
                Category = "Main Course",
                ImageUrl = "https://images.unsplash.com/photo-1559314809-0d155014e29e?auto=format&fit=crop&w=800&q=80",
                Ingredients = "Rice Noodles, Shrimp, Tofu, Egg, Peanuts",
                Status = "Complete"
            };

            var houseSalad = new MenuItem
            {
                Name = "Vegan House Salad",
                Price = 199.50m,
                Category = "Starters",
                ImageUrl = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=800&q=80",
                Ingredients = "Mixed Greens, Tomatoes, Cucumbers, Vinaigrette",
                Status = "Complete"
            };

            context.MenuItems.AddRange(cheeseburger, padThai, houseSalad);

            // 3. Create the Bridge Links
            var mappings = new List<MenuItemAllergen>
            {
                new MenuItemAllergen { MenuItem = cheeseburger, Allergen = dairy },
                new MenuItemAllergen { MenuItem = cheeseburger, Allergen = gluten },
                new MenuItemAllergen { MenuItem = padThai, Allergen = shellfish },
                new MenuItemAllergen { MenuItem = padThai, Allergen = peanuts },
                new MenuItemAllergen { MenuItem = padThai, Allergen = soy }
            };

            context.MenuItemAllergens.AddRange(mappings);
            context.SaveChanges();
        }
    }
}