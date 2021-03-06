﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using mmisharp;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using Discord.Rest;
using DiscordControler.Modules;
using System.Threading;

namespace DiscordControler
{
    class Controller
    {
        static void Main(string[] args) => new Controller().RunBotAsync().GetAwaiter().GetResult();

        private readonly ulong _defaultChannelId = 426423137073364995;
        private readonly ulong _defaultGuildId = 426423137073364993;
        private readonly ulong _userID = 414166532143316992;
        private readonly string _userNick = "IMStudent2018";
        private readonly string _botToken = "NDMxNTg4NTczMTI5NjA1MTIw.Dag8Yw.lW9VrG3H8cJLiFv8rg0eUBkvwBY";

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;
        private MmiCommunication _mmiComms;
        private Tts _tts;
        private dynamic lastJsonMessage;
        private SpeechTemplates _speechTemplates;

        public async Task RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            var comModule = new Coms(_userNick);
            _mmiComms = comModule.GetMmic();
            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            _tts = new Tts(comModule);
            _speechTemplates = new SpeechTemplates();
            
            _client.Log += Log;
            await RegisterCommandsAsync();
            _mmiComms.Message += MmiC_Message; // subscribe to the messages that come from the comMudole
            _mmiComms.Start();

            
            await _client.LoginAsync(TokenType.Bot, _botToken);

            
            await _client.StartAsync();
            
            _tts.Speak("Olá eu sou o wally, o teu bot do Discord! Se desejares podes-me perguntar o que é que eu sou capaz de fazer e que comandos estão disponíveis. So tenho um requisito, diz o meu nome antes de qualquer comando.");

            Thread.Sleep(2000);

            comModule.SendGuildInfo(_client.Guilds);
            await Task.Delay(-1);
        }

        
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync() {

            //_client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnouceUserJoined;
            //await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task AnnouceUserJoined(SocketGuildUser user)
        {
            _tts.Speak(_speechTemplates.GetGreeting(user.Username, user.Guild.Name));
        }



        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            
            dynamic json = PreProcessing(doc);
            Console.WriteLine(json);
            var action = json.recognized["action"] == null ? null : json.recognized.action.ToString() as String;
            var confirmation = json.recognized["confirmation"] == null ? null : json.recognized.confirmation.ToString() as String;
            var confidence = json.recognized.confidence.ToString() as String;
            Console.WriteLine(action);
            if (confidence.Equals("low confidence") && confirmation == null)
                _tts.Speak(_speechTemplates.GetLowConfidence());
            else if (confidence.Equals("explicit confirmation") && confirmation == null)
            {
                if (!action.Equals("USER_STATUS") && !action.Equals("SAY_COMMANDS") && !action.Equals("SAY_TODO"))
                    lastJsonMessage = json;
                executeCommand(json, action, confidence);
            }
            else if (confidence.Equals("implicit confirmation") && confirmation == null)
                executeCommand(json, action, confidence);
            else if (confirmation != null && lastJsonMessage != null)
            {
                if (confirmation.Equals("yes"))
                {
                    executeCommand(lastJsonMessage, lastJsonMessage.recognized.action.ToString() as String, "implicit confirmation");
                    lastJsonMessage = null;
                }
                else
                {
                    _tts.Speak(_speechTemplates.GetNoConfirmation());
                    lastJsonMessage = null;
                }
            }
        }

        private dynamic PreProcessing(XDocument xmlDoc)
        {
            var commands = xmlDoc.Descendants("command");
            string newJson = "{ \"recognized\": { ";

            if (commands.Count() == 1)
            {
                dynamic json = JsonConvert.DeserializeObject(commands.First().Value);
                var recognized = json.recognized ?? null;
                string modality = json.modality ?? null;

                if (recognized != null)
                    for (int i = 0; i < recognized.Count - 1; i += 2)
                    {
                        newJson += "\"" + recognized[i] + "\": \"" + recognized[i + 1] + "\", ";
                    }
                if (modality != null)
                {
                    var confidence = json.confidence;
                    newJson += "\"confidence\":" + "\"" + confidence + "\", ";

                }
            }
            else
            {
                foreach (var c in commands)
                {
                    dynamic json = JsonConvert.DeserializeObject(c.Value);
                    string modality = json.modality ?? null;

                    if (modality != null)
                    {
                        string confidence;
                        if (modality.Equals("speech"))
                            confidence = json.confidence;
                        else
                            confidence = "implicit confirmation";
                        newJson += "\"confidence\":" + "\"" + confidence + "\", ";

                    }
                    else
                    {

                        var recognized = json.recognized ?? null;

                        if (recognized != null)
                            for (int i = 0; i < recognized.Count - 1; i += 2)
                            {
                                newJson += "\"" + recognized[i] + "\": \"" + recognized[i + 1] + "\", ";
                            }
                    }

                }
            }
            

            newJson = newJson.Substring(0, newJson.Length - 2);

            newJson += "}}";

            Console.WriteLine(newJson);
            return JsonConvert.DeserializeObject(newJson);

        }

