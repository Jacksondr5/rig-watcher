using System.Collections.Generic;

namespace rig_watcher.console
{
    internal class TimeResponse 
    {
        public string ServerTime { get;set;} = "";
    }

    internal class GetRigsResponse
    {
        public string GroupName { get; set; } = "";
        public List<MiningRig> MiningRigs { get; set; } = new();
    }

    internal class MiningRig
    {
        public string RigId {get;set;} = "";
        public string Name {get;set;} = "";
        public string MinerStatus {get;set;} = "";
        public List<Device> Devices {get;set;} = new();
    }

    internal class Device
    {
        public string Id {get;set;} = "";
        public string Name {get;set;} = "";
        public DeviceStatus Status {get;set;} = new();
    }

    internal class DeviceStatus
    {
        public string EnumName {get;set;} = "";
        public string Description {get;set;} = "";
    }

    internal class RigActionRequest
    {
        public string rigId {get;set;} = "";
        public string action {get;set;} = "";
    }

    internal class RigActionResponse
    {
        public bool Success {get;set;} = false;
        public string SuccessType {get;set;} = "";
    }
}