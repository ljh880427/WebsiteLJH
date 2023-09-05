using System;
using System.IO;

namespace WebsiteLJH
{
    /// <summary>
    /// 게시판 헬퍼
    /// </summary>
    public class NoticeHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Static
        //////////////////////////////////////////////////////////////////////////////// Public

        #region REPLY 이미지 HTML 구하기 - GetReplyImageHTML(stepObject)

        /// <summary>
        /// REPLY 이미지 HTML 구하기
        /// </summary>
        /// <param name="stepObject">단계 객체</param>
        /// <returns>REPLY 이미지 HTML</returns>
        public static string GetReplyImageHTML(object stepObject)
        {
            int step = Convert.ToInt32(stepObject);

            string target = string.Empty;

            if(step == 0)
            {
                target = string.Empty;
            }
            else
            {
                for(int i = 0; i < step; i++)
                {
                    target = string.Format
                    (
                        "<img src=\"{0}\" height=\"{1}\" width=\"{2}\">",
                        "/Image/NoticeBoard/blank.gif",
                        "0",
                        (step * 15)
                    );
                }

                target += "<img src=\"/Image/NoticeBoard/reply.gif\">";
            }

            return target;
        }

        #endregion
        #region 댓글 수 HTML 구하기 - GetCommentCountHTML(commentCountObject)

        /// <summary>
        /// 댓글 수 HTML 구하기
        /// </summary>
        /// <param name="commentCountObject">댓글 수 객체</param>
        /// <returns>댓글 수 HTML</returns>
        public static string GetCommentCountHTML(object commentCountObject)
        {
            string target = string.Empty;

            try
            {
                if(Convert.ToInt32(commentCountObject) > 0)
                {
                    target =  "<img src=\"/Image/NoticeBoard/comment.gif\" />";
                    target += " (" + commentCountObject.ToString() + ")";
                }
            }
            catch(Exception)
            {
                target = string.Empty;
            }

            return target;
        }

        #endregion
        #region 신규 이미지 HTML 구하기 - GetNewImageHTML(dateObject)

        /// <summary>
        /// 신규 이미지 HTML 구하기
        /// </summary>
        /// <param name="dateObject">날짜 객체</param>
        /// <returns>신규 이미지 HTML</returns>
        public static string GetNewImageHTML(object dateObject)
        {
            if(dateObject != null)
            {
                if(!string.IsNullOrEmpty(dateObject.ToString()))
                {
                    DateTime date = Convert.ToDateTime(dateObject);

                    TimeSpan timeSpan = DateTime.Now - date;

                    string target = string.Empty;

                    if(timeSpan.TotalMinutes < 1440)
                    {
                        target = "<img src=\"/Image/NoticeBoard/new.gif\">";
                    }

                    return target;
                }
            }

            return string.Empty;
        }

        #endregion
        #region 날짜/시간 HTML 구하기 - GetDateTimeHTML(dateTimeObject)

        /// <summary>
        /// 날짜/시간 HTML 구하기
        /// </summary>
        /// <param name="dateTimeObject">날짜/시간 객체</param>
        /// <returns>날짜/시간 HTML</returns>
        public static string GetDateTimeHTML(object dateTimeObject)
        {
            if(dateTimeObject != null)
            {
                if(!string.IsNullOrEmpty(dateTimeObject.ToString()))
                {
                    string writeDateString = Convert.ToDateTime(dateTimeObject).ToString("yyyy-MM-dd");

                    if(writeDateString == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        return Convert.ToDateTime(dateTimeObject).ToString("hh:mm:ss");
                    }
                    else
                    {
                        return writeDateString;
                    }
                }
            }

            return "-";
        }

        #endregion
        #region 파일 크기 문자열 구하기 - GetFileSizeString(fileSize)

        /// <summary>
        /// 파일 크기 문자열 구하기
        /// </summary>
        /// <param name="fileSize">파일 크기</param>
        /// <returns>파일 크기 문자열</returns>
        public static string GetFileSizeString(int fileSize)
        {
            string target = string.Empty;

            if(fileSize >= 1048576)
            {
                target = string.Format("{0:F} MB", (fileSize / 1048576));
            }
            else
            {
                if(fileSize >= 1024)
                {
                    target = string.Format("{0} KB", (fileSize / 1024));
                }
                else
                {
                    target = fileSize + " Byte(s)";
                }
            }

            return target;
        }

        #endregion
        #region 이미지 파일 여부 구하기 - IsImageFile(filePath)

