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
        public string GetAvailableCommands()
        {
            List<string> availableCommands = new List<string> { " Wally apaga a última mensagem do canal teste.",
                                                       " Wally expulsa o utilizador Matos.",
                                                       " Wally remove o utilizador Matos.",
                                                       " Wally elimina o canal sueca.",
                                                       " Wally tira o bane do utilizador Matos no servidor Tópicos de Apicultura.",
                                                       " Wally quero sair do servidor Tópicos de Apicultura.",
                                                       " Wally qual é o estado do Matos.",
                                                       " Wally corta as comunicações com o Matos.",
                                                       " Wally retoma as comunicações com o utilizador Matos no servidor IMServer.",
                                                       " Wally desactiva o microfone do Matos.",
                                                       " Wally activa o microfone do utilizador Ambrósio.",
                                                       " Wally desactiva o áudio do Gustavo.",
                                                       " Wally activa o áudio do utilizador Matos."};
            string firstCommand = availableCommands[randomGen.Next(availableCommands.Count)];
            string secondCommand = null;
            do
            {
                secondCommand = availableCommands[randomGen.Next(availableCommands.Count)];
            } while (firstCommand.Equals(secondCommand));
            return "Podes pedir commandos do tipo."+firstCommand+secondCommand;
        }

        public string GetToDo()
        {
            return "Sou capaz de banir expulsar utilizadores, mais tarde posso remover esses banimentos, cortar as comunicações com certo utilizador, apagar canais e apagar as últimas mensagens dos canais. Se quiseres também posso te tirar dos servidores que não queres mais estar.";
        }
        public string GetGreeting(string userName, string serverName) {
            List<string> greetings = new List<string> {$"Olá {userName}.",
                                                       $"O {userName} entrou no servidor {serverName}."};
            return greetings[randomGen.Next(greetings.Count)];
        }

        public string GetUnkownUser()
        {
            List<string> unkownUsers = new List<string> {"Não sei de quem falas!",
                                                       "Esse utilizador não existe.",
                                                        "Esse utilizador não esta cá."};
            return unkownUsers[randomGen.Next(unkownUsers.Count)];
        }

        public string GetUnkownGuild(string guildName = "") {
            List<string> unkownGuild = new List<string> {"Desconheço esse servidor.",
                                                       "Não estás nesse servidor",
                                                        "Não encontro o servidor {0}."};
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
                                                       "Desculpa, não percebi. Achas que podes repetir?",
                                                        "Desculpa, mas hoje estou um bocado para o surdo. Podes repetir novamente?"}; //cego
            return lowConfidence[randomGen.Next(lowConfidence.Count)];
        }

        public string GetKickUserExplicit (string username, string guildName)
        {
            List<string> kickUserExplicit = new List<string> {$"Então, queres que tire o utilizador {username} do servidor {guildName}, correcto?",
                                                       $"Eu percebi que queres dar kick ao utilizador {username} no servidor {guildName}. Estou correcto?",
                                                        $"Penso que dissestes que queres kikar o utilizador {username} do servidor {guildName}. Verdade?"};
            return kickUserExplicit[randomGen.Next(kickUserExplicit.Count)];
        }

        public string GetBanUserExplicit(string username, string guildName)
        {
            List<string> banUserExplicit = new List<string> {$"Então, queres que bane o utilizador {username} do servidor {guildName}, correcto?",
                                                       $"Eu percebi que queres que o utilizador {username} não volte por uns tempos ao servidor {guildName}. Estou correcto?",
                                                        $"Acho que ouvi que queres expulsar o utilizador {username} do servidor {guildName}. Verdade?"}; //vi
            return banUserExplicit[randomGen.Next(banUserExplicit.Count)];
        }

        public string GetDeleteMessageExplicit(string channelName, string guildName)
        {
            List<string> deleteMessageExplicit = new List<string> {$"Então, queres que elimine a última mensagem do canal {channelName} do servidor {guildName}, correcto?",
                                                       $"Tens a certeza que queres apagar a última mensagem do canal {channelName} do servidor {guildName}?",
                                                        $"Percebi que queres remover a última mensagem do canal {channelName} do servidor {guildName}. Estou correcto?"};
            return deleteMessageExplicit[randomGen.Next(deleteMessageExplicit.Count)];
        }

        public string GetDeleteChannelExplicit(string channelName, string guildName)
        {
            List<string> deleteChannelExplicit = new List<string> {$"Então, queres que elimine o canal {channelName} do servidor {guildName}, correcto?",
                                                       $"Tens a certeza que queres apagar o canal {channelName} do servidor {guildName}?",
                                                        $"Percebi que queres remover o canal {channelName} do servidor {guildName}. Estou correcto?"};
            return deleteChannelExplicit[randomGen.Next(deleteChannelExplicit.Count)];
        }

        public string GetLeaveGuildExplicit(string guildName)
        {
            List<string> leaveGuildExplicit = new List<string> {$"Então, queres que te tira do servidor {guildName}, correcto?",
                                                       $"Percebi que queres sair do servidor {guildName}. Estou correcto?",
                                                        $"Penso que queres sair do servidor {guildName}. Verdade?"};
            return leaveGuildExplicit[randomGen.Next(leaveGuildExplicit.Count)];
        }

        public string GetRemoveBanExplicit(string username, string guildName)
        {
            List<string> removeBanExplicit = new List<string> {$"Então, queres remover o bane que foi colocado ao utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Acho que ouvi que queres tirar o bane colocado ao utilizador {username} no servidor {guildName}. Estou correcto?",//vi
                                                        $"Percebi que é para apagar o bane no utilizador {username} no servidor {guildName}. Verdade?"};
            return removeBanExplicit[randomGen.Next(removeBanExplicit.Count)];
        }

        public string GetMuteExplicit(string username, string guildName)
        {
            List<string> muteExplicit = new List<string> {$"Então, queres calar o utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Acho que ouvi que queres silenciar o utilizador {username} no servidor {guildName}. Estou correcto?",//vi
                                                        $"Tens mesmo a certeza que queres desactivar o microfone ao utilizador {username} no servidor {guildName}?"};
            return muteExplicit[randomGen.Next(muteExplicit.Count)];
        }

        public string GetSelfMuteExplicit(string guildName)
        {
            List<string> selfMuteExplicit = new List<string> {$"Então, queres que te cale no servidor {guildName}, correcto?",
                                                       $"Acho que percebi que queres desactivar o teu microfone no servidor {guildName}. Estou correcto?",
                                                        $"Tens mesmo a certeza que queres desactivar o teu microfone no servidor {guildName}?"};
            return selfMuteExplicit[randomGen.Next(selfMuteExplicit.Count)];
        }

        public string GetUnMuteExplicit(string username, string guildName)
        {
            List<string> unMuteExplicit = new List<string> {$"Então, queres dar voz ao utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Acho que ouvi que queres activar o microfone do utilizador {username} no servidor {guildName}. Estou correcto?",//vi
                                                        $"Tens mesmo a certeza que queres ouvir o utilizador {username} no servidor {guildName}?"};
            return unMuteExplicit[randomGen.Next(unMuteExplicit.Count)];
        }

        public string GetSelfUnMuteExplicit(string guildName)
        {
            List<string> selfUnMuteExplicit = new List<string> {$"Então, queres os utilizados no servidor {guildName} te oiçam, correcto?",
                                                       $"Acho que percebi que queres activar o teu microfone no servidor {guildName}. Estou correcto?",
                                                        $"Tens mesmo a certeza que queres activar o teu microfone no servidor {guildName}?"};
            return selfUnMuteExplicit[randomGen.Next(selfUnMuteExplicit.Count)];
        }

        public string GetDeafExplicit(string username, string guildName)
        {
            List<string> deafExplicit = new List<string> {$"Então, queres bloquear o som ao utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Penso que dissestes que queres tira a audição ao utilizador {username} no servidor {guildName}. Estou correcto?",
                                                        $"Tens mesmo a certeza que queres desactivar o áudio ao utilizador {username} no servidor {guildName}?"};
            return deafExplicit[randomGen.Next(deafExplicit.Count)];
        }

        public string GetUnDeafExplicit(string username, string guildName)
        {
            List<string> unMuteExplicit = new List<string> {$"Então, queres que o utilizador {username} oiça o pessoal do servidor {guildName}, correcto?",
                                                       $"Acho que ouvi que queres desbloquear o som ao utilizador {username} no servidor {guildName}. Estou correcto?",//vi
                                                        $"Tens mesmo a certeza que queres activar o áudio ao utilizador {username} no servidor {guildName}?"};
            return unMuteExplicit[randomGen.Next(unMuteExplicit.Count)];
        }

        public string GetMuteDeafExplicit(string username, string guildName)
        {
            List<string> muteDeafExplicit = new List<string> {$"Então, queres tirar a voz e a audição ao utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Penso que queres silenciar e bloquear o som do utilizador {username} no servidor {guildName}. Estou correcto?",
                                                        $"Percebi que queres calar e restingir a capacidade auditiva do utilizador {username} no servidor {guildName}. Estou correcto?"};
            return muteDeafExplicit[randomGen.Next(muteDeafExplicit.Count)];
        }

        public string GetUnMuteUnDeafExplicit(string username, string guildName)
        {
            List<string> unMuteUnDeafExplicit = new List<string> {$"Percebi que tens o desejo de dar voz e ouvidos ao utilizador {username} no servidor {guildName}, correcto?",
                                                       $"Acho que entendi que queres ouvir o utilizador {username} no servidor {guildName} e tambem queres que ele te oiça. Estou correcto?",
                                                        $"Tens mesmo a certeza que queres activar o microfone e o áudio ao utilizador {username} no servidor {guildName}?"};
            return unMuteUnDeafExplicit[randomGen.Next(unMuteUnDeafExplicit.Count)];
        }

        public string GetDeleteLastMessage(string channelName, string guildName){
            List<string> deleteLastMessageSpeech = new List<string> { $"Última mensagem do canal {channelName} apagada!",
                                                                             $"Mensagem apagada. Disseste alguma coisa embaraçosa?"};
            return deleteLastMessageSpeech[randomGen.Next(deleteLastMessageSpeech.Count)];

        }

        public string GetBanOnUnkwonUser(string userName)
        {
            List<string> banOnUnkownUserSpeech = new List<string> { $"Não há nenhum banimento no utilizador {userName}.",
                                                                    $"Esse coitado não fez nada de mal, não tens nenhum banimento nesse utilizador."};
            return banOnUnkownUserSpeech[randomGen.Next(banOnUnkownUserSpeech.Count)];

        }
        public string GetRemoveBan(string userName, string guildName)
        {
            List<string> removeBanImplicitSpeech = new List<string> {$"Volta {userName} estás perdoado.",
                                                       $"A remover o banimento do utilizador {userName} do servidor {guildName}."};
            return removeBanImplicitSpeech[randomGen.Next(removeBanImplicitSpeech.Count)];
        }

        public string GetUserStatus(string userName, string status)
        {
            string processedStatus = ProcessStatus(status);
            List<string> statusSpeech = new List<string> {$"O {userName} está {processedStatus}."};
            return statusSpeech[randomGen.Next(statusSpeech.Count)];
        }


        public string GetBanUser(string userName, string guildName)
        {
            List<string> banUserImplicit = new List<string> {$"Já podes dizer adeus ao {userName}!",
                                                       $"O utilizador {userName} está banido do servidor {guildName}."};
            return banUserImplicit[randomGen.Next(banUserImplicit.Count)];
        }

        public string GetNoConfirmation()
        {
            List<string> noConfirmation = new List<string> {"Ok, não o irei fazer. A espera de novos comandos.",
                                                            "Como pareces arrependido, eu não irei fazer isso, por agora. A espera de novos comandos.",
                                                            "Pronto, tu é que sabes. Pedido eliminado. A espera de novos comandos."};
            return noConfirmation[randomGen.Next(noConfirmation.Count)];
        }

        public string GetMuteUser(string userName, string guildName)
        {
            List<string> muteUserImplicitSpeech = new List<string> {$"O utilizador {userName} já não pode falar.",
                                                       $"Já estava farto de ouvir o utilizador {userName}.",
                                                        $"Provavelmente a guild {guildName} agradece."};
            return muteUserImplicitSpeech[randomGen.Next(muteUserImplicitSpeech.Count)];
        }

        public string GetSelfMute(string guildName)
        {
            List<string> selfMuteImplicitSpeech = new List<string> {$"Os utilizadores do servidor {guildName} já não podem ouvir te.",
                                                       $"Já estavas farto de falar?"};
            return selfMuteImplicitSpeech[randomGen.Next(selfMuteImplicitSpeech.Count)];
        }

        public string GetUnMuteUser(string userName, string guildName)
        {
            List<string> unMuteUserImplicitSpeech = new List<string> {$"O utilizador {userName} já pode falar.",
                                                       $"Fala lá {userName} a guild {guildName} já sentia a falta da tua voz."};
            return unMuteUserImplicitSpeech[randomGen.Next(unMuteUserImplicitSpeech.Count)];
        }

        public string GetSelfDeafImplicit() {
            List<string> deafImplicitSpeech = new List<string> {$"Eu entendo, também já estava farto dos ouvir.",
                                                       $"Ahhh finalmente! Silêncio."};
            return deafImplicitSpeech[randomGen.Next(deafImplicitSpeech.Count)];

        }

        public string GetSelfUnDeafImplicit()
        {
            List<string> unDeafImplicitSpeech = new List<string> {$"Já não estás surdo!",
                                                       $"Já podes ouvir mas se calhar devias meter uns auriculares."};
            return unDeafImplicitSpeech[randomGen.Next(unDeafImplicitSpeech.Count)];

        }
         public string GetSelfUnMute(string guildName)
        {
            List<string> selfUnMuteImplicitSpeech = new List<string> {$"Os utilizadores do servidor {guildName} já podem ouvir te.",
                                                       $"De certeza que os utilizadores do servidor {guildName} tinham saudades da tua voz."};
            return selfUnMuteImplicitSpeech[randomGen.Next(selfUnMuteImplicitSpeech.Count)];
        }

        public string GetDeafUserImplicit(string userName, string guildName)
        {
            List<string> deafImplicitSpeech = new List<string> {$"Já não ouves o {userName}.",
                                                       $"Estava farto de ouvir o utilizador {userName}? Eu também."};
            return deafImplicitSpeech[randomGen.Next(deafImplicitSpeech.Count)];
        }
        public string GetUnDeafUserImplicit(string userName, string guildName)
        {
            List<string> unDeafImplicitSpeech = new List<string> {$"Já podes ouvir o utilizador {userName}.",
                                                       $"Prepara os ouvidos, já consegues ouvir o utlizador {userName}."};
            return unDeafImplicitSpeech[randomGen.Next(unDeafImplicitSpeech.Count)];
        }

        public string GetMuteDeafImplicit(string userName, string guildName)
        {
            List<string> muteDeafImplicitSpeech = new List<string> {$"Comunicações cortadas com {userName}.",
                                                       $"O utilizador {userName} já não te chateia mais."};
            return muteDeafImplicitSpeech[randomGen.Next(muteDeafImplicitSpeech.Count)];
        }

        public string GetUnMuteDeafImplicit(string userName, string guildName)
        {
            List<string> unMuteDeafImplicitSpeech = new List<string> {$"Já podes voltar a comunicar com {userName}.",
                                                       $"Já podes interagir com {userName}, estavam chateados?"};
            return unMuteDeafImplicitSpeech[randomGen.Next(unMuteDeafImplicitSpeech.Count)];
        }

        public string GetDeleteMessageError(string channelName, string guildName)
        {
            List<string> deleteMessageError = new List<string> {$"Não existe qualquer mensagem no canal {channelName} do servidor {guildName}.",
                                                       $"Creio que estas enganado, não existe mensagens no canal {channelName} do servidor {guildName}",
                                                       $"Talvez estejas a ficar cego porque não existe neguma mensagem no canal {channelName} do servidor {guildName}."};
            return deleteMessageError[randomGen.Next(deleteMessageError.Count)];
        }

        private bool HasPlaceholder(string s)
        {
            return Regex.IsMatch(s, ".*{.*}.*");
        }

        private string ProcessStatus(string status) {
            switch (status) {
                case "AFK":
                    return "aa,f,k";
                case "DoNotDisturb":
                    return "ocupado";
                case "Idle":
                    return "a dormir";
                case "Invisible":
                    return "em modo invisível, deve-se achar ninja";
                default:
                    return status;
            }
        }
    }
}
