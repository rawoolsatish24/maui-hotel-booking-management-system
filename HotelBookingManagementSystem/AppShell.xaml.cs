namespace HotelBookingManagementSystem
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            updateAppTabBar();
        }

        public void updateAppTabBar()
        {
            bool adminVisibility = CommonUtilities.isAdminActive;
            if (adminVisibility)
            {
                tabMainPage.IsVisible = !adminVisibility;
                tabNewBooking.IsVisible = !adminVisibility;
                tabBookingsHistory.IsVisible = !adminVisibility;
                tabMenuPage.IsVisible = !adminVisibility;
                tabAdminPage.IsVisible = adminVisibility;
                tabLogoutPage.IsVisible = adminVisibility;
            }
            else
            {
                bool userVisibility = (CommonUtilities.loginUser != null);

                tabMainPage.IsVisible = !userVisibility;
                tabNewBooking.IsVisible = userVisibility;
                tabBookingsHistory.IsVisible = userVisibility;
                tabMenuPage.IsVisible = userVisibility;
                tabAdminPage.IsVisible = adminVisibility;
                tabLogoutPage.IsVisible = userVisibility;
            }
        }

        public void switchTab(int index)
        {
            updateAppTabBar();
            switch (index)
            {
                case 0: appTabBar.CurrentItem = tabMainPage; break;
                case 1: appTabBar.CurrentItem = tabNewBooking; break;
                case 2: appTabBar.CurrentItem = tabBookingsHistory; break;
                case 3: appTabBar.CurrentItem = tabMenuPage; break;
                case 4: appTabBar.CurrentItem = tabAdminPage; break;
                case 5: appTabBar.CurrentItem = tabAboutPage; break;
                case 6: appTabBar.CurrentItem = tabLogoutPage; break;
            };
        }
    }
}