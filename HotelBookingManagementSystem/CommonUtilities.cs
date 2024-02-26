using HotelBookingManagementSystem.Models;
using HotelBookingManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingManagementSystem
{
    public class CommonUtilities
    {
        public static readonly IHBMSRepository apiServices = new HBMSService();
        public static UserMaster loginUser = null;
        public static int activePageNo = 0;
        public static bool isAdminActive = false;

        public static async void apiServicesDown()
        {
            await Shell.Current.DisplayAlert("Error", "Sorry, API services are down, you are been logged out...!", "OK");
            loginUser = null;
            isAdminActive = false;
            activePageNo = 0;
            ((AppShell)App.Current.MainPage).switchTab(0);
        }
    }
}
