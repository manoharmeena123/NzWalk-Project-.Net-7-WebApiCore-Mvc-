using Microsoft.AspNetCore.Mvc;
using NZWalksUI.Models;
using NZWalksUI.Models.DTO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalksUI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        //GET==========================================================================>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                // Get all region from Web  API

                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7150/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
              response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
                ViewBag.Response = response;
            }
            catch (Exception ex)
            {

        // Log the exception
            }

            return View(response);
        }



        
        [HttpGet]  
        public IActionResult Add()
        {
            return View();
        }

        //POST=========================================================================>
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7150/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

         var httpResponsMessage =   await client.SendAsync(httpRequestMessage);
            httpResponsMessage.EnsureSuccessStatusCode();

            var respons = await httpResponsMessage.Content.ReadFromJsonAsync<RegionDto>();
            if(respons != null)
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();
        }

        //Get For EDIT==================================================================>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7150/api/regions/{id}");
            if(response != null)
            {
                return View(response);
            }
            return View(null);
        }


        //Post For Edit=================================================================>

        [HttpPost]

        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7150/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponsMessage = await client.SendAsync(httpRequestMessage);
            httpResponsMessage.EnsureSuccessStatusCode();

            var respons = await httpResponsMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (respons != null)
            {
                return RedirectToAction("Index","Regions");
            }
            return View();


            
           

        }
        
        
        //POST for Delete=======================================================================>

              [HttpPost]
            public async Task<IActionResult>Delete(RegionDto request)
             {
                try
                {
                    var client = httpClientFactory.CreateClient();
                    var httpResponsMessage = await client.DeleteAsync($"https://localhost:7150/api/regions/{request.Id}");
                    httpResponsMessage.EnsureSuccessStatusCode();
                    return RedirectToAction("Index", "Regions");
                }
                catch (Exception ex)
                {
                    //Console
                   
                }
                return View("Edit");
            }

    }
}
