using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CombinedAPI.Models;
using CombinedAPI.Repositories;
using CombinedAPI.Services;
using CombinedAPI.Interfaces;

namespace CombinedAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IProductManager _productManager;

    public ProductController(IProductManager productManager)
    {
      _productManager = productManager;
    }

    // GET: api/product/id/{id}
    [HttpGet("id/{id}")]
    public IActionResult GetProductById(int id)
    {
      Console.WriteLine($"Getting product by ID: {id}");
      var product = _productManager.GetProductById(id);
      if (product == null)
      {
        return NotFound("Product not found.");
      }
      return Ok(product);
    }

    // GET : api/product/name/{name}
    [HttpGet("name/{name}")]
    public IActionResult GetProductByName(string name) 
    {
      Console.WriteLine($"Getting product by Name: {name}");
      var product = _productManager.GetProductByName(name);
      if (product == null)
      {
        return NotFound("Product not found.");
      }
      return Ok(product);
    }

    // GET: api/product/category/{id}
    [HttpGet("category/{id}")]
    public IActionResult GetProductByCategory(int id) 
    {
      Console.WriteLine($"Getting product by Category: {id}");
      var products = _productManager.GetProductByCategory(id);
      if (products == null || !products.Any())
      {
        return NotFound("Product in this category not found.");
      }
      return Ok(products);
    }
    
    // GET: api/product
    [HttpGet]
    public IActionResult GetAllProducts()
    {
      var products = _productManager.GetAllProducts();
      if (products == null || !products.Any())
      {
        return NotFound("No Products found.");
      }
      return Ok(products);
    }

    // PUT: api/product/update-stock
    [HttpPut("update-stock")]
    public IActionResult UpdateProductStock([FromBody] UpdateStockRequest request)
    {
      Console.WriteLine($"Updating product stock for productID: {request.productId}");
      var success = _productManager.UpdateProductStock(request.productId, request.stock);
      if (!success)
      {
        return BadRequest("Failed to update stock.");
      }
      return Ok("Stock updated successfully.");
    }
  }

  // Request models for updating stock and discount
  public class UpdateStockRequest
  {
    public int productId { get; set; }
    public int stock { get; set; }
  }

}
