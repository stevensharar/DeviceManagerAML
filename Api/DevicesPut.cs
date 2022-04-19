using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Data;

namespace Api;

public class DevicesPut
{
    private readonly IDeviceData deviceData;

    public DevicesPut(IDeviceData deviceData)
    {
        this.deviceData = deviceData;
    }

    [FunctionName("DevicesPut")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "devices")] HttpRequest req,
        ILogger log)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var device = JsonSerializer.Deserialize<Device>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var updatedDevice = await deviceData.UpdateDevice(device);
        return new OkObjectResult(updatedDevice);
    }
}
