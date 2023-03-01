namespace Cinnamon.Api.Core.Config;

public class Payment 
{
    public bool Active {get; set;}
    public IEnumerable<Account> Accounts {get; set;}
    
    public class Account 
    {
        public bool Active {get; set;}
        public string Name {get; set;}
        public IEnumerable<Setting> Settings {get; set;}
        public IEnumerable<PaymentMethod> PaymentMethods {get; set;}

        public class Setting 
        {
            public string Name {get; set;}
            public string Value {get; set;}
        }

        public class PaymentMethod 
        {
            public string Name {get; set;}
            public bool Active {get; set;}
            public string Driver {get; set;}
            public IEnumerable<Channel> Channels {get; set;}

            public class Channel
            {
                public string Name {get; set;}
                public string Code {get; set;}
                public bool Active {get; set;}
                public string Type {get; set;}
            }
        }
    }
}