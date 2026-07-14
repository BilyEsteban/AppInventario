using SQLite;

namespace AppInventario.Models
{
    
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull, Unique]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100), NotNull]
        public string Username { get; set; } = string.Empty;

        [MaxLength(255), NotNull]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [NotNull]
        public UserRole Role { get; set; } = UserRole.User;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }

        [Ignore]
        public string DisplayName => !string.IsNullOrEmpty(FullName) ? FullName : Username;

        [Ignore]
        public bool IsAdmin => Role == UserRole.Admin;
    }
}