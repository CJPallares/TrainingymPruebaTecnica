using Microsoft.AspNetCore.Mvc;
using Test_TG_Carlos.Domain.Models;
using Test_TG_Carlos.Infrastructure.Helpers;
using Test_TG_Carlos.Application.Services;

namespace Test_TG_Carlos.Controllers;

[ApiController, Produces("application/json")]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly List<Customer> customers;
        private readonly List<Product> products;
        private readonly List<SalesOrderHeader> sales;

        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customers = CustomerHelper.generateCustomers();
            this.products = ProductHelper.generateProducts();
            this.sales = SaleHelper.getSales();

            this.customerService = customerService;
        }

        /// <summary>
        /// Endpoint para obtener un customer
        /// </summary>
        /// <returns>Un objeto Customer</returns>
        [HttpGet("getCustomer/{idCustomer}")]
        public IActionResult GetCustomer(int idCustomer)
        {
            Customer? customer = this.customers.FirstOrDefault(c => c.CustomerID == idCustomer);
            return Ok(customer);
        }

        /// <summary>
        /// Endpoint para obtener las compras dado un customerId
        /// </summary>
        /// <returns>Listado de SalesOrderHeader</returns>
        [HttpGet("getSalesByCustomer/{idCustomer}/")]
        public IActionResult GetSalesByCustomer(int idCustomer)
        {
            List<SalesOrderHeader> listSales = this.sales.Where(s => s.CustomerID == idCustomer).ToList();
            return Ok(listSales);
        }

        /// <summary>
        /// Endpoint para obtener los clientes y sus productos comprados
        /// </summary>
        /// <returns>Listado de customer con array de products</returns>
        [HttpGet("getCustomerProducts")]
        public IActionResult GetCustomerProducts()
        {
            var result = customers
                   .Select(customer => new
                   {
                       Customer = customer,
                       Products = sales
                           .Where(sale => sale.CustomerID == customer.CustomerID)
                           .SelectMany(sale => sale.SalesOrderDetails)
                           .Join(products, detail => detail.ProductID, product => product.ProductID,
                               (detail, product) => product)
                           .ToList()
                   })
                   .Select(customerWithProducts => new Customer
                   {
                       CustomerID = customerWithProducts.Customer.CustomerID,
                       Name = customerWithProducts.Customer.Name,
                       EmailAddress = customerWithProducts.Customer.EmailAddress,
                       Products = customerWithProducts.Products.ToList()
                   })
                   .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Endpoint para obtener un customer usando async/await
        /// </summary>
        /// <returns>Un objeto Customer</returns>
        [HttpGet("getCustomerasync/{idCustomer}")]
        public async Task<IActionResult> GetCustomerAsync(int idCustomer)
        {
            var customers = await customerService.getCustomerFromApiAsync();

            var customer = customers.FirstOrDefault(c => c.CustomerID == idCustomer);

            if (customer == null)
            {
                return NotFound($"No se encontró un cliente con el Id {idCustomer}.");
            }

            if (customer.SalesOrderHeader == null)
            {
                customer.SalesOrderHeader = new List<SalesOrderHeader>();
            }

            // Obtener las ventas con detalles y asignarlas al cliente
            var salesForCustomer = SaleHelper.getSales()
                .Where(sale => sale.CustomerID == idCustomer);

            foreach (var sale in salesForCustomer)
            {
                customer.SalesOrderHeader.Add(sale);
            }

            // Asignar solo los productos correspondientes al cliente con sus detalles de orden de venta
            customer.Products = products
                .Where(p =>
                    customer.SalesOrderHeader
                        .SelectMany(soh => soh.SalesOrderDetails ?? Enumerable.Empty<SalesOrderDetail>())
                        .Any(sod => sod.ProductID == p.ProductID)
                )
                .Select(p =>
                {
                    p.SalesOrderDetail = customer.SalesOrderHeader
                        .SelectMany(soh => soh.SalesOrderDetails ?? Enumerable.Empty<SalesOrderDetail>())
                        .Where(sod => sod.ProductID == p.ProductID)
                        .ToList();

                    return p;
                })
                .ToList();

            return Ok(customer);
        }
    }

