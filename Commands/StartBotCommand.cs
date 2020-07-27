using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class StartBotCommand : ICommandHandler
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
            return "Start bot;";
        }

        public void handle(string[] args)
        {
            string name = args[0];
            foreach (Bot bot in Program.Bots)
            {
                if (bot.Name == name)
                {
                    Program.Log($"Started {name}");
                    bot.Status = BotStatus.Running;
                    return;
                }
            }
            Program.Log($"Wrong name: {name}");

        }
    }
}
