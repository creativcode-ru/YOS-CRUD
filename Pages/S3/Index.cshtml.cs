using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using Amazon;


namespace YOS_CRUD.Pages.S3
{
    public class IndexModel : PageModel
    {
        //C# S3 EXAMPLES https://docs.ceph.com/en/quincy/radosgw/s3/csharp/

        readonly string accessKey = "ajec0sjvpkkc5rcdh24l";
        readonly string secretKey = "AQVNzAcn_NKMLtjYiJUuHblqPnHJL-qRQnb_qMNH";

        public void OnGet()
        {

            //AmazonS3Config configsS3 = new AmazonS3Config
            //{
            //    ServiceURL = "https://s3.yandexcloud.net"
            //};

            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "https://s3.yandexcloud.net";

            AmazonS3Client s3Client = new AmazonS3Client(
            accessKey,
            secretKey,
            config
            );


            //AmazonS3Config config = new AmazonS3Config(serv);
            //config.ServiceURL = "objects.dreamhost.com";

            //AmazonS3Client s3Client = new AmazonS3Client(
            //accessKey,
            //secretKey,
            //config
            //);
            //LISTING
        }
    }
}
