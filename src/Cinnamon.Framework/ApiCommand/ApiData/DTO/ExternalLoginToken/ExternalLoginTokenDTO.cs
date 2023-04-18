namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.ExternalLoginToken
{
    public class ExternalLoginTokenDTO
    {
        public int Id { get; set; }
        public string Token {get; set;}
        public string Guid {get; set;}
        public bool IsUsed {get; set;}
        public string Email {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public DateTime DateGenerated {get; set;}
    }
}
