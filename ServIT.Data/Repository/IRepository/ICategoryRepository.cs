using ServIT.Models;


namespace ServIT.Data.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Task<bool> Create(Category payload);
        Task<ICollection<Category>> GetAll();
        Task<Category> GetByID(Guid Id);
        Task<bool> Update(Category payload);
        Task<bool> CategoryNameExists(string Name);
        Task<bool> Save();
    }
}
