namespace rpmFuelPricingTask.Helpers
{
    public class ApiSettings
    {
        public string ApiUrl { get; set; }

        public PeriodType ExecutionDelayPeriodType { get; set; }

        public int ExecutionDelay { get; set; }

        public int NumberOfDays { get; set; }
    }
}