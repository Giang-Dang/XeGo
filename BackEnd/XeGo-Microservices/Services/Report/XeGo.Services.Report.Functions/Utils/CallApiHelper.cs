using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XeGo.Shared.Lib.Entities;

namespace XeGo.Services.Report.Functions.Utils
{
    public class CallApiHelper
    {
        public static async Task<List<T>> GetListFromUrl<T>(string url, ILogger? logger = null)
        {
            try
            {
                var http = new HttpHelpers();

                var response = await http.Get(url, null);

                var responseString = JsonConvert.SerializeObject(response);

                logger?.LogInformation($"{responseString}");

                var wrapper = JsonConvert.DeserializeObject<ResponseWrapper>(responseString);

                if (wrapper == null)
                {
                    throw new Exception("wrapper == null");
                }

                var valueJson = JObject.Parse(wrapper.Value);

                if (valueJson["data"] == null)
                {
                    throw new Exception("valueJson[\"data\"] == null");
                }

                var data = valueJson["data"]!.ToObject<List<T>>();

                if (data == null)
                {
                    throw new Exception("data == null");
                }

                return data;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
