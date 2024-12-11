using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader
{
    
    class webConnect
    {
        WebClient client;

        public string httpGet(string url,string reff="") {
            
            var request = (HttpWebRequest)WebRequest.Create(url);


            if (reff != "")
            {
                request.Referer = reff;
                request.Headers.Add("Origin", "https://contentx.me");
            }
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            request.Headers.Add("Upgrade-Insecure-Requests","1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/538.36";
            var response = (HttpWebResponse)request.GetResponse();
            string responseString;
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            return responseString;
        }
        public string httpPost(string url,string videoId)
        {

            // Gönderilecek URL


            // POST verisi (örnek JSON formatında)
            var content = new
            {
                videoId = videoId,
                context = new
                {
                    client = new
                    {
                        clientName = "IOS",
                        clientVersion = "19.29.1",
                        deviceMake = "Apple",
                        deviceModel = "iPhone16,2",
                        hl = "en",
                        osName = "iPhone",
                        osVersion = "17.5.1.21F90",
                        timeZone = "UTC",
                        userAgent = "com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X;)",
                        gl = "US",
                        utcOffsetMinutes = 0
                    }
                }
            };

            string postData = JsonConvert.SerializeObject(content);

            // WebRequest nesnesini oluştur
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST"; // POST isteği yapacağımızı belirtiyoruz
            request.UserAgent = "com.google.ios.youtube/19.29.1 (iPhone16,2; U; CPU iOS 17_5_1 like Mac OS X)";
            // İstek başlıkları (ContentType gibi)
            request.ContentType = "application/json"; // JSON formatında veri gönderiyoruz

            // POST verisini byte dizisine dönüştür
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // İstek gövdesine veri ekle
            request.ContentLength = byteArray.Length;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            // İstekten yanıtı al ve işle
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseText = reader.ReadToEnd();
                            return responseText;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Hata durumunu yakala
                using (Stream responseStream = ex.Response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string errorResponse = reader.ReadToEnd();
                        return errorResponse;
                    }
                }
            }
        }
    }
}
