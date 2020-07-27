using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class HelpCommand : ICommandHandler
    {
        public int Args
        {
            get
            {
                return 0;
            }
        }

        public string GetDescription()
        {
            return "Shows the list of available commands;";
        }

        public void handle(string[] args)
        {
            Program.Log("List of available commands:");
            foreach (string command in Program.Commands.Keys)
            {
                Console.WriteLine($" + {command} - {Program.Commands[command].GetDescription()}");
            }
            Console.WriteLine("+ + + + +");

        }
    }
}
