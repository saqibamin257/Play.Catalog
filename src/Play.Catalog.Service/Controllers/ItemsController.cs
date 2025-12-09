using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        public static readonly List<ItemDto> items = new()
        {
          new ItemDto(Guid.NewGuid(), "First Item", "This is the first item", 9.99m, DateTimeOffset.UtcNow),
          new ItemDto(Guid.NewGuid(), "Second Item", "This is the second item", 19.99m, DateTimeOffset.UtcNow),
          new ItemDto(Guid.NewGuid(),"Third Item", " This is the third item", 29.99m, DateTimeOffset.UtcNow)
        };

        //GET /items
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if (item is null)
                return NotFound();

            return item;
        }

        //POST /items
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto itemDto)
        {
            var item = new ItemDto(Guid.NewGuid(),
                                   itemDto.Name,
                                   itemDto.Description,
                                   itemDto.Price,
                                   DateTimeOffset.UtcNow
                                   );
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        //PUT/items /{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(items => items.Id == id).SingleOrDefault();

            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;
            return NoContent();
        }

        //Delete /items/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var index = items.FindIndex(exsitingItem => exsitingItem.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            items.RemoveAt(index);
            return NoContent();
        }
    }
}