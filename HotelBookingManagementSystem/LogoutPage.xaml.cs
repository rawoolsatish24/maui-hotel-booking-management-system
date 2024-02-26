namespace HotelBookingManagementSystem;

public partial class LogoutPage : ContentPage
{
    public LogoutPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        confirmAction();
    }

    private async void confirmAction()
	{
        if (await Shell.Current.DisplayAlert("Confirmation", "Do you really want to logout?", "Yes", "No"))
        {
            CommonUtilities.activePageNo = 0;
            CommonUtilities.loginUser = null;
            CommonUtilities.isAdminActive = false;
            ((AppShell)App.Current.MainPage).switchTab(0);
        } else { ((AppShell)App.Current.MainPage).switchTab(CommonUtilities.activePageNo); }
    }
}