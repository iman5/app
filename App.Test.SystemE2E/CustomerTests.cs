using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using App.Library.Models;
using Bogus;

namespace App.Test.SystemE2E;

public class CustomerTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CustomerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CustomerWorkflow_ShouldWorkCorrectly()
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

        // Act - Add Customer
        var addResponse = await _client.PostAsJsonAsync("/api/v1/customers", newCustomer);
        Assert.Equal(HttpStatusCode.Created, addResponse.StatusCode);

        // Act - Get Customer
        var getResponse = await _client.GetAsync($"/api/v1/customers/{newCustomer.CustomerId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var customer = await getResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.Equal(newCustomer.FirstName, customer.FirstName);
        Assert.Equal(newCustomer.LastName, customer.LastName);
        Assert.Equal(newCustomer.Email, customer.Email);

        // Act - Update Customer
        customer.FirstName = "UpdatedName";
        var updateResponse = await _client.PutAsJsonAsync($"/api/v1/customers/{newCustomer.CustomerId}", customer);
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        // Act - Get Updated Customer
        var getUpdatedResponse = await _client.GetAsync($"/api/v1/customers/{newCustomer.CustomerId}");
        Assert.Equal(HttpStatusCode.OK, getUpdatedResponse.StatusCode);
        var updatedCustomer = await getUpdatedResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.Equal("UpdatedName", updatedCustomer.FirstName);

        // Act - Delete Customer
        var deleteResponse = await _client.DeleteAsync($"/api/v1/customers/{newCustomer.CustomerId}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Act - Verify Deletion
        var getDeletedResponse = await _client.GetAsync($"/api/v1/customers/{newCustomer.CustomerId}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
}