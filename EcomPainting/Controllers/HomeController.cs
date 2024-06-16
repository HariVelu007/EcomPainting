using EcomPainting.Dtos;
using EcomPainting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.IO;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
namespace EcomPainting.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {        
        private readonly EcomContext _context;
        
        public HomeController( EcomContext context)
        {            
            _context = context;
        
        }
        
        [HttpGet]
        public IActionResult Index(ItemFilter filter,int PageNo = 1)
        {
            ViewBag.PageNo = PageNo;
            ItemList list = new ItemList();
            list.Items = _context.Items.Skip(10 * (PageNo - 1)).Take(10).ToList();
            list.TotalRecords = _context.Items.Count();
            ItemPageDto dto = new ItemPageDto();
            dto.itemList = list;
            dto.filter = filter;
            return View(dto);         
        }
        [HttpGet]
        public IActionResult View(int? id)
        {
            if (id == null)
            {
                ViewBag.Status = "Invalid Id";
                return View("");
            }
            Item item = _context.Items.Find(id);
            ItemDto dto = new ItemDto()
            {
                Id = item.ID,
                Name = item.Name,
                Description = item.Description,
                Material = item.Material,
                Price = item.Price,
                Type = item.Type,
                Url = item.Url
            };
            return View(dto);
        }
        [HttpGet]
        public IActionResult Add(int? id)
        {
            if (id == null)
            {
                ViewBag.Status = "Invalid Id";
                return View("");
            }
            Item item = _context.Items.Find(id);
            Cart cart = new Cart()
            {                
                ItemId = item.ID,
                UserId =Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value)               
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Cart()
        {
            int userid = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Cart> cartItems = _context.Carts.Where(e => e.UserId == userid).Include(i => i.Item).ToList();
            return View(cartItems);
        }
        [HttpGet]
        public IActionResult Order()
        {
            int userid = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<Order> orders = _context.Orders.Where(e => e.UserId == userid).Include(i => i.Item).ToList();            
            return View(orders);
        }
        [HttpGet]
        public IActionResult Buy(int? itemId)
        {
            if (itemId == null)
            {
                ViewBag.Status = "Invalid Id";
                return View("");
            }
            
            Item item = _context.Items.Find(itemId);
            Order order = new Order()
            {
                ItemId = item.ID,
                UserId = Convert.ToInt16(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };
            _context.Orders.Add(order);
            bool status= _context.SaveChanges()>0;
            if(status)
            {
                Cart cart= _context.Carts.Where(i=>i.ItemId==itemId).FirstOrDefault();
                if(cart != null)
                {
                    _context.Carts.Remove(cart);
                    _context.SaveChanges();                  
                }
            }
            return RedirectToAction("Cart");
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Forbidden()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       

    }
}
