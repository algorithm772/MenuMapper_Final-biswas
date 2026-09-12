using Azure.Core;
using MenuMapper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuMapper.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MenuApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/menu (Fetches all items for the React Dashboard & Customer App)
        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var items = await _context.MenuItems
                .Include(m => m.MenuItemAllergens)
                .ThenInclude(ma => ma.Allergen)
                .ToListAsync();

            var reactData = items.Select(i => new {
                id = i.Id,
                name = i.Name,
                category = i.Category,
                price = i.Price,
                ingredients = string.IsNullOrEmpty(i.Ingredients) ? new string[0] : i.Ingredients.Split(',').Select(x => x.Trim()),
                allergens = i.MenuItemAllergens.Select(ma => ma.Allergen.Name).ToList(),
                crossContact = new string[0],
                imageUrl = i.ImageUrl,
                status = i.Status
            });

            return Ok(reactData);
        }

        // POST: api/menu (Receives form submission from React UI and saves to SQL)
        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuItemRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Invalid menu item data.");
            }

            // 1. Create and save the main Menu Item record
            var newItem = new MenuItem
            {
                Name = request.Name,
                Category = request.Category,
                Price = request.Price,
                Status = request.Status,
                ImageUrl = request.ImageUrl,
                Ingredients = request.Ingredients != null ? string.Join(", ", request.Ingredients) : ""
            };

            _context.MenuItems.Add(newItem);
            await _context.SaveChangesAsync(); // Saves to get the auto-incremented Item ID

            // 2. Map and link selected allergens via the bridge table
            if (request.Allergens != null && request.Allergens.Count > 0)
            {
                var allDbAllergens = await _context.Allergens.ToListAsync();

                foreach (var allergenName in request.Allergens)
                {
                    // Find if allergen already exists in DB
                    var dbAllergen = allDbAllergens.FirstOrDefault(a => a.Name.ToLower() == allergenName.ToLower());

                    // If it's a brand new custom allergen, create it dynamically
                    if (dbAllergen == null)
                    {
                        dbAllergen = new Allergen { Name = allergenName };
                        _context.Allergens.Add(dbAllergen);
                        await _context.SaveChangesAsync();
                        allDbAllergens.Add(dbAllergen);
                    }

                    // Insert into the bridge table to link item <-> allergen
                    _context.MenuItemAllergens.Add(new MenuItemAllergen
                    {
                        MenuItemId = newItem.Id,
                        AllergenId = dbAllergen.Id
                    });
                }

                await _context.SaveChangesAsync();
            }

            return Ok(newItem);
        }

        // DELETE: api/menu/{id} (Removes item and its bridge records from SQL)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var item = await _context.MenuItems
                .Include(m => m.MenuItemAllergens)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item != null)
            {
                // Clear bridge table links first to avoid SQL Foreign Key constraint errors
                _context.MenuItemAllergens.RemoveRange(item.MenuItemAllergens);

                // Remove the menu item itself
                _context.MenuItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }

    // Payload model that matches the JSON structure sent by the React form
    public class MenuItemRequest
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; } // Add this line
    }
}