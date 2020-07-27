using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    class SaveCommand : ICommandHandler
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
            return "Save configuration and bots;";
        }

        public void handle(string[] args)
        {
            Program.Save();
        }
    }
}
