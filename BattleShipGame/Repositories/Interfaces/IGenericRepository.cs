namespace BattleShipGame.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IEnumerable<TEntity> GetAll();

    Task Create(TEntity entity);

    Task Delete(int id);

    Task Delete(TEntity entity);

    Task<TEntity?> GetById(int id);

    Task Update(TEntity entity);

    Task Update(int id, TEntity entity);
}