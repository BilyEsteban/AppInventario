using SQLite;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppInventario.Models
{
    /// <summary>
    /// Contexto de base de datos para manejar operaciones CRUD con SQLite
    /// </summary>
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseContext(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        /// <summary>
        /// Inicializa la base de datos creando las tablas necesarias
        /// </summary>
        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<Product>();
            await _database.CreateTableAsync<User>();
            await EnsureDefaultAdminAsync();
        }

        // ==================== USUARIOS ====================

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _database.Table<User>().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _database.Table<User>().OrderBy(u => u.Username).ToListAsync();
        }

        public async Task<User?> AuthenticateAsync(string emailOrUsername, string password)
        {
            var normalized = emailOrUsername.Trim().ToLowerInvariant();
            var user = await _database.Table<User>()
                .Where(u => u.Email.ToLower() == normalized || u.Username.ToLower() == normalized)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var providedHash = HashPassword(password);
            return providedHash.Equals(user.PasswordHash, System.StringComparison.Ordinal) ? user : null;
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var normalizedUsername = username.Trim().ToLowerInvariant();

            var existing = await _database.Table<User>()
                .Where(u => u.Email.ToLower() == normalizedEmail || u.Username.ToLower() == normalizedUsername)
                .FirstOrDefaultAsync();

            return existing != null;
        }

        public async Task<int> SaveUserAsync(User user)
        {
            user.UpdatedAt = DateTime.Now;

            if (user.Id == 0)
            {
                user.CreatedAt = DateTime.Now;
                return await _database.InsertAsync(user);
            }

            return await _database.UpdateAsync(user);
        }

        public async Task EnsureDefaultAdminAsync()
        {
            var existingAdmin = await _database.Table<User>()
                .Where(u => u.Role == UserRole.Admin)
                .FirstOrDefaultAsync();

            if (existingAdmin != null)
            {
                return;
            }

            var admin = new User
            {
                Email = "admin@appinventario.com",
                Username = "admin",
                PasswordHash = HashPassword("admin123"),
                FullName = "Administrador",
                Phone = "0000000000",
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _database.InsertAsync(admin);
        }

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        // ==================== PRODUCTOS ====================

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _database.Table<Product>().OrderBy(p => p.Name).ToListAsync();
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _database.Table<Product>().FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Obtiene un producto por su código
        /// </summary>
        public async Task<Product?> GetProductByCodeAsync(string code)
        {
            return await _database.Table<Product>().FirstOrDefaultAsync(p => p.Code == code);
        }

        /// <summary>
        /// Busca productos por nombre o código
        /// </summary>
        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            var term = searchTerm.ToLower();
            return await _database.Table<Product>()
                .Where(p => p.Name.ToLower().Contains(term) || p.Code.ToLower().Contains(term))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos con stock bajo
        /// </summary>
        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _database.Table<Product>()
                .Where(p => p.CurrentStock <= p.MinimumStock)
                .OrderBy(p => p.CurrentStock)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos disponibles (con stock > 0)
        /// </summary>
        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await _database.Table<Product>()
                .Where(p => p.CurrentStock > 0)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos por categoría
        /// </summary>
        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _database.Table<Product>()
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Guarda (inserta o actualiza) un producto
        /// </summary>
        public async Task<int> SaveProductAsync(Product product)
        {
            product.UpdatedAt = DateTime.Now;

            if (product.Id == 0)
            {
                product.CreatedAt = DateTime.Now;
                return await _database.InsertAsync(product);
            }
            else
            {
                return await _database.UpdateAsync(product);
            }
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        public async Task<int> DeleteProductAsync(Product product)
        {
            return await _database.DeleteAsync(product);
        }

        /// <summary>
        /// Elimina un producto por ID
        /// </summary>
        public async Task<int> DeleteProductByIdAsync(int id)
        {
            return await _database.DeleteAsync<Product>(id);
        }

        /// <summary>
        /// Actualiza el stock de un producto
        /// </summary>
        public async Task<int> UpdateStockAsync(int productId, int newStock)
        {
            var product = await GetProductByIdAsync(productId);
            if (product != null)
            {
                product.CurrentStock = newStock;
                product.UpdatedAt = DateTime.Now;
                return await _database.UpdateAsync(product);
            }
            return 0;
        }

        /// <summary>
        /// Obtiene el conteo total de productos
        /// </summary>
        public async Task<int> GetProductsCountAsync()
        {
            return await _database.Table<Product>().CountAsync();
        }

        /// <summary>
        /// Verifica si existe un producto con el código dado (excluyendo un ID específico)
        /// </summary>
        public async Task<bool> ExistsProductWithCodeAsync(string code, int excludeId = 0)
        {
            var query = _database.Table<Product>().Where(p => p.Code == code);
            
            if (excludeId > 0)
                query = query.Where(p => p.Id != excludeId);

            var product = await query.FirstOrDefaultAsync();
            return product != null;
        }
    }
}