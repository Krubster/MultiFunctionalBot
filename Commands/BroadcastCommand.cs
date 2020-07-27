using System;
using System.IO;

namespace MultiFunctionalBot.Commands
{
    class BroadcastCommand : ICommandHandler
    {
        public int Args
        {
            get
            {
                return 2;
            }
        }

        public string GetDescription()
        {
            return "Boradcast message on all run bots;";
        }

        public void handle(string[] args)
        {
            string type = args[0];
            string msg = "";
            if (type == "raw")
                msg = args[1];
            else if (type == "file" && File.Exists("Files\\" + args[1]))
            {
                msg = File.ReadAllText("Files\\" + args[1]);
            }
            foreach (Bot bot in Program.Bots)
            {
                bot.SendMessage(bot.FormatMessage(msg));
            }
        }
    }
}
