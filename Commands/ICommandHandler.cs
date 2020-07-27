using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot
{
    interface ICommandHandler
    {
        string GetDescription();
        int Args { get; }
        void handle(string[] args);
    }
}
