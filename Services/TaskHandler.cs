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
                var executionDelayTimespan = GetPeriod();

                var timer = new Timer((e) =>
                {
                    Run();
                }, null, TimeSpan.Zero, executionDelayTimespan);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return;
            }
        }

        private void Run()
        {
            Console.WriteLine($"Task executed with date {DateTime.Now}");

            var results = apiCaller.GetFuelPrices(Settings.ApiUrl, Settings.NumberOfDays).GetAwaiter().GetResult();

            Console.WriteLine($"Records fetched: {results.Count}");

            recordService.SaveRecords(results);

            Console.WriteLine($"Task done, next task execution in {Settings.ExecutionDelay} {Settings.ExecutionDelayPeriodType}.");
        }

        private TimeSpan GetPeriod()
        {
            switch (Settings.ExecutionDelayPeriodType)
            {
                case PeriodType.Hours:
                    return TimeSpan.FromHours(Settings.ExecutionDelay);

                case PeriodType.Minutes:
                    return TimeSpan.FromMinutes(Settings.ExecutionDelay);

                case PeriodType.Days:
                default:
                    // Because Timer 'period' parameter is expressed in milliseconds, and 43 days or more is greater than timespan max value
                    // It can be bypassed by using windows task scheduler but it would not be in line with the task description

                    if (Settings.ExecutionDelay > 42)
                        throw new ArgumentOutOfRangeException($"Execution delay is set to more than 42 days (currently set as {Settings.ExecutionDelay}).");

                    return TimeSpan.FromDays(Settings.ExecutionDelay);
            }
        }
    }
}