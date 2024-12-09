using App.Library.Models;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{ 
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to configure the context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    { 
    } 
    
    /// <summary>
    /// Gets or sets the Customers DbSet.
    /// </summary>
    public DbSet<Customer> Customers { get; set; } 
    
    /// <summary>
    /// Configures the model for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder); 
        
        modelBuilder.Entity<Customer>(entity => 
        { 
            entity.HasIndex(e => e.Email).IsUnique(); //Others are set via the annotations
        }); 
    } 
}
