using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;

//Загрузка и удаление файлов в облачном хранилище AWS S3 с помощью Asp.Net Core
//https://tutexchange.com/uploading-downloading-and-deleting-files-in-aws-s3-cloud-storage-using-asp-net-core/?amp=1
/*
 Интегрируем Amazon S3 в .Net приложение https://habr.com/ru/post/146223/

 */

namespace YOS_CRUD.Pages.S3
{

    public class IndexModel : PageModel
    {
       
        public readonly string yandexS3 = "https://s3.yandexcloud.net";
        public readonly string myBucket = "topfirm"; //ваш бакет

        private AmazonS3Client YandexClient() {
            //Примеры BasicAWSCredentials https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/Runtime/TBasicAWSCredentials.html

            //********* Статический ключ ****************
            // !!! удалите ключи из текста при сохранении и GIT -- никаких открытых источников, храните только локально, в серетном месте.
            string accessKey = "----------------"; //ваш идентификатор ключа 
            string secretKey = "------------------------------------------"; //ваш секретный ключ
            //  =============================== ⚠удалить ключи из текста⚠ ==============================
           
            BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Config config = new AmazonS3Config
            {
                //RegionEndpoint = Amazon.RegionEndpoint.APSouth1
                ServiceURL = yandexS3 
            };
            return new AmazonS3Client(credentials, config);
            
        }

        public string? Message { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            Message = "результат:<br /><br />";

            using AmazonS3Client client = YandexClient();

            Message += "***** Список бакетов: <br/>";
            ListBucketsResponse response = await client.ListBucketsAsync();
            foreach (S3Bucket b in response.Buckets)
            {
                Message += $"{b.BucketName}, {b.CreationDate} <br />";
            }
            Message += "<br/>";

            //Message += "***** Список объектов в бакете topfirm: <br/>";
            ListObjectsRequest requestGet = new ListObjectsRequest();
            requestGet.BucketName = myBucket;
            ListObjectsResponse response2 = await client.ListObjectsAsync(requestGet);
            foreach (S3Object o in response2.S3Objects)
            {
                Message += $"<a href=\"{yandexS3}/{myBucket}/{o.Key}\" target=\"_blank\">{o.Key}</a>, {o.Size}, {o.LastModified}<br />";
            }

            return Page();
        }

        public readonly string myFile = "hello.txt"; //ваш файл для тестирования
        [BindProperty]
        public string? myBody { get; set; }
        public async Task<IActionResult> OnPost()
        {
            Message = "для удаления введите del";
            if (string.IsNullOrEmpty(myBody)) return Page();
            
            using AmazonS3Client client = YandexClient();

            if (myBody != "del")
            {
                //*****Загрузка файла в бакет*****
                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = myBucket;
                request.Key = myFile;
                request.ContentType = "text/plain"; //для текстового файла
                request.ContentBody = myBody; //повторная запись обновит фай на том же url
                await client.PutObjectAsync(request);
                Message = "***** Загружено!<br />";
                Message += $"<a href=\"{yandexS3}/{myBucket}/{myFile}\" target=\"_blank\">{myFile}</a>";
            }
            //if (myBody == "url") 
            //{
            //    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            //    request.BucketName = myBucket;
            //    request.Key = $"secret_{myFile}";
            //    request.Expires = DateTime.Now.AddMinutes(10); //подпись действительна в течении 10 мин
            //    request.Protocol = Protocol.HTTP;
            //    string url = client.GetPreSignedURL(request);
            //    Message = $"***** Подпись для загрузки файла <b>secret_{myFile}</b><br />";
            //    Message += url;

            //}
            else
            {
                //*****Удаление файла*****
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.BucketName = myBucket;
                request.Key = myFile;
                await client.DeleteObjectAsync(request);
                Message = "***** Файл удален! <br />";
                Message += $"<a href=\"{yandexS3}/{myBucket}/{myFile}\" target=\"_blank\">{myFile}</a>";
            }
          
            return Page();
        }
    }
}

//Справочник по синтаксису Razor  https://learn.microsoft.com/ru-ru/aspnet/core/mvc/views/razor?view=aspnetcore-7.0
//Асинхронная страница Razor https://www.webtrainingroom.com/aspnetcore/call-async-property-in-razor-pagemodel
//Изучение Razor Pages Методы обработчика в Razor Pages https://www.learnrazorpages.com/razor-pages/handler-methods

/*
 C# S3 EXAMPLES https://docs.ceph.com/en/quincy/radosgw/s3/csharp/
 Создание динамического веб-приложения, которое анализирует фотографии с помощью AWS SDK для .NET.
 https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/dotnetv3/cross-service/PhotoAnalyzerApp


 */

