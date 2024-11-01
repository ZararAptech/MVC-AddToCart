using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly myDbContext conn;
        public HomeController()
        {
            conn = new myDbContext();
        }


        public IActionResult Index()
        {
            return View();

        }



        public IActionResult ListView()
        {

            var data = conn.Products.ToList();
            return View(data);



        }
        [HttpGet]
        public IActionResult signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult signup(User u)
        {
            conn.Users.Add(u);
            conn.SaveChanges();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User ud)
        {
            var user = conn.Users.Where(m => m.Email == ud.Email && m.Password == ud.Password).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                return RedirectToAction("ListView");
            }
            else
            {
                ViewBag.Message = "User Id Or Password Incorrect";
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(int ProductId)
        {
            var user = HttpContext.Session.GetString("UserId");
            ProductCart his = new ProductCart();
            {
                his.UserId = int.Parse(user);
                his.ProductId = ProductId;
            }
            conn.Productcarts.Add(his);
            conn.SaveChanges();

            TempData["SuccessMessage"] = "Item added to cart successfully!";

            return RedirectToAction("ListView");

        }

        [HttpPost]
        public IActionResult CheckOut()
        {


            var prdct = HttpContext.Session.GetString("UserId");
            if (int.TryParse(prdct, out int UserId))
            {


                var result = (from pc in conn.Productcarts
                              join p in conn.Products
                              on pc.ProductId equals p.ProductId
                              where pc.UserId == UserId
                              select new ProductCartViewModel
                              {
                                  UserId = pc.UserId,
                                  ProductId = pc.ProductId,
                                  ProductName = p.ProductName,
                                  ProductPrice = p.ProductPrice
                              }).ToList();
                return View(result);
            }
            return View(new List<ProductCartViewModel>());

        }
        public IActionResult Delete(int productId)
        {

            var userId = HttpContext.Session.GetString("UserId");

            if (userId != null)

            {

                var cartItem = conn.Productcarts.Where(pc => pc.UserId == int.Parse(userId) && pc.ProductId == productId).FirstOrDefault();

                if (cartItem != null)
                {

                    conn.Productcarts.Remove(cartItem);
                    conn.SaveChanges();
                }
                return RedirectToAction("CheckOut");
            }
            return View("Index");
        }


        public IActionResult ConfirmOrder(int orderId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            
            var order = conn.Productcarts.Where(pc => pc.UserId == int.Parse(userId) && pc.ProductId == orderId).FirstOrDefault();

            if (order != null)
            {
                order.OrderConfirmed = true;
                conn.SaveChanges();
                TempData["SuccessMessage"] = "Item  ordered  successfully!";

            }
            return RedirectToAction("CheckOut");
          
        }


        public IActionResult Corders()
        {
            var userId = HttpContext.Session.GetString("UserId");
           
                if (int.TryParse(userId, out int UserId))
                {


                    var result = (from pc in conn.Productcarts
                                  join p in conn.Products
                                  on pc.ProductId equals p.ProductId
                                  where pc.UserId == UserId && pc.OrderConfirmed == true
                                  select new ProductCartViewModel
                                  {
                                      UserId = pc.UserId,
                                      ProductId = pc.ProductId,
                                      ProductName = p.ProductName,
                                      ProductPrice = p.ProductPrice
                                  }).ToList();
                    return View(result);
                }
              
            return View(new List<ProductCartViewModel>());
        }
        
    }
};

