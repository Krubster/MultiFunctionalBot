using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class LoadBotTypeCommand : ICommandHandler
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
            return "Loads specified bot .dll into manager;";
        }

        public void handle(string[] args)
        {
            string typename = args[0];
            var DLL = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\Bots\\" + typename + ".dll");
            Assembly ass = AppDomain.CurrentDomain.Load(DLL.GetName());
            foreach (Type type in DLL.GetTypes())
            {
                if (type.BaseType == (typeof(Bot)))
                {
                    Program.AvailableBots.Add(type);
                    AppDomain.CurrentDomain.Load(DLL.GetName());
                    Console.WriteLine($"+ + {type} from {typename}.dll");
                }
            }
        }
    }
}
