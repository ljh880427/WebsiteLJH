namespace WebsiteLJH
{
    /// <summary>
    /// HTML 헬퍼
    /// </summary>
    public class HTMLHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region 인코딩하기 - Encode(source)

        /// <summary>
        /// 인코딩하기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <returns>인코딩 문자열</returns>
        public static string Encode(string source)
        {
            string target = string.Empty;

            if(string.IsNullOrEmpty(source))
            {
                target = string.Empty;
            }
            else
            {
                target = source;
                target = target.Replace("&"   , "&amp;" );
                target = target.Replace(">"   , "&gt;"  );
                target = target.Replace("<"   , "&lt;"  );
                target = target.Replace("\r\n", "<br />");
                target = target.Replace("\""  , "&#34;" );
            }

            return target;
        }

        #endregion
        #region 탭/공백 포함 인코딩하기 - EncodeIncludingTabAndSpace(source)

        /// <summary>
        /// 탭/공백 포함 인코딩하기
        /// </summary>
        /// <param name="source">소스 문자열</param>
        /// <returns>인코딩 문자열</returns>
        public static string EncodeIncludingTabAndSpace(string source)
        {
            return Encode(source)
                .Replace("\t"     , "&nbsp;&nbsp;&nbsp;&nbsp;")
                .Replace(" " + " ", "&nbsp;&nbsp;");
        }

        #endregion
    }
}