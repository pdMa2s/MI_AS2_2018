using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;

namespace AppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MmiCommunication mmiC;
        public MainWindow()
        {
            InitializeComponent();


            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);
            Console.WriteLine(json);
            var action = json.recognized.action;
            Console.WriteLine(action);
            switch ((string) action)
            {
                case "CREATE_GUILD":
                    var guildName = json.recognized.guildName;
                    break;
                case "CREATE_CHANNEL":
                    var channelName = json.recognized.channelName;
                    var guildNameToAddChannel = json.recognized.guildName;
                    break;
                case "ADD_USER":
                    var usernameToAdd = json.recognized.userName;
                    var guildNameToAddUser = json.recognized.guildName;
                    break;
                case "REMOVE_USER":
                    var usernameToRemove = json.recognized.userName;
                    var guildNameToRemoveUser = json.recognized.guildName;
                    break;
                case "BAN_USER":
                    var usernameToBan = json.recognized.userName;
                    var guildNameToBanUser = json.recognized.guildName;
                    var reason = json.recognized.reason;
                    break;
                case "SEND_MESSAGE":
                    var channelNameToSendMsg = json.recognized.channelName;
                    var messageToAdd = json.recognized.message;
                    break;
                case "EDIT_MESSAGE":
                    var channelNameToEditMsg = json.recognized.channelName;
                    var messageEdited = json.recognized.message;
                    break;
                case "DELETE_LAST_MESSAGE":
                    var channelNameToDeleteMsg = json.recognized.channelName;
                    break;
                case "DELETE_CHANNEL":
                    var channelNameToDelete = json.recognized.channelName;
                    break;
                case "LEAVE_GUILD":
                    var guildNameToLeave = json.recognized.guildName;
                    break;
            }
            /*Shape _s = null;
            switch ((string)json.recognized[0].ToString())
            {
                case "SQUARE": _s = rectangle;
                    break;
                case "CIRCLE": _s = circle;
                    break;
                case "TRIANGLE": _s = triangle;
                    break;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                switch ((string)json.recognized[1].ToString())
                {
                    case "GREEN":
                        _s.Fill = Brushes.Green;
                        break;
                    case "BLUE":
                        _s.Fill = Brushes.Blue;
                        break;
                    case "RED":
                        _s.Fill = Brushes.Red;
                        break;
                }
            });*/
            


        }
    }
}
