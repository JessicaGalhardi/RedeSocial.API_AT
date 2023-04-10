using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedeSocial.BLL.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RedeSocial.MVC.Controllers
{
    public class PostsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Post> postList = new List<Post>();

            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/Posts"))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();

                    postList = JsonConvert.DeserializeObject<List<Post>>(apiRresponse);

                }
            }

            return View(postList);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsPost(int id)
        {

            Post postList = new Post();

            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/Posts/" + id))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();

                    postList = JsonConvert.DeserializeObject<Post>(apiRresponse);

                }
            }

            return View(postList);
        }

        public ViewResult CreatePost() => View();


        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            List<Post> postList = new List<Post>();

            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                httpClient.BaseAddress = new Uri("https://localhost:5001/");
                httpClient.DefaultRequestHeaders.Accept.Clear();

                var response = await httpClient.PostAsJsonAsync("api/Posts", post);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    return View("Create");
                }



            }



        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            Post post = new Post();

            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.GetAsync("https://localhost:5001/api/Posts/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    post = JsonConvert.DeserializeObject<Post>(apiResponse);


                }
            }

            return View(post);

        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Post post)
        {
            List<Post> upPost = new List<Post>();

            var accessToken = HttpContext.Session.GetString("JWToken");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // httpClient.BaseAddress = new Uri("https://localhost:5001/");
                //httpClient.DefaultRequestHeaders.Accept.Clear();

                StringContent content = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:5001/api/Posts/" + post.Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Posts");
                    }
                    else
                    {
                        return View("EditPost");
                    }

                }


            }

        }

      
        public async Task<IActionResult> DeletePost(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var accessToken = HttpContext.Session.GetString("JWToken");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                using (var response = await httpClient.DeleteAsync("https://localhost:5001/api/Posts/" + id))
                {

                    string apiRresponse = await response.Content.ReadAsStringAsync();


                }

                return RedirectToAction("Index", "Posts");

            }

        }
    }
}

