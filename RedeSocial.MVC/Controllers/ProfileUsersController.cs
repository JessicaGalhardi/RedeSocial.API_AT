using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedeSocial.BLL.Models;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedeSocial.MVC.Controllers
{
    public class ProfileUsersController : Controller
    {

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<ProfileUser> profilelist = new List<ProfileUser>();

            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/ProfileUsers"))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();

                    profilelist = JsonConvert.DeserializeObject<List<ProfileUser>>(apiRresponse);

                }
            }

            return View(profilelist);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            ProfileUser profilelist = new ProfileUser();
            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/ProfileUsers/" + id))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();

                    profilelist = JsonConvert.DeserializeObject<ProfileUser>(apiRresponse);

                }
            }

            return View(profilelist);
        }

        public ViewResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> Create(ProfileUser profile)
        {
            ProfileUser createProfile = new ProfileUser();

            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                httpClient.BaseAddress = new Uri("https://localhost:5001/"); 
                httpClient.DefaultRequestHeaders.Accept.Clear();

                var response = await httpClient.PostAsJsonAsync("api/ProfileUsers", profile);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "ProfileUsers");
                }
                else
                {
                    return View("Index");
                }



            }



        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProfileUser profile = new ProfileUser();

            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/ProfileUsers/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    profile = JsonConvert.DeserializeObject<ProfileUser>(apiResponse);


                }
            }

            return View(profile);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileUser profile)
        {
            ProfileUser upProfile = new ProfileUser();

            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

               // httpClient.BaseAddress = new Uri("https://localhost:5001/");
                //httpClient.DefaultRequestHeaders.Accept.Clear();

                StringContent content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:5001/api/ProfileUsers/" + profile.Id, content))
                {
                    string apiResponse= await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "ProfileUsers");
                    }
                    else
                    {
                        return View("Edit");
                    }

                }
                

            }

           
        }
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.DeleteAsync("https://localhost:5001/api/ProfileUsers/" + id))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();


                }

                return RedirectToAction("Index");

            }
        }
    }
}
    