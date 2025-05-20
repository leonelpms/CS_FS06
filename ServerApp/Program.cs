namespace ServerApp
{   
    /// <summary>
    /// Representa una categor�a de producto.
    /// </summary>
    /// <param name="Id">Identificador �nico de la categor�a.</param>
    /// <param name="Name">Nombre de la categor�a.</param>
    public record Category(int Id, string Name);

    /// <summary>
    /// Representa un producto con informaci�n de categor�a.
    /// </summary>
    /// <param name="Id">Identificador �nico del producto.</param>
    /// <param name="Name">Nombre del producto.</param>
    /// <param name="Price">Precio del producto.</param>
    /// <param name="Stock">Cantidad disponible en inventario.</param>
    /// <param name="Category">Categor�a a la que pertenece el producto.</param>
    public record ProductListItem(int Id, string Name, double Price, int Stock, Category Category);

    /// <summary>
    /// Clase principal de la aplicaci�n ASP.NET Core.
    /// Configura y ejecuta la API de InventoryHub.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicaci�n.
        /// Configura servicios, middlewares y endpoints de la API.
        /// </summary>
        /// <param name="args">Argumentos de l�nea de comandos.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Agrega servicios de controladores al contenedor de dependencias.
            builder.Services.AddControllers();

            var app = builder.Build();

            // Redirige autom�ticamente las solicitudes HTTP a HTTPS.
            app.UseHttpsRedirection();

            // Configura CORS para permitir cualquier origen, m�todo y encabezado.
            // Esto es necesario para permitir que el frontend Blazor consuma la API sin restricciones.
            app.UseCors(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());

            // Habilita la autorizaci�n (no se usa en este ejemplo, pero es parte del pipeline).
            app.UseAuthorization();

            // Mapea los controladores de la API (no se usan en este ejemplo, pero permite agregar controladores personalizados).
            app.MapControllers();

            // Endpoint: /api/products
            // Devuelve una lista simple de productos sin informaci�n de categor�a.
            // �til para pruebas o integraciones b�sicas.
            app.MapGet("/api/products", () =>
            {
                return new[]
                {
                    new { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25 },
                    new { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100 }
                };
            });

            // Endpoint: /api/productlist
            // Devuelve una lista de productos con informaci�n de categor�a.
            // Utiliza modelos fuertemente tipados para garantizar la estructura JSON.
            app.MapGet("/api/productlist", () =>
            {
                var products = new[]
                {
                    new ProductListItem(1, "Laptop", 1200.50, 25, new Category(101, "Electronics")),
                    new ProductListItem(2, "Headphones", 50.00, 100, new Category(102, "Accessories")),
                    new ProductListItem(3, "Smartphone", 800.00, 50, new Category(101, "Electronics")),
                    new ProductListItem(4, "Office Chair", 150.00, 40, new Category(103, "Furniture")),
                    new ProductListItem(5, "Monitor", 300.00, 30, new Category(101, "Electronics")),
                    new ProductListItem(6, "Keyboard", 25.00, 200, new Category(102, "Accessories")),
                    new ProductListItem(7, "Desk Lamp", 35.00, 60, new Category(103, "Furniture")),
                    new ProductListItem(8, "Mouse", 20.00, 180, new Category(102, "Accessories"))
                };
                return Results.Json(products);
            });

            // Inicia la aplicaci�n web.
            app.Run();
        }
    }
}
