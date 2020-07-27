using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class StopCommand: ICommandHandler
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
            return "Stop the program;";
        }

        public void handle(string[] args)
        {
            Program.Stop();
        }
    }
}
