using Disqord;
using Qmmands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abyss
{
    [Name("Help")]
    public class HelpModule : AbyssModuleBase
    {
        private readonly HelpService _help;
        private readonly AbyssBot _bot;

        public HelpModule(HelpService help, AbyssBot bot)
        {
            _bot = bot;
            _help = help;
        }

        public async Task CommandSubroutine_HelpQueryAsync(string query)
        {
            // Search for command
            var search = _bot.FindCommands(query).ToList();
            if (search.Count == 0)
            {
                await ReplyAsync($"No command or command group found for `{query}`.");
                return;
            }

            await ReplyAsync(embed: await _help.CreateCommandEmbedAsync(search[0].Command, Context));
        }

        [Command("help", "commands")]
        [Description(
            "Retrieves a list of commands that you can use, or, if a command or module is provided, displays information on that command or module.")]
        public async Task Command_ListCommandsAsync(
            [Name("Query")]
            [Description("The command or module to view, or nothing to see a list of commands.")]
            [Remainder]
            string query = null)
        {
            if (query != null)
            {
                await CommandSubroutine_HelpQueryAsync(query);
                return;
            }

            var prefix = Context.Prefix;
            ;

            var embed = new LocalEmbedBuilder {Color = GetColor()};

            embed.WithTitle("Help listing for " + Context.BotMember.Format());

            embed.WithDescription(
                $"Listing all commands. Commands that are above your access level are hidden. You can use `{prefix}help <command/group>` for more details on a command or group.");

            var commands = new List<string>();
            foreach (var command in _bot.GetAllCommands())
            {
                if (await HelpService.CanShowCommandAsync(Context, command))
                {
                    var format = HelpService.FormatCommandShort(command);
                    if (format != null && !commands.Contains(format)) commands.Add(format);
                }
            }

            if (commands.Count != 0)
                embed.AddField("Commands", string.Join(", ", commands));

            await ReplyAsync(embed: embed);
        }
    }
}