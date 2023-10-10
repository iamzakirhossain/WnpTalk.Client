
namespace WnpTalk.Client
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();

            Routing.RegisterRoute("ChatRoomPage", typeof(ChatRoomPage));
            Routing.RegisterRoute("ListChatPage", typeof(ListChatPage));
            Routing.RegisterRoute("ListRoomChatPage", typeof(ListRoomChatPage));
            Routing.RegisterRoute("ChatPage", typeof(ChatPage));

            this.CurrentItem = loginPage;
        }

        //public AppShell()
        //{
        //    InitializeComponent();
        //}
    }
}