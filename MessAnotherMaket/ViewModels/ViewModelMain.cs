using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity;
using MessAnotherMaket.Models;
using Microsoft.Win32;
using System.Windows;
using System.IO;

namespace MessAnotherMaket
{
    public class ViewModelMain : ViewModel
    {
        #region Properties
        public int id { get; set; }

        private bool flag;

        private string _login;
        public string Login
        {
            get { return _login; }
            set => Set(ref _login, value);
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set => Set(ref _email, value);

        }
        private byte[] _image = null;
        public byte[] Image
        {
            get { return _image; }
            set => Set(ref _image, value);
        }

        public string LastSearchTxt { get; set; }
        private string _TxtEntered;
        public string TxtEntered
        {
            get { return _TxtEntered; }
            set
            {
                _TxtEntered = value;
                OnPropertyChanged("TxtEntered");
                Search();
            }
        }

        public string LastSeartchMessageText { get; set; }
        private string _SearchMessageText;
        public string SearchMessageText
        {
            get { return _SearchMessageText; }
            set
            {
               _SearchMessageText = value;
                OnPropertyChanged("SearchMessageText");
                SearchMessage();
            }
        }

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set=>Set(ref _messageText, value);
        }

        private bool _isSearchBoxOpen = false;
        public bool IsSearchBoxOpen
        {
            get { return _isSearchBoxOpen; }
            set
            {
                _isSearchBoxOpen = value;
                if (_isSearchBoxOpen == false)
                {
                    TxtEntered = string.Empty;
                }
                OnPropertyChanged("IsSearchBoxOpen");
            }
        }
        private bool _isMessageSearchBoxOpen = false;
        public bool IsMessageSearchBoxOpen
        {
            get { return _isMessageSearchBoxOpen; }
            set
            {
                _isMessageSearchBoxOpen = value;
                if (_isMessageSearchBoxOpen == false)
                {
                    SearchMessageText = string.Empty;
                }
                OnPropertyChanged("IsMessageSearchBoxOpen");
            }
        }
        private bool _isContactInfoOpen = false;
        public bool IsContactInfoOpen
        {
            get { return _isContactInfoOpen;}
            set =>Set(ref _isContactInfoOpen, value);
        }


        private bool _focusMessageBox;
        public bool FocusMessageBox
        {
            get { return _focusMessageBox; }
            set => Set(ref _focusMessageBox, value);
        }
        private bool _isThisReplyMessage;
        public bool IsThisReplyMessage
        {
            get { return _isThisReplyMessage; }
            set=>Set(ref _isThisReplyMessage, value);
        }
        private string _messageToReplyText=string.Empty;
        public string MessageToReplyText
        {
            get { return _messageToReplyText; }
            set=>Set(ref _messageToReplyText, value);
        }
        private Users userselected;
        public Users UserSelected
        {
            get { return userselected; }
            set => Set(ref userselected, value);
        }

        private List<Users> _usersList;
        public List<Users> UsersList
        {
            get { return _usersList; }
            set => Set(ref _usersList, value);
        }

        private List<Razgovor> _razgovorList;
        public List<Razgovor> RazgovorList
        {
            get { return _razgovorList; }
            set=>Set(ref _razgovorList, value);
        }

        private List<Participants> _participantsList;
        public List<Participants> ParticipantsList
        {
            get { return _participantsList; }
            set=>Set(ref _participantsList, value);
        }

        private ObservableCollection<Messages> _messagesList;
        public ObservableCollection<Messages> MessagesList
        {
            get { return _messagesList; }
            set=>Set(ref _messagesList, value);
        }

        //Все сообщения
        private ObservableCollection<MessagesListMy> _messagesListMy;
        public ObservableCollection<MessagesListMy> MessagesListMy
        {
            get { return _messagesListMy; }
            set => Set(ref _messagesListMy, value);
        }

        //Сообщения на выбранном разговоре
        private ObservableCollection<MessagesListMy> _messagesListNow;
        public ObservableCollection<MessagesListMy> MessagesListNow
        {
            get { return _messagesListNow; }
            set => Set(ref _messagesListNow, value);
        }

