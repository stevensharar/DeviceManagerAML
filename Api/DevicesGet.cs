using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;

namespace Api;

public class DevicesGet
{
    private readonly IDeviceData deviceData;

    public DevicesGet(IDeviceData deviceData)
    {
        this.deviceData = deviceData;
    }

    [FunctionName("DevicesGet")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")] HttpRequest req)
    {
        var devices = await deviceData.GetDevices();
        return new OkObjectResult(devices);
    }
}
