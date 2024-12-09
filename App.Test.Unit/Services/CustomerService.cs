using App.Library.Models;

namespace App.Test.Unit.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerService _customerService;

    public CustomerService(ICustomerService customerService)
    {
        this._customerService = customerService;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        return await _customerService.GetAllCustomersAsync();
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        return await _customerService.GetCustomerByIdAsync(customerId);
    }

    public async Task AddCustomerAsync(CustomerDto CustomerDto)
    {
        await _customerService.AddCustomerAsync(CustomerDto);
    }

    public async Task UpdateCustomerAsync(CustomerDto CustomerDto)
    {
        await _customerService.UpdateCustomerAsync(CustomerDto);
    }

    public async Task DeleteCustomerAsync(Guid customerId)
    {
        await _customerService.DeleteCustomerAsync(customerId);
    }
}