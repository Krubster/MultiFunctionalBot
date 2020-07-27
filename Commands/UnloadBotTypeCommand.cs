using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultiFunctionalBot.Commands
{
    public class UnloadBotTypeCommand : ICommandHandler
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
            return "Unloads specified bot type from assembly;";
        }

        public void handle(string[] args)
        {
            string typename = args[0];
            Type type = Type.GetType(typename);
            if (type != null)
            {
                if (Program.AvailableBots.Contains(type))
                {
                    ArrayList toRemove = new ArrayList();
                    foreach (Bot bot in Program.Bots)
                    {
                        if (bot.GetType() == type)
                        {
                            bot.Stop();
                            toRemove.Add(bot);
                        }
                    }
                    foreach (Bot bot in toRemove)
                    {
                        Program.Bots.Remove(bot);
                    }
                    Program.AvailableBots.Remove(type);
                }
            }
        }
    }
}