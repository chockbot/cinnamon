using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class DateOnwardsAttribute : ValidationAttribute 
{
    public override bool IsValid(object? value)
    {
        if(value == null) return false;

        var now = DateTime.Now;
        var date = Convert.ToDateTime(value);
        
        return date >= now;
    }
}