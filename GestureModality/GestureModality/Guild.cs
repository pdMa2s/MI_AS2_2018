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

        public string Name { get; set; }
        public string[] Channels { get; set; }
        public string[] Users { get; set; }

    }
}
