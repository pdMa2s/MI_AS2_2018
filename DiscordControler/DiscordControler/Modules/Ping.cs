using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordControler.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        //[Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Ao seu serviço");
        }
    }
}
