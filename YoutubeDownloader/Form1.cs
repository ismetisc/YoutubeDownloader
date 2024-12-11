using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async  void Form1_Load(object sender, EventArgs e)
        {

        }
        private async Task DownloadFileInParts(string url, string outputFilePath, int numberOfParts,string orjinalIsim)
        {
            HttpClient client = new HttpClient();
            List<Task> downloadTasks = new List<Task>();
           
            try
            {
                // Dosyanın boyutunu öğrenmek için HEAD isteği gönder
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("URL'e erişilemiyor.");
                    return;
                }

                long totalSize = response.Content.Headers.ContentLength ?? 0;
                if (totalSize == 0)
                {
                    MessageBox.Show("Dosya boyutu alınamadı.");
                    return;
                }

                long partSize = totalSize / numberOfParts;
                List<string> tempFiles = new List<string>();
                int saydir = (100 / numberOfParts);
                int countprogress = 1;
                int progress;
                // Parçaları indir
                for (int i = 0; i < numberOfParts; i++)
                {
                    long start = i * partSize;
                    long end = (i == numberOfParts - 1) ? totalSize - 1 : (start + partSize - 1);

                    string tempFilePath = $"part_{i}.tmp";
                    tempFiles.Add(tempFilePath);

                    // Her bir parça için indirme işlemini başlat
                    downloadTasks.Add(DownloadPart(url, tempFilePath, start, end, i, numberOfParts));
                    progress =  saydir * countprogress;
                    progressBar1.Value = progress;
                    countprogress++;
                }

                // Tüm indirme işlemleri bitene kadar bekle
                await Task.WhenAll(downloadTasks);

                // Parçaları birleştir
                MergeFiles(tempFiles, outputFilePath);
               
                progressBar1.Value = 97;
                String ffmpegCode = "-i  input.mp4 -vn -ar 44100 -ac 2 -b:a 192k output.mp3";
                bool ffmpeghataVar = false;
                ProcessStartInfo info = new ProcessStartInfo();
               info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.FileName = "ffmpeg.exe";
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.Arguments = ffmpegCode;
              info.RedirectStandardError = true;
                string ffmpegError;
                try
                {
                    using (Process process = Process.Start(info))
                    {
                        ffmpegError = process.StandardError.ReadToEnd();
                        process.WaitForExit(); // Çıkışı bekle
                        if (process.ExitCode != 0)
                        {
                            ffmpeghataVar = true;

                        }
                    }
                    if (!ffmpeghataVar)
                    {
                        progressBar1.Value = 100;
                    }
                    else
                    {
                        MessageBox.Show("dosya convert edilemedi..."+ ffmpegError);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                File.Delete("input.mp4");
                orjinalIsim =  orjinalIsim.Replace("|", "").Replace("/","").Replace(":","").Replace("*","").Replace("?","").Replace("\\","").Replace("\"","").Replace("<","").Replace(">","");
                if (File.Exists(orjinalIsim+".mp3"))
                {
                    File.Delete(orjinalIsim+".mp3");
                }
                File.Move("output.mp3", orjinalIsim+".mp3");
            }
            finally
            {
                client.Dispose();
            }
        }

        // Parçaları indiren fonksiyon
        private async Task DownloadPart(string url, string outputPath, long start, long end, int currentPart, int totalParts)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);

            using (var response = await client.SendAsync(request))
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }

            // ProgressBar'ı güncelle
          /*  Invoke(new Action(() =>
            {
                progressBar1.Value = (int)((double)(currentPart + 1) / totalParts * 100);
            }));*/
        }

        // Parçaları birleştiren fonksiyon
        private void MergeFiles(List<string> tempFiles, string outputFilePath)
        {
            using (var output = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                foreach (var tempFile in tempFiles)
                {
                    using (var input = new FileStream(tempFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        input.CopyTo(output);
                    }
                }
            }

            // Geçici dosyaları sil
            foreach (var tempFile in tempFiles)
            {
                try
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Geçici dosya silinirken bir hata oluştu: {tempFile}\n{ex.Message}");
                }
            }
        }

        private async void downloadBtn_Click(object sender, EventArgs e)
        {
            if (txtYoutubeUrl.Text != "")
            {
                string pattern = @"(?:v=|\/)([a-zA-Z0-9_-]{11})";

                Match match = Regex.Match(txtYoutubeUrl.Text, pattern);
                if (match.Success)
                {
                    try
                    {
                        string videoId = match.Groups[1].Value;
                        durum.Text = "Çözümleniyor...";
                        downloadBtn.Enabled = false;
                        webConnect request = new webConnect();
                        var cikti = request.httpPost("https://www.youtube.com/youtubei/v1/player?key=AIzaSyA8eiZmM1FaDVjRy-df2KTyQ_vz_yYM39w&hl=en", videoId);

                        dynamic jsondecoded = JsonConvert.DeserializeObject(cikti);
                        // JSON'u JArray olarak parse et
                        var count = jsondecoded.streamingData?.adaptiveFormats?.Count - 1;



                        cikti = jsondecoded.streamingData.adaptiveFormats[count].url;

                        string url = cikti; // İndirilecek dosyanın URL'si
                        string outputFilePath = "input.mp4";   // 
                        int numberOfParts = 6;                           //
                        progressBar1.Value = 0;                          // 
                        progressBar1.Maximum = 100;                      // 
                        string orjinalIsim = jsondecoded.videoDetails.title;
                        isim.Text = orjinalIsim;
                        durum.Text = "İndiriliyor...";
                        await DownloadFileInParts(url, outputFilePath, numberOfParts, orjinalIsim);
                        downloadBtn.Enabled = true;
                        durum.Text = "Tamamlandı...";

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }



                }
                else
                {
                    MessageBox.Show("Video ID alınamadı.");
                }


            }
            else {
                MessageBox.Show("Youtube Url yazın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
