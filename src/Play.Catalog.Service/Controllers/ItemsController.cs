using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;


namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]

    public class ItemsController : ControllerBase
    {
        private readonly Play.Common.IRepository<Item> itemsRepository;

        public ItemsController(Play.Common.IRepository<Item> itemsRepository)
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