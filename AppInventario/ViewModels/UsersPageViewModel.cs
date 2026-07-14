using AppInventario.Models;
using System.Collections.ObjectModel;

namespace AppInventario.ViewsModels
{
    public class UsersPageViewModel
    {
        private readonly IAuthService _authService;

        public UsersPageViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public async Task LoadUsersAsync()
        {
            var users = await _authService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}
