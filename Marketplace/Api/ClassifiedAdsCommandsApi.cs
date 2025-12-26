using Marketplace.Application;
using Marketplace.Framework;
using Microsoft.AspNetCore.Mvc;
using static Marketplace.Application.Contracts.ClassifiedAds;

namespace Marketplace.Api;

[Route("ad")]
public class ClassifiedAdsCommandsApi : Controller
{
    private readonly IApplicationService _applicationService;

    private static ILogger Log = (ILogger)Serilog.Log.ForContext<ClassifiedAdsCommandsApi>();

    public ClassifiedAdsCommandsApi(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }


    [HttpPost]
    public async Task<IActionResult> Post(V1.Create request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [HttpPut("name")]
    public async Task<IActionResult> Put(V1.SetTitle request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [HttpPut("text")]
    public async Task<IActionResult> Put(V1.UpdateText request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [HttpPut("price")]
    public async Task<IActionResult> Put(V1.UpdatePrice request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [HttpPut("publish")]
    public async Task<IActionResult> Put(V1.RequestToPublish request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    private async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler)
    {
        try
        {
            Log.LogDebug("Handlign HTTP request of type {type}", typeof(T).Name);
            await handler(request);
            return Ok();
        }
        catch (Exception e)
        {
            Log.LogError("Error handling the request", e);
            return new BadRequestObjectResult (new { error = e.Message, stackTrace = e.StackTrace });
        }
    }
}
