using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;

public interface IGenerateResponseHandler : IInteractorHandler<GenerateResponseArgs,AppResult<GenerateResponseResult>>  {}