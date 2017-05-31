using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OneSignal.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //SendPushNotification();
            SendPushNotificationUsingCSharpSDk();
        }

        private static void SendPushNotification()
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic M2QwNDM1NTktNjc2Yi00OWY1LTg5ZjYtZjlhNzE2ZjJjMGFm");

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "9423ab70-f216-4a77-a364-cdf74f40e4fb",
                contents = new { en = "English Message" },
                headings = new { en = "TEST" },
                data = new { notificationType  = "SimpleTextMessage", messageId= "03c2de5a-d08b-4b32-9723-d17034a64305" },
                include_player_ids = new string[] { "17135056-a51d-4b45-bc7c-731b4b3a79eb", "82828ec2-7a9b-46c6-a781-ab07fb2a5998" }
            };

            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }

        private static void SendPushNotificationUsingCSharpSDk()
        {
            NotificationCreateResult result = null;

            var client = new OneSignalClient("M2QwNDM1NTktNjc2Yi00OWY1LTg5ZjYtZjlhNzE2ZjJjMGFm");

            var options = new NotificationCreateOptions();

            options.AppId = new Guid("9423ab70-f216-4a77-a364-cdf74f40e4fb");
            //options.IncludedSegments = new List<string> { "All" };
            options.Contents.Add(LanguageCodes.English, "Hello world!");
            options.Headings.Add(LanguageCodes.English, "Hello!");
            options.IncludePlayerIds = new List<string> { "17135056-a51d-4b45-bc7c-731b4b3a79eb","82828ec2-7a9b-46c6-a781-ab07fb2a5998" };
            options.Data = new Dictionary<string, string>();
            options.Data.Add("notificationType", "SimpleTextMessage");
            options.Data.Add("messageId", "03c2de5a-d08b-4b32-9723-d17034a64305");
            options.AndroidLedColor = "FF0000FF";
            options.IosBadgeType = IosBadgeTypeEnum.SetTo;
            options.IosBadgeCount = 10;
            options.AndroidAccentColor = "FFFF0000";
            options.Priority = 10;
            options.DeliverToAndroid = false;
            try
            {
                result = client.Notifications.Create(options);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
