using InternetShopWebApp.Context;
using InternetShopWebApp.Models;

namespace InternetShopWebApp.Repository
{
    public class UnitOfWork : IDisposable
    {
        private InternetShopContext context = new InternetShopContext();
        private GenericRepository<CategoryTable> _categoryRepository;
        private GenericRepository<LocationTable> _locationRepository;
        private GenericRepository<ProductTable> _productRepository;
        private GenericRepository<OrderItemTable> _orderItemRepository;
        private GenericRepository<OrderTable> _orderRepository;

        public GenericRepository<CategoryTable> CategoryRepository
        {
            get
            {

                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new GenericRepository<CategoryTable>(context);
                }
                return _categoryRepository;
            }
        }

        public GenericRepository<LocationTable> LocationRepository
        {
            get
            {

                if (this._locationRepository == null)
                {
                    this._locationRepository = new GenericRepository<LocationTable>(context);
                }
                return _locationRepository;
            }
        }

        public GenericRepository<ProductTable> ProductRepository
        {
            get
            {

                if (this._productRepository == null)
                {
                    this._productRepository = new GenericRepository<ProductTable>(context);
                }
                return _productRepository;
            }
        }

        public GenericRepository<OrderItemTable> OrderItemRepository
        {
            get
            {

                if (this._orderItemRepository == null)
                {
                    this._orderItemRepository = new GenericRepository<OrderItemTable>(context);
                }
                return _orderItemRepository;
            }
        }

        public GenericRepository<OrderTable> OrderRepository
        {
            get
            {

                if (this._orderRepository == null)
                {
                    this._orderRepository = new GenericRepository<OrderTable>(context);
                }
                return _orderRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
