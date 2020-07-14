using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class AirportCodesController : Controller
    {
        // GET: /<controller>/
        public async Task<ActionResult> Index(Airport? airport)
        {

            if(airport.Iata != null)
            {
                HttpClient client = new HttpClient();

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://airport-info.p.rapidapi.com/airport?iata=" + airport.Iata),
                    Method = HttpMethod.Get,
                };
                request.Headers.Add("X-RapidAPI-Key", "060f515249mshd32f27de8dcac1ap12474djsnce8aec08381f");

                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    Airport a = JsonConvert.DeserializeObject<Airport>(data);
                    a.maps = String.Format("https://www.google.com/maps/search/?api=1&query={0},{1}", a.Latitude, a.Longitude);

                    return View(a);
                }
                else
                {
                    return View(new Airport());
                }
            }
            else
            {
                return View(new Airport());
            }
      
        }

        

    }
}

