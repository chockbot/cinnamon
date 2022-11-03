namespace Cinnamon.Core.Module.EmailService.Handler.MicrosoftGraph.ResponseObject;

public class BodyPayload
{
    public Message message { get; set; } = new Message();

    public class Message 
    {
        public string subject { get; set; }
        public Body body { get; set; } = new Body();
        public IList<Recipients> toRecipients { get; set; }
        public From from { get; set; }
    }

    public class Body 
    {
        public string contentType { get; set; }
        public string content { get; set;}
    }

    public class Recipients
    {
        public EmailAddress emailAddress { get; set; }
    }

    public class EmailAddress {
        public string address { get; set; }
    }

    public class From 
    {
        public EmailAddress emailAddress { get; set; }
    }
}