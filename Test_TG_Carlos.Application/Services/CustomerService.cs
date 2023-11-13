using System.Collections.Generic;
using System.Threading.Tasks;
using Test_TG_Carlos.Infrastructure.Helpers;
using Test_TG_Carlos.Domain.Models;

namespace Test_TG_Carlos.Application.Services;

public class CustomerService : ICustomerService
{
    public Task<List<Customer>> getCustomerFromApiAsync()
    {
        return Task.FromResult(CustomerHelper.generateCustomers());
    }
}
