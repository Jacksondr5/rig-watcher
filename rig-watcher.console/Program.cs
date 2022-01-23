using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace rig_watcher.console
{
    public class Program
    {
        private static string _baseUrl = "https://api2.nicehash.com";
        public static int Main(string[] args)
        {
            Console.WriteLine("Beginning Rig Watcher");
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            Console.WriteLine($"Environment: {env}");
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            if (env == "Development")
            {
                builder.AddUserSecrets<Program>();
            }
            var config = builder.Build();
            var settings = config
                .GetRequiredSection("RigManager")
                .Get<RigManagerConfig>();
            CheckConfig(settings);

            var api = new Api(
                _baseUrl,
                settings.OrganizationId,
                settings.ApiKeyCode,
                settings.ApiSecretKeyCode
            );
            //get server time
            var timeResponse = GetAndDeserialize<TimeResponse>(
                api,
                HttpMethod.Get,
                "/api/v2/time",
                "server time"
            );
            var time = timeResponse.ServerTime;

            //get rigs
            var rigsResponse = GetAndDeserialize<GetRigsResponse>(
                api,
                HttpMethod.Get,
                "/main/api/v2/mining/rigs2",
                "rig list",
                true,
                time
            );
            var retVal = 0;
            foreach (var rig in rigsResponse.MiningRigs)
            {
                if (rig.MinerStatus.Equals("OFFLINE"))
                {
                    Console.WriteLine($"Rig {rig.Name} offline, can't do anything");
                    continue;
                }
                else if (rig.MinerStatus.Equals("MINING"))
                {
                    Console.WriteLine($"Rig {rig.Name} is mining");
                    continue;
                }
                else if (rig.MinerStatus.Equals("STOPPED"))
                {
                    Console.WriteLine($"Rig {rig.Name} is stopped, attempting to restart...");
                    var actionRequest = new RigActionRequest
                    {
                        rigId = rig.RigId,
                        action = "START",
                    };
                    var actionResponse = GetAndDeserialize<RigActionResponse>(
                        api,
                        HttpMethod.Post,
                        "/main/api/v2/mining/rigs/status2",
                        $"activate rig {rig.Name}",
                        true,
                        time,
                        JsonConvert.SerializeObject(actionRequest)
                    );
                    if (actionResponse.Success)
                    {
                        Console.WriteLine($"Rig {rig.Name} restarted");
                    }
                    else
                    {
                        Console.WriteLine($"Rig {rig.Name} failed to restart");
                        Console.WriteLine($"Response: {actionResponse.SuccessType}");
                        retVal = 1;
                    }
                }
                else 
                {
                    Console.WriteLine($"Unknown rig status: {rig.MinerStatus}");
                }
            }
            if (retVal == 0)
            {
                Console.WriteLine("All rigs are in a valid state");
            }
            else
            {
                Console.WriteLine("Some rigs are in an invalid state");
            }
            return retVal;
        }

        private static T GetAndDeserialize<T>(
            Api api,
            HttpMethod method,
            string url,
            string targetObject,
            bool auth = false,
            string? time = null,
            string? body = null
        )
        {
            T? retVal;
            try 
            {
                if (method == HttpMethod.Get)
                {
                    retVal = JsonConvert.DeserializeObject<T>(api.get(url, auth, time));
                }
                else if (method == HttpMethod.Post)
                {
                    var requestId = Guid.NewGuid().ToString();
                    if (body == null)
                    {
                        throw new Exception("body is null");
                    }
                    if (time == null)
                    {
                        throw new Exception("time is null");
                    }
                    retVal = JsonConvert.DeserializeObject<T>(api.post(url, body, time, true));
                }
                else
                {
                    throw new Exception($"Unknown method: {method}");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error while deserializing {targetObject} response", e);
            }
            if (retVal == null)
            {
                throw new Exception($"{targetObject} response is null");
            }
            return retVal;
        }
        private static void CheckConfig(RigManagerConfig settings)
        {
            if (settings.OrganizationId == null)
            {
                throw new Exception("OrganizationId is null");
            }
            if (settings.ApiKeyCode == null)
            {
                throw new Exception("ApiKeyCode is null");
            }
            if (settings.ApiSecretKeyCode == null)
            {
                throw new Exception("ApiSecretKeyCode is null");
            }
        }
    }

    internal class RigManagerConfig 
    {
        public string ApiKeyCode { get;set;} = "";
        public string ApiSecretKeyCode { get;set;} = "";
        public string OrganizationId { get;set;} = "";
    }
}
