using Microsoft.AspNetCore.Mvc;
using Test_TG_Carlos.Domain.Models;
using Test_TG_Carlos.Infrastructure.Helpers;
using Test_TG_Carlos.Application.Services;

namespace Test_TG_Carlos.Controllers;

[ApiController, Produces("application/json")]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly List<Product> products;
        private readonly List<SalesOrderHeader> sales;
        public SalesController(ICustomerService customerService)
        {
            this.products = ProductHelper.generateProducts();
            this.sales = SaleHelper.getSales();
        }

    /// <summary>
    /// Endpoint para obtener la venta más pesada (la suma de los pesos de los productos
    /// </summary>
    /// <returns>Un objeto SalesOrderHeader</returns>
    [HttpGet("getWeightestSale")]
        public IActionResult GetWeightestSale()
        {
            var weightestSale = sales
                 .Select(sale => new
                 {
                     Sale = sale,
                     TotalWeight = sale.SalesOrderDetails
                         .Join(products, detail => detail.ProductID, product => product.ProductID,
                             (detail, product) => new { ProductWeight = product.Weight ?? 0 })
                         .Sum(p => p.ProductWeight)
                 })
                 .OrderByDescending(x => x.TotalWeight)
                 .FirstOrDefault();

            if (weightestSale == null)
            {
                return NotFound("No hay ventas disponibles.");
            }

        return Ok(weightestSale.Sale);
    }
}


