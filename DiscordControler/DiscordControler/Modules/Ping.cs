using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordControler.Modules
{
    [Group("ping")]
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultPing()
        {
            //await ReplyAsync("Ao seu serviço");
            //await ReplyAsync($"User: {Context.User}");
            //await ReplyAsync($"User: {Context.Guild.Name}");
            //await ReplyAsync($"User: {Context.Client.CurrentUser}");
            //await ReplyAsync($"User: {Context.Message.Content}");
            await ReplyAsync("Pong!");
        }

        [Command("user"), RequireOwner]
        public async Task PingUser(SocketGuildUser user) {
            await ReplyAsync($"Olá {user.Mention}");
        }
    }
}