        private List<ConservationsMy> _chats;
        public List<ConservationsMy> Chats
        {
            get { return _chats; }
            set => Set(ref _chats, value);
            
        }
        private ObservableCollection<Razgovor> _razgovor;
        public ObservableCollection<Razgovor> Razgovor
        {
            get { return _razgovor; }
            set => Set(ref _razgovor, value);

        }
        //Закрепленные чаты
        private ObservableCollection<ConservationsMy> _pinnedConservations;
        public ObservableCollection<ConservationsMy> PinnedConservations
        {
            get { return _pinnedConservations; }
            set => Set(ref _pinnedConservations, value);
            
        }
        private ObservableCollection<ConservationsMy> _filteredConservations;
        public ObservableCollection<ConservationsMy> FilteredConservations
        {
            get { return _filteredConservations; }
            set => Set(ref _filteredConservations, value);

        }
        private ObservableCollection<ConservationsMy> _conservationsMies;
        public ObservableCollection<ConservationsMy > ConservationsMies
        {
            get { return _conservationsMies; }
            set=>Set(ref _conservationsMies, value);
        }
        public ObservableCollection<ConservationsMy> FilteredPinnedConservations { get; set; }
        private ObservableCollection<ConservationsMy> _archivedConservations;
        public ObservableCollection<ConservationsMy> ArchivedConservations
        {
            get { return _archivedConservations; }
            set =>Set(ref _archivedConservations, value);
        }
        private ConservationsMy selectedCons;
        public ConservationsMy SelectedCons
        {
            get { return selectedCons; }
            set
            {
                //BooksViewModel book=value;
                selectedCons = value;

                if (selectedCons != null)
                {
                    MessagesListMy.Clear();
                    MessagesListNow.Clear();
                    SearchMessageText = "";
                    Login = selectedCons.Login;
                    Image = selectedCons.Image;
                    bool flag;
                    id=selectedCons.Id;
                    using (MessengEntities messeng = new MessengEntities()) {
                        foreach (Messages item in messeng.Messages)
                        {
                            if (item.Razgovor_id == id)
                            {
                                flag = false;
                                if (item.Sender_id == LoginMod.IdUserNow)
                                {
                                      flag = true;
                                }
                                var Messages = new MessagesListMy
                                {
                                    Id=item.Id,
                                    ImagePho=null,
                                    Message = item.Message,
                                    Created_at=item.Created_at,
                                    Deleted_at=item.Deleted_at,
                                    Razgovor_id=item.Razgovor_id,
                                    Sender_id=item.Sender_id,
                                    Type_id=item.Type_id,
                                    IsSender=flag
                                };
                                MessagesListMy.Add(Messages);
                                MessagesListNow.Add(Messages);
                            }
                        }
                    }
                }
                OnPropertyChanged("SelectedCons");
            }
        }
        #endregion
        #region Methods
        
