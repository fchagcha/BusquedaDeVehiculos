# Sistema de Búsqueda Avanzada de Vehículos

La solución implementa un sistema de búsqueda avanzado de vehículos utilizando una **arquitectura limpia** orientada a **microservicios**, aplicando **SOLID**, **DRY** y **KISS**. Se ha integrado el patrón **CQRS** para separar las operaciones de escritura y lectura, y se utiliza el patrón **Event-Driven Architecture** (EDA) con **OurboxMessege** para la comunicación entre microservicios, compatible con **RabbitMQ** y **Azure Service Bus**.

La persistencia de datos se realiza en una base de datos SQL, con configuración para **MySQL y SqlServer**. Además, se emplea el patrón **Domain-Driven Design** (**DDD**) para el modelado del dominio utilizando paquetería propia, y el **Result Pattern** para el control de errores. **Exceptionless** se utiliza para el registro de excepciones no controladas, garantizando la estabilidad y fiabilidad del sistema.

Esta solución utiliza .NET 8 como tecnología principal.

NOTA:
- Script de base de datos para la funcionalidad del sistema en la carpeta bdd.
- Modificar archivo appsettings para configurar la conexión a la base de datos.
- La aplicación ejecuta Swagger apuntando al Api Bdv.Reservas.Api.Query, el cual tiene un método para listar vehículos disponibles.
