namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Generic repository interface for managing data in the database. Provides basic CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/> from the database.
    /// </summary>
    /// <returns>A collection of all entities of type <typeparamref name="T"/>.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Finds a single <typeparamref name="T"/> by its Id.
    /// </summary>
    /// <param name="id">The Id to search for.</param>
    /// <returns>The <typeparamref name="T"/>, or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Adds a new <typeparamref name="T"/> to the database.
    /// </summary>
    /// <param name="entity">The <typeparamref name="T"/> to create.</param>
    /// <returns>The created <typeparamref name="T"/> with the generated Id.</returns>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Updates an existing <typeparamref name="T"/> in the database.
    /// </summary>
    /// <param name="entity">The <typeparamref name="T"/> with updated values.</param>
    /// <returns></returns>
    Task UpdateAsync(T entity);
    
    /// <summary>
    /// Deletes a <typeparamref name="T"/> from the database.
    /// </summary>
    /// <param name="entity">The <typeparamref name="T"/> to delete.</param>
    /// <returns></returns>
    Task DeleteAsync(T entity);
}
