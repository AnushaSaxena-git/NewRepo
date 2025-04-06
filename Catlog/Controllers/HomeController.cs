using Catlog.DTO;
using Catlog.Models;
using Catlog.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Catlog.Models.Enum;
using System.Linq;
using Catlog.Models.EnumExtensions;
using AutoMapper;
using System.Security.Cryptography;
using Catlog.Repositories;
using Microsoft.CodeAnalysis;
using System;




public class HomeController : Controller
{
    private readonly IProductSkuRepository _productSkuRepository;
    private readonly IProductRepository _productRepository;
    private readonly Ishippingmodelrepository _shippingmodelrepository;
    private readonly IMapper _mapper;
    private readonly IProduct_ImageRepository _product_ImageRepository;
    private readonly ICartService _cartService;
    private readonly IAddressRepository _addressRepository;
    private readonly CatlogDBcontext _dbcontext;


    public HomeController(IProductRepository productRepository, IProductSkuRepository productSkuRepository, Ishippingmodelrepository shippingmodelrepository, IMapper mapper,
        IProduct_ImageRepository product_ImageRepository, ICartService cartService, IAddressRepository addressRepository, CatlogDBcontext catlogDBcontext)

    {
        _productRepository = productRepository;
        _productSkuRepository = productSkuRepository;
        _shippingmodelrepository = shippingmodelrepository;
        _mapper = mapper;
        _product_ImageRepository = product_ImageRepository;
        _cartService = cartService;
        _addressRepository = addressRepository;
        _dbcontext = catlogDBcontext;
    }
    public async Task<IActionResult> Index(
     string name,
     string searchTerm = null,
     int? categoryId = null,
     string color = null,
     string size = null,
     string sortBy = "product_title",
     string sortDirection = "ASC",
     int pageNumber = 1,
     int pageSize = 2,
     decimal? minPrice = null,
     decimal? maxPrice = null)
    {
        // Fetch categories
        var categories = await _productRepository.getcategories();
        ViewBag.Categories = categories; // Pass categories to the view

        // Fetch product data
        var products = await _productRepository.getproduct();

        // Group products by product_Id to get unique listings
        var groupedProducts = products
            .GroupBy(product => product.product_Id)
            .Select(group => group.First()) // Get the first product in each group
            .AsQueryable();

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            groupedProducts = groupedProducts
                .Where(product => product.product_title.Contains(searchTerm) || product.description.Contains(searchTerm));
        }

        // Apply category filter if provided
        if (categoryId.HasValue)
        {
            groupedProducts = groupedProducts.Where(product => product.category_Id == categoryId.Value);
        }

        // Apply price filter if provided
        if (minPrice.HasValue)
        {
            groupedProducts = groupedProducts.Where(product => product.ProductSkus != null
                                                          && product.ProductSkus.Any(sku => sku.Price >= minPrice.Value));
        }

        if (maxPrice.HasValue)
        {
            groupedProducts = groupedProducts.Where(product => product.ProductSkus != null
                                                          && product.ProductSkus.Any(sku => sku.Price <= maxPrice.Value));
        }

        // Sorting by specified field and direction
        switch (sortBy.ToLower())
        {
            case "product_title":
                groupedProducts = sortDirection.ToUpper() == "ASC"
                    ? groupedProducts.OrderBy(product => product.product_title)
                    : groupedProducts.OrderByDescending(product => product.product_title);
                break;
            default:
                groupedProducts = sortDirection.ToUpper() == "ASC"
                    ? groupedProducts.OrderBy(product => product.product_title)
                    : groupedProducts.OrderByDescending(product => product.product_title);
                break;
        }

        // Apply pagination
        var pagedProducts = groupedProducts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Get total number of products for pagination controls
        var totalProducts = groupedProducts.Count();

        // Create the final list of ProductWithSkuDto based on paged products
        var result = new List<ProductWithSkuDto>();

        foreach (var product in pagedProducts)
        {
            var productImages = await _product_ImageRepository.GetProductImagesByProductId(product.product_Id);
            var productImage = productImages.FirstOrDefault()?.ImageName;

            var productWithSku = new ProductWithSkuDto
            {
                Product = new ProductDto
                {
                    product_Id = product.product_Id,
                    product_title = product.product_title,
                    description = product.description,
                    country_origin = product.country_origin,
                    category_Id = product.category_Id,
                    ProductImage = productImage
                }
            };

            result.Add(productWithSku);
        }

        // Pass total products and current page to the view for pagination controls
        ViewBag.TotalProducts = totalProducts;
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;

