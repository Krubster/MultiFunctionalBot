using System;

namespace MultiFunctionalBot.Commands
{
    class ListBotCommand : ICommandHandler
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
            return "Show the list of run bots;";
        }

        public void handle(string[] args)
        {
            Console.WriteLine("Bots status:");
            foreach(Bot bot in Program.Bots)
            {
                Console.WriteLine($" - {bot.Name} (Status: {bot.Status})");
            }
        }
    }
}
