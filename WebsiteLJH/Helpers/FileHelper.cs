using System.IO;

namespace WebsiteLJH
{
    /// <summary>
    /// 파일 헬퍼
    /// </summary>
    public class FileHelper
    {
        ////////////////////////////////////////////////////////////////////////////////////////// Method
        //////////////////////////////////////////////////////////////////////////////// Static
        ////////////////////////////////////////////////////////////////////// Public

        #region 유니크 파일명 구하기 - GetUniqueFileName(sourceDirectoryPath, fileName)

        /// <summary>
        /// 유니크 파일명 구하기
        /// </summary>
        /// <param name="sourceDirectoryPath">소스 디렉토리 경로</param>
        /// <param name="fileName">파일명</param>
        /// <returns>유니크 파일명</returns>
        public static string GetUniqueFileName(string sourceDirectoryPath, string fileName)
        {
            string temporaryName = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);

            bool exist = true;

            int i = 0;

            while(exist)
            {
                if(File.Exists(Path.Combine(sourceDirectoryPath, fileName)))
                {
                    fileName = temporaryName + "(" + ++i + ")" + fileExtension;
                }
                else
                {
                    exist = false;
                }
            }

            return fileName;
        }

        #endregion
    }
}