using Quartz;

namespace APIProject.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdatePlanJob : IJob
    {
        private readonly ILogger<UpdatePlanJob> _logger;

        public UpdatePlanJob(ILogger<UpdatePlanJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("job executed");
            _logger.LogInformation("Hello world!");
            return Task.CompletedTask;
        }
    }
}
