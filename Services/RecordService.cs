using rpmFuelPricingTask.Context;
using rpmFuelPricingTask.DbModels;
using rpmFuelPricingTask.Services.Interfaces;

namespace rpmFuelPricingTask.Services
{
    public class RecordService : IRecordService
    {
        public void SaveRecords(List<RecordModel> model)
        {
            using (var context = new AppDbContext())
            {
                var periodsInDb = context.Records.Select(r => r.Period);

                var newItemsToAdd = model.Where(x => !periodsInDb.Contains(x.Period));

                var newItemsCount = newItemsToAdd.Count();

                context.Records.AddRange(newItemsToAdd);

                context.SaveChanges();

                Console.WriteLine($"Records added to database: {newItemsCount}");
            }
        }
    }
}