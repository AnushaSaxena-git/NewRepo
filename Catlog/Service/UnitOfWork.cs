using AutoMapper;
using Catlog.Models;
using Catlog.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catlog.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatlogDBcontext _context;
        private readonly IMapper _mapper;
        private IProductSkuRepository _productSkuRepository;
        private IProduct_ImageRepository _productImageRepository;
        private IProductRepository _productRepository;

        public UnitOfWork(CatlogDBcontext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // Lazy initialization of ProductImageRepository
        public IProduct_ImageRepository ProductImageRepository =>
            _productImageRepository ??= new Product_ImageRepository(_context, _mapper);

        // Lazy initialization of ProductSkuRepository
        public IProductSkuRepository ProductSkuRepository =>
            _productSkuRepository ??= new ProductskuRepo(_context, _mapper, ProductImageRepository);

        // Lazy initialization of ProductRepository
        public IProductRepository ProductRepository =>
            _productRepository ??= new ProductRepository(_context, _mapper, this);

        // Methods for handling transactions
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Unchanged;
            }
        }

        public void CreateTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
