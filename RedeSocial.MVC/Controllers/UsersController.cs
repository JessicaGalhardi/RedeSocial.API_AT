using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedeSocial.BLL.Models;
using System.Text;

namespace RedeSocial.MVC.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Usuarios usuarios)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/Json");

                var response = await httpClient.PostAsync("https://localhost:5001/api/Users/api/Usuarios/Register/", content);


            }

            return Redirect("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuarios usuarios)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:5001/api/Users/Login/", stringContent))
                {
                    string token = await response.Content.ReadAsStringAsync();



                    if (token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect UserId or Password!";
                        return View("Login");
                    }

                    HttpContext.Session.SetString("JWToken", token);

                }

                return RedirectToAction("Index", "ProfileUsers");
            }
        }





    }
}

    

