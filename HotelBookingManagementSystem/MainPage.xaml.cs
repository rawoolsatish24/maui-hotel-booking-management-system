using HotelBookingManagementSystem.Models;

namespace HotelBookingManagementSystem
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            resetForms();
            lblStatus.Text = "";
            CommonUtilities.activePageNo = 0;
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Shell.Current.DisplayAlert("Forgot Password", "Please contact the admin to reset your password...!", "OK");
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            lblStatus.Text = "Please wait, validating credentials...!";
            dataProcessing(true);

            string email = txtLEmailId.Text;
            string password = txtLPassword.Text;

            if (string.IsNullOrEmpty(email.Trim()) || string.IsNullOrEmpty(password.Trim()))
            {
                lblStatus.Text = "All are required fields...!";
            }
            else
            {
                CommonUtilities.isAdminActive = false;
                if (email.ToLower() == "admin@gmail.com" && password.ToLower() == "admin@123")
                {
                    CommonUtilities.isAdminActive = true;
                    lblStatus.Text = "Login successfull, Welcome Admin...!";
                    resetForms();
                    await Shell.Current.DisplayAlert("Success", "Login successfull, Welcome Admin...!", "OK");
                    ((AppShell)App.Current.MainPage).switchTab(4);
                }
                else
                {
                    Object result = await CommonUtilities.apiServices.ValidateLogin(email, password);
                    if (result == null) { lblStatus.Text = "Login failed, invalid credentials or user account not found...!"; }
                    else if (result is UserMaster)
                    {
                        CommonUtilities.loginUser = (UserMaster)result;
                        lblStatus.Text = "Login successfull, Welcome " + CommonUtilities.loginUser.FullName + "...!";
                        resetForms();
                        await Shell.Current.DisplayAlert("Success", "Login successfull, Welcome " + CommonUtilities.loginUser.FullName + "...!", "OK");
                        ((AppShell)App.Current.MainPage).switchTab(1);
                    }
                    else if (((int)result) == -108)
                    {
                        lblStatus.Text = "API services are inactive...!";
                        CommonUtilities.apiServicesDown();
                    }
                    else { lblStatus.Text = "Some issues while trying to login...!"; }
                }
            }

            dataProcessing(false);
        }

        private async void btnSignup_Clicked(object sender, EventArgs e)
        {
            lblStatus.Text = "Please wait, validating new account details...!";
            dataProcessing(true);

            string fullName = txtSFullName.Text;
            string email = txtSEmailId.Text;
            string mobile = txtSMobile.Text;
            string password = txtSPassword.Text;
            string confirmPassword = txtSCPassword.Text;

            if (string.IsNullOrEmpty(fullName.Trim()) || string.IsNullOrEmpty(email.Trim()) || string.IsNullOrEmpty(mobile.Trim()) || string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(confirmPassword.Trim()))
            {
                lblStatus.Text = "All are required fields...!";
            }
            else
            {
                if (password != confirmPassword) { lblStatus.Text = "Password and confirm password didn't matched...!"; }
                else
                {
                    int result = await CommonUtilities.apiServices.CreateUser(new UserMaster
                    {
                        FullName = fullName,
                        Email = email,
                        Mobile = mobile,
                        Password = password,
                    });
                    if (result == 0) { lblStatus.Text = "User account with same email already exists...!"; }
                    else if (result == -1) { lblStatus.Text = "Some issues while creating new user account...!"; }
                    else if (result == -108)
                    {
                        lblStatus.Text = "API services are inactive...!";
                        CommonUtilities.apiServicesDown();
                    }
                    else
                    {
                        lblStatus.Text = "New account created successfully...!";
                        resetForms();
                    }
                }
            }

            dataProcessing(false);
        }

        private void resetForms()
        {
            txtLEmailId.Text = "";
            txtLPassword.Text = "";

            txtSFullName.Text = "";
            txtSEmailId.Text = "";
            txtSMobile.Text = "";
            txtSPassword.Text = "";
            txtSCPassword.Text = "";

            dataProcessing(false);
        }

        private void dataProcessing(bool processing)
        {
            txtLEmailId.IsEnabled = !processing;
            txtLPassword.IsEnabled = !processing;
            btnLogin.IsEnabled = !processing;
            txtSFullName.IsEnabled = !processing;
            txtSEmailId.IsEnabled = !processing;
            txtSMobile.IsEnabled = !processing;
            txtSPassword.IsEnabled = !processing;
            txtSCPassword.IsEnabled = !processing;
            btnSignup.IsEnabled = !processing;
            lblForgotPassword.IsEnabled = !processing;
        }
    }
}