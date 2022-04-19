using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api;

public class DevicesDelete
{
    private readonly IDeviceData deviceData;

    public DevicesDelete(IDeviceData deviceData)
    {
        this.deviceData = deviceData;
    }

    [FunctionName("DevicesDelete")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "devices/{deviceId:int}")] HttpRequest req,
        int deviceId,
        ILogger log)
    {
        var result = await deviceData.DeleteDevice(deviceId);

        if (result)
        {
            return new OkResult();
        }
        else
        {
            return new BadRequestResult();
        }
    }
}
