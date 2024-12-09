using App.API.Interfaces;
using App.Library.Models;
using AutoMapper;

namespace App.API.Services;

/// <summary>
/// Service for managing customer-related operations.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        this._mapper = mapper;
    }
    
    /// <summary>
    /// Gets all customers asynchronously.
    /// </summary>
    /// <returns>The task result contains a list of customer DTOs.</returns>
    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllCustomersAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    /// <summary>
    /// Gets a customer by ID asynchronously.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <returns>The task result contains the customer DTO with the specified ID.</returns>
    public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        return _mapper.Map<CustomerDto>(customer);
    }

    /// <summary>
    /// Adds a new customer asynchronously.
    /// </summary>
    /// <param name="customerDto">The customer data transfer object.</param>
    public async Task AddCustomerAsync(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        await _customerRepository.AddCustomerAsync(customer);
    }

    /// <summary>
    /// Updates an existing customer asynchronously.
    /// </summary>
    /// <param name="customerDto">The customer data transfer object.</param>
    public async Task UpdateCustomerAsync(CustomerDto customerDto)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerDto.CustomerId); 
        if (customer == null) return;

        _mapper.Map(customerDto, customer);
        await _customerRepository.UpdateCustomerAsync(customer);
    }

    /// <summary>
    /// Deletes a customer by ID asynchronously.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    public async Task DeleteCustomerAsync(Guid customerId)
    {
        await _customerRepository.DeleteCustomerAsync(customerId);
    }
}
