using APIProject.DTO;
using APIProject.DTO.Category;
using APIProject.ZaloPay;
using APIProject.ZaloPay.Crypto;
using APIProject.ZaloPay.Models;
using AutoMapper;
using BusinessObjects;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BalancesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository repository;
        private readonly IUserRepository userRepository;
        private readonly ITransactionRepository transactionRepository;
        private IConfiguration _config;
        public BalancesController(IMapper mapper, IOrderRepository repository, IConfiguration config, IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            _mapper = mapper;
            this.repository = repository;
            _config = config;
            this.userRepository= userRepository;
            this.transactionRepository= transactionRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddBalanceModel addBalanceModel)
        {
            var embeddata = NgrokHelper.CreateEmbeddataWithPublicUrl();

            var orderData = new OrderData(addBalanceModel.Amount, "", "", embeddata);
            var order = await ZaloPayHelper.CreateOrder(orderData);
            var returncode = (long)order["returncode"];
            if (returncode == 1)
            {
                repository.SaveOrder(new Order
                {
                    Apptransid = orderData.Apptransid,
                    UserId = LoggedUserId(),
                    Amount = addBalanceModel.Amount,
                    Status = false
                });
                var orderurl = order["orderurl"].ToString();
                var response = new AddBalanceResponseModel
                {
                    OrderUrl = orderurl,
                    Apptransid = orderData.Apptransid
                };
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }        
        [HttpPost("/CheckOrder")]
        public async Task<IActionResult> CheckOrder([FromBody] CheckOrderDTO addBalanceModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var order = repository.GetOrderById(addBalanceModel.Apptransid);
            //return if already checked
            if (order.Status)
            {
                return BadRequest();
            }
            var param = new Dictionary<string, string>();
            param.Add("appid", _config["ZaloPayConfig:Appid"]);
            param.Add("apptransid", addBalanceModel.Apptransid);
            var data = _config["ZaloPayConfig:Appid"] + "|" + addBalanceModel.Apptransid + "|" + _config["ZaloPayConfig:Key1"];

            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, _config["ZaloPayConfig:Key1"], data));

            var result = await HttpHelper.PostFormAsync(_config["ZaloPayConfig:ZaloPayApiGetOrderStatus"], param);

            if ((Int64)result["returncode"] == 1)
            {
                //change status of order
                order.Status = true;
                repository.UpdateOrder(order);

                //add to transaction
                transactionRepository.SaveTransaction(new Transaction
                {
                    TransactionDescription = $"Add {order.Amount} VND to account via ZaloPay",
                    TransactionDate = DateTime.Now,
                    TransactionType = TransactionType.Add,
                    UserId = order.UserId,
                    IsDelete = false,
                    CreatedBy = 1
                });
                //add user balance
                User user = userRepository.GetUserById(order.UserId);
                user.Balance += order.Amount;
                userRepository.UpdateUser(user);

                return Ok("done");
            }
            return Ok("processing");
        } 
        private int LoggedUserId()
        {
            var userIdString = User.Claims.ToList()[4].Value;
            int userId = Int32.Parse(userIdString);
            return userId;
        }
    }
}
