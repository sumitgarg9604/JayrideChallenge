using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Jayride_Challenge.Models;
using System.Net.Http;
using Jayride_Challenge.SampleData;
using Microsoft.Extensions.Configuration;

namespace Jayride_Challenge.Controllers
{
    [ApiController]
    public class ApiTasks : ControllerBase
    {
        
        private readonly ILogger<ApiTasks> _logger;
        private IConfiguration config;

        public ApiTasks(ILogger<ApiTasks> logger,IConfiguration config)
        {
            _logger = logger;
            this.config = config;
        }

        /// <summary>
        /// Get all candidates
        /// </summary>
        /// <returns>Name and Phone of candidates</returns>
        [HttpGet]
        [Route("candidate")]
        public IActionResult GetCandidate()
        {
            try
            {
                //creating object of data creation class to load Test candidate 
                var obj = new dataCreation();
                var data = obj.CandidateTest();

                if (data is null) return NotFound("Data not found");
                return Ok(data);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>
        /// Get Name of City from IP Address
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Location/{ip}")]
        public async Task<IActionResult> GetCityFromIP(string ip)
        {
            try 
            {
                using (var client = new HttpClient())
                {
                    var url = "http://api.ipstack.com/" + ip + "?access_key=" + config["AccessKey"];
                    var uri = new Uri(url);
                    var response =  await client.GetAsync(uri);
                    var resultString = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IpModel>(resultString);
                    if (result.Ip is null) return BadRequest("Invaid IP address");
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// Get total price of listings with all details for valid listings
        /// </summary>
        /// <param name="numPassengers"></param>
        /// <returns>Ordered Listings by total price</returns>
        [HttpGet]
        [Route("Listings/{numPassengers}")]
        public async Task<IActionResult> GetTotalPrice(int numPassengers)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    //move uri and access key to web.config
                    var uri = "https://jayridechallengeapi.azurewebsites.net/api/QuoteRequest";
                    var response = await client.GetAsync(uri);
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Quote>(data);

                    //assuming invalid listing as the ones where max passengers is 0
                    var query = jsonObj.listings.Where(x => x.vehicleType.maxPassengers != 0);

                    //calculating total price of the listings
                    foreach (var listing in query)
                    {
                        listing.totalPrice = Math.Round((listing.pricePerPassenger * numPassengers),2);
                    }

                    //ordering by totalprice
                    var orderedListings = query.OrderByDescending(x => x.totalPrice);

                    return Ok(orderedListings);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
