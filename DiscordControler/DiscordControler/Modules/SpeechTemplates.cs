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
            return (HasPlaceholder(speech) && guildName.Length != 0) ? string.Format(speech, guildName) : speech;

        }
        public string GetLeaveGuild(string guildName = "") {
            List<string> leaveGuild = new List<string> {"De certeza que eles vão ter saudades tuas.",
                                                       "O pessoal do {0} manda abraços."};
            string speech = leaveGuild[randomGen.Next(leaveGuild.Count)];
            return HasPlaceholder(speech) && guildName.Length != 0 ? string.Format(speech, guildName) : speech;

        }

        public string GetDeleteChannel (string channelName = "")
        {
            List<string> deleteChannelSpeech = new List<string> {"Canal {0} apagado!",
                                                       "O canal {0} deixou de existir."};
            string speech = deleteChannelSpeech[randomGen.Next(deleteChannelSpeech.Count)];
            return HasPlaceholder(speech) && channelName.Length != 0 ? string.Format(speech, channelName) : speech;

        }

        public string GetKickUser(string userName = "")
        {
            List<string> kickUserlSpeech = new List<string> {"Canal {0} apagado!",
                                                       "O canal {0} deixou de existir."};
            string speech = kickUserlSpeech[randomGen.Next(kickUserlSpeech.Count)];
            return HasPlaceholder(speech) && userName.Length != 0 ? string.Format(speech, userName) : speech;

        }

        private bool HasPlaceholder(string s)
        {
            return Regex.IsMatch(s, ".*{.*}.*");
        }
    }
}
