using System;
using System.Collections;

namespace MultiFunctionalBot.Commands
{
    public class AddBotCommand : ICommandHandler
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
            return "Adds a specified bot;";
        }

        public void handle(string[] args)
        {
            string typename = args[0];
            Type type = Program.GetBotType(typename);

            string bot_name = args[1];

            if (Program.HasBot(bot_name))
            {
                Program.Log("Bot with that name already exists!");
                return;
            }

            //Getting ctor arguments
            object[] ctor = null;
            if (args.Length > 2)
            {
                ctor = new object[args.Length - 2];

                for (int i = 2; i < args.Length; ++i)
                {
                    int n;
                    var isNumeric = int.TryParse(args[i], out n);
                    if (isNumeric)
                        ctor[i - 2] = n;
                    else
                        ctor[i - 2] = args[i];
                }
            }
            if (type != null)
            {
                try
                {
                    Bot bot = Activator.CreateInstance(type, ctor) as Bot;
                    bot.Name = bot_name;
                    bot.Status = BotStatus.Idle;

                    if (bot != null)
                    {
                        Program.AddBot(bot);
                        bot.Greet();
                        Program.Log("Bot Added!");
                    }
                }
                catch (MissingMethodException e)
                {
                    Program.Log("Wrong constructor! Available:");
                    Program.ListCtors(type);
                }
            }
            else
            {
                Program.Log("Unknown Type! Check out available bot types!");
                Program.ListBots();
            }
        }
    }
}
