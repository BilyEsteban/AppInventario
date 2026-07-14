using System;
using AppInventario.Models;
public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public User? User { get; set; }
    public DateTime ExpiresAt { get; set; }
}