using App.Library.Models;
using AutoMapper;

namespace App.API.Mappings;

/// <summary>
/// Profile for mapping between Customer and CustomerDto.
/// </summary>
public class CustomerProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerProfile"/> class.
    /// </summary>
    public CustomerProfile()
    {
        /// <summary>
        /// Maps the Customer entity to the CustomerDto.
        /// </summary>
        CreateMap<Customer, CustomerDto>();
        
        /// <summary>
        /// Maps the CustomerDto to the Customer entity.
        /// </summary>
        CreateMap<CustomerDto, Customer>();
    }
}
