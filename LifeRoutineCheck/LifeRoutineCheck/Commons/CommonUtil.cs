using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LifeRoutineCheck.Commons
{
    public class CommonUtil
    {
        #region 初期設定
        private static IAmazonS3 client;
        private static NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        #region S3へのアクセス画像URLの取得処理
        /// <summary> S3へのアクセス画像URLの取得処理 </summary>
        /// <param name="file_nm"></param>
        /// <returns>画像のURL</returns>
        public static string GetS3AccessUrl(string file_nm, string bucket_nm)
        {
            string url = "";

            try
            {
                if (!string.IsNullOrEmpty(file_nm))
                {
                    var secretData = new SecretData();
                    //S3から画像情報を取得
                    using (var client = new AmazonS3Client(secretData.GetAccessKey(), secretData.GetSecretKey(), Amazon.RegionEndpoint.USEast1))
                    {
                        GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                        {
                            BucketName = bucket_nm,
                            Key = file_nm,
                            Expires = DateTime.Now.AddMinutes(60)
                        };

                        //画像のURLを発行
                        url = client.GetPreSignedURL(request1);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("S3アクセス用URLの取得エラー");
                throw;
            }

            return url;
        }
        #endregion

        #region S3へのファイルアップロード処理
        /// <summary> S3へのファイルアップロード処理 </summary>
        /// <param name="file_Path"> アップロードするファイルのフルパス</param>
        /// <param name="bucket_Nm"> S3のバケット名 </param>
        /// <param name="extention"> ファイルの拡張子 </param>
        /// <param name="file_Nm"> ファイル名 </param>
        /// <returns> アップロードしたファイル名 </returns>
        public static async Task<string> S3FileUpload(string file_Path, string bucket_Nm, string extention, string file_Nm = null)
        {
            var fullFile_Nm = string.Empty;

            try
            {
                var secretData = new SecretData();

                using (client = new AmazonS3Client(secretData.GetAccessKey(), secretData.GetSecretKey(), Amazon.RegionEndpoint.USEast1))
                {
                    if (string.IsNullOrEmpty(file_Nm))
                    {
                        file_Nm = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyyMMddhhmm");
                    }

                    fullFile_Nm = file_Nm + extention;

                    // アップロード対象を定義
                    var putRequest = new PutObjectRequest
                    {
                        BucketName = bucket_Nm,
                        Key = fullFile_Nm,
                        FilePath = file_Path,
                    };

                    var responce = await client.PutObjectAsync(putRequest);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("S3アクセス用URLの取得エラー");
                throw;
            }

            return fullFile_Nm;
        }
        #endregion
    }
}
