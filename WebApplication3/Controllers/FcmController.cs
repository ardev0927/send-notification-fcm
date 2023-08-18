using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FcmController : ControllerBase
    {
        [Route("GetCustomers")]
        [HttpGet]
        public List<String> GetCustomers()
        {
            List<String> customers = new List<String> { "asdfsd", "add"};
            return customers;
        }

        [Route("SendNotification")]
        [HttpGet]
        public string SendNotification(String chatUserPhoneNumberId, String platform, String appUserPhone, String title, String body)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("key.json"),
            });

            //var registrationTokens = JsonConvert.DeserializeObject<List<string>>(token);
            List<string> tokens = new List<string>();
            tokens.Add("ff2doK6MS2C3fz6dr1ajOd:APA91bElw7Lj7XbGUHhxSezFcg_VsaeqMqopHyR-h24YRH87KQGnOBKTY1AyR1HA_GaAf1qmDEAHR3Ohe1p0qfkA2vDfmniaWtwAIqIFPZvPbCoL_8qpK4hepnjESi4CzKUl-VgyJN9o");

            var message = new FirebaseAdmin.Messaging.MulticastMessage()
            {
                /*Tokens = registrationTokens,

                Data = new Dictionary<string, string>()
                {
                    { "chatUser", JsonConvert.SerializeObject(chatUser) }
                },*/
                Tokens = tokens,
                Android = new AndroidConfig()
                {
                    Notification = new FirebaseAdmin.Messaging.AndroidNotification()
                    {
                        Body = body,
                        Title = title,
                        ChannelId = "high_importance_channel",
                        Priority = (NotificationPriority?)Priority.High
                    }
                },
                Apns = new ApnsConfig()
                {
                    Headers = new Dictionary<string, string>()
                    {
                        { "apns-priority", "5" },
                    },
                    Aps = new Aps()
                    {
                        Alert = new ApsAlert()
                        {
                            Title = title,
                            Body = body,
                        },
                        Sound = "default",
                    }
                }
            };

            var response = FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).Result;

            return response.ToString();
        }
    }
}
