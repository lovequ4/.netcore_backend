using backend.Models;
using backend.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
[ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;
        public ProductController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>>GetAllProducts()
        {
            if(_dbContext.products == null)
            {
                return NotFound();
            }
            return await _dbContext.products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>>GetProductById(int id)
        {
            if(_dbContext.products == null)
            {
                return NotFound();
            }

            var product = await _dbContext.products.FindAsync(id);

            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _dbContext.products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<IEnumerable<Product>>>EditProduct(int id, Product updatedProduct)
        {
            var Product = await _dbContext.products.FirstOrDefaultAsync(p => p.Id == id);

            if(Product==null){
                return NotFound();
            }

            Product.ProductName = updatedProduct.ProductName;
            Product.Description = updatedProduct.Description;
            Product.Price = updatedProduct.Price;
            Product.Quantity = updatedProduct.Quantity;

            await _dbContext.SaveChangesAsync();
            return Ok(Product);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productToDelete = await _dbContext.products.FindAsync(id);

            if (productToDelete == null)
            {
                return NotFound();
            }

            _dbContext.products.Remove(productToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent(); 
        }
    }
}