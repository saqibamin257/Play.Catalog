using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    #region  sample code without DB and Repository
    // public class ItemsController : ControllerBase
    // {
    //     public static readonly List<ItemDto> items = new()
    //     {
    //       new ItemDto(Guid.NewGuid(), "First Item", "This is the first item", 9.99m, DateTimeOffset.UtcNow),
    //       new ItemDto(Guid.NewGuid(), "Second Item", "This is the second item", 19.99m, DateTimeOffset.UtcNow),
    //       new ItemDto(Guid.NewGuid(),"Third Item", " This is the third item", 29.99m, DateTimeOffset.UtcNow)
    //     };

    //     //GET /items
    //     [HttpGet]
    //     public IEnumerable<ItemDto> Get()
    //     {
    //         return items;
    //     }

    //     //GET /items/{id}
    //     [HttpGet("{id}")]
    //     public ActionResult<ItemDto> GetById(Guid id)
    //     {
    //         var item = items.Where(item => item.Id == id).SingleOrDefault();
    //         if (item is null)
    //             return NotFound();

    //         return item;
    //     }

    //     //POST /items
    //     [HttpPost]
    //     public ActionResult<ItemDto> Post(CreateItemDto itemDto)
    //     {
    //         var item = new ItemDto(Guid.NewGuid(),
    //                                itemDto.Name,
    //                                itemDto.Description,
    //                                itemDto.Price,
    //                                DateTimeOffset.UtcNow
    //                                );
    //         items.Add(item);
    //         return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    //     }

    //     //PUT/items /{id}
    //     [HttpPut("{id}")]
    //     public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    //     {
    //         var existingItem = items.Where(items => items.Id == id).SingleOrDefault();

    //         if (existingItem == null)
    //         {
    //             return NotFound();
    //         }

    //         var updatedItem = existingItem with
    //         {
    //             Name = updateItemDto.Name,
    //             Description = updateItemDto.Description,
    //             Price = updateItemDto.Price
    //         };

    //         var index = items.FindIndex(existingItem => existingItem.Id == id);
    //         items[index] = updatedItem;
    //         return NoContent();
    //     }

    //     //Delete /items/{id}
    //     [HttpDelete("{id}")]
    //     public IActionResult Delete(Guid id)
    //     {
    //         var index = items.FindIndex(exsitingItem => exsitingItem.Id == id);
    //         if (index < 0)
    //         {
    //             return NotFound();
    //         }
    //         items.RemoveAt(index);
    //         return NoContent();
    //     }
    // }
    #endregion
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemsRepository;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;

        }

        //Get
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                      .Select(item => item.AsDto());
            return items;
        }
        //Get by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }
        //Post
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto itemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item.AsDto());
        }
        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var exsitingItem = await itemsRepository.GetAsync(id);
            if (exsitingItem is null)
            {
                return NotFound();
            }

            exsitingItem.Name = updateItemDto.Name;
            exsitingItem.Description = updateItemDto.Description;
            exsitingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(exsitingItem);
            return NoContent();
        }
        //Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingItem = await itemsRepository.GetAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await itemsRepository.RemoveAsync(existingItem.Id);
            return NoContent();
        }

    }
}