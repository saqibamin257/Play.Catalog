using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
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
        private readonly IPublishEndpoint publishEndpoint;


        public ItemsController(Play.Common.IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
        }

        //Get
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                      .Select(item => item.AsDto());
            return Ok(items);
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
            //publish a message to message broker after create operation
            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

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
            //publish a message to message broker after create operation
            await publishEndpoint.Publish(new CatalogItemUpdated(exsitingItem.Id, exsitingItem.Name, exsitingItem.Description));

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
            //publish a message to message broker after delete operation
            await publishEndpoint.Publish(new CatalogItemDeleted(id));
            return NoContent();
        }

    }
}