using APIProject.DTO.Transaction;
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
    [Authorize(Roles = "Administrator,VIP,Normal")]
    public class TransactionsController : ODataController
    {
        private ITransactionRepository repository;
        private readonly IMapper _mapper;

        public TransactionsController(IMapper mapper, ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            repository= transactionRepository;
        }
        [Authorize(Roles = "Administrator,VIP,Normal")]
        [EnableQuery(PageSize = 5)]
        public ActionResult<IQueryable<GetTransactionResponseDTO>> Get()
        {
            List<Transaction> transactions = repository.GetTransactions();
            List<GetTransactionResponseDTO> getTransactionResponseDTOs = _mapper.Map<List<GetTransactionResponseDTO>>(transactions);
            return Ok(getTransactionResponseDTOs);
        }
        //[Authorize(Roles = "Administrator,VIP,Normal")]
        //[HttpGet("/MyTransaction")]
        //[EnableQuery(PageSize = 5)]
        //public IQue GetMyTransaction()
        //{
        //    List<Transaction> transactions = repository.GetMyTransactions(LoggedUserId());
        //    List<GetTransactionResponseDTO> getTransactionResponseDTOs = _mapper.Map<List<GetTransactionResponseDTO>>(transactions);
        //    return getTransactionResponseDTOs.AsQueryable();
        //}
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult<GetTransactionResponseDTO> Get([FromRoute] int key)
        {
            Transaction transaction = repository.GetTransactionById(key);
            if (transaction == null)
            {
                return NotFound();
            }
            GetTransactionResponseDTO getTransactionResponseDTO = _mapper.Map<GetTransactionResponseDTO>(transaction);
            return Ok(getTransactionResponseDTO);
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public IActionResult Post([FromBody] CreateTransactionRequestDTO createTransactionRequestDTO)
        {
            try
            {
                Transaction transaction = _mapper.Map<Transaction>(createTransactionRequestDTO);
                transaction.CreatedBy = LoggedUserId();
                repository.SaveTransaction(transaction);

                GetTransactionResponseDTO responseDTO = _mapper.Map<GetTransactionResponseDTO>(transaction);
                return Created(responseDTO);
            } catch(Exception ex)
            {
                return BadRequest(new {message = "error"});
            }
        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] UpdateTransactionRequestDTO updateTransactionRequestDTO)
        {
            try
            {
                var userId = LoggedUserId();
                if (key != updateTransactionRequestDTO.TransactionId)
                {
                    return BadRequest();
                }
                Transaction tempTransaction = repository.GetTransactionById(key);
                if (tempTransaction == null)
                {
                    return NotFound();
                }
                //Transaction transaction = _mapper.Map<Transaction>(updateTransactionRequestDTO);
                tempTransaction.TransactionDescription = updateTransactionRequestDTO.TransactionDescription;
                tempTransaction.ModifiedBy= LoggedUserId();
                tempTransaction.ModifiedDate = DateTime.Now;
                repository.UpdateTransaction(tempTransaction);
                return Updated(tempTransaction);
            } catch(Exception ex)
            {
                return BadRequest(new { message = "error" });
            }

        }
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public ActionResult Delete([FromRoute] int key)
        {
            try
            {
                int LoggedUser = LoggedUserId();
                Transaction tempTransaction = repository.GetTransactionById(key);
                if (tempTransaction == null)
                {
                    return NotFound();
                }
                tempTransaction.IsDelete = true;
                tempTransaction.ModifiedDate= DateTime.Now;
                tempTransaction.ModifiedBy = LoggedUser;
                repository.UpdateTransaction(tempTransaction);
                return NoContent();
            } catch(Exception ex)
            {
                return BadRequest(new { message = "error" });
            }

        }
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
