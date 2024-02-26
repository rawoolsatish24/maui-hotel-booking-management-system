using HotelBookingManagementSystem.Models;

namespace HotelBookingManagementSystem;

public partial class AdminPage : ContentPage
{
    IEnumerable<BookingHistory> bookingsList;

    public AdminPage()
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
        if (!CommonUtilities.isAdminActive)
        {
            await Shell.Current.DisplayAlert("Invalid access", "Sorry, you cannot access this page without login...!", "OK");
            ((AppShell)App.Current.MainPage).switchTab(0);
        }
        else { loadBookings(); }
        CommonUtilities.activePageNo = 4;
    }

    private void btnRefreshData_Clicked(object sender, EventArgs e) { loadBookings(); }

    async void loadBookings()
    {
        var result = await CommonUtilities.apiServices.GetUserBookingHistory("admin");
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
        if (selectedBooking != null)
        {
            Object result = await CommonUtilities.apiServices.SearchUser(selectedBooking.UserId.ToString());
            if (result == null) { await Shell.Current.DisplayAlert("Message", "Sorry, booking user details not found...!", "Ok"); }
            else if (result is UserMaster)
            {
                UserMaster searchUser = (UserMaster)result;
                string message = "Booking user details are as below:\n";
                message += "\nFull name: " + searchUser.FullName;
                message += "\nEmail: " + searchUser.Email;
                message += "\nMobile: " + searchUser.Mobile;
                message += "\nCreated On: " + searchUser.CreatedOn.ToString("dd MMM, yyyy");
                await Shell.Current.DisplayAlert("Success", message, "OK");
            }
            else if (((int)result) == -108) { CommonUtilities.apiServicesDown(); }
            else { await Shell.Current.DisplayAlert("Message", "Some issues while getting booking user details...!", "Ok"); }
        }
        if (selectedBooking.Status == "PENDING")
        {
            string input = await Shell.Current.DisplayPromptAsync("Confirmation", "Do you really want to update status?\nRemember this can't be reversed once status updated!\n", maxLength: 1, placeholder: "Enter [A] to Approve or [R] to Reject booking");
            if (input != null)
            {
                string newStatus = (input.ToUpper() == "A") ? "APPROVED" : (input.ToUpper() == "R") ? "REJECTED" : "";
                if(newStatus != "")
                {
                    selectedBooking.Status = newStatus;
                    int result = await CommonUtilities.apiServices.UpdateBooking(selectedBooking);
                    if (result == -108) { CommonUtilities.apiServicesDown(); }
                    else if (result > 0)
                    {
                        await Shell.Current.DisplayAlert("Message", "Booking " + ((newStatus == "APPROVED") ? "approved" : "rejected") + " successfully...!", "Ok");
                        loadBookings();
                    }
                    else { await Shell.Current.DisplayAlert("Message", "Some issues while updating booking status...!", "Ok"); }
                }
            }
        }
    }
}