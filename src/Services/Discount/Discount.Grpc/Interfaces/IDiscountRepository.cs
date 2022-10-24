﻿using MyMicroServices.Discount.API.Entities;

namespace Discount.Grpc.Interfaces;

public interface IDiscountRepository
{

    Task<Coupon> GetDiscount(string productName);
    Task<Coupon> CreateDiscount(Coupon coupon);
    Task<Coupon> UpdateDiscount(Coupon coupon);
    Task<bool> RemoveDiscount(string productName);
}
