using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace ProductsMinApi;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Кавомашина", Price = 15000, Photo = "https://images.philips.com/is/image/philipsconsumer/d747e1a49a094f4db399b0d800ef03f9?wid=420&hei=360&$jpglarge$" },
            new Product { Id = 2, Name = "Велосипед", Price = 20000, Photo = "https://encrypted-tbn1.gstatic.com/shopping?q=tbn:ANd9GcQo4M8BE_bg9aZnMiWTjsMPFIUog9ONRdlFov5RNhRloy-hMc0CBBrNzgdjo6JVoL9_5RcIOmufIRKIuYbKw0PgDcOtcb6OSMB_tVLsThGxQVTVyyXHpkLx6FAkuYa1aQ&usqp=CAc" },
            new Product { Id = 3, Name = "Термос", Price = 300, Photo = "https://encrypted-tbn3.gstatic.com/shopping?q=tbn:ANd9GcTG9IpU3MZcIxnE9gAB6xy8Ypj-ZHy8rCUv8kZ9UZn6bNFbZe6gT5WuIPmubEX81MhDeP1ymoHAAqQ0cvIyaEOgaCAABHbbs7tM_jQZf8L3VoDf2raMDSA89YeWJc9o0Y4mERAWOw&usqp=CAc" }
        };

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return Ok(products);
    }

    [HttpGet("{productId}")]
    public IActionResult GetProductById(int productId)
    {
        var product = products.Find(p => p.Id == productId);

        if (product == null)
            return NotFound();

        var productWithId = new { Id = product.Id, Name = product.Name, Price = product.Price, Photo = product.Photo };

        return Ok(productWithId);
    }

    [HttpPost]
    public ActionResult<Product> AddProduct(Product product)
    {
        product.Id = products.Count + 1;
        products.Add(product);
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product updatedProduct)
    {
        var existingProduct = products.Find(p => p.Id == id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.Name = updatedProduct.Name;
        existingProduct.Price = updatedProduct.Price;
        existingProduct.Photo = updatedProduct.Photo;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var existingProduct = products.Find(p => p.Id == id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        products.Remove(existingProduct);
        return NoContent();
    }

    [HttpPost("/api/login")]
    public IActionResult Login([FromForm] string login, [FromForm] string password)
    {
        if (login == "admin" && password == "admin")
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            };

            Response.Cookies.Append("authenticated", "true", cookieOptions);
        }
        else
        {
            Response.Cookies.Append("authenticated", "false", new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
        }

        return Redirect("/index");
    }

    [HttpGet("/api/check-auth")]
    public IActionResult CheckAuthentication()
    {
        var isAuthenticated = Request.Cookies.TryGetValue("authenticated", out string authenticatedValue) && authenticatedValue == "true";
        return Ok(isAuthenticated);
    }

}
