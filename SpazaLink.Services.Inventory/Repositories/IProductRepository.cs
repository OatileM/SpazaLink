using SpazaLink.Shared.Models;
namespace SpazaLink.Services.Inventory.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> SearchProductsAsync(string? category = null, string? area = null, decimal? minPrice = null, decimal? maxPrice = null);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(Product product);
    }
}
