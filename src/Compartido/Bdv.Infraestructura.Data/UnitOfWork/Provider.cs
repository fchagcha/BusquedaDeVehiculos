namespace Bdv.Infraestructura.Data.UnitOfWork
{
    public class Provider(IServiceProvider serviceProvider) : IProvider
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly Dictionary<Type, object> _unitOfWorkInstances = [];

        public T CrearNuevaInstancia<T>() where T : new()
        {
            return new T();
        }

        public T ObtenerServicio<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public T ObtenerUnitOfWork<T>() where T : IUnitOfWork
        {
            Type unitOfWorkType = typeof(T);

            if (_unitOfWorkInstances.TryGetValue(unitOfWorkType, out object unitOfWorkInstance))
                return (T)unitOfWorkInstance;

            T unitOfWork = _serviceProvider.GetRequiredService<T>();
            _unitOfWorkInstances.Add(unitOfWorkType, unitOfWork);

            return unitOfWork;
        }
    }
}