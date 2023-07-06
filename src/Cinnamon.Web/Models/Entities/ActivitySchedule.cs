using System.Text.RegularExpressions;

namespace Cinnamon.Web.Models.Entities;

public class ActivitySchedule 
{

    public int Id {get; set;}
    public int ActivityId {get; set; }
    private string _name;

    public string Name
    {
        get {
            if (!string.IsNullOrEmpty(_name))
            {
                string input = _name;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _name; 
        }
        set { _name = value; }
    }

    private string _dateTime;

    public string DateTime
    {
        get {
            if (!string.IsNullOrEmpty(_dateTime))
            {
                string input = _dateTime;
                string result = MaskEmail(input);

                result = MaskPhone(result);

                return result;
            }

            return _dateTime; 
        }
        set { _dateTime = value; }
    }

    public decimal Price {get; set;}
    public string UnitPrice {get; set;} = "PHP";
    public int PerUnit1 {get; set;}
    public string PriceUnit1 {get; set;} = "Head";
    public int PerUnit2 {get; set;}
    public string PriceUnit2 {get; set;} = "Session";
    public int TempId {get; set;}
    public int Order {get; set;}
    public bool IsActiveSchedule { get; set; } = true;
    public bool IsSetSession { get; set; } = false;
    public string SessionName { get; set; }
    public int HasExpiration { get; set; } = 0;
    public DateTime? StartDate { get; set; }

    public long LongPrice { 
        get{
            return (long)Price;
        }
        set {
            Price = value;
        }
    }
    public bool IsNew { get; set; }

    string MaskEmail(string input)
    {
        string pattern = @"([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})|((?i)\b((?:https?://|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'\""\.,<>?«»“”‘’]))\b)";
        return Regex.Replace(input, pattern, m => new string('*', m.Length));
    }

    string MaskPhone(string input)
    {
        string pattern = @"(\(?\d{3}\)?-? *\d{3}-? *-?\d{4})";
        return Regex.Replace(input, pattern, m => new string('*', m.Length));
    }
    
}