        private async Task executeCommand(dynamic json, string action, string confidence)
        {
            switch (action)
            {
                case "REMOVE_USER":
                    var usernameToRemove = json.recognized.userName.ToString() as String;
                    var guildNameToRemoveUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    var kickReason = json.recognized["reason"] == null ? null : json.recognized.reason.ToString() as String;
                    await KickUser(usernameToRemove, guildNameToRemoveUser, kickReason, confidence);
                    break;
                case "BAN_USER": 
                    var usernameToBan = json.recognized.userName.ToString() as String;
                    var guildNameToBanUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    var banReason = json.recognized["reason"] == null ? null : json.recognized.reason.ToString() as String;
                    await BanUser(usernameToBan, guildNameToBanUser, banReason, confidence);
                    break;
                case "DELETE_LAST_MESSAGE": 
                    var channelNameToDeleteMsg = json.recognized["channelName"] == null ? null : json.recognized.channelName.ToString() as String;
                    var guildNameToDeleteMsg = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await DeleteLastMessage(channelNameToDeleteMsg, guildNameToDeleteMsg, confidence);
                    break;
                case "DELETE_CHANNEL":  
                    var channelNameToDelete = json.recognized.channelName.ToString() as String;
                    var guildNameToDeleteChannel = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await DeleteChannel(channelNameToDelete, guildNameToDeleteChannel, confidence);
                    break;
                case "LEAVE_GUILD": 
                    var guildNameToLeave = json.recognized.guildName.ToString() as String;
                    await LeaveGuild(guildNameToLeave, confidence);
                    break;
                case "REMOVE_BAN":
                    var userNameToRemBan = json.recognized.userName.ToString() as String;
                    var guildNameToRemBan = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await RemoveBan(userNameToRemBan, guildNameToRemBan, confidence);
                    break;
                case "USER_STATUS":
                    var userNameToKnowStatus = json.recognized.userName.ToString() as String;
                    var guildNameToKnowStatus = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    UserStatus(userNameToKnowStatus, guildNameToKnowStatus);
                    break;
                case "MUTE_USER":
                    var userNameToMute = json.recognized.userName.ToString() as String;
                    var guildNameToMuteUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeMuteUser(userNameToMute, guildNameToMuteUser, confidence);
                    break;
                case "SELF_MUTE":
                    var guildNameToSelfMute = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeSelfMute(guildNameToSelfMute, confidence);
                    break;
                case "DEAF_USER":
                    var userNameToDeaf = json.recognized.userName.ToString() as String;
                    var guildNameToDeafUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeDeafUser(userNameToDeaf, guildNameToDeafUser, confidence);
                    break;
                case "UNMUTE_USER":
                    var userNameToUnMute = json.recognized.userName.ToString() as String;
                    var guildNameToUnMuteUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeMuteUser(userNameToUnMute, guildNameToUnMuteUser, confidence);
                    break;
                case "UNDEAF_USER":
                    var userNameToUnDeaf = json.recognized.userName.ToString() as String;
                    var guildNameToUnDeafUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeDeafUser(userNameToUnDeaf, guildNameToUnDeafUser, confidence);
                    break;
                case "UNMUTE_UNDEAF_USER":
                    var userNameToUnMuteUnDeaf = json.recognized.userName.ToString() as String;
                    var guildNameToUnMuteUnDeafUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeMuteDeafUser(userNameToUnMuteUnDeaf, guildNameToUnMuteUnDeafUser, false, confidence);
                    break;
                case "MUTE_DEAF_USER":
                    var userNameToMuteDeaf = json.recognized.userName.ToString() as String;
                    var guildNameToMuteDeafUser = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await ChangeMuteDeafUser(userNameToMuteDeaf, guildNameToMuteDeafUser, true, confidence);
                    break;
                case "SELF_DEAF":
                    var guildNameToSelfDeaf = json.recognized["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                    await SelfDeaf(guildNameToSelfDeaf, confidence);
                    break;
                case "SAY_COMMANDS":
                    _tts.Speak(_speechTemplates.GetAvailableCommands());
                    break;
                case "SAY_TODO":
                    _tts.Speak(_speechTemplates.GetToDo());
                    break;
                default:
                    Console.WriteLine("Invalid action!");
                    break;
            }
        }

        private async Task KickUser(string userName, string guildName, string kickReason, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            if (user == null)
            {
                await channel.SendMessageAsync(_speechTemplates.GetUnkownUser(userName));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetKickUserExplicit(userName, guild.Name));
                return;
            }

            if (kickReason == null)
                await user.KickAsync();
            else
                await user.KickAsync(reason: kickReason);
            _tts.Speak(_speechTemplates.GetKickUser(userName));
        }

