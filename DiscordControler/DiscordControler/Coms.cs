using mmisharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiscordControler
{
    class Coms
    {
        private MmiCommunication mmiC;
        public Coms() {
            
            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            //mmiC.Message += MmiC_Message;
            //mmiC.Start();

        }
        public MmiCommunication GetMmic() {
            return mmiC;
        }
        
    }
}
