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

public class DevicesPost
{
    private readonly IDeviceData deviceData;

    public DevicesPost(IDeviceData deviceData)
    {
        this.deviceData = deviceData;
    }

    [FunctionName("DevicesPost")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices")] HttpRequest req,
        ILogger log)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var device = JsonSerializer.Deserialize<Device>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var newDevice = await deviceData.AddDevice(device);
        return new OkObjectResult(newDevice);
    }
}

