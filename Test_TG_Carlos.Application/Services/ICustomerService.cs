using Test_TG_Carlos.Domain.Models;

namespace Test_TG_Carlos.Application.Services;

public interface ICustomerService
{
    Task<List<Customer>> getCustomerFromApiAsync();

}
