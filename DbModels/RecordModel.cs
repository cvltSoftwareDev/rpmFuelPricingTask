namespace rpmFuelPricingTask.DbModels
{
    public class RecordModel
    {
        public int Id { get; set; }

        public DateTime Period { get; set; }

        public decimal Price { get; set; }
    }
}