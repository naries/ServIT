using ServIT.Business.GenericResponse;
using ServIT.Data.DTO.Category;
using ServIT.Data.DTO.SubCategory;
using ServIT.Models;

namespace ServIT.Data.Repository.IRepository;

public interface ISubCategoryRepository
{
    Task<bool> Create(SubCategory payload);
    Task<ICollection<SubCategory>> GetAll(Guid Id);
    Task<SubCategory> GetByID(Guid Id);
    Task<bool> Update(SubCategory payload);
    Task<bool> SubCategoryNameExists(string SubCategoryName, Guid categoryId);
    Task<bool> _save();
}
