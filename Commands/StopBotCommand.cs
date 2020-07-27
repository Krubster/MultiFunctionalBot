using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class StopBotCommand : ICommandHandler
    {
        public int Args
        {
            get
            {
                return 1;
            }
        }

        public string GetDescription()
        {
            return "Stop bot;";
        }

        public void handle(string[] args)
        {
            string name = args[0];
            foreach (Bot bot in Program.Bots)
            {
                if (bot.Name == name)
                {
                    Program.Log($"Stopped {name}");
                    bot.Status = BotStatus.Stopped;
                }
            }
            Program.Log($"Wrong name: {name}");

        }
    }
}
