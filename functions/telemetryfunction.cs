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
                    var Part = deviceMessage["body"]["Part"];
                    var Station = deviceMessage["body"]["Station"];
                    var Serial = deviceMessage["body"]["Serial"];
                    var AdjJudge = deviceMessage["body"]["AdjJudge"];
                    var Pressure = deviceMessage["body"]["Pressure"];
                    var IP1 = deviceMessage["body"]["IP1"];
                    var AdjRetry = deviceMessage["body"]["AdjRetry"];
                    var CrimpJudge = deviceMessage["body"]["CrimpJudge"];
                    var PerformJudge = deviceMessage["body"]["PerformJudge"];
                    var I1 = deviceMessage["body"]["I1"];
                    var I2 = deviceMessage["body"]["I2"];
                    var I3 = deviceMessage["body"]["I3"];
                    var I4 = deviceMessage["body"]["I4"];
                    var I5 = deviceMessage["body"]["I5"];
                    var I10 = deviceMessage["body"]["I10"];
                    var I12 = deviceMessage["body"]["I12"];
                    var I13 = deviceMessage["body"]["I13"];
                    var I14 = deviceMessage["body"]["I14"];
                    var I15 = deviceMessage["body"]["I15"];
                    var I1I15 = deviceMessage["body"]["I1I15"];
                    var I2I14 = deviceMessage["body"]["I2I14"];
                    var I3I13 = deviceMessage["body"]["I3I13"];
                    var I4I12 = deviceMessage["body"]["I4I12"];
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
                    var Alert = deviceMessage["body"]["Alert"];


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
                        ["AdjRetry"] = AdjRetry,
                        ["CrimpJudge"] = CrimpJudge,
                        ["PerformJudge"] = PerformJudge,
                        ["I1"] = I1,
                        ["I2"] = I2,
                        ["I3"] = I3,
                        ["I4"] = I4,
                        ["I5"] = I5,
                        ["I10"] = I10,
                        ["I12"] = I12,
                        ["I13"] = I13,
                        ["I14"] = I14,
                        ["I15"] = I15,
                        ["I1I15"] = I1I15,
                        ["I2I14"] = I2I14,
                        ["I3I13"] = I3I13,
                        ["I4I12"] = I4I12,
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
                        ["Alert"] = Alert,
                    };

                    updateProperty.AppendReplace("/MachineId", ID.Value<string>());
                    updateProperty.AppendReplace("/Time", Time.Value<string>());
                    updateProperty.AppendReplace("/Part", Part.Value<int>());
                    updateProperty.AppendReplace("/Station", Station.Value<string>());
                    updateProperty.AppendReplace("/Serial", Serial.Value<int>());
                    updateProperty.AppendReplace("/AdjJudge", AdjJudge.Value<string>());
                    updateProperty.AppendReplace("/Pressure", Pressure.Value<double>());
                    updateProperty.AppendReplace("/IP1", IP1.Value<double>());
                    updateProperty.AppendReplace("/AdjRetry", AdjRetry.Value<int>());
                    updateProperty.AppendReplace("/CrimpJudge", CrimpJudge.Value<string>());
                    updateProperty.AppendReplace("/PerformJudge", PerformJudge.Value<string>());
                    updateProperty.AppendReplace("/I1", I1.Value<double>());
                    updateProperty.AppendReplace("/I2", I2.Value<double>());
                    updateProperty.AppendReplace("/I3", I3.Value<double>());
                    updateProperty.AppendReplace("/I4", I4.Value<double>());
                    updateProperty.AppendReplace("/I5", I5.Value<double>());
                    updateProperty.AppendReplace("/I10", I10.Value<double>());
                    updateProperty.AppendReplace("/I12", I12.Value<double>());
                    updateProperty.AppendReplace("/I13", I13.Value<double>());
                    updateProperty.AppendReplace("/I14", I14.Value<double>());
                    updateProperty.AppendReplace("/I15", I15.Value<double>());
                    updateProperty.AppendReplace("/I1I15", I1I15.Value<double>());
                    updateProperty.AppendReplace("/I2I14", I2I14.Value<double>());
                    updateProperty.AppendReplace("/I3I13", I3I13.Value<double>());
                    updateProperty.AppendReplace("/I4I12", I4I12.Value<double>());
                    updateProperty.AppendReplace("/Stick1", Stick1.Value<double>());
                    updateProperty.AppendReplace("/Stick3", Stick3.Value<double>());
                    updateProperty.AppendReplace("/Stick4", Stick4.Value<double>());
                    updateProperty.AppendReplace("/Flow", Flow.Value<double>());
                    updateProperty.AppendReplace("/PerformRetry", PerformRetry.Value<int>());
                    updateProperty.AppendReplace("/Resp1_T1", Resp1_T1.Value<double>());
                    updateProperty.AppendReplace("/Resp1_T2", Resp1_T2.Value<double>());
                    updateProperty.AppendReplace("/Resp1_P5", Resp1_P5.Value<double>());
                    updateProperty.AppendReplace("/Resp1_P6", Resp1_P6.Value<double>());
                    updateProperty.AppendReplace("/Resp2_T1", Resp2_T1.Value<double>());
                    updateProperty.AppendReplace("/Resp2_T2", Resp2_T2.Value<double>());
                    updateProperty.AppendReplace("/Resp2_P5", Resp2_P5.Value<double>());
                    updateProperty.AppendReplace("/Resp2_P6", Resp2_P6.Value<double>());
                    updateProperty.AppendReplace("/Alert", Alert.Value<bool>());

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