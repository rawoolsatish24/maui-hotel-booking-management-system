using HotelBookingManagementSystem.Models;

namespace HotelBookingManagementSystem;

public partial class BookingsHistory : ContentPage
{
    IEnumerable<BookingHistory> bookingsList;

    public BookingsHistory()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        checkLogin();
    }

    private async void checkLogin()
    {
        if (CommonUtilities.loginUser == null)
        {
            await Shell.Current.DisplayAlert("Invalid access", "Sorry, you cannot access this page without login...!", "OK");
            ((AppShell)App.Current.MainPage).switchTab(0);
        }
        else { loadBookings(); }
        CommonUtilities.activePageNo = 2;
    }

    private void btnRefreshData_Clicked(object sender, EventArgs e) { loadBookings(); }

    async void loadBookings()
    {
        var result = await CommonUtilities.apiServices.GetUserBookingHistory(CommonUtilities.loginUser.UserId.ToString());
        if (result == null) { await Shell.Current.DisplayAlert("Message", "Sorry, no bookings found to display...!", "Ok"); }
        else if (result is List<BookingHistory>)
        {
            bookingsList = (List<BookingHistory>)result;
            lsBookings.ItemsSource = bookingsList;
            if (bookingsList.Count() == 0) { await Shell.Current.DisplayAlert("Message", "Sorry, no bookings found to display...!", "Ok"); }
        }
        else if (((int)result) == -108) { CommonUtilities.apiServicesDown(); }
        else { await Shell.Current.DisplayAlert("Message", "Some issues while fetching bookings history...!", "Ok"); }
    }

    async void lsBookings_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        BookingHistory selectedBooking = (BookingHistory)e.Item;
        if (selectedBooking.Status == "PENDING" || selectedBooking.Status == "APPROVED")
        {
            if (await Shell.Current.DisplayAlert("Confirmation", "Do you really want to cancel booking, you can't undo this action?", "Yes", "No"))
            {
                selectedBooking.Status = "CANCELED";
                int result = await CommonUtilities.apiServices.UpdateBooking(selectedBooking);
                if (result == -108) { CommonUtilities.apiServicesDown(); }
                else if (result > 0)
                {
                    await Shell.Current.DisplayAlert("Message", "Booking canceled successfully...!\nYour full payment of booking will be refunded within 3-5 working days.", "Ok");
                    loadBookings();
                }
                else { await Shell.Current.DisplayAlert("Message", "Some issues while updating booking status...!", "Ok"); }
                loadBookings();
            }
        }
    }
}