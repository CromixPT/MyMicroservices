using Dapper;
using Discount.Grpc.Interfaces;
using MyMicroServices.Discount.API.Entities;
using Npgsql;

namespace Discount.Grpc.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;


    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;


    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        await using var connection = GetConnection();

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
            "SELECT * FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });

        if (coupon == null)
        {
            return new Coupon()
            {
                ProductName = "No Discount",
                Description = "No existing Discount"
            };
        }

        return coupon;
    }

    public async Task<Coupon> CreateDiscount(Coupon coupon)
    {
        await using var connection = GetConnection();

        var affected = await connection.ExecuteAsync(
            "INSERT INTO Coupon (ProductName,description,Amount) VALUES (@ProductName, @Description,@Amount)",
            new { coupon.ProductName, coupon.Description, coupon.Amount });

        if (affected == 0)
        {
            return new Coupon()
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No existing Discount"
            };
        }

        return await GetDiscount(coupon.ProductName);

    }

    public async Task<Coupon> UpdateDiscount(Coupon coupon)
    {
        await using var connection = GetConnection();

        var affected = await connection.ExecuteAsync(
            "UPDATE Coupon SET ProductName = @ProductName ,description = @Description,Amount = @Amount WHERE Id = @Id",
            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

        if (affected == 0)
        {
            return new Coupon()
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No existing Discount"
            };
        }

        return await GetDiscount(coupon.ProductName);
    }

    public async Task<bool> RemoveDiscount(string productName)
    {
        await using var connection = GetConnection();

        var affected = await connection.ExecuteAsync(
            "DELETE FROM Coupon WHERE ProductName=@ProductName",
            new { ProductName = productName });

        if (affected == 0)
        {
            return false;
        }

        return true;
    }

    private NpgsqlConnection GetConnection()
    {
        var connString = _configuration.GetConnectionString("DiscountDb");
        return new NpgsqlConnection(connString);

    }
}
