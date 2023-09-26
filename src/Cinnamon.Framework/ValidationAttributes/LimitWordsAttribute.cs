using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ValidationAttributes;

public class LimitWordsAttribute : ValidationAttribute 
{
    public int Max {get; set;} = 0;
    public int Min {get; set;} = 0;

    public override bool IsValid(object? value)
    {
        if(value == null) return false;

        string sentence = value.ToString() ?? string.Empty;

        var words = sentence.Split(" ").Where(s => !string.IsNullOrEmpty(s));

        return words.Count() <= Max && words.Count() >= Min;
    }
}