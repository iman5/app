using App.API.Interfaces;
using App.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace App.API.Repositories;

/// <summary>
/// Repository for managing customer data.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all customers asynchronously.
    /// </summary>
    /// <returns>The task result contains a list of customers.</returns>
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    /// <summary>
    /// Gets a customer by ID asynchronously.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <returns>The task result contains the customer with the specified ID.</returns>
    public async Task<Customer> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Entry(customer).State = EntityState.Detached;
            return customer;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Adds a new customer asynchronously.
    /// </summary>
    /// <param name="customer">The customer to add.</param>
    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing customer asynchronously.
    /// </summary>
    /// <param name="customer">The customer to update.</param>
    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a customer by ID asynchronously.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
