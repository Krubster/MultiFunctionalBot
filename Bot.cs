using System;
using System.Xml;
using System.Xml.Serialization;

namespace MultiFunctionalBot
{
    public enum BotStatus
    {
        Idle,
        Running,
        Stopped,
    }

    [XmlRoot("Bots")]
    public abstract class Bot
    {
        string m_Name;
        BotStatus m_Status;

        [XmlElement]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [XmlElement]
        public BotStatus Status { get { return m_Status; } set { m_Status = value;  } }

        public Bot() { }

        public abstract void Greet();
        public abstract void Start();
        public abstract string FormatMessage(string raw);
        public abstract void SendMessage(string message);
        public abstract void Stop();
    }
}
