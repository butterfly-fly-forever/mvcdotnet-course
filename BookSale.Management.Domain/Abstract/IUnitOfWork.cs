using System.Threading.Tasks;
using BookSale.Management.Domain.Abstract;
using BookSale.Management.DataAccess.Repository;

namespace BookSale.Management.DataAccess.Abstract
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IGenreRepository GenreRepository { get; }
        IUserAddressRepository UserAddressRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICartRepository CartRepository { get; }

        Task BeginTransaction();
        Task SaveChangeAsync();
        Task CommitTransactionAsync();
        void Dispose();
        Task RollbackTransactionAsync();
        IRepository<T> Repository<T>() where T : class;
    }
}