using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Interactors
{
    public class UpdateRequestRefundArgs : IInteractor
    {
        public int RefundId { get; set; }
        public int Status { get; set; }
        public decimal? RefundAmountGiven { get; set; }
    }
}
