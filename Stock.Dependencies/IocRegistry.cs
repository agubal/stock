using Stock.DataAccess;
using Stock.Services;
using Stock.Services.Stocks;
using StructureMap;

namespace Stock.Dependencies
{
    /// <summary>
    /// Inversion of Control container
    /// </summary>
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Register();
        }

        /// <summary>
        /// Registers project dependencies and puts them to IoC Container
        /// </summary>
        private void Register()
        {
            //Generics:
            For(typeof(IRepository<,>)).Use(typeof(GenericRepository<,>));
            For(typeof(IService<,>)).Use(typeof(GenericService<,>));

            //Services:
            For<IStockService>().Use<StockService>();
        }
    }
}
