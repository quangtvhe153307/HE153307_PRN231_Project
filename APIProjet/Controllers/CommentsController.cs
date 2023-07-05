using APIProject.DTO.Comment;
using AutoMapper;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repository.IRepository;
using Repository.Repository;
using System.Data;

namespace APIProject.Controllers
{
    [Authorize(Roles = "Administrator")]
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
        [EnableQuery]
        public ActionResult<IQueryable<GetCommentResponseDTO>> Get([FromRoute] int key)
        {
            List<Comment> comments = repository.GetCommentByMovieId(key);
            List<GetCommentResponseDTO> getCommentResponseDTOs = _mapper.Map<List<GetCommentResponseDTO>>(comments);
            return Ok(getCommentResponseDTOs);
        }
        [EnableQuery]
        public IActionResult Post([FromBody] CreateCommentRequestDTO createCommentRequestDTO)
        {
            try
            {
                Comment comment = _mapper.Map<Comment>(createCommentRequestDTO);
                comment.UserId = LoggedUserId();
                comment.CommentedDate = DateTime.Now;
                repository.SaveComment(comment);

                GetCommentResponseDTO responseDTO = _mapper.Map<GetCommentResponseDTO>(comment);
                return Created(responseDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = "error"});
            }
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
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
