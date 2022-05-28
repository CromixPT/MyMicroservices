using Microsoft.AspNetCore.Mvc;
using MyMicroServices.Catalog.API.Domain.Entities;
using MyMicroServices.Catalog.API.Repositories;

namespace MyMicroServices.Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }


    [HttpGet(Name = "GetAllProducts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(IEnumerable<Product>))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await _repository.GetAllProducts());
    }


    [HttpGet("{id}", Name = "GetProductById")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var product = await _repository.GetProductById(id);
        if (product == default)
        {
            return NotFound();
        }

        return Ok(product);
    }
    
    [HttpPost(Name = "AddProduct")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddProduct([FromBody] Product newProduct)
    {
        await _repository.AddProduct(newProduct);
        return CreatedAtRoute(nameof(GetProductById), new {id = newProduct.Id}, newProduct);
    }
    
    [HttpGet("name/{name}", Name = "GetProductByName")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
    {
        var products = await _repository.GetProductByName(name);
        if (!products.Any())
        {
            return NotFound();
        }

        return Ok(products);

    }

    [HttpGet("/category/{category}", Name = "GetProductsByCategory")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        var products = await _repository.GetProductByCategory(category);
        if (!products.Any())
        {
            return NotFound();
        }

        return Ok(products);
    }

    [HttpPut("{id}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
    {
        if (id!=product.Id)
        {
            return BadRequest();
        }

        var existingProduct = _repository.GetProductById(product.Id);
        if (existingProduct == default)
        {
            return NotFound();
        }

        await _repository.UpdateProduct(product);
        
        return NoContent();
    }
    
    [HttpDelete("{id}", Name = "DeleteProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(string id)
    {
        var existingProduct =await _repository.GetProductById(id);
        if (existingProduct == default)
        {
            return NotFound();
        }

        await _repository.DeleteProduct(id);
        
        return NoContent();
    }

    

    
}