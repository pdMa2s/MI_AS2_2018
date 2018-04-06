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
