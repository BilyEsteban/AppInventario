using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppInventario.Models
{
    /// <summary>
    /// Implementación del servicio de base de datos
    /// </summary>
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseContext _context;

        public DatabaseService(string dbPath)
        {
            _context = new DatabaseContext(dbPath);
        }

        public async Task InitializeAsync()
        {
            await _context.InitializeAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.GetProductByIdAsync(id);
        }

        public async Task<Product?> GetProductByCodeAsync(string code)
        {
            return await _context.GetProductByCodeAsync(code);
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.SearchProductsAsync(searchTerm);
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _context.GetLowStockProductsAsync();
        }

        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await _context.GetAvailableProductsAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _context.GetProductsByCategoryAsync(category);
        }

        public async Task<int> SaveProductAsync(Product product)
        {
            return await _context.SaveProductAsync(product);
        }

        public async Task<int> DeleteProductAsync(Product product)
        {
            return await _context.DeleteProductAsync(product);
        }

        public async Task<int> DeleteProductByIdAsync(int id)
        {
            return await _context.DeleteProductByIdAsync(id);
        }

        public async Task<int> UpdateStockAsync(int productId, int newStock)
        {
            return await _context.UpdateStockAsync(productId, newStock);
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _context.GetProductsCountAsync();
        }

        public async Task<bool> ExistsProductWithCodeAsync(string code, int excludeId = 0)
        {
            return await _context.ExistsProductWithCodeAsync(code, excludeId);
        }
    }
}