using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.CatalogBrands.AnyAsync())
            {
                await catalogContext.CatalogBrands.AddRangeAsync(
                    GetPreconfiguredCatalogBrands());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogTypes.AnyAsync())
            {
                await catalogContext.CatalogTypes.AddRangeAsync(
                    GetPreconfiguredCatalogTypes());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogItems.AnyAsync())
            {
                await catalogContext.CatalogItems.AddRangeAsync(
                    GetPreconfiguredItems());

                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;
            
            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
            {
                new("Adidas"),
                new("Nike"),
                new("Levis"),
                new("Skybag"),
                new("Other")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
            {
                new("Shoes"),
                new("T-Shirt"),
                new("Jeans"),
                new("Bag")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
            {
                new(2,2, "Nike Oversized T-Shirt", "Nike Oversized T-Shirt", 19.5M,  "http://catalogbaseurltobereplaced/images/products/1.png"),
                new(1,1, "Adidas Men Trainers Shoes", "Adidas Men Trainers Shoes",  12, "http://catalogbaseurltobereplaced/images/products/7.png"),
                new(2,2, "Nike T-Shirt Dri-FIT", "Nike T-Shirt Dri-FIT", 12, "http://catalogbaseurltobereplaced/images/products/6.png"),
                new(2,2, "Nike T-shirt 2024", "Nike T-shirt 2024", 12, "http://catalogbaseurltobereplaced/images/products/4.png"),
                new(3,3, "Levis Men's Fit Jeans", "Levis Men's Fit Jeans", 8.5M, "http://catalogbaseurltobereplaced/images/products/5.png"),
                new(4,1, "Adidas Classic Bag", "Adidas Classic Bag", 8.5M, "http://catalogbaseurltobereplaced/images/products/8.png"),
                new(1,2, "Nike Mens Dart Shoes", "Nike Mens Dart Shoes", 8.50M, "http://catalogbaseurltobereplaced/images/products/2.png"),
                new(4,4, "Skybag astro Bag", "Skybag astro Bag", 12, "http://catalogbaseurltobereplaced/images/products/9.png"),
                new(1,2, "Nike Blue Shoes", "Nike Blue Shoes", 12, "http://catalogbaseurltobereplaced/images/products/10.png"),
                new(2,1, "Adidas essential T-shirt", "Adidas essential T-shirt", 12,  "http://catalogbaseurltobereplaced/images/products/3.png"),
                new(1,1, "Adidas Samba Shoes", "Adidas Samba Shoes", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
                new(3,3, "Levis blue Jeans", "Levis blue Jeans", 12, "http://catalogbaseurltobereplaced/images/products/12.png")
            };
    }
}
