using Azure;
using Azure.Core.Pipeline;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace My.Function
{
    // This class processes telemetry events from IoT Hub, reads temperature of a device
    // and sets the "Temperature" property of the device with the value of the telemetry.
    public class telemetryfunction
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string adtServiceUrl = Environment.GetEnvironmentVariable("ADT_SERVICE_URL");

        [FunctionName("telemetryfunction")]
        public async void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            try
            {
                // After this is deployed, you need to turn the Managed Identity Status to "On",
                // Grab Object Id of the function and assigned "Azure Digital Twins Owner (Preview)" role
                // to this function identity in order for this function to be authorized on ADT APIs.
                //Authenticate with Digital Twins
                var credentials = new DefaultAzureCredential();
                log.LogInformation(credentials.ToString());
                DigitalTwinsClient client = new DigitalTwinsClient(
                    new Uri(adtServiceUrl), credentials, new DigitalTwinsClientOptions
                    { Transport = new HttpClientTransport(httpClient) });
                log.LogInformation($"ADT service client connection created.");
                if (eventGridEvent.Data.ToString().Contains("Alert"))
                {
                    JObject alertMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                    string deviceId = (string)alertMessage["systemProperties"]["iothub-connection-device-id"];
                    var ID = alertMessage["body"]["MachineId"];
                    var alert = alertMessage["body"]["Alert"];
                    log.LogInformation($"Device:{deviceId} Device Id is:{ID}");
                    log.LogInformation($"Device:{deviceId} Alert Status is:{alert}");

                    var updateProperty = new JsonPatchDocument();
                    updateProperty.AppendReplace("/Alert", alert.Value<bool>());
                    updateProperty.AppendReplace("/MachineId", ID.Value<string>());
                    log.LogInformation(updateProperty.ToString());
                    try
                    {
                        await client.UpdateDigitalTwinAsync(deviceId, updateProperty);
                    }
                    catch (Exception e)
                    {
                        log.LogInformation(e.Message);
                    }
                }
                else if (eventGridEvent != null && eventGridEvent.Data != null)
                {

                    JObject deviceMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                    string deviceId = (string)deviceMessage["systemProperties"]["iothub-connection-device-id"];
                    var ID = deviceMessage["body"]["MachineId"];
                    var Time = deviceMessage["body"]["Time"];
                    var Part = deviceMessage["body"]["Part"];
                    var Station = deviceMessage["body"]["Station"];
                    var Serial = deviceMessage["body"]["Serial"];
                    var AdjJudge = deviceMessage["body"]["AdjJudge"];
                    var Pressure = deviceMessage["body"]["Pressure"];
                    var IP1 = deviceMessage["body"]["IP1"];
                    var IP2 = deviceMessage["body"]["IP2"];
                    var AdjRetry = deviceMessage["body"]["AdjRetry"];
                    var CrimpJudge = deviceMessage["body"]["CrimpJudge"];
                    var PerformJudge = deviceMessage["body"]["PerformJudge"];
                    var I1 = deviceMessage["body"]["I1"];
                    var I2 = deviceMessage["body"]["I2"];
                    var I3 = deviceMessage["body"]["I3"];
                    var I4 = deviceMessage["body"]["I4"];
                    var I5 = deviceMessage["body"]["I5"];
                    var I6 = deviceMessage["body"]["I6"];
                    var I7 = deviceMessage["body"]["I7"];
                    var I8 = deviceMessage["body"]["I8"];
                    var I9 = deviceMessage["body"]["I9"];
                    var I10 = deviceMessage["body"]["I10"];
                    var I11 = deviceMessage["body"]["I11"];
                    var I12 = deviceMessage["body"]["I12"];
                    var I13 = deviceMessage["body"]["I13"];
                    var I14 = deviceMessage["body"]["I14"];
                    var I15 = deviceMessage["body"]["I15"];
                    var I1I15 = deviceMessage["body"]["I1I15"];
                    var I2I14 = deviceMessage["body"]["I2I14"];
                    var I3I13 = deviceMessage["body"]["I3I13"];
                    var I4I12 = deviceMessage["body"]["I4I12"];
                    var I5I11 = deviceMessage["body"]["I5I11"];
                    var I6I10 = deviceMessage["body"]["I6I10"];
                    var I7I9 = deviceMessage["body"]["I7I9"];
                    var Stick1 = deviceMessage["body"]["Stick1"];
                    var Stick3 = deviceMessage["body"]["Stick3"];
                    var Stick4 = deviceMessage["body"]["Stick4"];
                    var Flow = deviceMessage["body"]["Flow"];
                    var PerformRetry = deviceMessage["body"]["PerformRetry"];
                    var Resp1_T1 = deviceMessage["body"]["Resp1_T1"];
                    var Resp1_T2 = deviceMessage["body"]["Resp1_T2"];
                    var Resp1_P5 = deviceMessage["body"]["Resp1_P5"];
                    var Resp1_P6 = deviceMessage["body"]["Resp1_P6"];
                    var Resp2_T1 = deviceMessage["body"]["Resp2_T1"];
                    var Resp2_T2 = deviceMessage["body"]["Resp2_T2"];
                    var Resp2_P5 = deviceMessage["body"]["Resp2_P5"];
                    var Resp2_P6 = deviceMessage["body"]["Resp2_P6"];
                    var Resp3_T1 = deviceMessage["body"]["Resp3_T1"];
                    var Resp3_T2 = deviceMessage["body"]["Resp3_T2"];
                    var Resp3_P5 = deviceMessage["body"]["Resp3_P5"];
                    var Resp3_P6 = deviceMessage["body"]["Resp3_P6"];
                    var RespRetry = deviceMessage["body"]["RespRetry"];


                    var updateProperty = new JsonPatchDocument();


                    var machineTelemetry = new Dictionary<string, Object>()
                    {
                        ["MachineId"] = ID,
                        ["Time"] = Time,
                        ["Part"] = Part,
                        ["Station"] = Station,
                        ["Serial"] = Serial,
                        ["AdjJudge"] = AdjJudge,
                        ["Pressure"] = Pressure,
                        ["IP1"] = IP1,
                        ["IP2"] = IP2,
                        ["AdjRetry"] = AdjRetry,
                        ["CrimpJudge"] = CrimpJudge,
                        ["PerformJudge"] = PerformJudge,
                        ["I1"] = I1,
                        ["I2"] = I2,
                        ["I3"] = I3,
                        ["I4"] = I4,
                        ["I5"] = I5,
                        ["I6"] = I6,
                        ["I7"] = I7,
                        ["I8"] = I8,
                        ["I9"] = I9,
                        ["I10"] = I10,
                        ["I11"] = I11,
                        ["I12"] = I12,
                        ["I13"] = I13,
                        ["I14"] = I14,
                        ["I15"] = I15,
                        ["I1I15"] = I1I15,
                        ["I2I14"] = I2I14,
                        ["I3I13"] = I3I13,
                        ["I4I12"] = I4I12,
                        ["I5I11"] = I5I11,
                        ["I6I10"] = I6I10,
                        ["I7I9"] = I7I9,
                        ["Stick1"] = Stick1,
                        ["Stick3"] = Stick3,
                        ["Stick4"] = Stick4,
                        ["Flow"] = Flow,
                        ["PerformRetry"] = PerformRetry,
                        ["Resp1_T1"] = Resp1_T1,
                        ["Resp1_T2"] = Resp1_T2,
                        ["Resp1_P5"] = Resp1_P5,
                        ["Resp1_P6"] = Resp1_P6,
                        ["Resp2_T1"] = Resp2_T1,
                        ["Resp2_T2"] = Resp2_T2,
                        ["Resp2_P5"] = Resp2_P5,
                        ["Resp2_P6"] = Resp2_P6,
                        ["Resp3_T1"] = Resp3_T1,
                        ["Resp3_T2"] = Resp3_T2,
                        ["Resp3_P5"] = Resp3_P5,
                        ["Resp3_P6"] = Resp3_P6,
                        ["RespRetry"] = RespRetry,
                    };
                    updateProperty.AppendAdd("/MachineId", ID.Value<string>());

                    log.LogInformation(updateProperty.ToString());
                    try
                    {
                        await client.PublishTelemetryAsync(deviceId, Guid.NewGuid().ToString(), JsonConvert.SerializeObject(machineTelemetry));
                    }
                    catch (Exception e)
                    {
                        log.LogInformation(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
            }
        }
    }
}