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
        
        //********* Статический ключ ****************
        readonly string accessKey = "YCAJE8V2Xq2HSWNLgwoxS1Pgo";
        readonly string secretKey = "YCMu9IMiVqt0N2x6xksun8SVnN4sbozMcjCM-XKz";
        
        public string? Message { get; private set; }
        
        
        public async Task<IActionResult> OnGet()
        {
            Message = "результат:<br /><br />";
            //BasicAWSCredentials https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/Runtime/TBasicAWSCredentials.html
            BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);

            AmazonS3Config config = new AmazonS3Config
            {
                //RegionEndpoint = Amazon.RegionEndpoint.APSouth1
                ServiceURL = "https://s3.yandexcloud.net"
            };

            using AmazonS3Client client = new AmazonS3Client(credentials, config);

            Message += "***** Список бакетов: <br/>";
            ListBucketsResponse response = await client.ListBucketsAsync();
            foreach (S3Bucket b in response.Buckets)
            {
                //Message += string.Format("{0}   {1}   {2}<br />", o.Key, o.Size, o.LastModified);
                Message += $"{b.BucketName}   {b.CreationDate} <br />";
            }
            Message += "<br/>";


            Message += "***** Список объектов в бакете topfirm: <br/>";
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = "topfirm";
            ListObjectsResponse response2 = await client.ListObjectsAsync(request);
            foreach (S3Object o in response2.S3Objects)
            {
                //Message += string.Format("{0}   {1}   {2}<br />", o.Key, o.Size, o.LastModified);
                Message += $"{o.Key}   {o.Size}   {o.LastModified}<br />";

            }

            /* ***** Загрузка файла в бакет *****
            PutObjectRequest request = new PutObjectRequest();
            request.BucketName = "topfirm";
            request.Key = "hello.txt"; //название файла
            request.ContentType = "text/plain";
            request.ContentBody = "test-10- кракозябли)"; //повторная запись обновит фай на том же url
            await client.PutObjectAsync(request);
            Message = Message + "***** Загружено!";
            */

            return Page();
        }
    }
}

//Справочник по синтаксису Razor  https://learn.microsoft.com/ru-ru/aspnet/core/mvc/views/razor?view=aspnetcore-7.0
//Асинхронная страница Razor https://www.webtrainingroom.com/aspnetcore/call-async-property-in-razor-pagemodel


/*
 C# S3 EXAMPLES https://docs.ceph.com/en/quincy/radosgw/s3/csharp/
 Создание динамического веб-приложения, которое анализирует фотографии с помощью AWS SDK для .NET.
 https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/dotnetv3/cross-service/PhotoAnalyzerApp


    ********* Статически ключ ****************
     Идентификатор ключа: YCAJE8V2Xq2HSWNLgwoxS1Pgo
     YCAJE8V2Xq2HSWNLgwoxS1Pgo
     секретный ключ: YCMu9IMiVqt0N2x6xksun8SVnN4sbozMcjCM-XKz
     YCMu9IMiVqt0N2x6xksun8SVnN4sbozMcjCM-XKz
     Сохраните идентификатор и ключ. После закрытия диалога значение ключа будет недоступно.

     ********* API-ключ ************* не срабатывает
     Идентификатор ключа: ajec0sjvpkkc5rcdh24l
     ajec0sjvpkkc5rcdh24l
     секретный ключ: AQVNzAcn_NKMLtjYiJUuHblqPnHJL-qRQnb_qMNH
     AQVNzAcn_NKMLtjYiJUuHblqPnHJL-qRQnb_qMNH
 */

