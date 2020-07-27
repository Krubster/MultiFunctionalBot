using System;
using MultiFunctionalBot;
using System.Threading.Tasks;
using Discord;
using System.Xml.Serialization;
using Discord.WebSocket;
using Discord.Net.Providers.WS4Net;

namespace MultiFunctionalBot.Bots
{
    [Serializable]
    [XmlInclude(typeof(Bot))]
    public class DiscordBot : Bot
    {
        string m_Key;
        ulong m_Channel;

        [NonSerialized]
        DiscordSocketClient m_Client;

        [XmlElement]
        public string Key { get { return m_Key; } set { m_Key = value; } }

        [XmlElement]
        public ulong AnnounceChannel { get { return m_Channel; } set { m_Channel = value; } }

        public DiscordBot()
        {

        }

        public DiscordBot(string key)
        {
            Key = key;
        }
        
        public override void Greet()
        {
            Console.WriteLine($"[DiscordBot-{Name}]: Hello! I'm the {Name} discord bot!");
        }

        public override void SendMessage(string message)
        {
            (m_Client.GetChannel(AnnounceChannel) as SocketTextChannel).SendMessageAsync(message);
        }

        public override void Start() => MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            m_Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                WebSocketProvider = WS4NetProvider.Instance
            });

           
            await m_Client.LoginAsync(TokenType.Bot, m_Key);
            await m_Client.StartAsync();
            m_Client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };
        }


        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            
            return Task.CompletedTask;
        }

        public override void Stop()
        {
            m_Client.StopAsync();
        }

        public override string FormatMessage(string raw)
        {
            string new_str = raw;
            new_str = new_str.Replace("[b]", "**");
            new_str = new_str.Replace("[/b]", "**");

            new_str = new_str.Replace("[i]", "*");
            new_str = new_str.Replace("[/i]", "*");

            new_str = new_str.Replace("[u]", "__");
            new_str = new_str.Replace("[/u]", "__");

            new_str = new_str.Replace("[s]", "~~");
            new_str = new_str.Replace("[/s]", "~~");

            new_str = new_str.Replace("[ui]", "__*");
            new_str = new_str.Replace("[/ui]", "*__");

            new_str = new_str.Replace("[ub]", "__**");
            new_str = new_str.Replace("[/ub]", "**__");

            new_str = new_str.Replace("[ubi]", "__***");
            new_str = new_str.Replace("[/ubi]", "***__");

            new_str = new_str.Replace("[c]", "'''");
            new_str = new_str.Replace("[/c]", "'''");

            new_str = new_str.Replace("[cs]", "'");
            new_str = new_str.Replace("[/cs]", "'");

            return new_str;
        }
    }
}
