using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureModality
{
    public class Guild
    {
        private string name;
        private string[] channels;
        private string[] users;

        public Guild(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } set { this.name = value; } }
        public string[] Channels { get { return channels; } set { this.channels = value; } }
        public string[] Users { get { return users; } set { this.users = value; } }


        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(name + "\n");
            for (int i = 0; i < channels.Length; i++) {
                sb.Append(channels[i] + " ");
            }
            sb.Append("\n");
            for (int i = 0; i < users.Length; i++)
            {
                sb.Append(users[i] + " ");
            }
            sb.Append("\n");
            return sb.ToString();
        }
    }
}