        public void Search()
        {
            if ((string.IsNullOrEmpty(TxtEntered) && string.IsNullOrEmpty(LastSearchTxt)) || string.Equals(LastSearchTxt, TxtEntered))
            {
                return;
            }
            if(string.IsNullOrEmpty(TxtEntered) || ConservationsMies==null || ConservationsMies.Count <= 0)
            {
                FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies?? Enumerable.Empty<ConservationsMy>());
                OnPropertyChanged("FilteredConservations");
                FilteredPinnedConservations = new ObservableCollection<ConservationsMy>(PinnedConservations ?? Enumerable.Empty<ConservationsMy>());
                OnPropertyChanged("FilteredPinnedConservations");

                LastSearchTxt = TxtEntered;
                return;
            }
            FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies.Where(chat => chat.Login.ToLower().Contains(TxtEntered)  ));
           OnPropertyChanged("FilteredConservations");
            FilteredPinnedConservations = new ObservableCollection<ConservationsMy>(PinnedConservations.Where(pinnedchat => pinnedchat.Login.ToLower().Contains(TxtEntered) ));
            OnPropertyChanged("FilteredPinnedConservations");
            LastSearchTxt=TxtEntered;
        }

        public void SearchMessage()
        {
            if ((string.IsNullOrEmpty(SearchMessageText) && string.IsNullOrEmpty(LastSeartchMessageText)) || string.Equals(LastSeartchMessageText, SearchMessageText))
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchMessageText) || ConservationsMies == null || ConservationsMies.Count <= 0)
            {
                MessagesListNow = new ObservableCollection<MessagesListMy>(MessagesListMy ?? Enumerable.Empty<MessagesListMy>());
                //OnPropertyChanged("FilteredConservations");
                LastSeartchMessageText = SearchMessageText;
                return;
            }
            MessagesListNow = new ObservableCollection<MessagesListMy>(MessagesListMy.Where(chat => chat.Message.ToLower().Contains(SearchMessageText)));
            //OnPropertyChanged("FilteredConservations");
           
            LastSeartchMessageText = SearchMessageText;
        }
        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageText))
            {
                int count = MessagesListNow.Count;

                MessagesListMy cons = new MessagesListMy
                {
                    Id = ++count,
                    ImagePho=null,
                    Message = MessageText,
                    Created_at = DateTime.Now,
                    Deleted_at = null,
                    Sender_id = LoginMod.IdUserNow,
                    Type_id=null,
                    IsSender=true
                    
                };
                MessagesListNow.Add(cons);
                MessagesListMy.Add(cons);
                MessageText=string.Empty;
                IsThisReplyMessage = false;
                MessageToReplyText=string.Empty;
                //Добавить сюда добавление в БД
            }
        }
        #endregion
        #region Constructor
        public ViewModelMain()
        {
            
            FilteredPinnedConservations = new ObservableCollection<ConservationsMy>();
            PinnedConservations = new ObservableCollection<ConservationsMy>();
            ConservationsMies = new ObservableCollection<ConservationsMy>();
            ArchivedConservations = new ObservableCollection<ConservationsMy>();
            MessagesListMy = new ObservableCollection<MessagesListMy>();
            MessagesListNow = new ObservableCollection<MessagesListMy>();
            using (MessengEntities messeng=new MessengEntities())
            {
                //var conserv = messeng.Razgovor.Include(c => c.Participants.Select(p => p.Users)).ToList();
                //для всех
                
                //Razgovor = new ObservableCollection<Razgovor>(messeng.Razgovor);
                RazgovorList = new List<Razgovor>();
                MessagesList = new ObservableCollection<Messages>();
                ParticipantsList = new List<Participants>();
                
                UsersList = new List<Users>(messeng.Users);
               
                var conserv = messeng.Razgovor.Include(c => c.Participants.Select(p => p.Users)).Where(c => c.Participants.Any(p => p.Users.Id == LoginMod.IdUserNow)).ToList();
                foreach (var item in conserv)
                {
                    var participants = item.Participants.FirstOrDefault(p => p.Users.Id != LoginMod.IdUserNow);
                    var lastMessage = messeng.Messages.Where(m => m.Razgovor_id == item.Id).OrderByDescending(m => m.Id).FirstOrDefault();
                    string lastMessageText = "Отправьте сообщение";
                    Nullable<System.DateTime> dateTime = null;
                    if (lastMessage != null)
                    {
                        lastMessageText = lastMessage.Message;
                        dateTime = lastMessage.Created_at;
                    }
                    var conservation = new ConservationsMy
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Created_at = item.Created_at,
                        Deleted_at = item.Deleted_at,
                        Creator_id = item.Creator_id,
                        lastMessageId = item.lastMessageId,
                        Login = participants.Users.Login,
                        Image = participants.Users.Image,
                        LastMessage = lastMessageText,
                        DateOfLastMessage=dateTime,
                        Is_Pinned = item.Is_Pinned,
                        Is_Archived = item.Is_Archived
                    };
                    ConservationsMies.Add(conservation);
                }
                Image=ConservationsMies[0].Image;
                // var conservation = messeng.Database.SqlQuery<Razgovor>("Select * from Razgovor ");  
                foreach (Participants participants in messeng.Participants)
                {
                    
                    if (participants.Razgovor.Id == participants.Conseravtion_id)
                    {
                        ParticipantsList.Add(participants);
                    }
                }
                foreach (Razgovor razgovor in messeng.Razgovor)
                {
                    foreach(Participants participants in ParticipantsList)
                    {
                        if (participants.Users_id == LoginMod.IdUserNow && razgovor.Id==participants.Users_id)
                        {
                            RazgovorList.Add(razgovor);
                        }
                    }
                }
                foreach (Messages item in messeng.Messages)
                {
                    if (item.Razgovor_id == id)
                    {
                        MessagesList.Add(item);
                        flag = false;
                        if (item.Sender_id == LoginMod.IdUserNow)
                        {
                            flag = true;
                        }
                        var Messages = new MessagesListMy
                        {
                            Id = item.Id,
                            Message = item.Message,
                            Created_at = item.Created_at,
                            Deleted_at = item.Deleted_at,
                            Razgovor_id = item.Razgovor_id,
                            Sender_id = item.Sender_id,
                            Type_id = item.Type_id,
                            IsSender = flag
                        };
                        MessagesListMy.Add(Messages);
                    }
                }

                foreach (Participants participants in messeng.Participants)
                {
                    foreach (Razgovor razgovors in RazgovorList)
                    {
                        if (participants.Conseravtion_id==razgovors.Id)
                        {
                            ParticipantsList.Add(participants);
                        }
                    }
                }

            }
            
            FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies.OrderByDescending(x => x.DateOfLastMessage));
        }
        #endregion
        #region Commands
        private RelayCommand _openContactInfo;

        public RelayCommand OpenContactInfo
        {
            get
            {
                return _openContactInfo ?? (_openContactInfo = new RelayCommand(obj =>
                {
                    if (IsContactInfoOpen == false)
                    {
                        IsContactInfoOpen = true;
                    }
                    else
                    {
                        IsContactInfoOpen = false;
                    }
                }));
            }
        }
        private RelayCommand _closeContactInfo;

        public RelayCommand CloseContactInfo
        {
            get
            {
                return _closeContactInfo ?? (_closeContactInfo = new RelayCommand(obj =>
                {
                     IsContactInfoOpen = false;
                }));
            }
        }

        private RelayCommand _openSearchCommand;

        public RelayCommand OpenSearchCommand
        {
            get
            {
                return _openSearchCommand ?? (_openSearchCommand = new RelayCommand(obj =>
                {
                    if (IsSearchBoxOpen == false)
                    {
                        IsSearchBoxOpen = true;
                    }
                    else
                    {
                        IsSearchBoxOpen = false;
                    }
                }));
            }
        }
        private RelayCommand _clearSearchCommand;

        public RelayCommand ClearSearchCommand
        {
            get
            {
                return _clearSearchCommand ?? (_clearSearchCommand = new RelayCommand(obj =>
                {
                    if (!string.IsNullOrEmpty(TxtEntered)) 
                    {
                        TxtEntered = "";
                    }
                    else
                    {
                        IsSearchBoxOpen=false;
                    }
                }));
            }
        }

        private RelayCommand _openMessageSearchCommand;

        public RelayCommand OpenMessageSearchCommand
        {
            get
            {
                return _openMessageSearchCommand ?? (_openMessageSearchCommand = new RelayCommand(obj =>
                {
                    if (IsMessageSearchBoxOpen == false)
                    {
                        IsMessageSearchBoxOpen = true;
                    }
                    else
                    {
                        IsMessageSearchBoxOpen = false;
                    }
                }));
            }
        }
        private RelayCommand _clearMessageSearchCommand;

        public RelayCommand ClearMessageSearchCommand
        {
            get
            {
                return _clearMessageSearchCommand ?? (_clearMessageSearchCommand = new RelayCommand(obj =>
                {
                    if (!string.IsNullOrEmpty(TxtEntered))
                    {
                        SearchMessageText = "";
                    }
                    else
                    {
                        IsMessageSearchBoxOpen = false;
                    }
                }));
            }
        }

        private RelayCommand _PinChatCommand;

        public RelayCommand PinChatCommand
        {
            get
            {
                return _PinChatCommand ?? (_PinChatCommand = new RelayCommand(obj =>
                {
                    if(obj is ConservationsMy v)
                    {
                        if (!FilteredPinnedConservations.Contains(v))
                        {
                            ConservationsMies.Remove(v);
                            FilteredConservations.Remove(v);
                            PinnedConservations.Add(v);
                            FilteredPinnedConservations.Add(v);
                            v.Is_Pinned = true;
                            //v.ChatIsPinned = true;
                            if (ArchivedConservations != null)
                            {
                                if (ArchivedConservations.Contains(v))
                                {
                                    ArchivedConservations.Remove(v);
                                    v.Is_Archived = false;
                                }
                            }
                        }
                       // OnPropertyChanged("PinnedChats");
                        //OnPropertyChanged("FilteredPinnedChats");
                       // OnPropertyChanged("UsersList");
                       // OnPropertyChanged("FilteredChats");
                    }
                }));
            }
        }
        private RelayCommand _UnPinChatCommand;

        public RelayCommand UnPinChatCommand
        {
            get
            {
                return _UnPinChatCommand ?? (_UnPinChatCommand = new RelayCommand(obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!FilteredConservations.Contains(v))
                        {
                            ConservationsMies.Add(v);
                            FilteredConservations.Add(v);
                            PinnedConservations.Remove(v);
                            FilteredPinnedConservations.Remove(v);
                            v.Is_Pinned = false;
                            //v.IsPinned = false;

                        }
                        // OnPropertyChanged("PinnedChats");
                        //OnPropertyChanged("FilteredPinnedChats");
                        // OnPropertyChanged("UsersList");
                        // OnPropertyChanged("FilteredChats");
                    }
                }));
            }
        }

        private RelayCommand _archiveChatCommand;
        public RelayCommand ArchiveChatCommand
        {
            get
            {
                return _archiveChatCommand ?? (_archiveChatCommand = new RelayCommand(obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!ArchivedConservations.Contains(v))
                        {
                            
                            ArchivedConservations.Add(v);
                            ConservationsMies.Remove(v);
                            FilteredConservations.Remove(v);
                            PinnedConservations.Remove(v);
                            FilteredPinnedConservations.Remove(v);
                            v.Is_Archived = true;
                            v.Is_Pinned = false;

                        }
                    }
                }));
            }
        }
        private RelayCommand _unarchiveChatCommand;
        public RelayCommand UnArchiveChatCommand
        {
            get
            {
                return _unarchiveChatCommand ?? (_unarchiveChatCommand = new RelayCommand(obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!FilteredConservations.Contains(v))
                        {
                            ConservationsMies.Add(v);
                            FilteredConservations.Add(v);
                        }
                            ArchivedConservations.Remove(v);
                            v.Is_Archived = false;
                            v.Is_Pinned = false;
                    }
                }));
            }
        }
        private RelayCommand _replyCommand;

        public RelayCommand ReplyCommand
        {
            get
            {
                return _replyCommand ?? (_replyCommand = new RelayCommand(obj =>
                {
                    if(obj is MessagesListMy v)
                    {
                        MessageToReplyText = v.Message;
                        //MessageToReplyText=v.ReceivedMessage
                    }
                    else
                    {
                       //MessageToReplyText=v.SentMessage
                       
                    }

                    //Показ MessageBox когда мы нажимаем кнопку переслать
                    FocusMessageBox = true;

                    //Сделать это сообщение пересланным
                    IsThisReplyMessage = true;
                }));
            }
        }
        private RelayCommand _cancelReplyCommand;

        public RelayCommand CancelReplyCommand
        {
            get
            {
                return _cancelReplyCommand ?? (_cancelReplyCommand = new RelayCommand(obj =>
                {
                    IsThisReplyMessage = false;
                    MessageToReplyText=string.Empty;
                }));
            }
        }
        private RelayCommand _sendMessageCommand;

        public RelayCommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand ?? (_sendMessageCommand = new RelayCommand(obj =>
                {
                    SendMessage();
                }));
            }
        }
        private RelayCommand _openFileDialogCommand;

        public RelayCommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommand(obj =>
                {
                    OpenFileDialog dlg = new OpenFileDialog()
                    {
                        Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                    };
                    if (dlg.ShowDialog() == true)
                    {
                        //ImagePhoto.Source = new BitmapImage(new Uri(dlg.FileName));
                        int count = MessagesListNow.Count;

                        MessagesListMy cons = new MessagesListMy
                        {
                            Id = ++count,
                            ImagePho = File.ReadAllBytes(dlg.FileName),
                            Message = null,
                            Created_at = DateTime.Now,
                            Deleted_at = null,
                            Sender_id = LoginMod.IdUserNow,
                            Type_id = null,
                            IsSender = true

                        };
                        MessagesListNow.Add(cons);
                        MessagesListMy.Add(cons);
                        MessageText = string.Empty;
                        IsThisReplyMessage = false;
                        MessageToReplyText = string.Empty;
                    }
                    else
                    {
                        
                    }

                }));
            }
        }
        #endregion Commands

    }
}
