namespace HotelBookingManagementSystem;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CommonUtilities.activePageNo = 5;
    }
}