using Microsoft.EntityFrameworkCore;
using HotelBooking.Db.Data;
using HotelBooking.Db.Interfaces;

namespace HotelBooking.Db.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly HotelBookingDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(HotelBookingDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _dbSet.CountAsync();
        var items = await _dbSet
            .OrderBy(e => EF.Property<int>(e, "Id"))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}