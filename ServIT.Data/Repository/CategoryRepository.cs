using Microsoft.EntityFrameworkCore;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;

namespace ServIT.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ServITDBContext _db;

        public CategoryRepository(ServITDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Category payload)
        {
            await _db.Categories.AddAsync(payload);
            return await Save();
        }

        public async Task<ICollection<Category>> GetAll()
        {
            ICollection<Category> categories = await _db.Categories.Where(categoryItem => !categoryItem.IsDeleted).ToListAsync();
            return categories;
        }

        public async Task<Category> GetByID(Guid Id)
        {
            Category category = await _db.Categories.Where(categoryItem => categoryItem.Id == Id && !categoryItem.IsDeleted).FirstOrDefaultAsync();
            return category;
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Category payload)
        {
            _db.Categories.Update(payload);
            return await Save();
        }

        public async Task<bool> CategoryNameExists(string Name)
        {
            Category category = await _db.Categories.Where(categoryItem => categoryItem.CategoryName == Name && !categoryItem.IsDeleted).FirstOrDefaultAsync();
            return category != null;
        }
    }
}
