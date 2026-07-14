using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppInventario.Models
{
    /// <summary>
    /// Interfaz para el servicio de base de datos - permite inyección de dependencias y testing
    /// </summary>
    public interface IDatabaseService
    {
        Task InitializeAsync();

        // Productos
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductByCodeAsync(string code);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
        Task<List<Product>> GetLowStockProductsAsync();
        Task<List<Product>> GetAvailableProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category);
        Task<int> SaveProductAsync(Product product);
        Task<int> DeleteProductAsync(Product product);
        Task<int> DeleteProductByIdAsync(int id);
        Task<int> UpdateStockAsync(int productId, int newStock);
        Task<int> GetProductsCountAsync();
        Task<bool> ExistsProductWithCodeAsync(string code, int excludeId = 0);
    }
}