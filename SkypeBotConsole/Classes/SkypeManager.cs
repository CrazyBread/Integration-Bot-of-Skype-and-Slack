using Interop.SKYPE4COMLib;
using SkypeBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SkypeBotConsole
{
    static class SkypeManager
    {
        private static Skype _skype = new Skype();
        private static string _SkypeChatUniqueCode = String.Empty;
        private static string _BotSkypeName = String.Empty;

        static SkypeManager() {
            _SkypeChatUniqueCode = ConfigurationManager.AppSettings["SkypeChatUniqueCode"];
            _BotSkypeName = ConfigurationManager.AppSettings["SkypeBotName"];
        }

        public static void AttachSkype()
        {
            try
            {
                _skype.MessageStatus += _OnMessageReceived;
                _skype.Attach(5, true);
                Console.WriteLine("skype attached");
            }
            catch (Exception ex)
            {
                Console.WriteLine("top lvl exception : " + ex.ToString());
            }
        }

        private static void _OnMessageReceived(ChatMessage pMessage, TChatMessageStatus status)
        {
            if ((status == TChatMessageStatus.cmsReceived || status == TChatMessageStatus.cmsSent) && pMessage.ChatName.IndexOf(_SkypeChatUniqueCode) >= 0)
            {
                SlackSender.SendMessage("*" + (String.IsNullOrEmpty(pMessage.Sender.DisplayName) ? _BotSkypeName : pMessage.Sender.DisplayName) + "* : " + pMessage.Body);
            }
        }
    }
}
