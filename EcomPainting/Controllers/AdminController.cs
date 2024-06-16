using EcomPainting.Dtos;
using EcomPainting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcomPainting.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EcomContext _context;
        private readonly IWebHostEnvironment _env;
        public AdminController(EcomContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Items( int PageNo=1)
        {
            ViewBag.PageNo = PageNo;
            ItemList list = new ItemList();
            list.Items = _context.Items.Skip(10*(PageNo-1)).Take(10).ToList();
            list.TotalRecords = _context.Items.Count();
            return View(list);
        }
        
        [HttpGet]
        public IActionResult AddItem()
        {
            return View(new ItemDto());
        }
        
        [HttpPost]
        public IActionResult AddItem(ItemDto item)
        {
            
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            if (item.ItemImage == null)
            {
                ModelState.AddModelError("ItemImage", "Item image missing");
                return View(item);
            }
            string fileurl = UploadFile(item.ItemImage);
            Item itemModel = new Item()
            {
                Description = item.Description,
                Material = item.Material,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type,
                Url = fileurl,
            };
            _context.Items.Add(itemModel);
            bool status = _context.SaveChanges() > 0 ? true : false;
            ViewBag.Status = status;
            return View(new ItemDto());//model not cleared after save
        }
        [HttpGet]
        public IActionResult EditItem(int? id)
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
            return View("AddItem", dto);
        }
        [HttpPost]
        public IActionResult UpdateItem(ItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("AddItem", dto);
            }
            Item item = _context.Items.Find(dto.Id);
            bool filestatus = CheckImageIsUpdated(item.Url, dto.Url);
            string fileurl = "";
            if (filestatus)
            {
                DeleteUploadedFile(item.Url);
                fileurl = UploadFile(dto.ItemImage);
            }

            item.Description = dto.Description;
            item.Material = dto.Material;
            item.Name = dto.Name;
            item.Price = dto.Price;
            item.Type = dto.Type;
            item.Url = fileurl == "" ? item.Url : fileurl;

            _context.Items.Update(item);
            bool status = _context.SaveChanges() > 0 ? true : false;
            ViewBag.Status = status;
            return View("AddItem", new ItemDto());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteItem(int? id)
        {
            if (id == null)
            {
                ViewBag.Status = "Invalid Id";
            }
            Item item = _context.Items.Find(id);
            _context.Items.Remove(item);
            DeleteUploadedFile(item.Url);
            bool status = _context.SaveChanges() > 0 ? true : false;
            ViewBag.Status = status;
            return RedirectToAction("Items");
        }


        private string UploadFile(IFormFile file)
        {
            string fileFullName = "";
            string basePath = Path.Combine(_env.WebRootPath, "Images", "UploadedImages");
            bool basePathExists = Directory.Exists(basePath);
            if (!basePathExists)
                Directory.CreateDirectory(basePath);
            var extension = Path.GetExtension(file.FileName);
            fileFullName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(basePath, fileFullName);

            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return $@"~/Images/UploadedImages/{fileFullName}";
        }
        private void DeleteUploadedFile(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        private bool CheckImageIsUpdated(string oldImage, string newImage)
        {
            string OldFileName = Path.GetFileName(oldImage);
            string NewFileName = Path.GetFileName(newImage);
            return OldFileName == NewFileName ? false : true;
        }
    }
}
