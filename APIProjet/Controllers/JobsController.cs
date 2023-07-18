using APIProjet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using Quartz;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ISchedulerFactory factory;
        private readonly ILogger<JobsController> _logger;
        private readonly IConfiguration _config;

        public JobsController(ISchedulerFactory factory, ILogger<JobsController> logger, IConfiguration config)
        {
            this.factory = factory;
            _logger = logger;
            _config = config;
        }
        [HttpGet("UpdatePlanJob")]
        public async Task<OkObjectResult> Run()
        {
            IScheduler scheduler = await factory.GetScheduler();
            var jobKey = new JobKey(_config["Jobs:UpdatePlan"]);

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.TriggerJob(jobKey);
            }
            return Ok("");
        }
    }
}
