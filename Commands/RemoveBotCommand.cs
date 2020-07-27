using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    class RemoveBotCommand : ICommandHandler
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
            return "Remove bot;";
        }

        public void handle(string[] args)
        {
            string name = args[0];
            Bot bot = Program.GetBot(name);
            if (bot != null)
                Program.Bots.Remove(bot);
            else
                Console.WriteLine("Wrong name!");
        }
    }
}
