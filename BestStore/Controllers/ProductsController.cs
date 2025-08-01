using AspNetCoreGeneratedDocument;
using BestStore.Data;
using BestStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BestStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private const int PageSize = 5; // Số sản phẩm mỗi trang

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        // Danh sách sản phẩm với phân trang và tìm kiếm
        public IActionResult Index(string? searchString, int page = 1)
        {
            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            int totalItems = products.Count();
            var productList = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            return View(productList);
        }

        // Form tạo mới
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo mới
        [HttpPost]
        public IActionResult Create(ProductDTO productDTO)
        {
            if (productDTO.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }

            // Tạo tên file ảnh
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                 Path.GetExtension(productDTO.ImageFile!.FileName);

            // Đường dẫn lưu ảnh
            string uploadsFolder = Path.Combine(environment.WebRootPath, "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string imageFullPath = Path.Combine(uploadsFolder, newFileName);

            // Lưu ảnh vào wwwroot/products
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDTO.ImageFile.CopyTo(stream);
            }

            // Lưu DB
            Product product = new Product
            {
                Name = productDTO.Name,
                Brand = productDTO.Brand,
                Category = productDTO.Category,
                Price = decimal.Parse(productDTO.Price),
                Description = productDTO.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
                Weight = decimal.Parse(productDTO.Weight)
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        // Form chỉnh sửa
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDTO = new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price.ToString("F2"),
                Description = product.Description,
                Weight = product.Weight.ToString("F2")
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            return View(productDTO);
        }

        // Xử lý chỉnh sửa
        [HttpPost]
        public IActionResult Edit(int id, ProductDTO productDTO)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");
                return View(productDTO);
            }

            string newFileName = product.ImageFileName;

            // Nếu có upload ảnh mới
            if (productDTO.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                              Path.GetExtension(productDTO.ImageFile.FileName);

                string uploadsFolder = Path.Combine(environment.WebRootPath, "products");
                string newImagePath = Path.Combine(uploadsFolder, newFileName);

                // Lưu ảnh mới
                using (var stream = System.IO.File.Create(newImagePath))
                {
                    productDTO.ImageFile.CopyTo(stream);
                }

                // Xóa ảnh cũ
                string oldImagePath = Path.Combine(uploadsFolder, product.ImageFileName);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Cập nhật dữ liệu sản phẩm
            product.Name = productDTO.Name;
            product.Brand = productDTO.Brand;
            product.Category = productDTO.Category;
            product.Price = decimal.Parse(productDTO.Price);
            product.Description = productDTO.Description;
            product.ImageFileName = newFileName;
            product.Weight = decimal.Parse(productDTO.Weight);

            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
    
public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            // Xóa ảnh nếu tồn tại
            string uploadsFolder = Path.Combine(environment.WebRootPath, "products");
            string imagePath = Path.Combine(uploadsFolder, product.ImageFileName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Details(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(product);
        }
    }
}
