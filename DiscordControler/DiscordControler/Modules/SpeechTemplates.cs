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

        public string GetKickUserExplicit (string username, string guildName)
        {
            List<string> kickUserExplicit = new List<string> {$"Então, queres que tire o utilizador {username} do servidor {guildName}, correto?",
                                                       $"Eu percebi que queres dar kick ao utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Penso que dizestes que queres kikar o utilizador {username} do servidor {guildName}. Verdade?"};
            return kickUserExplicit[randomGen.Next(kickUserExplicit.Count)];
        }

        public string GetBanUserExplicit(string username, string guildName)
        {
            List<string> banUserExplicit = new List<string> {$"Então, queres que bane o utilizador {username} do servidor {guildName}, correto?",
                                                       $"Eu percebi que queres que o utilizador {username} não volte por uns tempos ao servidor {guildName}. Estou correto?",
                                                        $"Acho que ouvi que queres expulsar o utilizador {username} do servidor {guildName}. Verdade?"};
            return banUserExplicit[randomGen.Next(banUserExplicit.Count)];
        }

        public string GetLeaveGuildExplicit(string guildName)
        {
            List<string> leaveGuildExplicit = new List<string> {$"Então, queres que te tira do servidor {guildName}, correto?",
                                                       $"Percebi que queres sair do servidor {guildName}. Estou correto?",
                                                        $"Penso que queres sair do servidor {guildName}. Verdade?"};
            return leaveGuildExplicit[randomGen.Next(leaveGuildExplicit.Count)];
        }

        public string GetRemoveBanExplicit(string username, string guildName)
        {
            List<string> removeBanExplicit = new List<string> {$"Então, queres remover o bane que foi colocado ao utilizador {username} no servidor {guildName}, correto?",
                                                       $"Acho que ouvi que queres tirar o bane colocado ao utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Percebi que é para apagar o bane no utilizador {username} no servidor {guildName}. Verdade?"};
            return removeBanExplicit[randomGen.Next(removeBanExplicit.Count)];
        }

        public string GetMuteExplicit(string username, string guildName)
        {
            List<string> muteExplicit = new List<string> {$"Então, queres calar o utilizador {username} no servidor {guildName}, correto?",
                                                       $"Acho que ouvi que queres silenciar o utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Tens mesmo a certeza que queres desativar o microfone ao utilizador {username} no servidor {guildName}?"};
            return muteExplicit[randomGen.Next(muteExplicit.Count)];
        }

        public string GetUnMuteExplicit(string username, string guildName)
        {
            List<string> unMuteExplicit = new List<string> {$"Então, queres dar voz o utilizador {username} no servidor {guildName}, correto?",
                                                       $"Acho que ouvi que queres ativar o microfone do utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Tens mesmo a certeza que queres ouvir o utilizador {username} no servidor {guildName}?"};
            return unMuteExplicit[randomGen.Next(unMuteExplicit.Count)];
        }

        public string GetDeafExplicit(string username, string guildName)
        {
            List<string> deafExplicit = new List<string> {$"Então, queres bolquear o som ao utilizador {username} no servidor {guildName}, correto?",
                                                       $"Penso que dizestes que queres tira a audição ao utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Tens mesmo a certeza que queres desativar o audio ao utilizador {username} no servidor {guildName}?"};
            return deafExplicit[randomGen.Next(deafExplicit.Count)];
        }

        public string GetUnDeafExplicit(string username, string guildName)
        {
            List<string> unMuteExplicit = new List<string> {$"Então, queres que o utilizador {username} oiça o pessoal do servidor {guildName}, correto?",
                                                       $"Acho que ouvi que queres desbloquear o som ao utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Tens mesmo a certeza que queres ativar o audio ao utilizador {username} no servidor {guildName}?"};
            return unMuteExplicit[randomGen.Next(unMuteExplicit.Count)];
        }

        public string GetMuteDeafExplicit(string username, string guildName)
        {
            List<string> muteDeafExplicit = new List<string> {$"Então, queres tirar a voz e a audição ao utilizador {username} no servidor {guildName}, correto?",
                                                       $"Penso que queres silenciar e bolquear o som do utilizador {username} no servidor {guildName}. Estou correto?",
                                                        $"Percebi que queres calar e restingir a capacidade auditiva do utilizador {username} no servidor {guildName}. Estou correto?"};
            return muteDeafExplicit[randomGen.Next(muteDeafExplicit.Count)];
        }

        public string GetUnMuteUnDeafExplicit(string username, string guildName)
        {
            List<string> unMuteUnDeafExplicit = new List<string> {$"Percebi que tens o desejo de dar voz e ouvidos ao utilizador {username} no servidor {guildName}, correto?",
                                                       $"Acho que entendi que queres ouvir o utilizador {username} no servidor {guildName} e tambem queres ele te oiça. Estou correto?",
                                                        $"Tens mesmo a certeza que queres ativar o microfone e o audio ao utilizador {username} no servidor {guildName}?"};
            return unMuteUnDeafExplicit[randomGen.Next(unMuteUnDeafExplicit.Count)];
        }

        public string GetBanUSer(string userName, string guildName)
        {
            List<string> banUserImplicit = new List<string> {$"Já podes dizer adeus ao {userName}!",
                                                       $"O utilizador {userName} está banido do servidor {guildName}."};
            return banUserImplicit[randomGen.Next(banUserImplicit.Count)];
        }

        private bool HasPlaceholder(string s)
        {
            return Regex.IsMatch(s, ".*{.*}.*");
        }

    }
}
