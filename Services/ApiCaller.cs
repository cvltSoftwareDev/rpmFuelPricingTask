using Newtonsoft.Json.Linq;
using rpmFuelPricingTask.DbModels;
using rpmFuelPricingTask.Services.Interfaces;

namespace rpmFuelPricingTask.Services
{
    public class ApiCaller : IApiCaller
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<RecordModel>> GetFuelPrices(string urlSetting, int numberOfDaysSetting)
        {
            var resultsToSave = new List<RecordModel>();

            var response = await client.GetAsync(urlSetting);

            if (!response.IsSuccessStatusCode)
            {
                Console.Write($"Api call error! {response.StatusCode}");
            }

            try
            {
                var result = await response.Content.ReadAsStringAsync();

                dynamic dbResultsList = JObject.Parse(result);
                var onlyData = dbResultsList.response.data; // Extracting the list of records

                resultsToSave = ((JArray)onlyData).Select(x => new RecordModel
                {
                    Period = Convert.ToDateTime(x["period"]),
                    Price = Convert.ToDecimal(x["value"])
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return TrimRecords(resultsToSave, numberOfDaysSetting);
        }

        private List<RecordModel> TrimRecords(List<RecordModel> records, int numberOfDaysSetting)
        {
            var currentDate = DateTime.Now;

            return records.Where(x => (currentDate - x.Period).Days <= numberOfDaysSetting).ToList();
        }
    }
}