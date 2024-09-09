
using Cinnamon.Framework.Enums;
using System.Text.RegularExpressions;

namespace Cinnamon.Web.Models.Entities
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromLastName { get; set; }
        public string FromFirstName { get; set; }
        public string ToLastName { get; set; }
        public string ToFirstName { get; set; }
        public DateTime DateCreated { get; set; }
        private string _message;

        public string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(_message))
                {
                   /* string input = _message;
                    string result = MaskEmail(input);

                    result = MaskPhone(result);

                    return result; */
                }

                return _message;
            }
            set { _message = value; }
        }
        public string? FromProfilePath { get; set; }
        public string? ToProfilePath { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
        public bool HasNewMessage { get; set; }
        public string FromProfileLink { get; set; }
        public string TypingStatus { get; set; }
        public Framework.Enums.Enums.ChatType ChatType { get; set; }
        public string GroupName { get; set; }
        public string ChatName { get; set; }

       /* string MaskEmail(string input)
        {
            string pattern = @"([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})|((?i)\b((?:https?://|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'\""\.,<>?«»“”‘’]))\b)";
            return Regex.Replace(input, pattern, m => new string('*', m.Length));
        }

        string MaskPhone(string input)
        {
            string pattern = @"(\(?\d{3}\)?-? *\d{3}-? *-?\d{4})";
            return Regex.Replace(input, pattern, m => new string('*', m.Length));
        } */
        
        public bool HasTemporaryChatRoomId { get; set; }
    }
}
