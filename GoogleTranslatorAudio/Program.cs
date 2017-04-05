using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleTranslatorAudio
{
    class Program
    {
        static void Main(string[] args)
        {
            Translate();
        }

        private static void Translate()
        {
            for (int numberOfUnit = 1; numberOfUnit <= 10; numberOfUnit++)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo($"DownloadedFiles/{numberOfUnit}");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }



                string text = System.IO.File.ReadAllText($"Phrases{numberOfUnit}.txt").Replace('\r', '.');
                string[] phrasesCollection = text.Split('\n');

                int counter = 0;
                foreach (var item in phrasesCollection)
                {
                    HttpClient client = new HttpClient();

                    var _address = new Uri($"https://translate.googleapis.com/translate_tts?ie=UTF-8&q={item}&tl=en&total=1&idx=0&textlen={item.Length}&client=gtx");

                    // Send asynchronous request
                    HttpResponseMessage response = client.GetAsync(_address).Result;

                    // Check that response was successful or throw exception
                    response.EnsureSuccessStatusCode();

                    // Read response asynchronously and save asynchronously to file
                    for (int i = 0; i < 2; i++)
                    {
                        using (FileStream fileStream = new FileStream($"DownloadedFiles/{numberOfUnit}/{counter}. {item.Replace(':', ' ').Replace("/", " or ").Replace("?", " ")}.mp3", FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            response.Content.CopyToAsync(fileStream).Wait();
                        }
                        counter++;
                    }
                }
            }
        }

        //public static Task ReadAsFileAsync(this HttpContent content, string filename, bool overwrite)
        //{
        //    string pathname = Path.GetFullPath(filename);
        //    if (!overwrite && File.Exists(filename))
        //    {
        //        throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
        //    }

        //    FileStream fileStream = null;
        //    try
        //    {
        //        fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
        //        return content.CopyToAsync(fileStream).ContinueWith(
        //            (copyTask) =>
        //            {
        //                fileStream.Dispose();
        //            });
        //    }
        //    catch
        //    {
        //        if (fileStream != null)
        //        {
        //            fileStream.Dispose();
        //        }

        //        throw;
        //    }
        //}
    }
}
