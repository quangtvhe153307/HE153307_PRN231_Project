using BusinessObjects;
using DataAccess;
using Quartz;
using Repository.IRepository;
using Repository.Repository;

namespace APIProject.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdatePlanJob : IJob
    {
        private readonly ILogger<UpdatePlanJob> _logger;
        private readonly IUserRepository repository;
        private readonly ITransactionRepository transactionRepository;
        public UpdatePlanJob(ILogger<UpdatePlanJob> logger, IUserRepository userRepository, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            repository = userRepository;
            this.transactionRepository = transactionRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var usersList = repository.GetUsers();
            for (int i = 0; i < usersList.Count; i++)
            {
                if (usersList[i].RoleId == 2 && usersList[i].ExpirationDate != null && usersList[i].ExpirationDate < DateTime.Today)
                {
                    if (usersList[i].Balance >= 100)
                    {
                        usersList[i].Balance -= 100;
                        usersList[i].ExpirationDate = ((DateTime)usersList[i].ExpirationDate).AddMonths(1);
                        transactionRepository.SaveTransaction(new BusinessObjects.Transaction
                        {
                            TransactionDescription = "Subtract 100 for maintain premium plan",
                            TransactionDate = DateTime.Now,
                            TransactionType = TransactionType.Purchase,
                            UserId = usersList[i].UserId,
                            CreatedBy = 1
                        });
                    } else
                    {
                        usersList[i].RoleId = 3;
                        usersList[i].ExpirationDate = null;
                    }
                }
            }
            repository.SaveUser(usersList);
            Console.WriteLine("job executed");
            _logger.LogInformation("Hello world!");
            return Task.CompletedTask;
        }
    }
}
