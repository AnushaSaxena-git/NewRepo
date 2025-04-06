namespace Catlog.Service
{
    public interface IUnitOfWork
    {
        void CreateTransaction();
        //IProductRepository products { get; }
        void Commit();
        void Rollback();

    }
}
