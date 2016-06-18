using StructureMap;

namespace Stock.Dependencies
{
    /// <summary>
    /// Static class which represents Inversion of Control container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Initializes Inversion of Control container
        /// </summary>
        /// <returns>IoC Container instance</returns>
        public static IContainer Initialize()
        {
            return new Container(c => c.AddRegistry<IocRegistry>());
        }
    }
}
