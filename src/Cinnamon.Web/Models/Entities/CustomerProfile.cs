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
        public string PhoneNumber { get; set; }
        public DateTime DateJoined { get; set; }
        public string ProfilePath { get; set; }
        public int IsVerified { get; set; }
        public DateTime IsVerifiedDate { get; set; }
        public bool IsOG { get; set; }
        public DateTime IsOGDate { get; set; }
        public bool IsOfficial { get; set; }
        public DateTime IsOfficialDate { get; set; }
        public string About { get; set; }
        public string ProfileImg { get; set; }
        public string Handler {get; set;}
        public string FrontIdImagePath { get; set; }
        public string BackIdImagePath { get; set; }
        public decimal TotalCredits {get; set;}
        public bool IsAccountBan { get; set; }

        public IList<FamilyMember> FamilyMembers {get; set;}

        public CustomerPricing Pricing {get; set;}

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public string HasVerificationRequest
        {
            get { return !string.IsNullOrEmpty(FrontIdImagePath) && !string.IsNullOrEmpty(BackIdImagePath) ? "Yes" : "No"; }
        }

        public class CustomerPricing 
        {
            public decimal Rate {get; set;}
            public bool IsManualPayment {get; set;}
            public bool InclusivePricing {get; set;}

            // extra fields
            public string ExtraClass {get ;set;}
            public bool IsSubmitting {get; set;}
        }
    }
}
