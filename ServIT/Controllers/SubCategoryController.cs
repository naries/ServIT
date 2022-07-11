using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServIT.Business.GenericResponse;
using ServIT.Data.AppConstants;
using ServIT.Data.DTO.SubCategory;
using ServIT.Data.Repository;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;

namespace ServIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryRepository> _logger;
        private readonly ICategoryRepository _cRepository;

        public SubCategoryController(ISubCategoryRepository repository, IMapper mapper, ILogger<SubCategoryRepository> logger, ICategoryRepository cRepository)
        {
            _repository = repository;
            _cRepository = cRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{Id:guid}", Name = "GetSubcategory")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesResponseType(400, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesDefaultResponseType(typeof(BaseResponse<SubCategoryDTO>))]
        public async Task<IActionResult> GetSubCategory (Guid Id)
        {
            try {
                SubCategory subCategory = await _repository.GetByID(Id);
                if (subCategory == null)
                {
                    return StatusCode(ServITConstants.NotFoundStatusCode, new BaseResponse<SubCategoryDTO>
                    {
                        ResponseMessage = "Sub category not found",
                        Data = null,
                        ResponseCode = ServITConstants.NotFoundStatusCode.ToString()
                    });
                }

                SubCategoryDTO subCategoryDTO = _mapper.Map<SubCategoryDTO>(subCategory);
                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<SubCategoryDTO>
                {
                    ResponseMessage = "Retrieval successful",
                    Data = subCategoryDTO,
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            } catch(Exception ex) {
                _logger.LogError($"MethodName: GetCategory() ===> {ex.Message}");
                throw;
            }
        }

        [HttpGet("{categoryId:guid}/all", Name = "GetSubCategories")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<ICollection<SubCategoryDTO>>))]
        [ProducesResponseType(400, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesDefaultResponseType(typeof(BaseResponse<SubCategoryDTO>))]
        public async Task<IActionResult> GetSubCategories(Guid categoryId)
        {
            try
            {
                ICollection<SubCategory> subCategories = await _repository.GetAll(categoryId);
                List<SubCategoryDTO> subCategoriesDTO = new();
                foreach (var subCategory in subCategories)
                {
                    subCategoriesDTO.Add(_mapper.Map<SubCategoryDTO>(subCategory));
                }

                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<List<SubCategoryDTO>>
                {
                    ResponseMessage = "Retrieval successful",
                    Data = subCategoriesDTO,
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetCategory() ===> {ex.Message}");
                throw;
            }
        }


        [HttpPut("{subCategoryId:guid}", Name = "updateSubCategory")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateSubCategory([FromBody] UpsertSubCategoryDTO subCategoryDTO, Guid subCategoryId)
        {
            try
            {

                SubCategory subCategory = await _repository.GetByID(subCategoryId);
                if (subCategory == null)
                {
                    return StatusCode(ServITConstants.NotFoundStatusCode, new BaseResponse<SubCategoryDTO>
                    {
                        ResponseMessage = "SubCategory not found",
                        Data = null,
                        ResponseCode = ServITConstants.NotFoundStatusCode.ToString()
                    });
                }

                // Category newCategory = _mapper.Map<Category>(categoryDTO);
                subCategory.SubCategoryName = subCategoryDTO.SubCategoryName;
                subCategory.DateUpdated = DateTime.Now;
               
                _repository.Update(subCategory);

                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<SubCategoryDTO>
                {
                    ResponseMessage = "Update successful.",
                    Data = _mapper.Map<SubCategoryDTO>(subCategory),
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: UpdateSubCategory() ===> {ex.Message}");
                throw;
            }
        }

        [HttpPost("{categoryId:guid}", Name = "CreateSubCategories")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSubCategories([FromBody] UpsertSubCategoryDTO subCategory, Guid categoryId)
        {
            try
            {

                string message = $"Successfully created Category {subCategory.SubCategoryName}";
                int responseCode = ServITConstants.CreatedStatusCode;
                // check if categoryId exists
                Category check = await _cRepository.GetByID(categoryId);
                if(check == null)
                {
                    message = "Category does not exist";
                    responseCode = ServITConstants.BadRequestStatusCode;
                }

                SubCategory subCategoryObj = _mapper.Map<SubCategory>(subCategory);
                if (subCategory.SubCategoryName.Trim().Length == 0)
                {
                    message = "Please provide a category name";
                    responseCode = ServITConstants.BadRequestStatusCode;
                }

                if (await _repository.SubCategoryNameExists(subCategory.SubCategoryName.Trim(), categoryId))
                {
                    message = "SubCategory already exists!";
                    responseCode = ServITConstants.BadRequestStatusCode;
                }

                if (responseCode != ServITConstants.CreatedStatusCode)
                {
                    return StatusCode(responseCode, new BaseResponse<UpsertSubCategoryDTO>
                    {
                        ResponseMessage = message,
                        Data = null,
                        ResponseCode = responseCode.ToString(),
                    });
                }

                subCategoryObj.CategoryId = categoryId;
                subCategoryObj.DateCreated = DateTime.Now;
                subCategoryObj.DateUpdated = DateTime.Now;
                subCategoryObj.IsDeleted = false;
                subCategoryObj.IsActive = true;

                var categorySaved = await _repository.Create(subCategoryObj);

                if (!categorySaved)
                {
                    return StatusCode(500, new BaseResponse<UpsertSubCategoryDTO>
                    {
                        ResponseMessage = "something went wrong",
                        Data = subCategory,
                        ResponseCode = ServITConstants.InternalServerError.ToString(),
                    });
                }

                SubCategoryDTO createdCategory = _mapper.Map<SubCategoryDTO>(subCategoryObj);

                return StatusCode(ServITConstants.CreatedStatusCode, new BaseResponse<SubCategoryDTO>
                {
                    ResponseCode = ServITConstants.CreatedStatusCode.ToString(),
                    Data = createdCategory,
                    ResponseMessage = $"Successfully created Category {subCategory.SubCategoryName}"
                });
            }
            catch (Exception ex) {
                _logger.LogError($"MethodName: CreateSubCategories() ===> {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{subCategoryId:guid}", Name = "DeleteSubCategory")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<SubCategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCategory(Guid subCategoryId)
        {

            try
            {
                SubCategory subCategory = await _repository.GetByID(subCategoryId);
                if (subCategory == null)
                {
                    return StatusCode(ServITConstants.BadRequestStatusCode, new BaseResponse<SubCategoryDTO>
                    {
                        ResponseMessage = "Category not found",
                        Data = null,
                        ResponseCode = ServITConstants.BadRequestStatusCode.ToString()
                    });
                }
                subCategory.IsDeleted = true;
                await _repository.Update(subCategory);
                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<SubCategoryDTO>
                {
                    ResponseMessage = "Delete successful.",
                    Data = _mapper.Map<SubCategoryDTO>(subCategory),
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetCategory() ===> {ex.Message}");
                throw;
            }
        }
    }
}
