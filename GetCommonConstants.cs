using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MyApp
{
    class SendRegningClient
    {
        private const string BaseUrl = "https://www.sendregning.no";
        private const string Username = "...@...";
        private const string Password = "...";
 
        public CommonConstants GetCommonConstants()
        {
            var client = new WebClient();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password));
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            string contentAsJson = client.DownloadString(BaseUrl + "/common/constants");
            var jsonDeserializer = new DataContractJsonSerializer(typeof(CommonConstants));

            using (Stream stream = GetStream(contentAsJson))
            {
                var commonConstants = jsonDeserializer.ReadObject(stream) as CommonConstants;
                return commonConstants;
            }
        }

        private static Stream GetStream(string value)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(value);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

    [DataContract]
    class CommonConstants
    {
        [DataMember]
        int dunningFee;

        [DataMember]
        decimal interestRate;

        [DataMember]
        int[] taxRates;

        [DataMember]
        int defaultTaxRate;
    }
}
