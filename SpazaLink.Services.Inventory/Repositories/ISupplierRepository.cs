using SpazaLink.Shared.Models;

namespace SpazaLink.Services.Inventory.Repositories
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetSuppliersByAreaAsync(string area);
        Task<Supplier?> GetSupplierByIdAsync(Guid id);
    }
}
