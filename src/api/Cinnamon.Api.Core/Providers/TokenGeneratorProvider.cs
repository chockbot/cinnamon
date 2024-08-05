using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Providers;

public class TokenGeneratorProvider : ITokenGeneratorProvider
{
    public TokenGeneratorResult Generator()
    {
        var validGuid = Guid.NewGuid();
        var validTimestamp = DateTime.UtcNow;
        
        byte[] validTtime = BitConverter.GetBytes(validTimestamp.ToBinary());
        byte[] validKey = validGuid.ToByteArray();
        var token = Convert.ToBase64String(validTtime.Concat(validKey).ToArray());
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        return new TokenGeneratorResult { Guid = validGuid.ToString(), Token = encodedToken };
    }
}

public interface ITokenGeneratorProvider 
{
    TokenGeneratorResult Generator();
}

public class TokenGeneratorResult 
{
    public string Token {get; set;}
    public string Guid {get; set;}
}