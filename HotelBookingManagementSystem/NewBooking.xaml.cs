using HotelBookingManagementSystem.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace HotelBookingManagementSystem;

public partial class NewBooking : ContentPage
{
    int netAmount = 0;

    public NewBooking()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblStatus.Text = "";
        checkLogin();
    }

    private async void checkLogin()
    {
        if (CommonUtilities.loginUser == null)
        {
            await Shell.Current.DisplayAlert("Invalid access", "Sorry, you cannot access this page without login...!", "OK");
            ((AppShell)App.Current.MainPage).switchTab(0);
        }
        else { resetForms(); }
        CommonUtilities.activePageNo = 1;
    }

    private void pkrTypeOfRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
        calculateNetAmount();
    }

    private void resetForms()
    {
        netAmount = 0;
        pkrTypeOfRoom.SelectedIndex = 0;
        pkrBeddingType.SelectedIndex = 0;
        txtNoOfAdults.Text = "";
        txtNoOfChilds.Text = "";

        dtpCheckIn.Date = DateTime.Now;
        dtpCheckIn.MinimumDate = dtpCheckIn.Date;
        dtpCheckIn.MaximumDate = dtpCheckIn.Date.AddMonths(1);

        dtpCheckOut.Date = dtpCheckIn.Date.AddDays(1);
        dtpCheckOut.MinimumDate = dtpCheckOut.Date;
        dtpCheckOut.MaximumDate = dtpCheckOut.Date.AddMonths(1);

        lblNetAmount.Text = "Net Amount: -NILL";
    }

    private void pkrBeddingType_SelectedIndexChanged(object sender, EventArgs e)
    {
        calculateNetAmount();
    }

    private void btnResetForm_Clicked(object sender, EventArgs e)
    {
        resetForms();
        lblStatus.Text = "";
    }

    private async void btnConfirmBooking_Clicked(object sender, EventArgs e)
    {
        lblStatus.Text = "Please wait, validating booking details...!";

        string typeOfRoom = pkrTypeOfRoom.SelectedItem.ToString();
        string beddingType = pkrBeddingType.SelectedItem.ToString();
        DateTime checkIn = dtpCheckIn.Date;
        DateTime checkOut = dtpCheckOut.Date;
        if (pkrBeddingType.SelectedIndex == 0 || pkrTypeOfRoom.SelectedIndex == 0 || string.IsNullOrEmpty(txtNoOfAdults.Text.Trim()) || string.IsNullOrEmpty(txtNoOfChilds.Text.Trim())) { lblStatus.Text = "All are required fields...!"; }
        else
        {
            try
            {
                int noOfAdults = int.Parse(txtNoOfAdults.Text.Trim());
                int noOfChilds = int.Parse(txtNoOfChilds.Text.Trim());
                if(checkOut > checkIn)
                {
                    if(noOfAdults > 0 && noOfAdults <= 10)
                    {
                        if (noOfChilds >= 0 && noOfChilds <= 10)
                        {
                            string bookingDetailsConfirmation = "Please confirm your booking with below details and complete the payment.";
                            bookingDetailsConfirmation += "\n\nRoom Type: " + typeOfRoom;
                            bookingDetailsConfirmation += "\nBedding Type: " + beddingType;
                            bookingDetailsConfirmation += "\nNo. Of Persons: " + noOfAdults.ToString();
                            if (noOfChilds > 0) { bookingDetailsConfirmation += " (" + noOfChilds.ToString() + " childs)"; }
                            bookingDetailsConfirmation += "\nDuration: " + checkIn.ToString("dd MMM, yyyy") + " - " + checkOut.ToString("dd MMM, yyyy");
                            bookingDetailsConfirmation += "\n\nNet Amount: $" + netAmount.ToString();
                            if (await Shell.Current.DisplayAlert("Confirmation", bookingDetailsConfirmation, "Pay & Confirm", "Cancel"))
                            { makeBooking(typeOfRoom, beddingType, noOfAdults, noOfChilds, checkIn, checkOut, netAmount); }
                        } else { lblStatus.Text = "No. of childs must be positive and max 10 are allowed...!"; }
                    } else { lblStatus.Text = "No. of adults must be positive and atleast 1 and max 10 are allowed...!"; }
                } else { lblStatus.Text = "Check-out date must be greater than check-in date...!"; }
            }
            catch { lblStatus.Text = "No. of adults and no. of childs must be positive number...!"; }
        }
    }

    private async void makeBooking(String typeOfRoom, String beddingType, int noOfAdults, int noOfChilds, DateTime checkIn, DateTime checkOut, int netAmount)
    {
        int result = await CommonUtilities.apiServices.AddBooking(new BookingHistory
        {
            UserId = CommonUtilities.loginUser.UserId,
            RoomType = typeOfRoom,
            BeddingType = beddingType,
            NoOfAdults = noOfAdults,
            NoOfChilds = noOfChilds,
            CheckIn = checkIn,
            CheckOut = checkOut,
            NetAmount = netAmount,
        });
        if (result == -1) { lblStatus.Text = "Some issues while making your new booking...!"; }
        else if (result == -108)
        {
            lblStatus.Text = "API services are inactive...!";
            CommonUtilities.apiServicesDown();
        }
        else
        {
            await Shell.Current.DisplayAlert("Success", "Congratulations, payment done and your booking has been confirmed...!", "OK");
            lblStatus.Text = "Booking confirmed, keep checking bookings history for status...!";
            resetForms();
        }
    }

    private void dtpCheckIn_DateSelected(object sender, DateChangedEventArgs e)
    {
        dtpCheckOut.Date = dtpCheckIn.Date.AddDays(1);
        dtpCheckOut.MinimumDate = dtpCheckOut.Date;
        dtpCheckOut.MaximumDate = dtpCheckOut.Date.AddMonths(1);
    }

    private void calculateNetAmount()
    {
        lblNetAmount.Text = "Net Amount: -NILL";

        DateTime checkIn = dtpCheckIn.Date;
        DateTime checkOut = dtpCheckOut.Date;
        if (!(pkrBeddingType.SelectedIndex == 0 || pkrTypeOfRoom.SelectedIndex == 0 || string.IsNullOrEmpty(txtNoOfAdults.Text.Trim()) || string.IsNullOrEmpty(txtNoOfChilds.Text.Trim())))
        {
            try
            {
                int noOfAdults = int.Parse(txtNoOfAdults.Text.Trim());
                int noOfChilds = int.Parse(txtNoOfChilds.Text.Trim());
                int noOfDaysStay = (checkOut - checkIn).Days;
                if (noOfAdults > 0 && noOfChilds >= 0 && checkOut > checkIn && noOfDaysStay > 0)
                {
                    double roomTypePersons = double.Parse(pkrTypeOfRoom.SelectedItem.ToString().Split(" : ")[2].Replace("Persons", "").Trim());
                    int noOfRequiredRooms = (int)Math.Ceiling((noOfAdults + noOfChilds) / roomTypePersons);
                    if (noOfRequiredRooms == 0) { noOfRequiredRooms = 1; }

                    double roomTypePrice = double.Parse(pkrTypeOfRoom.SelectedItem.ToString().Split(" : ")[1].Replace("$", "").Trim());
                    double beddingTypeRatio = double.Parse(pkrBeddingType.SelectedItem.ToString().Split(" : ")[1].Replace("x", "").Trim());
                    double costPerDay = (1 + (beddingTypeRatio / 100)) * roomTypePrice;
                    netAmount = (int)Math.Ceiling(costPerDay * noOfDaysStay * noOfRequiredRooms);

                    lblNetAmount.Text = "Net Amount: $" + netAmount.ToString();
                }
            } catch {}
        }
    }

    private void txtNoOfAdults_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (txtNoOfAdults.Text == "") { return; }
            txtNoOfAdults.Text = String.Join("", Regex.Matches(txtNoOfAdults.Text, @"\d+"));
            int noOfAdults = int.Parse(txtNoOfAdults.Text);
            if (noOfAdults < 1) { txtNoOfAdults.Text = "1"; }
            else if (noOfAdults > 10) { txtNoOfAdults.Text = "10"; }
        }
        catch { txtNoOfAdults.Text = ""; }
        calculateNetAmount();
    }

    private void txtNoOfChilds_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (txtNoOfChilds.Text == "") { return; }
            txtNoOfChilds.Text = String.Join("", Regex.Matches(txtNoOfChilds.Text, @"\d+"));
            int noOfChilds = int.Parse(txtNoOfChilds.Text);
            if (noOfChilds < 0) { txtNoOfChilds.Text = "0"; }
            else if (noOfChilds > 10) { txtNoOfChilds.Text = "10"; }
        }
        catch { txtNoOfChilds.Text = ""; }
        calculateNetAmount();
    }

    private void dtpCheckOut_DateSelected(object sender, DateChangedEventArgs e)
    {
        calculateNetAmount();
    }
}