using rpmFuelPricingTask.DbModels;

namespace rpmFuelPricingTask.Services.Interfaces
{
    public interface IApiCaller
    {
        Task<List<RecordModel>> GetFuelPrices(string urlSetting, int numberOfDaysSetting);
    }
}