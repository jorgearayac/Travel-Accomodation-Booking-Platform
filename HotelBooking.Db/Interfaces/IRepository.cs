namespace HotelBooking.Db.Interfaces;

/// <summary>
/// Generic repository interface for managing data in the database. Provides basic CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type T from the database.
    /// </summary>
    /// <returns>A collection of all entities of type T.</returns>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// Finds a single entity by its Id.
    /// </summary>
    /// <param name="id">The Id to search for.</param>
    /// <returns>The entity, or null if not found.</returns>
    Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>The created entity with the generated Id.</returns>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    /// <param name="entity">The entity with updated values.</param>
    /// <returns></returns>
    Task UpdateAsync(T entity);
    
    /// <summary>
    /// Deletes an entity from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns></returns>
    Task DeleteAsync(T entity);
}
