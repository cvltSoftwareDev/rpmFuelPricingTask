using rpmFuelPricingTask.DbModels;

namespace rpmFuelPricingTask.Services.Interfaces
{
    public interface IRecordService
    {
        void SaveRecords(List<RecordModel> model);
    }
}