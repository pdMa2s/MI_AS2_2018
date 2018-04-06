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
                                                        "Não o encontro servidor {0}."};
            string speech = unkownGuild[randomGen.Next(unkownGuild.Count)];
            return (HasPlaceholder(speech) && guildName.Length != 0) ? string.Format(speech, guildName) : speech;

        }

        public string GetUnkownChannel(string channelName, string guildName)
        {
            List<string> unkownChannel = new List<string> {"O servidor {0} não tem nenhum canal com o nome de {1}.",
                                                       "Não encontro esse canal."};
            string speech = unkownChannel[randomGen.Next(unkownChannel.Count)];
            return (HasPlaceholder(speech) && channelName.Length != 0) ? string.Format(speech, channelName, guildName) : speech;

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
            List<string> kickUserlSpeech = new List<string> {"O {0} vai lá fora apanhar ar.",
                                                       "kick no {0}"};
            string speech = kickUserlSpeech[randomGen.Next(kickUserlSpeech.Count)];
            return HasPlaceholder(speech) && userName.Length != 0 ? string.Format(speech, userName) : speech;
        }
        public string GetLowConfidence()
        {
            List<string> lowConfidence = new List<string> {"Desculpa, estava desatento! Podes repetir se faz favor?",
                                                       "Desculpa, não percebi. Achas que podes repetir novamente mais perto do microfone se faz favor?",
                                                        "Desculpa, mas hoje estou um bocado para o surdo. Podes repetir novamente, mas alto se faz favor?"};
            return lowConfidence[randomGen.Next(lowConfidence.Count)];
        }

        public string GetKickUserConfirmation(string username, string guildName)
        {
            List<string> kickUserConfirmation = new List<string> {$"Então, queres que tire o utilizador {username} do servidor {guildName}, correto?",
                                                       $"Eu percebi que queres dar kick ao utilizador {username} do servidor {guildName}. Estou correto?",
                                                        $"Penso que dizestes que queres kikar o utilizador {username} do servidor {guildName}. Verdade?"};
            return kickUserConfirmation[randomGen.Next(kickUserConfirmation.Count)];
        }

        public string GetBanUserConfirmation(string username, string guildName)
        {
            List<string> banUserConfirmation = new List<string> {$"Então, queres que bane o utilizador {username} do servidor {guildName}, correto?",
                                                       $"Eu percebi que queres que o utilizador {username} não volte por uns tempos ao servidor {guildName}. Estou correto?",
                                                        $"Acho que ouvi que queres expulsar o utilizador {username} do servidor {guildName}. Verdade?"};
            return banUserConfirmation[randomGen.Next(banUserConfirmation.Count)];
        }

        private bool HasPlaceholder(string s)
        {
            return Regex.IsMatch(s, ".*{.*}.*");
        }
    }
}
