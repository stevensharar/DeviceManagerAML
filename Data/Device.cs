namespace Data
{
    public class Device
    {
        public int Index { get; set; }
        public string ?DeviceId { get; set; }
        public string ?Model { get; set; }
        public bool Registered { get; set; }
        public bool On { get; set; }
        public double Battery { get; set; }
        public string ?IpAddress { get; set; }
        public string ?Version { get; set; }
        public string ?WifiMac { get; set; }
        public string ?BtMac { get; set; }
        public List<string> ?Application { get; set; }
        public List<string> ?Files { get; set; }
    }
}