        private async Task BanUser(string userName, string guildName, string banReason, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            if (user == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownUser(userName));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetBanUserExplicit(userName, guild.Name));
                return;
            }

            if (banReason == null)
                await guild.AddBanAsync(user.Id);
            else
                await guild.AddBanAsync(user.Id, reason: banReason);

            _tts.Speak(_speechTemplates.GetBanUser(userName, guild.Name));
        }

        private async Task DeleteLastMessage(string channelName, string guildNameToDeleteMsg, string confidence)
        {
            var guild = FindGuild(guildNameToDeleteMsg);
            var channel = (SocketTextChannel)FindChannel(guild, channelName);
            if (channel == null) {
                await channel.SendMessageAsync(_speechTemplates.GetUnkownChannel(channelName, guild.Name));
                return;
            }
            var message = await channel.GetMessagesAsync(1).Flatten();
            
            if (message.Count() == 0)
            {
                _tts.Speak(_speechTemplates.GetDeleteMessageError(channelName, guild.Name));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetDeleteMessageExplicit(channelName, guild.Name));
                return;
            }
            await channel.DeleteMessagesAsync(message);
            _tts.Speak(_speechTemplates.GetDeleteLastMessage(channelName, guild.Name));
        }

        private async Task DeleteChannel(string channelName, string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            if (guild == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownGuild(guild.Name));
                return;
            }

            var channel = FindChannel(guild, channelName);
            if (channel == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownChannel(channelName, guild.Name));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetDeleteChannelExplicit(channelName, guild.Name));
                return;
            }

            await channel.DeleteAsync();
            _tts.Speak(_speechTemplates.GetDeleteChannel(channelName));
        }

        private async Task LeaveGuild(string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, _userNick);

            if (user == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownGuild(guild.Name));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetLeaveGuildExplicit(guild.Name));
                return;
            }

            await user.KickAsync();

            _tts.Speak(_speechTemplates.GetLeaveGuild(guild.Name));
        }

        private async Task RemoveBan(string userNameToRemBan, string guildNameToRemBan, string confidence)
        {
            var guild = FindGuild(guildNameToRemBan);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            var bans = await guild.GetBansAsync();
            var banToRemove = (RestBan) null;
            var user = (RestUser) null;
            foreach (RestBan ban in bans)
            {
                if (ban.User.Username.Equals(userNameToRemBan))
                {
                    banToRemove = ban;
                    user = ban.User;
                    break;
                }
            }

            if (banToRemove == null)
            {
                _tts.Speak(_speechTemplates.GetBanOnUnkwonUser(userNameToRemBan));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                _tts.Speak(_speechTemplates.GetRemoveBanExplicit(userNameToRemBan, guild.Name));
                return;
            }

            await channel.SendMessageAsync(_speechTemplates.GetRemoveBan(userNameToRemBan, guild.Name));
            await guild.RemoveBanAsync(user.Id);
            
        }

        private async void UserStatus(string userName, string guildName)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            if (user == null)
            {
                await channel.SendMessageAsync(_speechTemplates.GetUnkownUser(userName));
                return;
            }
            var status = user.Status;
            _tts.Speak(_speechTemplates.GetUserStatus(userName, status.ToString()));
        }

        private async Task ChangeMuteUser(string userNameToMute, string guildNameToMuteUser, string confidence)
        {
            var guild = FindGuild(guildNameToMuteUser);
            var user = FindUser(guild, userNameToMute);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;
            if (user == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownUser(userNameToMute));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                if (!user.IsMuted)
                    _tts.Speak(_speechTemplates.GetMuteExplicit(userNameToMute, guild.Name));
                else
                    _tts.Speak(_speechTemplates.GetUnMuteExplicit(userNameToMute, guild.Name));
                return;
            }
            var muted = user.IsMuted;
            await user.ModifyAsync(x => x.Mute = !muted);
            if (!muted)
                _tts.Speak(_speechTemplates.GetMuteUser(userNameToMute, guild.Name));
            else
                await channel.SendMessageAsync(_speechTemplates.GetUnMuteUser(userNameToMute, guild.Name));
        }

        private async Task ChangeSelfMute(string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, _userNick);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            var mute = !user.IsMuted;

            if (confidence.Equals("explicit confirmation"))
            {
                if (mute)
                    _tts.Speak(_speechTemplates.GetSelfMuteExplicit(guild.Name));
                else
                    _tts.Speak(_speechTemplates.GetSelfUnMuteExplicit(guild.Name));
                return;
            }

            await user.ModifyAsync(x => x.Mute = mute);
            if (mute)
                _tts.Speak(_speechTemplates.GetSelfMute(guild.Name));
            else
                await channel.SendMessageAsync(_speechTemplates.GetSelfUnMute(guild.Name));
        }

        private async Task ChangeDeafUser(string userNameToDeaf, string guildNameToDeafUser, string confidence)
        {
            var guild = FindGuild(guildNameToDeafUser);
            var user = FindUser(guild, userNameToDeaf);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            if (user == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownUser(userNameToDeaf));
                return;
            }

            var deaf = !user.IsDeafened;

            if (confidence.Equals("explicit confirmation"))
            {
                if (deaf)
                    _tts.Speak(_speechTemplates.GetDeafExplicit(userNameToDeaf, guild.Name));
                else
                    _tts.Speak(_speechTemplates.GetUnDeafExplicit(userNameToDeaf, guild.Name));
                return;
            }

            await user.ModifyAsync(x => x.Deaf = deaf);
            if (deaf)
                _tts.Speak(_speechTemplates.GetDeafUserImplicit(userNameToDeaf, guild.Name));
            else
                await channel.SendMessageAsync(_speechTemplates.GetUnDeafUserImplicit(userNameToDeaf, guild.Name));
        }
        private async Task SelfDeaf(string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild,_userNick);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;
            if (user.IsDeafened)
            {
                await user.ModifyAsync(x => x.Deaf = false);
                await channel.SendMessageAsync(_speechTemplates.GetSelfUnDeafImplicit());
            }

            else
            {
                await user.ModifyAsync(x => x.Deaf = true);
                _tts.Speak(_speechTemplates.GetSelfDeafImplicit());

            }
        }
        private async Task ChangeMuteDeafUser(string userNameToMuteDeaf, string guildNameToMuteDeafUser, bool muteDeaf, string confidence)
        {
            var guild = FindGuild(guildNameToMuteDeafUser);
            var user = FindUser(guild, userNameToMuteDeaf);
            SocketTextChannel channel = FindChannel(guild, "geral") as SocketTextChannel;

            if (user == null)
            {
                _tts.Speak(_speechTemplates.GetUnkownUser(userNameToMuteDeaf));
                return;
            }

            if (confidence.Equals("explicit confirmation"))
            {
                if (muteDeaf)
                    _tts.Speak(_speechTemplates.GetMuteDeafExplicit(userNameToMuteDeaf, guild.Name));
                else
                    _tts.Speak(_speechTemplates.GetUnMuteUnDeafExplicit(userNameToMuteDeaf, guild.Name));
                return;
            }

            await user.ModifyAsync(x => x.Deaf = muteDeaf);
            await user.ModifyAsync(x => x.Mute = muteDeaf);
            if (muteDeaf)
                _tts.Speak(_speechTemplates.GetMuteDeafImplicit(userNameToMuteDeaf, guild.Name));
            else
                await channel.SendMessageAsync(_speechTemplates.GetUnMuteDeafImplicit(userNameToMuteDeaf, guild.Name));
        }

        private SocketGuild FindGuild(string guildName) {
            if(guildName == null)
                return _client.GetGuild(_defaultGuildId);
            var guildsOfClient = _client.Guilds;
            var guildsFiltered = guildsOfClient.Where(g => g.Name.Equals(guildName));
            var guild = (SocketGuild)null;
            if (guildsFiltered.Count() == 0)
            {
                guild = _client.GetGuild(_defaultGuildId);
            }
            else
            {
                guild = guildsFiltered.First();
            }
            return guild;
        }

        private SocketGuildUser FindUser(SocketGuild guild , string userName) {
            foreach (var u in guild.Users)
            {
                if (u.Username.Equals(userName))
                    return u;
            }
            return null;
        }

        private SocketGuildChannel FindChannel(SocketGuild guild, string channelName) {
            if (channelName == null)
                return guild.GetChannel(_defaultChannelId);
            foreach (var c in guild.Channels) {
                if (c.Name != null && c.Name.Equals(channelName))
                    return c;
            }
            return null;
        }
       

    }
}
