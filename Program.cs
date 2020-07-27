using MultiFunctionalBot.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace MultiFunctionalBot
{
    class Program
    {
        static bool m_run = true;
        static MBConfiguration m_Config;
        private static Dictionary<string, ICommandHandler> m_commands = new Dictionary<string, ICommandHandler>();

        public static Dictionary<string, ICommandHandler> Commands { get { return m_commands; } }

        public static void Log(string msg)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} [MFBot]: {msg}");
        }

        internal static Type GetBotType(string typename)
        {
            foreach (Type type in m_AvailableBots)
            {
                if (type.Name == typename)
                    return type;
            }
            return null;
        }

        internal static bool HasBot(string bot_name)
        {
            foreach (Bot bot in Bots)
            {
                if (bot.Name == bot_name)
                    return true;
            }
            return false;
        }

        internal static Bot GetBot(string name)
        {
            foreach (Bot bot in Bots)
            {
                if (bot.Name == name)
                    return bot;
            }
            return null;
        }
        private static readonly string BOTS_PATH = "Bots";
        private static readonly string LIBS_PATH = "Libs";

        private static readonly string CONFIG = "config.xml";
        private static ArrayList m_AvailableBots = new ArrayList();

        public static ArrayList AvailableBots { get { return m_AvailableBots; } }
        public static List<Bot> Bots { get { return m_Config.m_Run_Bots; } }

        public static void Stop()
        {
            m_run = false;
        }

        static void Main(string[] args)
        {
            Log("Starting Multi funcitonal Bot!");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            m_commands.Add("stop", new StopCommand());

            m_commands.Add("add", new AddBotCommand());
            m_commands.Add("botlist", new ListBotCommand());
            m_commands.Add("startbot", new StartBotCommand());
            m_commands.Add("stopbot", new StopBotCommand());
            m_commands.Add("removebot", new RemoveBotCommand());

            m_commands.Add("save", new SaveCommand());

            m_commands.Add("say", new SayCommand());
            m_commands.Add("broadcast", new BroadcastCommand());

            m_commands.Add("help", new HelpCommand());

            m_commands.Add("load", new LoadBotTypeCommand());
            m_commands.Add("reload_asm", new ReloadAssemblyCommand());
            m_commands.Add("unload", new UnloadBotTypeCommand());

            LoadLibs();
            GatherBotInformation();
            TryLoadBots();

            foreach(Bot bot in Bots)
            {
                if (bot.Status == BotStatus.Running)
                    bot.Start();
                else if (bot.Status == BotStatus.Stopped)
                    bot.Stop();
            }

            if (!Directory.Exists("Files"))
                Directory.CreateDirectory("Files");

            while (m_run)
            {
                string command = Console.ReadLine();
                string[] arr = command.Split(' ');
                if (arr.Length > 0 && m_commands.ContainsKey(arr[0]))
                {
                    string[] finalargs = ProcessArgs(command);
                    if (finalargs.Length >= m_commands[arr[0]].Args)
                        m_commands[arr[0]].handle(finalargs);
                    else
                        Log("Wrong parameters!");

                }
                else
                {
                    Log("Unknown Command!");
                }
            }
        }
        public static IDictionary<string, Assembly> Additional { get { return additional; } }

        private static IDictionary<string, Assembly> additional =
    new Dictionary<string, Assembly>();
        private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly res;
            additional.TryGetValue(args.Name.Split(',')[0], out res);
            return res;
        }

        internal static void ListBots()
        {
            foreach (Type type in m_AvailableBots)
            {
                Console.WriteLine($" - {type.Name}");
            }
        }

        internal static void ListCtors(Type type)
        {
            ConstructorInfo[] ctors = type.GetConstructors();
            foreach (ConstructorInfo info in ctors)
            {
                Console.Write(" -");
                foreach (ParameterInfo arg in info.GetParameters())
                {
                    Console.Write($" {arg.ParameterType} {arg.Name}, ");

                }
                Console.WriteLine();
            }
        }

        public static void LoadLibs()
        {
            if (!Directory.Exists(LIBS_PATH))
                Directory.CreateDirectory(LIBS_PATH);
            foreach (string filename in Directory.GetFiles(LIBS_PATH))
            {
                string[] arr = filename.Split('.');
                if (arr.Length > 0 && arr[arr.Length - 1] == "dll")
                {
                    var DLL = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + filename);

                    try
                    {
                        Assembly ass = AppDomain.CurrentDomain.Load(DLL.GetName());
                        if (!Additional.ContainsKey(ass.GetName().Name))
                        {
                            additional.Add(ass.GetName().Name, ass);
                            Console.WriteLine($"Injected {ass.GetName().Name}");
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
            }
        }
        private static void GatherBotInformation()
        {
          
            try
            {
                Log("Loading bots...");

                if (!Directory.Exists(BOTS_PATH))
                    Directory.CreateDirectory(BOTS_PATH);
                foreach (string filename in Directory.GetFiles(BOTS_PATH))
                {
                    string[] arr = filename.Split('.');
                    if (arr.Length > 0 && arr[arr.Length - 1] == "dll")
                    {
                        var DLL = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + filename);

                        try
                        {
                            foreach (Type type in DLL.GetTypes())
                            {
                                if (type.BaseType == (typeof(Bot)))
                                {
                                    m_AvailableBots.Add(type);
                                    AppDomain.CurrentDomain.Load(DLL.GetName());
                                    Console.WriteLine($"+ + {type} from {filename}");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Log($"Loaded {m_AvailableBots.Count} bots!");
        }

        static string[] ProcessArgs(string command)
        {
            ArrayList list = new ArrayList();

            if (command.IndexOf(' ') >= 0)
            {
                string raw_args = command.Substring(command.IndexOf(' '));
                string word = "";
                bool ignoreSpace = false;
                for (int i = 0; i < raw_args.Length; ++i)
                {
                    if (raw_args[i] == ' ' && !ignoreSpace)
                    {
                        if (word != "") //to avoid multiple spaces
                        {
                            list.Add(word);
                            word = "";
                        }
                    }
                    else if (raw_args[i] == '\"')
                    {
                        if (ignoreSpace) //End of string
                        {
                            ignoreSpace = false;
                            list.Add(word);
                            word = "";
                        }
                        else //begin of string
                        {
                            ignoreSpace = true;
                        }
                    }
                    else
                    {
                        word += raw_args[i];
                    }
                }
                if (word != "")
                    list.Add(word);
                //foreach (string str in list)
                //     Console.WriteLine("Arg: " + str);
            }
            return (string[])list.ToArray(typeof(string));
        }

        static void TryLoadBots()
        {
            if (!File.Exists(CONFIG))
            {
                Log("Null configuration file! Creating blank one");
                m_Config = new MBConfiguration();
                m_Config.m_Run_Bots = new List<Bot>();
                return;
            }
            FileStream stream = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(MBConfiguration), (Type[])m_AvailableBots.ToArray(typeof(Type)));
                stream = new FileStream(CONFIG, FileMode.Open);
                m_Config = (MBConfiguration)ser.Deserialize(stream);
                Log($"Success! Loaded {m_Config.m_Run_Bots.Count} run bots");

                stream.Close();
            }
            catch (InvalidOperationException e)
            {
                Log("Something wrong with your config.xml! Back it up and create a blank one...");
                Console.WriteLine($"Error: {e.Message}");
                m_Config = new MBConfiguration();
                m_Config.m_Run_Bots = new List<Bot>();
                stream.Close();
                return;
            }
        }

        public static void AddBot(Bot bot)
        {
            m_Config.m_Run_Bots.Add(bot);
        }

        internal static void Save()
        {
            FileStream stream = null;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(MBConfiguration), (Type[])m_AvailableBots.ToArray(typeof(Type)));
                stream = new FileStream(CONFIG, FileMode.Create);
                ser.Serialize(stream, m_Config);
                stream.Close();
                Log("Saved!");
            }
            catch (Exception ec)
            {
                stream.Close();
                Log("Error saving config.xml!");
            }
        }
    }

    public struct MBConfiguration
    {
        [XmlArray("Bots"), XmlArrayItem(typeof(Bot), ElementName = "bot")]
        public List<Bot> m_Run_Bots;
    }
}
