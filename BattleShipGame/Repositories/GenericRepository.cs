using BattleShipGame.Database;
using Microsoft.EntityFrameworkCore;

namespace BattleShipGame.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly BattleShipGameContext context;
    protected readonly DbSet<TEntity> set;

    public GenericRepository(BattleShipGameContext context)
    {
        this.context = context;
        set = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return set.AsNoTracking();
    }

    public async Task Create(TEntity entity)
    {
        await set.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await GetById(id);

        if(entity is null) return;

        set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(TEntity entity)
    {
        set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await set.FindAsync(id);
    }

    public async Task Update(TEntity entity)
    {
        set.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task Update(int id, TEntity entity)
    {
        var entry = set.Find(id);

        if(entry is null) return;

        set.Entry(entry).CurrentValues.SetValues(entity);

        await context.SaveChangesAsync();
    }
}