        /// <summary>
        /// 이미지 파일 여부 구하기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>이미지 파일 여부</returns>
        public static bool IsImageFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).Replace(".", "").ToLower();

            bool result = false;

            if(fileExtension == "gif" || fileExtension == "jpg" || fileExtension == "jpeg" || fileExtension == "png")
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        #endregion
        #region 파일 다운로드 링크 HTML 구하기 - GetFileDownloadLinkHTML(id, filePath, fileSizeString)

        /// <summary>
        /// 파일 다운로드 링크 HTML 구하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="filePath">파일 경로</param>
        /// <param name="fileSizeString">파일 크기 문자열</param>
        /// <returns>파일 다운로드 링크 HTML 구하기</returns>
        public static string GetFileDownloadLinkHTML(int id, string filePath, string fileSizeString)
        {
            if(filePath.Length > 0)
            {
                return string.Format
                (
                    "<a href=\"/NoticeBoard/Download?id={0}\">{1}</a>",
                    id.ToString(),
                    GetFileExtensionImageHTML(filePath, filePath + "(" + GetFileSizeString(Convert.ToInt32(fileSizeString)) + ")")
                );
            }
            else
            {
                return "-";
            }
        }

        #endregion
        #region 파일 확장자 이미지 HTML 구하기 - GetFileExtensionImageHTML(filePath, alternateString)

        /// <summary>
        /// 파일 확장자 이미지 HTML 구하기
        /// </summary>
        /// <param name="filePath">파일 이름</param>
        /// <param name="alternateString">대체 문자열</param>
        /// <returns>파일 확장자 이미지 HTML</returns>
        public static string GetFileExtensionImageHTML(string filePath, string alternateString)
        {
            string format = "<img src='{0}' border='0' alt='{1}'>";

            string fileExtension = Path.GetExtension(filePath).Replace(".", "").ToLower();

            string target = string.Empty;

            switch(fileExtension)
            {
                case "ace"     : target = string.Format(format, "/Image/FileExtension/ext_ace.gif"    , alternateString); break;
                case "ai"      : target = string.Format(format, "/Image/FileExtension/ext_ai.gif"     , alternateString); break;
                case "alz"     : target = string.Format(format, "/Image/FileExtension/ext_alz.gif"    , alternateString); break;
                case "arj"     : target = string.Format(format, "/Image/FileExtension/ext_arj.gif"    , alternateString); break;
                case "asa"     : target = string.Format(format, "/Image/FileExtension/ext_asa.gif"    , alternateString); break;
                case "asax"    : target = string.Format(format, "/Image/FileExtension/ext_asax.gif"   , alternateString); break;
                case "ascx"    : target = string.Format(format, "/Image/FileExtension/ext_ascx.gif"   , alternateString); break;
                case "asf"     : target = string.Format(format, "/Image/FileExtension/ext_asf.gif"    , alternateString); break;
                case "asmx"    : target = string.Format(format, "/Image/FileExtension/ext_asmx.gif"   , alternateString); break;
                case "asp"     : target = string.Format(format, "/Image/FileExtension/ext_asp.gif"    , alternateString); break;
                case "aspx"    : target = string.Format(format, "/Image/FileExtension/ext_aspx.gif"   , alternateString); break;
                case "asx"     : target = string.Format(format, "/Image/FileExtension/ext_asx.gif"    , alternateString); break;
                case "au"      : target = string.Format(format, "/Image/FileExtension/ext_au.gif"     , alternateString); break;
                case "avi"     : target = string.Format(format, "/Image/FileExtension/ext_avi.gif"    , alternateString); break;
                case "bat"     : target = string.Format(format, "/Image/FileExtension/ext_bat.gif"    , alternateString); break;
                case "bmp"     : target = string.Format(format, "/Image/FileExtension/ext_bmp.gif"    , alternateString); break;
                case "c"       : target = string.Format(format, "/Image/FileExtension/ext_c.gif"      , alternateString); break;
                case "cs"      : target = string.Format(format, "/Image/FileExtension/ext_cs.gif"     , alternateString); break;
                case "csproj"  : target = string.Format(format, "/Image/FileExtension/ext_csproj.gif" , alternateString); break;
                case "cab"     : target = string.Format(format, "/Image/FileExtension/ext_cab.gif"    , alternateString); break;
                case "chm"     : target = string.Format(format, "/Image/FileExtension/ext_chm.gif"    , alternateString); break;
                case "com"     : target = string.Format(format, "/Image/FileExtension/ext_com.gif"    , alternateString); break;
                case "config"  : target = string.Format(format, "/Image/FileExtension/ext_config.gif" , alternateString); break;
                case "cpp"     : target = string.Format(format, "/Image/FileExtension/ext_cpp.gif"    , alternateString); break;
                case "css"     : target = string.Format(format, "/Image/FileExtension/ext_css.gif"    , alternateString); break;
                case "csv"     : target = string.Format(format, "/Image/FileExtension/ext_csv.gif"    , alternateString); break;
                case "disco"   : target = string.Format(format, "/Image/FileExtension/ext_disco.gif"  , alternateString); break;
                case "dll"     : target = string.Format(format, "/Image/FileExtension/ext_dll.gif"    , alternateString); break;
                case "doc"     : target = string.Format(format, "/Image/FileExtension/ext_doc.gif"    , alternateString); break;
                case "eml"     : target = string.Format(format, "/Image/FileExtension/ext_eml.gif"    , alternateString); break;
                case "exe"     : target = string.Format(format, "/Image/FileExtension/ext_exe.gif"    , alternateString); break;
                case "gif"     : target = string.Format(format, "/Image/FileExtension/ext_gif.gif"    , alternateString); break;
                case "gz"      : target = string.Format(format, "/Image/FileExtension/ext_gz.gif"     , alternateString); break;
                case "h"       : target = string.Format(format, "/Image/FileExtension/ext_h.gif"      , alternateString); break;
                case "hlp"     : target = string.Format(format, "/Image/FileExtension/ext_hlp.gif"    , alternateString); break;
                case "htm"     : target = string.Format(format, "/Image/FileExtension/ext_htm.gif"    , alternateString); break;
                case "html"    : target = string.Format(format, "/Image/FileExtension/ext_html.gif"   , alternateString); break;
                case "hwp"     : target = string.Format(format, "/Image/FileExtension/ext_hwp.gif"    , alternateString); break;
                case "hwt"     : target = string.Format(format, "/Image/FileExtension/ext_hwt.gif"    , alternateString); break;
                case "inc"     : target = string.Format(format, "/Image/FileExtension/ext_inc.gif"    , alternateString); break;
                case "ini"     : target = string.Format(format, "/Image/FileExtension/ext_ini.gif"    , alternateString); break;
                case "jpg"     : target = string.Format(format, "/Image/FileExtension/ext_jpg.gif"    , alternateString); break;
                case "jpeg"    : target = string.Format(format, "/Image/FileExtension/ext_jpeg.gif"   , alternateString); break;
                case "js"      : target = string.Format(format, "/Image/FileExtension/ext_js.gif"     , alternateString); break;
                case "lzh"     : target = string.Format(format, "/Image/FileExtension/ext_lzh.gif"    , alternateString); break;
                case "m3u"     : target = string.Format(format, "/Image/FileExtension/ext_m3u.gif"    , alternateString); break;
                case "max"     : target = string.Format(format, "/Image/FileExtension/ext_max.gif"    , alternateString); break;
                case "mdb"     : target = string.Format(format, "/Image/FileExtension/ext_mdb.gif"    , alternateString); break;
                case "mid"     : target = string.Format(format, "/Image/FileExtension/ext_mid.gif"    , alternateString); break;
                case "mov"     : target = string.Format(format, "/Image/FileExtension/ext_mov.gif"    , alternateString); break;
                case "mp2"     : target = string.Format(format, "/Image/FileExtension/ext_mp2.gif"    , alternateString); break;
                case "mp3"     : target = string.Format(format, "/Image/FileExtension/ext_mp3.gif"    , alternateString); break;
                case "mpe"     : target = string.Format(format, "/Image/FileExtension/ext_mpe.gif"    , alternateString); break;
                case "mpeg"    : target = string.Format(format, "/Image/FileExtension/ext_mpeg.gif"   , alternateString); break;
                case "mpg"     : target = string.Format(format, "/Image/FileExtension/ext_mpg.gif"    , alternateString); break;
                case "msi"     : target = string.Format(format, "/Image/FileExtension/ext_exe.gif"    , alternateString); break;
                case ""        : target = string.Format(format, "/Image/FileExtension/ext_none.gif"   , alternateString); break;
                case "pcx"     : target = string.Format(format, "/Image/FileExtension/ext_pcx.gif"    , alternateString); break;
                case "pdb"     : target = string.Format(format, "/Image/FileExtension/ext_pdb.gif"    , alternateString); break;
                case "pdf"     : target = string.Format(format, "/Image/FileExtension/ext_pdf.gif"    , alternateString); break;
                case "pls"     : target = string.Format(format, "/Image/FileExtension/ext_pls.gif"    , alternateString); break;
                case "png"     : target = string.Format(format, "/Image/FileExtension/ext_png.gif"    , alternateString); break;
                case "ppt"     : target = string.Format(format, "/Image/FileExtension/ext_ppt.gif"    , alternateString); break;
                case "psd"     : target = string.Format(format, "/Image/FileExtension/ext_psd.gif"    , alternateString); break;
                case "ra"      : target = string.Format(format, "/Image/FileExtension/ext_ra.gif"     , alternateString); break;
                case "ram"     : target = string.Format(format, "/Image/FileExtension/ext_ram.gif"    , alternateString); break;
                case "rar"     : target = string.Format(format, "/Image/FileExtension/ext_rar.gif"    , alternateString); break;
                case "reg"     : target = string.Format(format, "/Image/FileExtension/ext_reg.gif"    , alternateString); break;
                case "resx"    : target = string.Format(format, "/Image/FileExtension/ext_resx.gif"   , alternateString); break;
                case "rm"      : target = string.Format(format, "/Image/FileExtension/ext_rm.gif"     , alternateString); break;
                case "rmi"     : target = string.Format(format, "/Image/FileExtension/ext_rmi.gif"    , alternateString); break;
                case "rtf"     : target = string.Format(format, "/Image/FileExtension/ext_rtf.gif"    , alternateString); break;
                case "sql"     : target = string.Format(format, "/Image/FileExtension/ext_sql.gif"    , alternateString); break;
                case "swf"     : target = string.Format(format, "/Image/FileExtension/ext_swf.gif"    , alternateString); break;
                case "sys"     : target = string.Format(format, "/Image/FileExtension/ext_sys.gif"    , alternateString); break;
                case "tar"     : target = string.Format(format, "/Image/FileExtension/ext_tar.gif"    , alternateString); break;
                case "tga"     : target = string.Format(format, "/Image/FileExtension/ext_tga.gif"    , alternateString); break;
                case "tif"     : target = string.Format(format, "/Image/FileExtension/ext_tif.gif"    , alternateString); break;
                case "ttf"     : target = string.Format(format, "/Image/FileExtension/ext_ttf.gif"    , alternateString); break;
                case "txt"     : target = string.Format(format, "/Image/FileExtension/ext_txt.gif"    , alternateString); break;
                case "vb"      : target = string.Format(format, "/Image/FileExtension/ext_vb.gif"     , alternateString); break;
                case "vbs"     : target = string.Format(format, "/Image/FileExtension/ext_vbs.gif"    , alternateString); break;
                case "vbdisco" : target = string.Format(format, "/Image/FileExtension/ext_vbdisco.gif", alternateString); break;
                case "wav"     : target = string.Format(format, "/Image/FileExtension/ext_wav.gif"    , alternateString); break;
                case "wax"     : target = string.Format(format, "/Image/FileExtension/ext_wax.gif"    , alternateString); break;
                case "webinfo" : target = string.Format(format, "/Image/FileExtension/ext_webinfo.gif", alternateString); break;
                case "wma"     : target = string.Format(format, "/Image/FileExtension/ext_wma.gif"    , alternateString); break;
                case "wmv"     : target = string.Format(format, "/Image/FileExtension/ext_wmv.gif"    , alternateString); break;
                case "wmx"     : target = string.Format(format, "/Image/FileExtension/ext_wmx.gif"    , alternateString); break;
                case "wri"     : target = string.Format(format, "/Image/FileExtension/ext_wri.gif"    , alternateString); break;
                case "wvx"     : target = string.Format(format, "/Image/FileExtension/ext_wvx.gif"    , alternateString); break;
                case "xls"     : target = string.Format(format, "/Image/FileExtension/ext_xls.gif"    , alternateString); break;
                case "xml"     : target = string.Format(format, "/Image/FileExtension/ext_xml.gif"    , alternateString); break;
                case "zip"     : target = string.Format(format, "/Image/FileExtension/ext_zip.gif"    , alternateString); break;
                default        : target = string.Format(format, "/Image/FileExtension/ext_unknown.gif", alternateString); break;
            }

            return target;
        }

        #endregion
    }
}