        return View(result); // Return the final list of products with SKU data
    }



    //public async Task<IActionResult> Detailss(int id)
    //{
    //    // Fetch the product from the Product table
    //    var product = await _productRepository.GetProductById(id);
    //    if (product == null)
    //        return NotFound();

    //    // Fetch the product SKU List<ProductSkuDto>
    //    var productSkus = await _productSkuRepository.GetProductSkuById(id);
    //    if (productSkus == null || !productSkus.Any()) // Ensure this is checking a collection
    //        return NotFound();

    //    // Fetch the product images from the Product_Image table
    //    var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);

    //    // Map Product_Image to Product_ImagesDTO
    //    var productImagesDto = productImages.Select(img => new Product_ImagesDTO
    //    {
    //        PID = img.PID,
    //        ImageName = img.ImageName,
    //        ProductId = img.ProductId ?? 0,
    //        sku_id = img.sku_Id // the property in the database for SKU ID is SkuId
    //    }).ToList();

    //    // Prepare a list to hold the product with SKUs
    //    var productWithSkus = new List<ProductWithSkuDto>();

    //    // Add SKUs to productWithSkus
    //    foreach (var sku in productSkus)
    //    {
    //        var productSkuDto = new ProductWithSkuDto
    //        {
    //            SkuId = sku.sku_Id,
    //            SkuName = sku.skuname,
    //            Price = sku.Price,
    //            StockQuantity = sku.stockQuantity,
    //            Images = productImagesDto.Where(img => img.sku_id == sku.sku_Id).ToList()
    //        };
    //        productWithSkus.Add(productSkuDto);
    //    }

    //    // Prepare the ProductDto including the main image
    //    var productDto = new ProductDto
    //    {
    //        product_Id = product.product_Id,
    //        product_title = product.product_title,
    //        description = product.description,
    //        country_origin = product.country_origin,
    //        category_Id = product.category_Id,
    //        ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? "" // Main product image
    //    };

    //    // Combine product details and SKU details into a view model
    //    var viewModel = new ProductDetailsViewModel
    //    {
    //        Product = productDto,
    //        ProductSkus = productWithSkus
    //    };

    //    return View(viewModel);
    //}
    //-----------------------------------------------------------------------------------------------
    public async Task<IActionResult> Details(int id)
    {
        // Fetch the product from the Product table
        var product = await _productRepository.GetProductById(id);
        if (product == null)
            return NotFound();

        // Fetch the product SKU List<ProductSkuDto>
        var productSkus = await _productSkuRepository.GetProductSkuById(id);

        if (productSkus == null || !productSkus.Any()) // Ensure this is checking a collection
            return NotFound();

        // Fetch the product images from the Product_Image table
        var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);

        // Map Product_Image to Product_ImagesDTO
        var productImagesDto = productImages.Select(img => new Product_ImagesDTO
        {
            PID = img.PID,                  // the property name in Product_Image is PID
            ImageName = img.ImageName,       // the property name in Product_Image is ImageName
            ProductId = img.ProductId ?? 0,  // Use 0 as a default value if ProductId is null
            sku_id = img.sku_Id               // the property in the database for SKU ID is SkuId
        }).ToList(); // Converts to List<Product_ImagesDTO>

        // Prepare a list to hold the product with SKUs
        var productWithSkus = new List<ProductWithSkuDto>();

        // Create the product DTO to include the main image
        var productDto = new ProductDto
        {
            product_Id = product.product_Id,
            product_title = product.product_title,
            description = product.description,
            country_origin = product.country_origin,
            category_Id = product.category_Id,
            ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? "" // Main product image
        };


        // Iterate over the fetched product SKUs
        foreach (var productSku in productSkus)
        {
            // Create a DTO object with SKU information
            var skuDto = new ProductWithSkuDto
            {
                Product = productDto,  // Include main product image here
                Colors = new List<string> { productSku.color },
                Sizes = new List<string> { productSku.size.ToString() },
                Materials = new List<string> { productSku.material },
                Price = productSku.Price,
                StockQuantity = productSku.stockQuantity,
                SkuId = productSku.sku_Id,
                SkuName = productSku.skuname,
                // Fetch images related to this SKU ID
                Images = productImagesDto.Where(img => img.sku_id == productSku.sku_Id).ToList()
            };

            // Add the SKU DTO to the list
            productWithSkus.Add(skuDto);
        }

        // Pass the entire list of SKUs to the view
        return View(productWithSkus);
    }
    //  ------------------------------------------------------------/   


    public async Task<IActionResult> Detailsss(int id, int skuId, string selectedColor = null, string selectedSize = null, string selectedMaterial = null)
    {
        // Fetch the product from the Product table
        var product = await _productRepository.GetProductById(id);
        if (product == null)
            return NotFound();

        // Fetch the product SKU List<ProductSkuDto>
        var productSkus = await _productSkuRepository.GetProductSkuById(id);
        if (productSkus == null || !productSkus.Any())
            return NotFound();

        // Filter by selected color, size, or material if available
        if (!string.IsNullOrEmpty(selectedColor))
        {
            productSkus = productSkus.Where(sku => sku.color == selectedColor).ToList();
        }
        if (!string.IsNullOrEmpty(selectedSize))
        {
            productSkus = productSkus.Where(sku => sku.size == selectedSize).ToList();
        }
        if (!string.IsNullOrEmpty(selectedMaterial))
        {
            productSkus = productSkus.Where(sku => sku.material == selectedMaterial).ToList();
        }

        // Fetch the product images from the Product_Image table
        var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);
        var productImagesDto = productImages.Select(img => new Product_ImagesDTO
        {
            PID = img.PID,
            ImageName = img.ImageName,
            ProductId = img.ProductId ?? 0,
            sku_id = img.sku_Id
        }).ToList();

        // Prepare SKU details
        var productWithSkus = new List<ProductWithSkuDto>();
        foreach (var sku in productSkus)
        {
            var productSkuDto = new ProductWithSkuDto
            {
                SkuId = sku.sku_Id,
                SkuName = sku.skuname,
                Price = sku.Price,
                StockQuantity = sku.stockQuantity,
                Colors = new List<string> { sku.color },
                Sizes = new List<string> { sku.size },
                Materials = new List<string> { sku.material },
                Images = productImagesDto.Where(img => img.sku_id == sku.sku_Id).ToList()
            };
            productWithSkus.Add(productSkuDto);
        }

        // Return the updated data for dynamic filtering
        var availableColors = productSkus.Select(sku => sku.color).Distinct().ToList();
        var availableSizes = productSkus.Select(sku => sku.size).Distinct().ToList();
        var availableMaterials = productSkus.Select(sku => sku.material).Distinct().ToList();

        var productImageUrl = productImagesDto.FirstOrDefault()?.ImageName ?? ""; // Display first image by default

        return View(new ProductDetailsViewModel
        {
            Product = product,
            ProductSkus = productWithSkus,
            AvailableColors = availableColors,
            AvailableSizes = availableSizes,
            AvailableMaterials = availableMaterials,
            ProductImageUrl = Url.Content("~/images/" + productImageUrl),
            Images = Url.Content("~/images/" + productImageUrl)
        });
    }


    public async Task<IActionResult> SkuDetails(int id)
    {
        // Ensure skuId is valid
        if (id <= 0)
        {
            return BadRequest("Invalid SKU ID.");
        }

        // Fetch the SKU from the ProductSku table based on the SKU ID
        var productSku = await _productSkuRepository.GetProductSkuByIdAsync(id);
        if (productSku == null)
            return NotFound(); // Return 404 if SKU not found

        // Fetch the product image details related to this SKU
        var productImages = await _product_ImageRepository.GetImagesForSkuAsync(id);

        // Map Product_Image to Product_ImagesDTO
        var productImagesDto = productImages.Select(img => new Product_ImagesDTO
        {
            PID = img.PID,
            ImageName = img.ImageName,
            ProductId = img.ProductId ?? 0,
            sku_id = img.sku_Id
        }).ToList();

        // Fetch the main product details for the product to which this SKU belongs
        var product = await _productRepository.GetProductById(productSku.product_Id);
        if (product == null)
            return NotFound(); // Return 404 if Product not found for this SKU

        // Create the product DTO with the main product image
        var productDto = new ProductDto
        {
            product_Id = product.product_Id,
            product_title = product.product_title,
            description = product.description,
            country_origin = product.country_origin,
            category_Id = product.category_Id,
            ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? "" // Main product image
        };

        // Create the SKU DTO with SKU details
        var skuDto = new ProductWithSkuDto
        {
            Product = productDto,  // Include main product image here
            Colors = new List<string> { productSku.color },
            Sizes = new List<string> { productSku.size.ToString() },
            Materials = new List<string> { productSku.material },
            Price = productSku.Price,
            StockQuantity = productSku.stockQuantity,
            SkuId = productSku.sku_Id,
            SkuName = productSku.skuname,
            Images = productImagesDto // Include SKU-specific images
        };

        // Pass the SKU details to the view
        return View(skuDto);
    }

    public async Task<IActionResult> DetailsOrSku(int id, bool isSku = false)
    {
        if (isSku)
        {
            // Fetch SKU details
            var productSku = await _productSkuRepository.GetProductSkuByIdAsync(id);
            if (productSku == null) return NotFound();

            var productImages = await _product_ImageRepository.GetImagesForSkuAsync(id);
            var productImagesDto = productImages.Select(img => new Product_ImagesDTO
            {
                PID = img.PID,
                ImageName = img.ImageName,
                ProductId = img.ProductId ?? 0,
                sku_id = img.sku_Id
            }).ToList();

            var product = await _productRepository.GetProductById(productSku.product_Id);
            if (product == null) return NotFound();

            var productDto = new ProductDto
            {
                product_Id = product.product_Id,
                product_title = product.product_title,
                description = product.description,
                country_origin = product.country_origin,
                category_Id = product.category_Id,
                ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? ""
            };

            var skuDto = new ProductWithSkuDto
            {
                Product = productDto,
                Colors = new List<string> { productSku.color },
                Sizes = new List<string> { productSku.size.ToString() },
                Materials = new List<string> { productSku.material },
                Price = productSku.Price,
                StockQuantity = productSku.stockQuantity,
                SkuId = productSku.sku_Id,
                SkuName = productSku.skuname,
                Images = productImagesDto
            };

            return View(skuDto); // Pass SKU data to the view
        }
        else
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return NotFound();

            var productSkus = await _productSkuRepository.GetProductSkuById(id);
            if (productSkus == null || !productSkus.Any()) return NotFound();

            var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);
            var productImagesDto = productImages.Select(img => new Product_ImagesDTO
            {
                PID = img.PID,
                ImageName = img.ImageName,
                ProductId = img.ProductId ?? 0,
                sku_id = img.sku_Id
            }).ToList();

            var productWithSkus = new List<ProductWithSkuDto>();
            var productDto = new ProductDto
            {
                product_Id = product.product_Id,
                product_title = product.product_title,
                description = product.description,
                country_origin = product.country_origin,
                category_Id = product.category_Id,
                ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? ""
            };

            foreach (var productSku in productSkus)
            {
                var skuDto = new ProductWithSkuDto
                {
                    Product = productDto,
                    Colors = new List<string> { productSku.color },
                    Sizes = new List<string> { productSku.size.ToString() },
                    Materials = new List<string> { productSku.material },
                    Price = productSku.Price,
                    StockQuantity = productSku.stockQuantity,
                    SkuId = productSku.sku_Id,
                    SkuName = productSku.skuname,
                    Images = productImagesDto.Where(img => img.sku_id == productSku.sku_Id).ToList()
                };

                productWithSkus.Add(skuDto);
            }


            return View(productWithSkus); // Pass all SKUs to the view
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddToCart(int id)
    {
        var productSkus = await _productSkuRepository.GetProductSkuById(id);
        if (productSkus == null || !productSkus.Any())
        {
            return NotFound("No SKUs available for this product");
        }

        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            return NotFound("Product not found");
        }


        var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);
        var productsku = await _product_ImageRepository.GetImagesForSkuAsync(id);
        var productImagesDto = productImages.Select(img => new Product_ImagesDTO
        {
            PID = img.PID,
            ImageName = img.ImageName,
            ProductId = img.ProductId ?? 0,
            sku_id = img.sku_Id
        }).ToList();

        // Prepare a list to hold the product with SKUs
        var productWithSkus = new List<ProductWithSkuDto>();

        // Create the product DTO to include the main image
        var productDto = new ProductDto
        {
            product_Id = product.product_Id,
            product_title = product.product_title,
            description = product.description,
            country_origin = product.country_origin,
            category_Id = product.category_Id,
            ProductImage = product.product_Image.FirstOrDefault()?.ImageName ?? "" // Main product image
        };

        // Iterate over the fetched product SKUs
        foreach (var productSku in productSkus)
        {
            // Create a DTO object with SKU information
            var skuDto = new ProductWithSkuDto
            {
                Product = productDto,  // Include main product image here
                Colors = new List<string> { productSku.color },
                Sizes = new List<string> { productSku.size.ToString() },
                Materials = new List<string> { productSku.material },
                Price = productSku.Price,
                StockQuantity = productSku.stockQuantity,
                SkuId = productSku.sku_Id,
                SkuName = productSku.skuname,
                // Fetch images related to this SKU ID
                Images = productImagesDto.Where(img => img.sku_id == productSku.sku_Id).ToList()
            };

            // Add the SKU DTO to the list
            productWithSkus.Add(skuDto);
        }

        // Pass the entire list of SKUs to the view
        return View(productWithSkus);
    }


    [HttpPost]
    public async Task<IActionResult> AddToCart(int? productId, int? skuId, int quantity)
    {
        // Check if productId or skuId is provide
        if (productId == null && skuId == null)
        {
            return BadRequest("Product or SKU must be selected");
        }

        // Create a cart item based on selection
        if (skuId.HasValue)
        {
            // Fetch SKU and its related product info
            var productSku = await _productSkuRepository.GetProductSkuByIdAsync(skuId.Value);
            if (productSku == null)
                return NotFound("SKU not found");

            var product = await _productRepository.GetProductById(productSku.product_Id);
            if (product == null)
                return NotFound("Product not found");
            var productImage = await _product_ImageRepository.GetProductImagesByProductId(product.product_Id);
            var productskuimage = await _product_ImageRepository.GetImagesForSkuAsync(productSku.sku_Id);
            // Add SKU to the cart this can be handled with a session cart service
            var cartItem = new CartItem
            {
                ProductId = product.product_Id,
                SkuId = productSku.sku_Id,
                SkuName = productSku.skuname,
                size = productSku.size,
                material = productSku.material,
                color = productSku.color,

                Quantity = quantity,
                Price = productSku.Price,
                ProductTitle = product.product_title,
                ImageName = productImage.First().ImageName,



            };

            //  we have a cart service or a session to store this information
            _cartService.AddToCart(cartItem);
        }
        else if (productId.HasValue)
        {
            // Fetch the product info
            var product = await _productRepository.GetProductById(productId.Value);
            if (product == null)
                return NotFound("Product not found");

            // Add product to the cart
            var cartItem = new CartItem
            {
                ProductId = product.product_Id,
                SkuId = null, // No SKU selected
                SkuName = null,
                Quantity = quantity,
                ProductTitle = product.product_title
            };
            //var Address = new AddressViewModel { 


            //FirstName=Address.FirstName,
            //LastName=LastName,
            //AddressLine1=Address.AddressLine1



            //};
            // _addressRepository.AddToCart(Address);
            // Add product to cart
            _cartService.AddToCart(cartItem);
        }

        // Redirect to the cart page or a confirmation page
        return RedirectToAction("Cart");

    }
    [HttpGet]
    public async Task<ViewResult> Cart()
    {
        var cartItems = _cartService.GetCartItems();

        return View(cartItems);
    }

    //public IActionResult NewPage() {

    //    var productDetails = new ProductDetailsViewModel { };

    //    return View(productDetails);

    //}


    [HttpGet]
    public IActionResult Address()
    {
        var AddressViewModel = _addressRepository.GetAddress();

        return View(AddressViewModel);
    }

    [HttpPost]
    public IActionResult Address(AddressViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Save the address in session or database as needed
            HttpContext.Session.SetObjectAsJson("ShippingAddress", model);

            // Redirect to a confirmation page or cart review
            return RedirectToAction("ConfirmOrder");
        }
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> ConfirmOrder()
    {
        var cartItems = _cartService.GetCartItems();
        var userId = User.Identity.Name;
        var shippingAddress = await _addressRepository.GetAddressAsync(userId);

        if (cartItems == null || !cartItems.Any())
        {
            return RedirectToAction("Cart");
        }

        var viewModel = new OrderConfirmationViewModel
        {
            CartItems = cartItems,
            ShippingAddress = shippingAddress
        };

        return View(viewModel);
    }




    [NonAction]
    public IActionResult Edit(int product_Id)
    {
        var product = new ProductDto
        {




        };

        return View();

    }

    [NonAction]
    public static void LogException(Exception ex)
    {
        try
        {
            // Get the AppData Local folder
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyApp", "Logs");

            //  the directory exists
            Directory.CreateDirectory(folderPath);

            // Define the log file path
            string logFilePath = Path.Combine(folderPath, "error_log.txt");

            // Write the exception details to the log file
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] Exception Message: {ex.Message}");
                writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                writer.WriteLine(); // Adds a blank line for readability
            }
        }
        catch (Exception logEx)
        {
            LogException(logEx);
            Console.WriteLine("error has been occured ");
            //Console.WriteLine("Error logging exception: " + logEx.Message);
        }
    }
    [NonAction]
    public IActionResult ship() { return View(); }





    [HttpGet]
    public async Task<ViewResult> InsertProductAndSku()
     {
        try
        {
    // Initialize the combined DTO for product and SKU
            var combinedDto = new ProductWithSkuDto
            {
                Colors = System.Enum.GetNames(typeof(Color)).ToList(),
                Sizes = System.Enum.GetNames(typeof(Size)).ToList(),
                Materials = System.Enum.GetNames(typeof(Material)).ToList(),
                Product = new ProductDto(),
                ShipmentDetails = new ShipmentDTO()
            };

            // Fetch categories from the database
            var categories = await _productRepository.getcategories();
            ViewBag.Categories = categories;

            var shipmentModel = new ShipmentModel();
            ViewBag.ShipmentOptions = shipmentModel.shipmentOptionsList;
            ViewBag.FedexOptions = shipmentModel.FedexShipmentOptionsList;
            ViewBag.UpsOptions = shipmentModel.UpsShipmentOptionsList;

            // Populate dropdowns for shipment options
            ViewBag.ShipmentOptions = System.Enum.GetValues(typeof(ShipmentModel.ShipmentOptions))
                .Cast<ShipmentModel.ShipmentOptions>()
                .Select(e => new
                {
                    Value = e,
                    Name = e.GetDisplayName()
                })
                .ToList();

            ViewBag.FedexOptions = System.Enum.GetValues(typeof(ShipmentModel.FedexOptions))
                .Cast<ShipmentModel.FedexOptions>()
                .Select(e => new
                {
                    Value = e,
                    Name = e.GetDisplayName()
                })
                .ToList();

            ViewBag.UpsOptions = System.Enum.GetValues(typeof(ShipmentModel.UpsOptions))
                .Cast<ShipmentModel.UpsOptions>()
                .Select(e => new
                {
                    Value = e,
                    Name = e.GetDisplayName()
                })
                .ToList();

            if (categories == null || !categories.Any())
            {
                TempData["Error"] = "No categories available.";
                return View();
            }

            return View(combinedDto);
        }
        catch (Exception logEx)
        {
            LogException(logEx);
            Console.WriteLine("An error has occurred");
            return View();
        }
    }
    [HttpPost]
    public async Task<IActionResult> InsertProductAndSku(
     string productName,
     int categoryId,
     string countryOrigin,
     string description,
     [FromForm] IFormFile[] product_image, // Handle multiple images
     [FromForm] IFormFile[] product_imagesku, // Handle multiple SKU images
     string[] size,
     string[] material,
     int[] price,
     int[] stockquantity,
     string[] color,
     string[] skuname,
     ShipmentModel model
 )
    {
        try
        {
            // Validate product data
            if (string.IsNullOrWhiteSpace(productName) || categoryId <= 0 || string.IsNullOrWhiteSpace(countryOrigin) || string.IsNullOrWhiteSpace(description))
            {
                TempData["Error"] = "All product fields are required.";
                return View();
            }

            // Insert the product into the Product table without image
            var newProduct = await _productRepository.InsertProductAsync(productName, categoryId, countryOrigin, description);
            int productId = newProduct.product_Id;

            // List to hold SKU image names
            List<string> skuImages = new List<string>();

            // Insert SKU images if there is any images avaialable 
            if (product_imagesku != null && product_imagesku.Length > 0)
            {
                foreach (var file in product_imagesku)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Check if the directory exists; if not, create it
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    string imageFilePath = Path.Combine(imageDirectory, fileName + extension);

                    try
                    {
                        // Save the file to the disk
                        using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Add image name to the list
                        skuImages.Add(Path.GetFileName(imageFilePath)); // Just store the file name
                    }
                    catch (Exception ex)
                    {
                        // Log any file saving errors
                        Console.WriteLine($"Error saving SKU image {file.FileName}: {ex.Message}");
                    }
                }
            }

            // Check if SKU images are available before passing the
            if (skuImages.Count == 0)
            {
                Console.WriteLine("Warning: No SKU images were uploaded.");
            }

            // Insert product SKUs into the database
            for (int i = 0; i < color.Length; i++)
            {
                await _productSkuRepository.AddProductSkuAsync(
                    new string[] { size[i] },            // Size
                    new string[] { material[i] },        // Material
                    new int[] { price[i] },              // Price
                    new int[] { stockquantity[i] },      // Stock Quantity
                    productId,                           // Product ID
                    new string[] { skuname[i] },         // SKU Name
                    new string[] { color[i] },           // Color
                    skuImages.ToArray()                  // Pass the list of SKU image names
                );
            }

            // Insert each product image into the Product_Images table
            if (product_image != null && product_image.Length > 0)
            {
                foreach (var file in product_image)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    string imageFilePath = Path.Combine(imageDirectory, fileName + extension);

                    try
                    {
                        using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Insert product image into the database
                        var productImage = new Product_Image
                        {
                            ProductId = productId, // Associate the image with the product
                            ImageName = Path.GetFileName(imageFilePath) // Store only the file name
                        };

                        await _product_ImageRepository.InsertProductImageAsync(productImage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving product image {file.FileName}: {ex.Message}");
                    }
                }
            }

            // Insert shipment data if provided

            var shipmentModel = new ShipmentModel
            {
                UpsShipment = model.UpsShipment,
                fedexShipments = model.fedexShipments,
                ShipmentOption = model.ShipmentOption,
                From = model.From,
                Destination = model.Destination.Normalize(),
            };

            _shippingmodelrepository.AddShippingType(shipmentModel);
            _shippingmodelrepository.Save();


            return RedirectToAction("Index");
        }
        catch (Exception logEx)
        {
            LogException(logEx);
            Console.WriteLine("An error has occurred");
            return View();
        }
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Checkout()
    {
        int userId = 2;

        var user = await _dbcontext
                    .User.FirstOrDefaultAsync(x => x.UserId == userId);

        CheckoutDTO checkoutDTO = new();

        if (user != null)
        {
            checkoutDTO.User = new UserDetailsDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
            };

            checkoutDTO.UserAddress = await _dbcontext
                            .UserAddress
                            .Where(x => x.UserId == userId)
                            .Select(x => new UserAddressDTO
                            {
                                addressId = x.addressId,
                                city = x.city,
                                addressLine1 = x.addressLine1,
                                addressLine2 = x.addressLine2,
                                postalCode = x.postalCode
                            })
                            .FirstOrDefaultAsync();

            var cartItem = _cartService.GetCartItems();
            checkoutDTO.CartItem = cartItem;
        }
        return View(checkoutDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(PlaceOrderDTO dto)
    {
        var cartItems = _cartService.GetCartItems();

        if (cartItems == null)
        {
            return RedirectToAction("Cart");
        }

        Order order = new Order
        {
            City = dto.City,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            Phone = dto.Phone,
            Email = dto.Email,
            TotalAmount = cartItems.Sum(x => (x.Quantity * x.Price)),
            CardNumber = dto.CardNumber,
            ExpDate = dto.ExpDate,
            CVV = dto.CVV,
            UserId = 2
        };

        await _dbcontext.Order.AddAsync(order);
        await _dbcontext.SaveChangesAsync();

        List<OrderItem> orderItems = [];
        foreach (var item in cartItems)
        {
            orderItems.Add(new OrderItem
            {
                orderId = order.orderId,
                productId = item.ProductId,
                size = item.size,
                price = item.Price,
                quantity = item.Quantity
            }
        );
        }

        if (orderItems.Count > 0)
        {
            await _dbcontext.OrderItems.AddRangeAsync(orderItems);
            await _dbcontext.SaveChangesAsync();
        }

        return View("PlacedOrder");
    }

    public IActionResult IncreaseDecreaseQty(int productId, int skuId, bool isIncrease)
    {
        var cartItems = _cartService.GetCartItems();

        if (cartItems?.Count > 0)
        {
            var index = cartItems.FindIndex(x => x.ProductId == productId && x.SkuId == skuId);
            if (index > -1)
            {
                cartItems[index].Quantity = isIncrease ? cartItems[index].Quantity + 1 : cartItems[index].Quantity - 1;

                if (cartItems[index].Quantity < 1)
                {
                    cartItems.RemoveAt(index);
                }
            }

            _cartService.UpdateCart(cartItems);
        }

        return RedirectToAction("Cart");
    }
    [HttpPost]
    public JsonResult RemoveFromCartt([FromBody] int skuId)
    {
        var cartItems = _cartService.GetCartItems();
        if (cartItems?.Count > 0)
        {
            _cartService.RemoveFromCart(skuId);
        }
        var success = _cartService.RemoveFromCartt(skuId);

        // Return the updated cart total
        if (success)
        {
            var updatedTotal = _cartService.GetTotal();
            return Json(new { success = true, newTotal = updatedTotal });
        }
        else
        {
            return Json(new { success = false });
        }
    }

    public IActionResult Remove(int productId)
    {
        var cartItems = _cartService.GetCartItems();
        if (cartItems?.Count > 0)
        {
            _cartService.RemoveFromCart(productId);
        }

        return RedirectToAction("Cart");
    }
    //[HttpGet]
    //public async Task<IActionResult> ProductDetails(int productId)
    //{
    //    try
    //    {
    //        // Fetch the product details by its ID
    //        var product = await _productRepository.GetProductById(productId);

    //        if (product == null)
    //        {
    //            TempData["Error"] = "Product not found.";
    //            return RedirectToAction("Index");
    //        }

    //        // Fetch associated SKUs
    //        var skus = await _productSkuRepository.GetProductSkuByIdAsync(productId);

    //        // Combine product and SKU data into a view model
    //        var productDetailsViewModel = new ProductDetailsViewModel
    //        {
    //            Product = product,
    //            ProductSkus = skus
    //        };

    //        return View(productDetailsViewModel);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error fetching product details: {ex.Message}");
    //        return RedirectToAction("Index");
    //    }
    //}
    [HttpGet]
    public async Task<IActionResult> EditProductAndSku(int productId)
    {
        try
        {
            // Fetch product details by ID
            var product = await _productRepository.GetProductById(productId);

            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            // Fetch associated SKUs
            var skus = await _productSkuRepository.GetProductSkuByIdAsyncall(productId);

            // Create a view model to display the product and SKU data
            var combinedDto = new ProductWithSkuDto
            {
                Product = new ProductDto
                {
                    product_Id = product.product_Id,
                    product_title = product.product_title
                    // Map other properties as necessary
                },
                ProductSkus = skus,
                Colors = System.Enum.GetNames(typeof(Color)).ToList(),
                Sizes = System.Enum.GetNames(typeof(Size)).ToList(),
                Materials = System.Enum.GetNames(typeof(Material)).ToList(),
                ShipmentDetails = new ShipmentDTO()
            };

            return View(combinedDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching product data for editing: {ex.Message}");
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditProductAndSku(
        int productId,
        string productName,
        int categoryId,
        string countryOrigin,
        string description,
        [FromForm] IFormFile[] product_image,
        [FromForm] IFormFile[] product_imagesku,
        string[] size,
        string[] material,
        int[] price,
        int[] stockquantity,
        string[] color,
        string[] skuname,
        ShipmentModel model)
    {
        try
        {
            // Validate data
            if (string.IsNullOrWhiteSpace(productName) || categoryId <= 0 || string.IsNullOrWhiteSpace(countryOrigin) || string.IsNullOrWhiteSpace(description))
            {
                TempData["Error"] = "All product fields are required.";
                return View();
            }

            // Update the product in the database
            await _productRepository.UpdateProductAsync(productId, productName, categoryId, countryOrigin, description);

            // Handle SKU updates (similar to creation process)
            for (int i = 0; i < color.Length; i++)
            {
                await _productSkuRepository.UpdateProductSkuAsync(
                    productId,
                    size[i],
                    material[i],
                    price[i],
                    stockquantity[i],
                    skuname[i],
                    color[i]
                );
            }

            // Handle file uploads and other updates here...

            return RedirectToAction("ProductDetails", new { productId = productId });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
            return View();
        }
    }
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            // Get the product to be deleted
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            // Delete associated product images first
            var productImages = await _product_ImageRepository.GetProductImagesByProductId(id);

            if (productImages != null && productImages.Any())
            {
                foreach (var image in productImages)
                {
                    if (image != null && !string.IsNullOrEmpty(image.ImageName)) // Ensure image and filename are valid
                    {
                        var imageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.ImageName);
                        if (System.IO.File.Exists(imageFilePath))
                        {
                            System.IO.File.Delete(imageFilePath); // Delete the image file from disk
                        }

                        // Now delete the product image from the database
                        await _product_ImageRepository.DeleteProductImageAsync(image.PID); // Delete image record
                    }
                }
            }

            // Delete associated SKUs that are related to the product
            var skus = await _productSkuRepository.GetProductSkuByIdAsyncall(id);

            if (skus != null && skus.Any())
            {
                foreach (var sku in skus)
                {
                    if (sku != null) // Check for null SKU
                    {
                        // Delete associated product images related to this SKU
                        var imagesForSku = await _product_ImageRepository.GetImagesForSkuAsync(sku.sku_Id);
                        foreach (var skuImage in imagesForSku)
                        {
                            if (skuImage != null && !string.IsNullOrEmpty(skuImage.ImageName))
                            {
                                var skuImageFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", skuImage.ImageName);
                                if (System.IO.File.Exists(skuImageFilePath))
                                {
                                    System.IO.File.Delete(skuImageFilePath); // Delete the image file from disk
                                }

                                await _product_ImageRepository.DeleteProductImageAsync(skuImage.PID); // Delete image record
                            }
                        }

                        // Now delete the SKU
                        await _productSkuRepository.DeleteProductSkuAsync(sku.sku_Id); // Delete SKU record
                    }
                }
            }

            // Finally, delete the product itself
            await _productRepository.DeleteProductAsync(id); // Delete product record

            TempData["Success"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            TempData["Error"] = "An error occurred while deleting the product.";
            return RedirectToAction("Index");
        }
    }

   

    //[HttpGet("GetStockQuantity")]
    //public JsonResult GetStockQuantity(int skuId)
    //{
    //    var stockQuantity = _cartService.(skuId); 
    //    return Json(new { stockQuantity });
    //}

}

//public IActionResult Cart()
//{
//    var cartItems = _cartService.GetCartItems();
//    return View(cartItems);
//}

