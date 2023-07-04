using APIProject.DTO.Category;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.IRepository;
using Repository.Repository;

namespace APIProject.Controllers
{
    [Authorize]
    public class CategoriesController : ODataController
    {
        private ICategoryRepository repository = new CategoryRepository();
        private readonly IMapper _mapper;

        public CategoriesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetCategoryResponseDTO>> Get()
        {
            List<Category> categorys = repository.GetCategories();
            List<GetCategoryResponseDTO> getCategoryResponseDTOs = _mapper.Map<List<GetCategoryResponseDTO>>(categorys);
            return Ok(getCategoryResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<GetCategoryResponseDTO> Get([FromRoute] int key)
        {
            Category category = repository.GetCategoryById(key);
            if (category == null)
            {
                return NotFound();
            }
            GetCategoryResponseDTO getCategoryResponseDTO = _mapper.Map<GetCategoryResponseDTO>(category);
            return Ok(getCategoryResponseDTO);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateCategoryRequestDTO createCategoryRequestDTO)
        {
            Category category = _mapper.Map<Category>(createCategoryRequestDTO);
            repository.SaveCategory(category);

            GetCategoryResponseDTO responseDTO = _mapper.Map<GetCategoryResponseDTO>(category);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateCategoryRequestDTO updateCategoryRequestDTO)
        {
            if (key != updateCategoryRequestDTO.CategoryId)
            {
                return BadRequest();
            }
            Category tempCategory = repository.GetCategoryById(key);
            if (tempCategory == null)
            {
                return NotFound();
            }
            Category category = _mapper.Map<Category>(updateCategoryRequestDTO);

            repository.UpdateCategory(category);
            return Updated(category);
        }
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            Category tempCategory = repository.GetCategoryById(key);
            if (tempCategory == null)
            {
                return NotFound();
            }
            repository.DeleteCategory(tempCategory);
            return NoContent();
        }
    }
}
