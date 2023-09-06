using NovaApp.Models;
using NovaApp.Services;
using NovaApp.Services.Auth;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NovaApp.ViewModels
{


    public class MainPageViewModel : BaseViewModel
    {
        public AuthRestService _authRestService { get; set; }

        private UserData _userDatat;
        public UserData UserData
        {
            get { return _userDatat; }
            set
            {
                if (_userDatat != value)
                {
                    _userDatat = value;
                    OnPropertyChanged(nameof(UserData));
                }
            }
        }

        public MainPageViewModel(AuthRestService authRestService)
        {
            _authRestService = authRestService;
            UserData = new UserData();
            _ = GetLocalData();
        }

        public async Task<bool> GetLocalData()
        {
            UserData.email = await SecureStorage.GetAsync("UserEmail");
            UserData.username = await SecureStorage.GetAsync("Username");

            var profileImage = await SecureStorage.GetAsync("UserProfile");
            if (profileImage != null)
            {
                UserData.profileImage = int.Parse(profileImage);
                Debug.WriteLine("dawbdnouawd: ", UserData.profileImage.ToString());

            }

            if (UserData.email == null)
            {
                return false;
            }

            return true;
        }

        public async Task onLogin(string email, string password)
        {
            UserData.email = email;
            UserData.password = password;
            await _authRestService.OnLogin(UserData);
        }
    }
}