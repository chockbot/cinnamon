using Cinnamon.Api.Core.Services.SystemService.Interactors;
using Cinnamon.Api.Core.Services.SystemService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SystemService.Handlers;

public interface IGenerateSitemapHandler : IInteractorHandler<GenerateSitemapArgs,AppResult<GenerateSitemapResult>>
{}