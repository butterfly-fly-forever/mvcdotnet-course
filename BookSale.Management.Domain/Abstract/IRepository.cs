namespace BookSale.Management.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
