using App.Library.Models;

namespace App.API.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> GetCustomerByIdAsync(Guid id);
    Task AddCustomerAsync(CustomerDto CustomerDto);
    Task UpdateCustomerAsync(CustomerDto CustomerDto);
    Task DeleteCustomerAsync(Guid id);
}