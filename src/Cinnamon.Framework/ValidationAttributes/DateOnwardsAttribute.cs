using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class DateOnwardsAttribute : ValidationAttribute 
{
    public bool Strict {get; set;} = true;

    public override bool IsValid(object? value)
    {
        if(value == null) return false;

        var now = DateTime.Now;
        var date = Convert.ToDateTime(value);

        if(!Strict)
        {
            now = now.Date;
            date = date.Date;
        }

        return date >= now;
    }
}