using Play.Catalog.Service.Entities;
namespace Play.Catalog.Service.Repositories
{
    // public interface IItemsRepository
    // {
    //     Task CreateAsync(Item entity);
    //     Task<IReadOnlyCollection<Item>> GetAllAsync();
    //     Task<Item> GetAsync(Guid id);
    //     Task RemoveAsync(Guid id);
    //     Task UpdateAsync(Item entity);
    // }
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}