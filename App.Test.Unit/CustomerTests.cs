
using App.Library.Models;
using App.Test.Unit.Services;
using Bogus;
using Moq;

namespace App.Test.Unit;

public class CustomerTests
{
    private readonly Mock<ICustomerService> _mockRepo;
    private readonly CustomerService _customerService;
    private readonly Faker<CustomerDto> _customerFaker;

    public CustomerTests()
    {
        _mockRepo = new Mock<ICustomerService>();
        _customerService = new CustomerService(_mockRepo.Object);
        _customerFaker = new Faker<CustomerDto>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.MiddleName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber());
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnCustomer()
    {
        // Arrange
        var expectedCustomer = _customerFaker.Generate();
        _mockRepo.Setup(repo => repo.GetCustomerByIdAsync(expectedCustomer.CustomerId)).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(expectedCustomer.CustomerId);

        // Assert
        Assert.Equal(expectedCustomer, result);
    }

    [Fact]
    public async Task AddCustomer_ShouldAddCustomer()
    {
        // Arrange
        var newCustomer = _customerFaker.Generate();

        // Act
        await _customerService.AddCustomerAsync(newCustomer);

        // Assert
        _mockRepo.Verify(repo => repo.AddCustomerAsync(newCustomer), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldUpdateCustomer()
    {
        // Arrange
        var existingCustomer = _customerFaker.Generate();

        // Act
        await _customerService.UpdateCustomerAsync(existingCustomer);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateCustomerAsync(existingCustomer), Times.Once);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldDeleteCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        // Act
        await _customerService.DeleteCustomerAsync(customerId);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteCustomerAsync(customerId), Times.Once);
    }
}