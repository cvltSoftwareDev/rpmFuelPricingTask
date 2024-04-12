using FluentScheduler;
using Microsoft.Extensions.Configuration;
using rpmFuelPricingTask.Helpers;
using rpmFuelPricingTask.Services.Interfaces;

namespace rpmFuelPricingTask.Services
{
    public class TaskHandler : ITaskHandler
    {
        private readonly IApiCaller apiCaller;

        private readonly IRecordService recordService;

        private readonly IConfiguration configuration;

        public TaskHandler(IApiCaller apiCaller, IRecordService recordService, IConfiguration configuration)
        {
            this.apiCaller = apiCaller;
            this.recordService = recordService;
            this.configuration = configuration;
        }

        private ApiSettings Settings => configuration.GetSection("ApiSettings").Get<ApiSettings>();

        public void HandleTask()
        {
            try
            {
                //As given task allowed any mechanism for scheduling I chose FluentScheduler package

                var registry = new Registry();

                switch (Settings.ExecutionDelayPeriodType)
                {
                    case PeriodType.Hours:
                        registry.Schedule(Run).ToRunNow().AndEvery(Settings.ExecutionDelay).Hours();
                        break;

                    case PeriodType.Minutes:
                        registry.Schedule(Run).ToRunNow().AndEvery(Settings.ExecutionDelay).Minutes();
                        break;

                    case PeriodType.Days:
                    default:
                        registry.Schedule(Run).ToRunNow().AndEvery(Settings.ExecutionDelay).Days();
                        break;
                }

                JobManager.Initialize(registry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return;
            }
        }

        private void Run()
        {
            Console.WriteLine("*** NEW EVENT ***");
            Console.WriteLine($"Task executed with date {DateTime.Now}");

            var results = apiCaller.GetFuelPrices(Settings.ApiUrl, Settings.NumberOfDays).GetAwaiter().GetResult();

            Console.WriteLine($"Records fetched: {results.Count}");

            recordService.SaveRecords(results);

            Console.WriteLine($"Task done, next task execution in {Settings.ExecutionDelay} {Settings.ExecutionDelayPeriodType}.");
        }
    }
}