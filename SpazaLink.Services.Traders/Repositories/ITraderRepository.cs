using SpazaLink.Shared.Models;

namespace SpazaLink.Services.Traders.Repositories
{
    /// <summary>
    /// Repository interface for managing trader data operations
    /// Provides abstraction layer for trader persistence operations
    /// </summary>
    public interface ITraderRepository
    {
        /// <summary>
        /// Creates a new trader record in the data store
        /// </summary>
        /// <param name="trader">The trader entity to create</param>
        /// <returns>The created trader with any generated fields populated</returns>
        /// <exception cref="ArgumentNullException">Thrown when trader is null</exception>
        Task<Trader> CreateTraderAsync(Trader trader);

        /// <summary>
        /// Retrieves a trader by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the trader</param>
        /// <returns>The trader if found, otherwise null</returns>
        Task<Trader?> GetTraderByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all traders operating in a specific area
        /// </summary>
        /// <param name="area">The area name to search for traders</param>
        /// <returns>List of traders in the specified area</returns>
        /// <exception cref="ArgumentException">Thrown when area is null or empty</exception>
        Task<List<Trader>> GetTradersByAreaAsync(string area);
    }
}
