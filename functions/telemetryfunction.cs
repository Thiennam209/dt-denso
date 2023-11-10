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
                    JObject AlertMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
                    string deviceId = (string)AlertMessage["systemProperties"]["iothub-connection-device-id"];
                    var ID = AlertMessage["body"]["MachineId"];
                    var Alert = AlertMessage["body"]["Alert"];
                    log.LogInformation($"Device:{deviceId} Device Id is:{ID}");
                    log.LogInformation($"Device:{deviceId} Alert Status is:{Alert}");

                    var updateProperty = new JsonPatchDocument();
                    updateProperty.AppendReplace("/Alert", Alert.Value<bool>());
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
                    var Part = deviceMessage["body"]["Part"] != null ? deviceMessage["body"]["Part"] : 0;
                    var Station = deviceMessage["body"]["Station"] != null ? deviceMessage["body"]["Station"] : "0";
                    var Serial = deviceMessage["body"]["Serial"] != null ? deviceMessage["body"]["Serial"] : 0;
                    var AdjJudge = deviceMessage["body"]["AdjJudge"] != null ? deviceMessage["body"]["AdjJudge"] : "0";
                    var Pressure = deviceMessage["body"]["Pressure"] != null ? deviceMessage["body"]["Pressure"] : 0;
                    var IP1 = deviceMessage["body"]["IP1"] != null ? deviceMessage["body"]["IP1"] : 0;
                    var CrimpJudge = deviceMessage["body"]["CrimpJudge"] != null ? deviceMessage["body"]["CrimpJudge"] : "0";
                    var PerformJudge = deviceMessage["body"]["PerformJudge"] != null ? deviceMessage["body"]["PerformJudge"] : "0";
                    var I1 = deviceMessage["body"]["I1"] != null ? deviceMessage["body"]["I1"] : 0;
                    var I2 = deviceMessage["body"]["I2"] != null ? deviceMessage["body"]["I2"] : 0;
                    var I3 = deviceMessage["body"]["I3"] != null ? deviceMessage["body"]["I3"] : 0;
                    var I4 = deviceMessage["body"]["I4"] != null ? deviceMessage["body"]["I4"] : 0;
                    var I1I15 = deviceMessage["body"]["I1I15"] != null ? deviceMessage["body"]["I1I15"] : 0;
                    var I2I14 = deviceMessage["body"]["I2I14"] != null ? deviceMessage["body"]["I2I14"] : 0;
                    var I3I13 = deviceMessage["body"]["I3I13"] != null ? deviceMessage["body"]["I3I13"] : 0;
                    var Stick1 = deviceMessage["body"]["Stick1"] != null ? deviceMessage["body"]["Stick1"] : 0;
                    var Stick3 = deviceMessage["body"]["Stick3"] != null ? deviceMessage["body"]["Stick3"] : 0;
                    var Flow = deviceMessage["body"]["Flow"] != null ? deviceMessage["body"]["Flow"] : 0;
                    var Resp1_P5 = deviceMessage["body"]["Resp1_P5"] != null ? deviceMessage["body"]["Resp1_P5"] : 0;
                    var Resp1_P6 = deviceMessage["body"]["Resp1_P6"] != null ? deviceMessage["body"]["Resp1_P6"] : 0;
                    var Resp2_P5 = deviceMessage["body"]["Resp2_P5"] != null ? deviceMessage["body"]["Resp2_P5"] : 0;
                    var Resp2_P6 = deviceMessage["body"]["Resp2_P6"] != null ? deviceMessage["body"]["Resp2_P6"] : 0;

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
                        ["CrimpJudge"] = CrimpJudge,
                        ["PerformJudge"] = PerformJudge,
                        ["I1"] = I1,
                        ["I2"] = I2,
                        ["I3"] = I3,
                        ["I4"] = I4,
                        ["I1I15"] = I1I15,
                        ["I2I14"] = I2I14,
                        ["I3I13"] = I3I13,
                        ["Stick1"] = Stick1,
                        ["Stick3"] = Stick3,
                        ["Flow"] = Flow,
                        ["Resp1_P5"] = Resp1_P5,
                        ["Resp1_P6"] = Resp1_P6,
                        ["Resp2_P5"] = Resp2_P5,
                        ["Resp2_P6"] = Resp2_P6,
                    };

                    updateProperty.AppendReplace("/MachineId", ID.Value<string>());
                    updateProperty.AppendReplace("/Time", Time.Value<string>());
                    updateProperty.AppendReplace("/Part", Part.Value<int>());
                    updateProperty.AppendReplace("/Station", Station.Value<string>());
                    updateProperty.AppendReplace("/Serial", Serial.Value<int>());
                    updateProperty.AppendReplace("/AdjJudge", AdjJudge.Value<string>());
                    updateProperty.AppendReplace("/Pressure", Pressure.Value<double>());
                    updateProperty.AppendReplace("/IP1", IP1.Value<double>());
                    updateProperty.AppendReplace("/CrimpJudge", CrimpJudge.Value<string>());
                    updateProperty.AppendReplace("/PerformJudge", PerformJudge.Value<string>());
                    updateProperty.AppendReplace("/I1", I1.Value<double>());
                    updateProperty.AppendReplace("/I2", I2.Value<double>());
                    updateProperty.AppendReplace("/I3", I3.Value<double>());
                    updateProperty.AppendReplace("/I4", I4.Value<double>());
                    updateProperty.AppendReplace("/I1I15", I1I15.Value<double>());
                    updateProperty.AppendReplace("/I2I14", I2I14.Value<double>());
                    updateProperty.AppendReplace("/I3I13", I3I13.Value<double>());
                    updateProperty.AppendReplace("/Stick1", Stick1.Value<double>());
                    updateProperty.AppendReplace("/Stick3", Stick3.Value<double>());
                    updateProperty.AppendReplace("/Flow", Flow.Value<double>());
                    updateProperty.AppendReplace("/Resp1_P5", Resp1_P5.Value<double>());
                    updateProperty.AppendReplace("/Resp1_P6", Resp1_P6.Value<double>());
                    updateProperty.AppendReplace("/Resp2_P5", Resp2_P5.Value<double>());
                    updateProperty.AppendReplace("/Resp2_P6", Resp2_P6.Value<double>());

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
            }
            catch (Exception e)
            {
                log.LogInformation(e.Message);
            }
        }
    }
}