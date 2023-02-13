namespace Cinnamon.Web.Models.Entities
{
    public class CustomerProfile
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsMaker { get; set; }
        public DateTime Birthdate { get; set; }
        public string ProfilePath { get; set; }
        public bool IsVerified { get; set; }
        public string About { get; set; }
        public string Handler {get; set;}

        public IList<FamilyMember> FamilyMembers {get; set;}
    }
}
