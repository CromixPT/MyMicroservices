using Microsoft.AspNetCore.Mvc;
using MyMicroServices.Discount.API.Entities;
using MyMicroServices.Discount.API.Interfaces;

namespace MyMicroServices.Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]/")]
public class DiscountController : ControllerBase
{

    private readonly IDiscountRepository _repository;

    public DiscountController(IDiscountRepository repository)
    {
        _repository = repository;
    }


    [HttpGet("{productName}",Name = "GetCoupon")]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Coupon))]
    public async Task<ActionResult<Coupon>> GetCoupon(string productName)
    {
        return Ok(await _repository.GetDiscount(productName));
    }

    [HttpDelete("{productName}",Name = "DeleteDiscount")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type=typeof(Coupon))]
    public async Task<ActionResult> Delete(string productName)
    {
        await _repository.RemoveDiscount(productName);
        return NoContent();
    }

    [HttpPost(Name = "AddDiscount")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Coupon))]
    public async Task<IActionResult> CreateDiscount([FromBody] Coupon newCoupon)
    {
        var createdCoupon = await _repository.CreateDiscount(newCoupon);
        return CreatedAtRoute(nameof(GetCoupon),new {productName = createdCoupon.ProductName},createdCoupon);
    }

    [HttpPut(Name = "EditDiscount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
    public async Task<IActionResult> EditDiscount([FromBody] Coupon newCoupon)
    {
        var createdCoupon = await _repository.UpdateDiscount(newCoupon);
        return Ok(createdCoupon);
    }


}