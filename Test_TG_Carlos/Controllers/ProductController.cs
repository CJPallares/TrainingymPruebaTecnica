using Microsoft.AspNetCore.Mvc;
using Test_TG_Carlos.Infrastructure.Helpers;
using Test_TG_Carlos.Domain.Models;
using Test_TG_Carlos.Application.Services;

namespace Test_TG_Carlos.Controllers;

[ApiController, Produces("application/json")]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly List<Product> products;
    private readonly List<SalesOrderHeader> sales;
    public ProductController(ICustomerService customerService)
    {
        this.products = ProductHelper.generateProducts();
        this.sales = SaleHelper.getSales();
    }

    /// <summary>
    /// Endpoint para obtener el producto más vendido
    /// </summary>
    /// <returns>Un objeto Product</returns>
    [HttpGet("getTopProduct")]
    public IActionResult GetTopProduct()
    {
        var topProducts = sales
            .SelectMany(x => x.SalesOrderDetails)
            .GroupBy(x => x.ProductID)
            .Select(x => new { ProductID = x.Key, Quantity = x.Count() })
            .OrderByDescending(x => x.Quantity)
            .ToList();

        if (topProducts.Count == 0)
        {
            // No hay productos vendidos
            return NotFound();
        }

        var maxQuantity = topProducts.First().Quantity;
        var topProductIds = topProducts
            .Where(x => x.Quantity == maxQuantity)
            .Select(x => x.ProductID)
        .ToList();

        var topProductsData = products
            .Where(x => topProductIds.Contains(x.ProductID))
            .ToList();

        return Ok(topProductsData);
    }
}


