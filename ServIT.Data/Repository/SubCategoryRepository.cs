using Microsoft.EntityFrameworkCore;
using ServIT.Business.GenericResponse;
using ServIT.Data.DTO.Category;
using ServIT.Data.DTO.SubCategory;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;

namespace ServIT.Data.Repository;
public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly ServITDBContext _db;

    public SubCategoryRepository(ServITDBContext db)
    {
        _db = db;
    }

    public async Task<bool> Create(SubCategory payload)
    {
        await _db.SubCategories.AddAsync(payload);
        return await _save();
    }

    public async Task<ICollection<SubCategory>> GetAll(Guid Id) // category Id
    {
        ICollection<SubCategory> subCategories = await _db.SubCategories.Where(
            subCategoryItem => subCategoryItem.CategoryId == Id && !subCategoryItem.IsDeleted
        ).ToListAsync();
        return subCategories;
    }

    public async Task<SubCategory> GetByID(Guid Id)
    {
        SubCategory subCategory = await _db.SubCategories.Where(categoryItem => categoryItem.Id == Id && !categoryItem.IsDeleted).FirstOrDefaultAsync();
        return subCategory;
    }

    public async Task<bool> SubCategoryNameExists(string subCategoryName, Guid CategoryId)
    {
        SubCategory subCategory = await _db.SubCategories.Where(subCategoryItem => subCategoryItem.SubCategoryName == subCategoryName && subCategoryItem.CategoryId == CategoryId
        ! && subCategoryItem.IsDeleted).FirstOrDefaultAsync();
        return subCategory != null;
    }

    public async Task<bool> Update(SubCategory payload)
    {
        _db.SubCategories.Update(payload);
        return await _save();
    }

    public async Task<bool> _save()
    {
        return await _db.SaveChangesAsync() > 0;
    }
}
