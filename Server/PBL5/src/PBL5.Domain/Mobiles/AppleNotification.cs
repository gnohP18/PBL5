using System;
using System.Text.Json.Serialization;

namespace PBL5.Mobiles
{
    public class AppleNotification
    {
        public class Notification
        {
            public string title { get; set; }
            public string body { get; set; }
        }

        public class AppleMessage
        {
            public string to { get; set; }
            public Notification notification { get; set; }
        }
    }
}
