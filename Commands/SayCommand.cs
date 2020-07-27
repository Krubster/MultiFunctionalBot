using System.IO;

namespace MultiFunctionalBot.Commands
{
    public class SayCommand : ICommandHandler
    {
        public int Args
        {
            get
            {
                return 3;
            }
        }

        public string GetDescription()
        {
            return "Say message with specified bot;";
        }

        public void handle(string[] args)
        {
            string name = args[0];
            string type = args[1];
            string msg = "";
            if (type == "raw")
                msg = args[2];
            else if(type == "file" && File.Exists("Files\\" + args[2]))
            {
                msg = File.ReadAllText("Files\\" + args[2]);
            }
            Bot bot = Program.GetBot(name);
            if (bot != null)
                bot.SendMessage(bot.FormatMessage(msg));
        }
    }
}
