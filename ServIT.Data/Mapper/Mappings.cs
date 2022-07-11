using AutoMapper;
using ServIT.Data.DTO.Category;
using ServIT.Data.DTO.SubCategory;
using ServIT.Data.DTO.User;
using ServIT.Models;
namespace ServIT.Data.Mapper;
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, UpsertCategoryDTO>().ReverseMap();
            CreateMap<Category, DeleteCategoryDTO>().ReverseMap();


            CreateMap<SubCategory, SubCategoryDTO>().ReverseMap();
            CreateMap<SubCategory, UpsertSubCategoryDTO>().ReverseMap();
            CreateMap<SubCategory, DeleteSubCategoryDTO>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UpsertUserDTO>().ReverseMap();
            CreateMap<User, DeleteUserDTO>().ReverseMap();
        }
    }