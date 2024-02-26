using HotelBookingManagementSystem.Models;
using System.Resources;

namespace HotelBookingManagementSystem
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            LoadTheme("Red");
            //autoLogin();
        }

        private async void autoLogin()
        {
            //var result = await CommonUtilities.apiServices.ValidateLogin("YOUcANTsEEmE@wwe.COM", "JoHN@CeNA");
            //CommonUtilities.loginUser = (UserMaster)result;
            //((AppShell)App.Current.MainPage).switchTab(2);

            //CommonUtilities.isAdminActive = true;
            //((AppShell)App.Current.MainPage).switchTab(4);
        }

        public void LoadTheme(string theme)
        {
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(() => LoadTheme(theme));
                return;
            }

            ResourceDictionary dictionary = theme switch
            {
                "Dark" => new Resources.Styles.Dark(),
                "Light" => new Resources.Styles.Light(),
                "Red" => new Resources.Styles.Red(),
                "Pink" => new Resources.Styles.Pink(),
                _ => null
            };

            if (dictionary != null)
            {
                Resources.MergedDictionaries.Clear();
                Resources.MergedDictionaries.Add(dictionary);
            }
        }
    }
}