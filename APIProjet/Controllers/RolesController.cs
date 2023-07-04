using APIProject.DTO.Role;
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
    public class RolesController : ODataController
    {
        private IRoleRepository repository = new RoleRepository();
        private readonly IMapper _mapper;

        public RolesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery]
        public ActionResult<IQueryable<GetRoleResponseDTO>> Get()
        {
            List<Role> roles = repository.GetRoles();
            List<GetRoleResponseDTO> getRoleResponseDTOs = _mapper.Map<List<GetRoleResponseDTO>>(roles);
            return Ok(getRoleResponseDTOs);
        }
        [EnableQuery]
        public ActionResult<GetRoleResponseDTO> Get([FromRoute] int key)
        {
            Role role = repository.GetRoleById(key);
            if (role == null)
            {
                return NotFound();
            }
            GetRoleResponseDTO getRoleResponseDTO = _mapper.Map<GetRoleResponseDTO>(role);
            return Ok(getRoleResponseDTO);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateRoleRequestDTO createRoleRequestDTO)
        {
            Role role = _mapper.Map<Role>(createRoleRequestDTO);
            repository.SaveRole(role);

            GetRoleResponseDTO responseDTO = _mapper.Map<GetRoleResponseDTO>(role);
            return Created(responseDTO);
        }
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateRoleRequestDTO updateRoleRequestDTO)
        {
            if (key != updateRoleRequestDTO.RoleId)
            {
                return BadRequest();
            }
            Role tempRole = repository.GetRoleById(key);
            if (tempRole == null)
            {
                return NotFound();
            }
            Role role = _mapper.Map<Role>(updateRoleRequestDTO);

            repository.UpdateRole(role);
            return Updated(role);
        }
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            Role tempRole = repository.GetRoleById(key);
            if (tempRole == null)
            {
                return NotFound();
            }
            repository.DeleteRole(tempRole);
            return NoContent();
        }
    }
}
