using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordControler.Modules
{
    class SpeechTemplates
    {
        private Random randomGen;
        public SpeechTemplates() {
            randomGen = new Random();
        }

        public string GetGreeting(string userName) {
            List<string> greetings = new List<string> {$"Olá {userName}.",
                                                       $"O {userName} entrou no server."};
            return greetings[randomGen.Next(greetings.Count)];
        }

        public string GetUnkownUser()
        {
            List<string> unkownUsers = new List<string> {"Não sei de quem  falas!",
                                                       "Esse utilizador não existe.",
                                                        "Esse utilizador não esta cá."};
            return unkownUsers[randomGen.Next(unkownUsers.Count)];
        }

        public string GetUnkownGuild(string guildName = "") {
            List<string> unkownGuild = new List<string> {"Desconheço esse servidor.",
                                                       "Não estás nesse servidor",
                                                        "Não o servidor {0}."};
            string speech = unkownGuild[randomGen.Next(unkownGuild.Count)];
            return HasPlaceholder(speech) ? string.Format(speech, guildName) : speech;

        }

        private bool HasPlaceholder(string s)
        {
            return Regex.IsMatch(s, ".*{.*}.*");
        }
    }
}
