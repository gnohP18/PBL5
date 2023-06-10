using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PBL5.Employees;
using Newtonsoft.Json;
using Volo.Abp.Domain.Services;
using static PBL5.Mobiles.AppleNotification;
using System.Net;

namespace PBL5.Mobiles
{
    public class MobileManager : DomainService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly string ServeKey = "AAAAUDtw9Tk:APA91bGRWs4df1aDHBLRO922WwEcFM_SEuFdchBpDt2TNlns10ZI-8wkgfV2WggH5R1w5DcTWJz5LWdrbt6t32J9EM1HW7yRzfeV89eKV0DcdQ0DZYVp4kgdh4qWLVX9TxVZzaSndOgJ";

        public MobileManager(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> SendNotificationAPN(string deviceId, string title, string body)
        {
            var noti = new Notification();
            var appleMessage = new AppleMessage();

            noti.title = title;
            noti.body = body;

            appleMessage.to = deviceId;
            appleMessage.notification = noti;

            HttpClient httpNoti = new HttpClient();
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + ServeKey);
            httpNoti.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
            var content = new StringContent(JsonConvert.SerializeObject(appleMessage), System.Text.Encoding.UTF8, "application/json");

            var response = await httpNoti.PostAsync("https://fcm.googleapis.com/fcm/send", content);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}