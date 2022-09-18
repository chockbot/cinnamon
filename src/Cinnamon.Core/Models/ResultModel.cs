namespace Cinnamon.Core
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

        public static ResultModel error(string msg) {
            ResultModel result = new ResultModel(); 
            result.setMessage(MessageType.Error, msg);
            return result;
        }

        public static ResultModel success(string msg) {
            ResultModel result = new ResultModel();
            result.setMessage(MessageType.Success, msg);
            return result;
        }
    }
}
