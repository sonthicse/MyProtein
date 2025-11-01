using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProtein.Models;

namespace MyApp.Namespace
{
    public class AdminController : Controller
    {
        private readonly MyProteinContext _context;

        public AdminController(MyProteinContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Products
        public async Task<IActionResult> Products(string searchTerm, int[] categoryIds, int[] manufacturerIds, int? minPrice, int? maxPrice, int pageNumber = 1, int pageSize = 10)
        {
            // 1. Bắt đầu với một IQueryable
            var query = _context.Products
                                .Include(p => p.Category)
                                .Include(p => p.Manufacturer)
                                .OrderByDescending(p => p.CreatedAt)
                                .AsQueryable();

            // 2. Áp dụng bộ lọc tìm kiếm (Search Term)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(term)) ||
                    (p.Manufacturer != null && p.Manufacturer.Name.ToLower().Contains(term))
                );
            }

            // 3. Áp dụng bộ lọc theo nhiều danh mục
            if (categoryIds != null && categoryIds.Length > 0)
            {
                query = query.Where(p => p.CategoryId.HasValue && categoryIds.Contains(p.CategoryId.Value));
            }

            // 4. Áp dụng bộ lọc theo nhiều nhà sản xuất
            if (manufacturerIds != null && manufacturerIds.Length > 0)
            {
                query = query.Where(p => p.ManufacturerId.HasValue && manufacturerIds.Contains(p.ManufacturerId.Value));
            }

            // 5. Áp dụng bộ lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // --- Bắt đầu phân trang trên kết quả đã lọc ---
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var productsForPage = await query
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();
            // --- Kết thúc phân trang ---

            // 6. Gửi dữ liệu và các tham số lọc tới View bằng ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            // Giữ lại giá trị các bộ lọc để hiển thị lại trên form
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCategoryIds = categoryIds ?? Array.Empty<int>();
            ViewBag.SelectedManufacturerIds = manufacturerIds ?? Array.Empty<int>();
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            // Lấy danh sách cho các dropdown filter
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Manufacturers = await _context.Manufacturers.OrderBy(m => m.Name).ToListAsync();

            return View(productsForPage);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Name");
            ViewData["FlavourId"] = new SelectList(_context.Flavours, "FlavourId", "FlavourName");
            ViewData["WeightId"] = new SelectList(_context.Weights, "WeightId", "WeightValue");
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sku,Name,Description,Price,SalePrice,CategoryId,ManufacturerId,Status")] Product product, List<ProductVariant> ProductVariants, List<IFormFile> files)
        {
            // Bỏ qua validation cho các thuộc tính điều hướng để tránh lỗi không cần thiết
            //ModelState.Remove("Category");
            //ModelState.Remove("Manufacturer");
            // Quan trọng: Bỏ qua validation cho chính danh sách ProductVariants,
            // vì chúng ta sẽ kiểm tra logic của nó thủ công nếu cần.
            //ModelState.Remove("ProductVariants");

            if (ModelState.IsValid)
            {
                // 1. Thêm sản phẩm chính (Product) vào DbContext
                _context.Add(product);

                // Cần lưu thay đổi ở bước này để CSDL tự động tạo ra ProductId cho sản phẩm mới
                await _context.SaveChangesAsync();

                // 2. Gán ProductId vừa tạo cho các biến thể và thêm chúng vào DbContext
                if (ProductVariants != null && ProductVariants.Any())
                {
                    foreach (var variant in ProductVariants)
                    {
                        // Gán ID của sản phẩm cha cho từng biến thể
                        variant.ProductId = product.ProductId;
                        _context.ProductVariants.Add(variant);
                    }
                }

                // 3. Xử lý upload và thêm các hình ảnh (ProductImage)
                if (files != null && files.Count > 0)
                {
                    // Lấy đường dẫn đến thư mục wwwroot
                    var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadPath = Path.Combine(webRootPath, "images", "products");

                    // Đảm bảo thư mục tồn tại, nếu chưa có thì tạo mới
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    foreach (var file in files)
                    {
                        // Kiểm tra lại file có thực sự được gửi lên không
                        if (file != null && file.Length > 0)
                        {
                            try
                            {
                                // Tạo tên file duy nhất để tránh ghi đè
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var extension = Path.GetExtension(file.FileName);
                                var newFileName = $"{Guid.NewGuid()}{extension}"; // Dùng Guid để đảm bảo tên là duy nhất
                                var filePath = Path.Combine(uploadPath, newFileName);

                                // Lưu file vào thư mục trên server
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                // Tạo đối tượng ProductImage để lưu vào CSDL
                                var productImage = new ProductImage
                                {
                                    ProductId = product.ProductId,
                                    ImageUrl = $"/images/products/{newFileName}", // Đường dẫn tương đối để thẻ <img> có thể đọc
                                    AltText = product.Name
                                };
                                _context.ProductImages.Add(productImage);
                            }
                            catch (Exception ex)
                            {
                                // Nếu có lỗi xảy ra trong quá trình lưu file, bạn có thể xử lý ở đây
                                // Ví dụ: log lỗi
                                Console.WriteLine($"Error uploading file: {ex.Message}");
                                // Có thể thêm lỗi vào ModelState để báo cho người dùng
                                ModelState.AddModelError("files", "An error occurred while uploading the files.");
                            }
                        }
                    }
                }

                // Lưu tất cả các thay đổi (variants và images) vào CSDL
                await _context.SaveChangesAsync();

                // Chuyển hướng về trang danh sách sản phẩm sau khi tạo thành công
                return RedirectToAction(nameof(Products));
            }

            // Nếu ModelState không hợp lệ (ví dụ: thiếu tên sản phẩm),
            // tải lại dữ liệu cho các dropdown và hiển thị lại form với các lỗi
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "Name", product.ManufacturerId);
            ViewData["FlavourId"] = new SelectList(_context.Flavours, "FlavourId", "FlavourName");
            ViewData["WeightId"] = new SelectList(_context.Weights.Select(w => new { w.WeightId, Text = $"{w.WeightValue}g ({w.Servings} servings)" }), "WeightId", "Text");

            return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
