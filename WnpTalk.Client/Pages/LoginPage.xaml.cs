
namespace WnpTalk.Client.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    //public LoginPage()
    //{
    //    InitializeComponent();

    //    //this.BindingContext = viewModel;
    //}
}