using HotelBookingManagementSystem.Models;

namespace HotelBookingManagementSystem;

public partial class MenuPage : ContentPage
{
    public MenuPage()
    {
        InitializeComponent();
        ColorSchemePicker.SelectedItem = "Red";
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
        CommonUtilities.activePageNo = 3;
    }

    private void OnColorSchemeSelected(object sender, EventArgs e)
    {
        var selectedScheme = ColorSchemePicker.SelectedItem.ToString();
        if (Application.Current is App app)
        {
            app.LoadTheme(selectedScheme);
        }
    }

    private async void btnPUpdate_Clicked(object sender, EventArgs e)
    {
        lblStatus.Text = "Please wait, validating new password details...!";
        dataProcessing(true);

        string oldPassword = txtPOPassword.Text.ToUpper();
        string newPassword = txtPPassword.Text.ToUpper();
        string confirmPassword = txtPCPassword.Text.ToUpper();

        if (string.IsNullOrEmpty(oldPassword.Trim()) || string.IsNullOrEmpty(newPassword.Trim()) || string.IsNullOrEmpty(confirmPassword.Trim())) { lblStatus.Text = "All are required fields...!"; }
        else
        {
            if (oldPassword != CommonUtilities.loginUser.Password) { lblStatus.Text = "Incorrect old password...!"; }
            else if (oldPassword == newPassword) { lblStatus.Text = "New password and old password cannot be same...!"; }
            else if (confirmPassword != newPassword) { lblStatus.Text = "New password and confirm password must be same...!"; }
            else
            {
                await updateUser(new UserMaster
                {
                    UserId = CommonUtilities.loginUser.UserId,
                    FullName = CommonUtilities.loginUser.FullName,
                    Email = CommonUtilities.loginUser.Email,
                    Mobile = CommonUtilities.loginUser.Mobile,
                    Password = newPassword,
                    CreatedOn = CommonUtilities.loginUser.CreatedOn
                });
            }
        }

        dataProcessing(false);
    }

    private async void btnAUpdate_Clicked(object sender, EventArgs e)
    {
        lblStatus.Text = "Please wait, validating new account details...!";
        dataProcessing(true);

        string fullName = txtAFullName.Text;
        string email = txtAEmailId.Text;
        string mobile = txtAMobile.Text;

        if (string.IsNullOrEmpty(fullName.Trim()) || string.IsNullOrEmpty(email.Trim()) || string.IsNullOrEmpty(mobile.Trim())) { lblStatus.Text = "All are required fields...!"; }
        else
        {
            await updateUser(new UserMaster
            {
                UserId = CommonUtilities.loginUser.UserId,
                FullName = fullName,
                Email = email,
                Mobile = mobile,
                Password = CommonUtilities.loginUser.Password,
                CreatedOn = CommonUtilities.loginUser.CreatedOn
            });
        }

        dataProcessing(false);
    }

    private async Task updateUser(UserMaster updateUserMaster)
    {
        Object result = await CommonUtilities.apiServices.UpdateUser(updateUserMaster);
        if (result != null)
        {
            if (result is UserMaster)
            {
                CommonUtilities.loginUser = result as UserMaster;
                lblStatus.Text = "Account details updated successfully...!";
                resetForms();
            }
            else if (((int)result) == -108)
            {
                lblStatus.Text = "API services are inactive...!";
                CommonUtilities.apiServicesDown();
            }
            else { lblStatus.Text = "Some issues while updating details...!"; }
        } else { lblStatus.Text = "Sorry, new email already exists...!"; }
    }

    private void resetForms()
    {
        lblAccountCreatedOn.Text = "   |   Account Created On: " + CommonUtilities.loginUser.CreatedOn.ToString("dd MMM, yyyy");

        txtAEmailId.Text = CommonUtilities.loginUser.Email;
        txtAFullName.Text = CommonUtilities.loginUser.FullName;
        txtAMobile.Text = CommonUtilities.loginUser.Mobile;
        txtPOPassword.Text = "";
        txtPPassword.Text = "";
        txtPCPassword.Text = "";

        dataProcessing(false);
    }

    private void dataProcessing(bool processing)
    {
        txtAFullName.IsEnabled = !processing;
        txtAEmailId.IsEnabled = !processing;
        txtAMobile.IsEnabled = !processing;
        btnAUpdate.IsEnabled = !processing;
        txtPOPassword.IsEnabled = !processing;
        txtPPassword.IsEnabled = !processing;
        txtPCPassword.IsEnabled = !processing;
        btnPUpdate.IsEnabled = !processing;
    }

    private async void btnDeleteAccount_Clicked(object sender, EventArgs e)
    {
        string result = await Shell.Current.DisplayPromptAsync("Confirmation", "Do you really want to delete your account permanently?\n\nRemember all of your booking details will be deleted too and you cannot recover that in future!\n", maxLength: 20, placeholder: "Enter your password");
        if (result != null)
        {
            if (result.ToUpper() == CommonUtilities.loginUser.Password)
            {
                int respose = await CommonUtilities.apiServices.DeleteUser(CommonUtilities.loginUser.UserId.ToString());
                if (respose == -108) {
                    lblStatus.Text = "API services are inactive...!";
                    CommonUtilities.apiServicesDown();
                }
                else if (respose == 1)
                {
                    lblStatus.Text = "Account deleted permanently...!";
                    await Shell.Current.DisplayAlert("Sad to see you go", "Your account has been deleted permanently...!", "OK");
                    CommonUtilities.loginUser = null;
                    ((AppShell)App.Current.MainPage).switchTab(0);
                }
                else { lblStatus.Text = "Some issues while deleting account...!"; }
            } else { await Shell.Current.DisplayAlert("Action failed", "You have entered incorrect password...!", "OK"); }
        }
    }
}