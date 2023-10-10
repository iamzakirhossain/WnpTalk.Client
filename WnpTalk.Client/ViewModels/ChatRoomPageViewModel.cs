using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WnpTalk.Client.Services.ChatRoomMessage;

namespace WnpTalk.Client.ViewModels
{
    public class ChatRoomPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
            ToRoomId = int.Parse(HttpUtility.UrlDecode(query["toRoomId"].ToString()));

        }

        private ServiceProvider _serviceProvider;
        private ChatHub _chatHub;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatRoomPageViewModel(ServiceProvider serviceProvider, ChatHub chatHub)
        {
            ChatRoomMessages = new ObservableCollection<ChatRoomMessage>();
            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
            _chatHub.Connect();

            SendMessageCommand = new Command(async () =>
            {
                try
                {
                    if (ChatRoomMessage.Trim() != "")
                    {
                        await _chatHub.SendMessageToRoom(FromUserId, ToRoomId, ChatRoomMessage);

                        //ChatRoomMessages.Add(new Models.ChatRoomMessage
                        //{
                        //    Content = ChatRoomMessage,
                        //    FromUserId = 1,
                        //    ToRoomId = 13,
                        //    SendDateTime = DateTime.Now
                        //});

                        ChatRoomMessage = "";
                    }
                }
                catch (Exception ex)
                {
                   
                    await AppShell.Current.DisplayAlert("WnpTalk", ex.Message, "OK");
                }
            });
        }

        async Task GetChatRoomMessages()
        {
            var request = new ChatRoomMessageInitializeRequest
            {
                FromUserId = FromUserId,
                ToRoomId = 13,
            };

            var response = await _serviceProvider.CallWebApi<ChatRoomMessageInitializeRequest, ChatRoomMessageInitializeReponse>
                ("/ChatRoomMessage/Initialize", HttpMethod.Post, request);

            if (response.StatusCode == 200)
            {
                RoomInfo = response.RoomInfo;
                ChatRoomMessages = new ObservableCollection<ChatRoomMessage>(response.ChatRoomMessages);
            }
            else
            {
                await AppShell.Current.DisplayAlert("WnpTalk", response.StatusMessage, "OK");
            }
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetChatRoomMessages();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }

        private void OnReceiveMessage(int fromUserId, string message)
        {


            ChatRoomMessages.Add(new Models.ChatRoomMessage
            {
                Content = message,
                SendDateTime = DateTime.Now
            });

        }

        private int fromUserId;
        private int toRoomId;
        private User roomInfo;
        private ObservableCollection<ChatRoomMessage> chatRoomMessages;
        private bool isRefreshing;
        private string chatRoomMessage;

        public int FromUserId
        {
            get { return fromUserId; }
            set { fromUserId = value; OnPropertyChanged(); }
        }

        public int ToRoomId
        {
            get { return toRoomId; }
            set { toRoomId = value; OnPropertyChanged(); }
        }

        public User RoomInfo
        {
            get { return roomInfo; }
            set { roomInfo = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ChatRoomMessage> ChatRoomMessages
        {
            get { return chatRoomMessages; }
            set { chatRoomMessages = value; OnPropertyChanged(); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public string ChatRoomMessage
        {
            get { return chatRoomMessage; }
            set { chatRoomMessage = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; set; }
    }
}
