using APIProject.DTO.Comment;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.IRepository;
using Repository.Repository;

namespace APIProject.Controllers
{
    public class CommentsController : ODataController
    {
        private ICommentRepository repository = new CommentRepository();
        private readonly IMapper _mapper;

        public CommentsController(IMapper mapper)
        {
            _mapper = mapper;
        }
        [EnableQuery(PageSize = 10)]
        public ActionResult<IQueryable<GetCommentResponseDTO>> Get()
        {
            List<Comment> comments = repository.GetComments();
            List<GetCommentResponseDTO> getCommentResponseDTOs = _mapper.Map<List<GetCommentResponseDTO>>(comments);
            return Ok(getCommentResponseDTOs);
        }
        //[EnableQuery]
        //public ActionResult<GetCommentResponseDTO> Get([FromRoute] int key)
        //{
        //    Comment comment = repository.GetCommentById(key);
        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }
        //    GetCommentResponseDTO getCommentResponseDTO = _mapper.Map<GetCommentResponseDTO>(comment);
        //    return Ok(getCommentResponseDTO);
        //}
        [EnableQuery]
        public IActionResult Post([FromBody] CreateCommentRequestDTO createCommentRequestDTO)
        {
            Comment comment = _mapper.Map<Comment>(createCommentRequestDTO);
            repository.SaveComment(comment);

            GetCommentResponseDTO responseDTO = _mapper.Map<GetCommentResponseDTO>(comment);
            return Created(responseDTO);
        }
        //[EnableQuery]
        //public ActionResult Put([FromRoute] int key, [FromBody] UpdateCommentRequestDTO updateCommentRequestDTO)
        //{
        //    if (key != updateCommentRequestDTO.CommentId)
        //    {
        //        return BadRequest();
        //    }
        //    Comment tempComment = repository.GetCommentById(key);
        //    if (tempComment == null)
        //    {
        //        return NotFound();
        //    }
        //    Comment comment = _mapper.Map<Comment>(updateCommentRequestDTO);
        //    repository.UpdateComment(comment);
        //    return Updated(comment);
        //}
        //[EnableQuery]
        //public ActionResult Delete([FromRoute] int key)
        //{
        //    Comment tempComment = repository.GetCommentById(key);
        //    if (tempComment == null)
        //    {
        //        return NotFound();
        //    }
        //    repository.DeleteComment(tempComment);
        //    return NoContent();
        //}
    }
}
