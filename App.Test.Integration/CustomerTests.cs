using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using App.Library.Models;
using Bogus;
using Microsoft.Extensions.DependencyInjection;

namespace App.Test.Integration;

public class CustomerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public CustomerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetCustomerById_ReturnsCustomer()
    {
        // Arrange
        var newCustomer = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .Generate();
        using (var scope = _factory.Services.CreateScope()) 
        { 
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); db.Customers.Add(newCustomer); db.SaveChanges(); 
        }

        // Act
        var response = await _client.GetAsync($"/api/v1/customers/{newCustomer.CustomerId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.Equal(newCustomer.CustomerId, customer.CustomerId);
        Assert.Equal(newCustomer.FirstName, customer.FirstName);
        Assert.Equal(newCustomer.MiddleName, customer.MiddleName);
        Assert.Equal(newCustomer.LastName, customer.LastName);
        Assert.Equal(newCustomer.Email, customer.Email);
        Assert.Equal(newCustomer.Phone, customer.Phone);
    }

    [Fact]
    public async Task AddCustomer_ReturnsCreatedCustomer()
    {
        // Arrange
        var newCustomer = new Faker<CustomerDto>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .Generate();

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/customers", newCustomer);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsNoContent()
    {
        // Arrange
        var existingCustomer = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Customers.Add(existingCustomer);
            db.SaveChanges();
        }

        var updatedCustomer = new CustomerDto
        {
            CustomerId = existingCustomer.CustomerId,
            FirstName = "UpdatedName",
            LastName = existingCustomer.LastName,
            MiddleName = existingCustomer.MiddleName,
            Email = existingCustomer.Email,
            Phone = existingCustomer.Phone
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/v1/customers/{existingCustomer.CustomerId}", updatedCustomer);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCustomer_ReturnsNoContent()
    {
        // Arrange
        var customerToDelete = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Customers.Add(customerToDelete);
            db.SaveChanges();
        }

        // Act
        var response = await _client.DeleteAsync($"/api/v1/customers/{customerToDelete.CustomerId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}



    //public CustomerTests(WebApplicationFactory<Program> factory)
    //{
    //    _factory = factory.WithWebHostBuilder(builder =>
    //    {
    //        builder.ConfigureServices(services =>
    //        {
    //            services.AddDbContext<ApplicationDbContext>(options =>
    //            {
    //                options.UseInMemoryDatabase("IntegrationTestDb");
    //            });

    //            var sp = services.BuildServiceProvider();

    //            using (var scope = sp.CreateScope())
    //            {
    //                var scopedServices = scope.ServiceProvider;
    //                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
    //                db.Database.EnsureCreated();
    //            }
    //        });
    //    });
    //    _client = _factory.CreateClient();
    //}
