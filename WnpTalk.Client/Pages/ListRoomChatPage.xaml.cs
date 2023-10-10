namespace WnpTalk.Client.Pages;

public partial class ListRoomChatPage : ContentPage
{
    public ListRoomChatPage(ListRoomChatPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    //public ListChatPage()
    //{
    //    InitializeComponent();
    //}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as ListRoomChatPageViewModel).Initialize();
    }
}