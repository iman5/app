using Microsoft.AspNetCore.Mvc;
using App.Library.Models;
using App.API.Services;

namespace App.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly ILogger<CustomersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomersController"/> class.
    /// </summary>
    /// <param name="customerService">Service for managing customers.</param>
    /// <param name="logger">Logger for logging messages.</param>
    public CustomersController(CustomerService customerService, ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        this._logger = logger;
    }
    
    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <returns>A list of customers.</returns>
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetCustomers()
    {
        _logger.LogInformation("Fetching all customers.");
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Gets a customer by ID.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <returns>The customer with the specified ID.</returns>
    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        _logger.LogInformation($"Fetching customer with ID: {id}");
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning($"Customer with ID: {id} not found.");
            return NotFound();
        }
        return Ok(customer);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customerDto">The customer data transfer object.</param>
    /// <returns>The created customer.</returns>
    [HttpPost]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> PostCustomer(CustomerDto customerDto)
    {
        _logger.LogInformation($"Creating a new customer: {customerDto.FirstName}");
        if (customerDto == null)
        {
            _logger.LogWarning("Customer was not provided.");
            return BadRequest();
        }

        await _customerService.AddCustomerAsync(customerDto);
        return CreatedAtAction(nameof(GetCustomer), new { id = customerDto.CustomerId }, customerDto);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <param name="customerDto">The customer data transfer object.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> PutCustomer(Guid id, CustomerDto customerDto)
    {
        if (customerDto == null || customerDto.CustomerId != id)
        {
            _logger.LogWarning("Customer ID mismatch or customer was not provided.");
            return BadRequest();
        }

        var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
        if (existingCustomer == null)
        {
            _logger.LogWarning($"Customer with ID: {id} not found.");
            return NotFound();
        }

        await _customerService.UpdateCustomerAsync(customerDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a customer by ID.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        _logger.LogInformation($"Deleting customer with ID: {id}");
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            _logger.LogWarning($"Customer with ID: {id} not found.");
            return NotFound();
        }

        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
