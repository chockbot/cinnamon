namespace Cinnamon.Core.Models
{
    public class ResultModel
    {
        public string? Message { get; private set; }
        public MessageType Type { get; private set; }
        public string? Id { get; private set; }

        public void setMessage(MessageType t, string msg, string? id = null)
        {
            Type = t;
            Message = msg;
        }
    }
}
