namespace App.UI.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Library.Models;

/// <summary>
/// Service for managing customer-related operations via API.
/// </summary>
public class CustomerService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets all customers asynchronously.
    /// </summary>
    /// <returns>The task result contains a list of customer DTOs.</returns>
    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("/api/v1/customers");
    }

    /// <summary>
    /// Gets a customer by ID asynchronously.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <returns>The task result contains the customer DTO with the specified ID.</returns>
    public async Task<CustomerDto> GetCustomerAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CustomerDto>($"/api/v1/customers/{id}");
    }

    /// <summary>
    /// Creates a new customer asynchronously.
    /// </summary>
    /// <param name="customerDto">The customer data transfer object.</param>
    public async Task CreateCustomerAsync(CustomerDto customerDto)
    {
        await _httpClient.PostAsJsonAsync("/api/v1/customers", customerDto);
    }

    /// <summary>
    /// Updates an existing customer asynchronously.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <param name="customerDto">The customer data transfer object.</param>
    public async Task UpdateCustomerAsync(Guid id, CustomerDto customerDto)
    {
        await _httpClient.PutAsJsonAsync($"/api/v1/customers/{id}", customerDto);
    }

    /// <summary>
    /// Deletes a customer by ID asynchronously.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    public async Task DeleteCustomerAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"/api/v1/customers/{id}");
    }
}
