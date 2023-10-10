namespace WnpTalk.Client.Pages;

public partial class ChatRoomPage : ContentPage
{
    private Timer refreshTimer;
    public ChatRoomPage(ChatRoomPageViewModel viewModel)
    {
        InitializeComponent();

       this.BindingContext = viewModel;
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as ChatRoomPageViewModel).Initialize();
    }



}