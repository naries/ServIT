using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServIT.Business.GenericResponse;
using ServIT.Data.AppConstants;
using ServIT.Data.DTO.Category;
using ServIT.Data.Repository;
using ServIT.Data.Repository.IRepository;
using ServIT.Models;

namespace ServIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryController(ICategoryRepository repository, IMapper mapper, ILogger<CategoryRepository> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a Category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BaseResponse<UpsertCategoryDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse<CategoryDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(BaseResponse<CategoryDTO>))]
        public async Task<IActionResult> CreateCategories([FromBody] UpsertCategoryDTO category)
        {
            string message = $"Successfully created Category {category.CategoryName}";
            int responseCode = ServITConstants.CreatedStatusCode;

            Category categoryObj = _mapper.Map<Category>(category);
            if (category.CategoryName.Trim().Length == 0)
            {
                message = "Please provide a category name";
                responseCode = ServITConstants.BadRequestStatusCode;
            }

            if(await _repository.CategoryNameExists(category.CategoryName.Trim()))
            {
                message = "Category already exists!";
                responseCode = ServITConstants.BadRequestStatusCode;
            }

            if (responseCode != ServITConstants.CreatedStatusCode)
            {
                return StatusCode(responseCode, new BaseResponse<UpsertCategoryDTO>
                {
                    ResponseMessage = message,
                    //Data = null,
                    ResponseCode = responseCode.ToString(),
                });
            }

            // Add dates
            categoryObj.DateCreated = DateTime.Now;
            categoryObj.DateUpdated = DateTime.Now;
            categoryObj.IsDeleted = false;
            categoryObj.IsActive = true;

            var categorySaved = await _repository.Create(categoryObj);


            if (!categorySaved)
            {
                return StatusCode(500, new BaseResponse<UpsertCategoryDTO>
                {
                    ResponseMessage = "something went wrong",
                    Data = category,
                    ResponseCode = ServITConstants.InternalServerError.ToString(),
                });
            }

            // map it back to DTO before returning
            CategoryDTO createdCategory = _mapper.Map<CategoryDTO>(categoryObj);

            return StatusCode(ServITConstants.CreatedStatusCode, new BaseResponse<CategoryDTO>
            {
                ResponseCode = ServITConstants.CreatedStatusCode.ToString(),
                Data = createdCategory,
                ResponseMessage = $"Successfully created Category {category.CategoryName}"
            });
        }

        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<List<CategoryDTO>>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllCategories()
        {
            ICollection<Category> categories = await _repository.GetAll();
            List<CategoryDTO> categoriesDTO = new();

            foreach (var category in categories)
            {
                categoriesDTO.Add(_mapper.Map<CategoryDTO>(category));
            }

            return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<List<CategoryDTO>>
            {
                ResponseMessage = "Retrieval successful",
                Data = categoriesDTO,
                ResponseCode = ServITConstants.SuccessStatusCode.ToString()
            });
        }

        [HttpGet("{categoryId:guid}", Name = "GetCategoryById")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<CategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            try
            {
                Category category = await _repository.GetByID(categoryId);
                if (category == null)
                {
                    return StatusCode(ServITConstants.NotFoundStatusCode, new BaseResponse<CategoryDTO>
                    {
                        ResponseMessage = "Category not found",
                        Data = null,
                        ResponseCode = ServITConstants.NotFoundStatusCode.ToString()
                    });
                }

                CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(category);
                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<CategoryDTO>
                {
                    ResponseMessage = "Retrieval successful",
                    Data = categoryDTO,
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetCategory() ===> {ex.Message}");
                throw;
            }
        }

        [HttpPut("{categoryId:guid}", Name = "updateCategoryById")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<CategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCategory([FromBody] UpsertCategoryDTO categoryDTO, Guid categoryId)
        {
            try
            {
                Category category = await _repository.GetByID(categoryId);
                if (category == null)
                {
                    return StatusCode(ServITConstants.NotFoundStatusCode, new BaseResponse<CategoryDTO>
                    {
                        ResponseMessage = "Category not found",
                        Data = null,
                        ResponseCode = ServITConstants.NotFoundStatusCode.ToString()
                    });
                }

                // Category newCategory = _mapper.Map<Category>(categoryDTO);
                category.CategoryName = categoryDTO.CategoryName;
                category.DateUpdated = DateTime.Now;
                _repository.Update(category);
                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<CategoryDTO>
                {
                    ResponseMessage = "Update successful.",
                    Data = _mapper.Map<CategoryDTO>(category),
                    ResponseCode = ServITConstants.SuccessStatusCode.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetCategory() ===> {ex.Message}");
                throw;
            }
        }

        [HttpDelete("{categoryId:guid}", Name = "DeleteCategory")]
        [ProducesResponseType(200, Type = typeof(BaseResponse<CategoryDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {

            try
            {
                Category category = await _repository.GetByID(categoryId);
                if (category == null)
                {
                    return StatusCode(ServITConstants.BadRequestStatusCode, new BaseResponse<CategoryDTO>
                    {
                        ResponseMessage = "Category not found",
                        Data = null,
                        ResponseCode = ServITConstants.BadRequestStatusCode.ToString()
                    });
                }
                category.IsDeleted = true;
                await _repository.Update(category);
                return StatusCode(ServITConstants.SuccessStatusCode, new BaseResponse<CategoryDTO>
                {
                    ResponseMessage = "Delete successful.",
                    Data = _mapper.Map<CategoryDTO>(category),
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
