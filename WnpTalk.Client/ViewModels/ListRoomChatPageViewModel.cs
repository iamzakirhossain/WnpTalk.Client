
namespace WnpTalk.Client.ViewModels
{
    public class ListRoomChatPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;
        private ChatHub _chatHub;

        public ListRoomChatPageViewModel(ServiceProvider serviceProvider, ChatHub chatHub)
        {
            UserInfo = new User();
            UserRooms = new ObservableCollection<User>();
            LastestChatRoomMessages = new ObservableCollection<LastestChatRoomMessage>();
            UserFriends = new ObservableCollection<User>();
            LastestMessages = new ObservableCollection<LastestMessage>();

            RefreshCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    IsRefreshing = true;
                    await GetListRooms();
                    await GetListFriends();
                }).GetAwaiter().OnCompleted(() =>
                {
                    IsRefreshing = false;
                });
            });

            OpenChatRoomPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatRoomPage?fromUserId={UserInfo.Id}&toRoomId={param}");
            });

            OpenChatPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
            });



            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.Connect();
            _chatHub.AddReceivedMessageHandler(OnReceivedMessage);

            //MessagingCenter.Send<string>("StartService", "MessageForegroundService");
            //MessagingCenter.Send<string, string[]>("StartService", "MessageNotificationService", new string[] { });

        }

        private User userInfo;
        private ObservableCollection<User> userRooms;
        private ObservableCollection<LastestChatRoomMessage> lastestChatRoomMessages;
        private ObservableCollection<User> userFriends;
        private ObservableCollection<LastestMessage> lastestMessages;
        private bool isRefreshing;

        async Task GetListRooms()
        {
            var response = await _serviceProvider.CallWebApi<int, ListRoomChatInitializeResponse>
                ("/ListRoomChat/Initialize", HttpMethod.Post, UserInfo.Id);

            if (response.StatusCode == 200)
            {
                UserInfo = response.User;
                UserRooms = new ObservableCollection<User>(response.UserRooms);
                LastestChatRoomMessages = new ObservableCollection<LastestChatRoomMessage>(response.LastestChatRoomMessages);
            }
            else
            {
                await AppShell.Current.DisplayAlert("ChatApp", response.StatusMessage, "OK");
            }
        }

        async Task GetListFriends()
        {
            var response = await _serviceProvider.CallWebApi<int, ListChatInitializeResponse>
                ("/ListChat/Initialize", HttpMethod.Post, UserInfo.Id);

            if (response.StatusCode == 200)
            {
                UserInfo = response.User;
                UserFriends = new ObservableCollection<User>(response.UserFriends);
                LastestMessages = new ObservableCollection<LastestMessage>(response.LastestMessages);
            }
            else
            {
                await AppShell.Current.DisplayAlert("ChatApp", response.StatusMessage, "OK");
            }
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetListRooms();
                await GetListFriends();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
        }

        void OnReceivedMessage(int fromUserId, string message)
        {
    
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    var lastestChatRoomMessage = LastestChatRoomMessages.FirstOrDefault(x => x.UserRoomInfo.Id == fromUserId);
                //    if (lastestChatRoomMessage != null)
                //        LastestChatRoomMessages.Remove(lastestChatRoomMessage);

                //    var newLastestChatRoomMessage = new LastestChatRoomMessage
                //    {
                //        UserId = userInfo.Id,
                //        Content = message,
                //        UserRoomInfo = UserRooms.FirstOrDefault(x => x.Id == fromUserId)
                //    };

                //    LastestChatRoomMessages.Insert(0, newLastestChatRoomMessage);
                //    OnPropertyChanged("LastestChatRoomMessages");


                //    var lastestMessage = LastestMessages.FirstOrDefault(x => x.UserFriendInfo.Id == fromUserId);
                //    if (lastestMessage != null)
                //        LastestMessages.Remove(lastestMessage);

                //    var newLastestMessage = new LastestMessage
                //    {
                //        UserId = userInfo.Id,
                //        Content = message,
                //        UserFriendInfo = UserFriends.FirstOrDefault(x => x.Id == fromUserId)
                //    };

                //    LastestMessages.Insert(0, newLastestMessage);
                //    OnPropertyChanged("LastestMessages");

                //    //MessagingCenter.Send<string, string[]>("Notify", "MessageNotificationService",
                //    //    new string[] { newLastestChatRoomMessage.UserRoomInfo.UserName, newLastestChatRoomMessage.Content });
                //});
        


  
            
        }


        public User UserInfo
        {
            get { return userInfo; }
            set { userInfo = value; OnPropertyChanged(); }
        }
        public ObservableCollection<User> UserRooms
        {
            get { return userRooms; }
            set { userRooms = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> UserFriends
        {
            get { return userFriends; }
            set { userFriends = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LastestChatRoomMessage> LastestChatRoomMessages
        {
            get { return lastestChatRoomMessages; }
            set { lastestChatRoomMessages = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LastestMessage> LastestMessages
        {
            get { return lastestMessages; }
            set { lastestMessages = value; OnPropertyChanged(); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; set; }

        public ICommand OpenChatRoomPageCommand { get; set; }
        public ICommand OpenChatPageCommand { get; set; }
    }
}
