using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    class ReloadAssemblyCommand : ICommandHandler
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
            return "Reloads \\Libs folder;";
        }

        public void handle(string[] args)
        {
            Program.LoadLibs();
        }
    }
}
