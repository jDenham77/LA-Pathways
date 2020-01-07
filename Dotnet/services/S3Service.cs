using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.Transfer;
using Amazon.S3;
using Sabio.Services.Interfaces;
using Sabio.Data.Providers;
using Amazon;
using Microsoft.AspNetCore.Http;
using Sabio.Models.Requests.Files;
using System.Data.SqlClient;
using System.Data;
using Sabio.Models.AppSettings;
using Amazon.Runtime;
using System.Linq;
using Sabio.Models.Domain;
using Sabio.Models.Enums;

namespace Sabio.Services
{
    public class S3Service : IS3Service
    {
        IDataProvider _data = null;
        Sabio.Models.AppSettings.AWSCredentials _awsCredentials = null;

        public S3Service(IDataProvider data, IOptions<Sabio.Models.AppSettings.AWSCredentials> awsCredentials)
        {
            _data = data;
            _awsCredentials = awsCredentials.Value;
        }


        public async Task<string> UploadFileAsync(IFormFile file)
        {

            string result = null;

            BasicAWSCredentials basicAWSCredentials = new BasicAWSCredentials(_awsCredentials.AccessKey, _awsCredentials.Secret);
            AmazonS3Client _s3Client = new AmazonS3Client(basicAWSCredentials, RegionEndpoint.USWest2);

            var fileTransferUtility = new TransferUtility(_s3Client);

            string filePath = System.IO.Path.GetTempFileName();

            string keyName = "LaPathways-" + Guid.NewGuid() + "-" + file.FileName;


            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                fileTransferUtility.Upload(stream, _awsCredentials.BucketName, keyName);
            }
            string url = "https://sabio-training.s3-us-west-2.amazonaws.com/" + keyName;

            result = url;


            return result;
        }
        public bool Add(FileAddRequest model, int currentUserId)
        {
            bool result = false;
            if (model.Urls.Count > 0)
            {

                string procName = "[dbo].[UrlFileTypes_Insert_Multiple]";
                _data.ExecuteNonQuery(procName,
                    inputParamMapper: delegate (SqlParameterCollection col)
                    {
                        if (model.Urls.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Url", typeof(string));
                            dt.Columns.Add("FileTypeId", typeof(int));
                            foreach (string item in model.Urls)
                            {
                                dt.Rows.Add(item, getFileTypeIdFromFileName(item));
                            };
                            col.AddWithValue("@UrlFileTypeList", dt);
                        }
                        col.AddWithValue("@CreatedBy", currentUserId);
                    },
                    returnParameters: null);
                result = true;
            }
            return result;
        }

        private int getFileTypeIdFromFileName(string str)
        {
            string result = null;
            int fileTypeId = 0;


            result = str.Split('.').Last();

            FileTypes fileTypes = new FileTypes();


            Enum.TryParse(result, true, out fileTypes);


            switch (fileTypes)
            {
                case FileTypes.PDF:
                    fileTypeId = 1;
                    break;
                case FileTypes.Text:
                    fileTypeId = 2;
                    break;
                case FileTypes.JPEG:
                    fileTypeId = 3;
                    break;
                case FileTypes.PNG:
                    fileTypeId = 4;
                    break;
                case FileTypes.Word:
                    fileTypeId = 5;
                    break;
                case FileTypes.Excel:
                    fileTypeId = 6;
                    break;
                case FileTypes.Powerpoint:
                    fileTypeId = 7;
                    break;
                case FileTypes.JPG:

                default:
                    fileTypeId = 8;
                    break;
            }

            return fileTypeId;

        }

    }
}
