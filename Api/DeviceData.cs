using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Data;

namespace Api;

public interface IDeviceData
{
    Task<Device> AddDevice(Device device);
    Task<bool> DeleteDevice(int id);
    Task<IEnumerable<Device>> GetDevices();
    Task<Device> UpdateDevice(Device device);
}

public class DeviceData : IDeviceData
{
    private readonly CosmosClient cosmosClient = new CosmosClient("https://aml-device-manager.documents.azure.com:443/", "MafSZSwJUjD1xkS2jv6JMEvgcFVIQgoFKGhmqNBeCSNzXLVOk2uH98MouvjvTOYtXJ4kg5QOBIsOMDDYfse4Qw==");

    private int GetRandomInt()
    {
        var random = new Random();
        return random.Next(100, 1000);
    }

    public async Task<Device> AddDevice(Device device)
    {
        Container deviceContainer = cosmosClient.GetContainer("DeviceManagerDatabase", "Devices");
        device.Index = GetRandomInt();
        ItemResponse<Device> response = await deviceContainer.CreateItemAsync(device);
        return device;
    }

    public async Task<Device> UpdateDevice(Device device)
    {
        Container deviceContainer = cosmosClient.GetContainer("DeviceManagerDatabase", "Devices");
        ItemResponse<Device> response = await deviceContainer.DeleteItemAsync<Device>(device.DeviceId, new PartitionKey(device.DeviceId));
        ItemResponse<Device> itemResponse = await deviceContainer.CreateItemAsync<Device>(device, new PartitionKey(device.DeviceId));
        return device;
    }

    public async Task<bool> DeleteDevice(int id)
    {
        bool exists = false;
        string valId = null;
        Container deviceContainer = cosmosClient.GetContainer("DeviceManagerDatabase", "Devices");
        FeedIterator<Device> iter = deviceContainer.GetItemQueryIterator<Device>();
        while (iter.HasMoreResults)
        {
            FeedResponse<Device> response = await iter.ReadNextAsync();
            foreach (var prod in response)
            {
                if (prod.Index == id)
                {
                    exists = true;
                    valId = prod.DeviceId;
                }
            }
        }
        if (exists && valId != null)
        {
            ItemResponse<Device> dresponse = await deviceContainer.DeleteItemAsync<Device>(valId, new PartitionKey(valId));
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Device>> GetDevices()
    {
        List<Device> deviceList = new List<Device>();
        Container deviceContainer = cosmosClient.GetContainer("DeviceManagerDatabase", "Devices");
        FeedIterator<Device> iter = deviceContainer.GetItemQueryIterator<Device>();
        while (iter.HasMoreResults)
        {
            FeedResponse<Device> response = await iter.ReadNextAsync();
            foreach (var prod in response)
            {
                deviceList.Add(prod);
            }
        }
        return deviceList.AsEnumerable();
    }
}
