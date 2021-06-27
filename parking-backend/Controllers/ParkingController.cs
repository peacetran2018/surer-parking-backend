using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using parking_backend.Configs;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace parking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("GetParkingLots")]
        public async Task<ParkingLotsResponse> GetParkingLots(string dateTimelots)
        {
            try
            {
                //AnTran@20210626: The url to get data.
                var url = "https://api.data.gov.sg/v1/transport/carpark-availability?";
                var client = new HttpClient();
                //AnTran@20210626: Pass in the full URL with parameter
                var response = await client.GetAsync(url + "date_time=" + dateTimelots);

                if(response.IsSuccessStatusCode)//If status code is 200
                {                    
                    var readStreamAsync = await response.Content.ReadAsStreamAsync();

                    //AnTran@20210626: close out the client
                    client.Dispose();

                    var streamReader = new StreamReader(readStreamAsync);
                    var jsonTextReader = new JsonTextReader(streamReader);

                    JsonSerializer jsonSerializer = new JsonSerializer();

                    try
                    {
                        return jsonSerializer.Deserialize<ParkingLotsResponse>(jsonTextReader);//Parsing to JSON
                    }
                    catch (JsonReaderException)
                    {
                        return null;
                    }
                }

                return null;
            }
            catch(HttpRequestException ex)
            {
                return null;
            }
           
        }
    }
}
