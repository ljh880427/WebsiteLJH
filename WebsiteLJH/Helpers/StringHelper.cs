using System.Globalization;

namespace WebsiteLJH
{
    /// <summary>
    /// 문자열 헬퍼
    /// </summary>
    public static class StringHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 문자열 잘라내기 - CutString(source, length)

        /// <summary>
        /// 문자열 잘라내기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <param name="length">길이</param>
        /// <returns>잘라낸 문자열</returns>
        public static string CutString(this string source, int length)
        {
            if(source.Length > (length - 3))
            {
                return source.Substring(0, length - 3) + "...";
            }

            return source;
        }

        #endregion
        #region 유니코드 문자열 잘라내기 - CutUnicodeString(source, length)

        /// <summary>
        /// 유니코드 문자열 잘라내기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <param name="length">길이</param>
        /// <returns>잘라낸 유니코드 문자열</returns>
        public static string CutUnicodeString(this string source, int length)
        {
            string target = source;

            StringInfo stringInfo = new StringInfo(source);

            if(stringInfo.LengthInTextElements > (length - 3))
            {
                target = stringInfo.SubstringByTextElements(0, length - 3) + "...";
            }

            return target;
        }

        #endregion
    }
}