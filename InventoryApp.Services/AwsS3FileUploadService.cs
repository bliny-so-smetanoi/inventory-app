using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using InventoryApp.Contracts.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace InventoryApp.Services
{
    public class AwsS3FileUploadService
    {
        private readonly AwsS3Options _options;
        public AwsS3FileUploadService(IOptions<AwsS3Options> options)
        {
            _options = options.Value;
        }

        public async Task<string> UploadXls(Stream file, string pathToDelete)
        {
            var bucketName = "inventory-app-aitu";

            var fileExt = ".xls";
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = file,
                    Key = docName,
                    BucketName = _options.BucketName
                };
                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (File.Exists(pathToDelete))
                {
                    File.Delete(pathToDelete);
                }
            }

            return "https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/" + docName;
        }

        public async Task<string> UploadLogs(Stream file)
        {
            var bucketName = "inventory-app-aitu";

            var fileExt = ".txt";
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = file,
                    Key = docName,
                    BucketName = _options.BucketName
                };
                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                
            }

            return "https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/" + docName;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var bucketName = "inventory-app-aitu";
            await using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = memoryStream,
                    Key = docName,
                    BucketName = _options.BucketName,
                };
                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);
            }
            catch (AmazonS3Exception ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw;
            }

            return "https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/" + docName;
        }

        public async Task<string> UploadReport(Stream file, string pathToDelete)
        {
            var bucketName = "inventory-app-aitu";

            var fileExt = ".pdf";
            var docName = $"{Guid.NewGuid()}{fileExt}";
            var credentials = new BasicAWSCredentials(_options.AccessKey, _options.SecretKey);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUNorth1
            };

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = file,
                    Key = docName,
                    BucketName = _options.BucketName
                };
                using var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (File.Exists(pathToDelete))
                {
                    File.Delete(pathToDelete);
                }
            }

            return "https://inventory-app-aitu.s3.eu-north-1.amazonaws.com/" + docName;
        }

        
    }
}

        
