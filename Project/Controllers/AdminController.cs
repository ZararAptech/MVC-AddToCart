using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Drawing;

namespace Project.Controllers
{
    public class AdminController : Controller
    {

        private readonly myDbContext conn;
        public AdminController()
        {
            conn = new myDbContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var zrr = conn.Products.ToList();
            return View(zrr);

           
        }
        [HttpPost]
        public ActionResult Index(Product pd)
        {
            return View();
        }
   
           
        




      
        public IActionResult HandleForm(Product pde,string action,int productId)
        {
            if (action == "Insert")
            {
                conn.Products.Add(pde);
                conn.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            else if (action == "Update")
            {

                var data = conn.Products.Find(productId);
                if (data != null)
                {
                    data.ProductName = pde.ProductName;
                    data.ProductPrice = pde.ProductPrice;
                    conn.Products.Update(data);
                    conn.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }


            }
            else if (action == "Delete")
            {

                var records = conn.Products.Find(productId);
                {
                    if(records != null)
                    {
                        conn.Remove(records);
                        conn.SaveChanges();
                        return RedirectToAction("Index", "Admin");
                    }
                }


                

          
            }
            return RedirectToAction("Index", "Admin");


        }

        public IActionResult Users()
        {
            var users = conn.Users.ToList();
            return View(users);
        }
        public IActionResult cform(User u, string action,int userId)
        {
            if(action == "Insert")
            {
                conn.Users.Add(u);
                conn.SaveChanges();
                return RedirectToAction("Users", "Admin");
            }

            else if(action == "Update")
            {
                var ud = conn.Users.Find(userId);
                if(ud != null)
                {
                    ud.UserName = u.UserName;
                    ud.Email = u.Email;
                    ud.Password = u.Password;
                    conn.Users.Update(ud);
                    conn.SaveChanges();
                    return RedirectToAction("Users", "Admin");
                }

            }
            else if( action == "Delete")
            {
                var records = conn.Users.Find(userId);
                if(records != null)
                {
                    conn.Remove(records);
                    conn.SaveChanges();
                    return RedirectToAction("Users", "Admin");
                }
            }


            return View("Index","Admin");
        }


    }
}
