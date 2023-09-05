using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;

namespace WebsiteLJH.TagHelpers
{
    /// <summary>
    /// 페이징 헬퍼
    /// </summary>
    public class PagingHelperTagHelper : TagHelper
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 레코드 수
        /// </summary>
        private int recordCount;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 검색 모드 - SearchMode

        /// <summary>
        /// 검색 모드
        /// </summary>
        public bool SearchMode { get; set; } = false;

        #endregion
        #region 검색 필드 - SearchField

        /// <summary>
        /// 검색 필드
        /// </summary>
        public string SearchField { get; set; }

        #endregion
        #region 검색 쿼리 - SearchQuery

        /// <summary>
        /// 검색 쿼리
        /// </summary>
        public string SearchQuery { get; set; }

        #endregion
        #region 페이지 인덱스 - PageIndex

        /// <summary>
        /// 페이지 인덱스
        /// </summary>
        /// <remarks>0부터 시작한다.</remarks>
        public int PageIndex { get; set; } = 0;

        #endregion
        #region 페이지 수 - PageCount

        /// <summary>
        /// 페이지 수
        /// </summary>
        public int PageCount { get; set; }

        #endregion
        #region 페이지 크기 - PageSize

        /// <summary>
        /// 페이지 크기
        /// </summary>
        public int PageSize { get; set; } = 10;

        #endregion
        #region URL - URL

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get; set; }

        #endregion
        #region 레코드 수 - RecordCount

        /// <summary>
        /// 레코드 수
        /// </summary>
        public int RecordCount
        {
            get
            {
                return this.recordCount;
            }
            set
            {
                this.recordCount = value;

                PageCount = ((this.recordCount - 1) / PageSize) + 1;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 처리하기 - Process(context, output)

        /// <summary>
        /// 처리하기
        /// </summary>
        /// <param name="context">컨텍스트</param>
        /// <param name="output">출력</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";

            output.Attributes.Add("class", "pagination pagination-sm");

            if(PageIndex == 0)
            {
                PageIndex = 1;
            }

            int i;

            StringBuilder stringBuilder = new StringBuilder();

            if(PageIndex > 10)
            {
                if(!SearchMode)
                {
                    stringBuilder.Append("<li><a href=\"");
                    stringBuilder.Append(URL);
                    stringBuilder.Append("?Page=");
                    stringBuilder.Append(Convert.ToString(((PageIndex - 1) / (int)10) * 10));
                    stringBuilder.Append("\">◀ </a></li>");
                }
                else
                {
                    stringBuilder.Append("<li><a href=\"");
                    stringBuilder.Append(URL);
                    stringBuilder.Append("?Page=");
                    stringBuilder.Append(Convert.ToString(((PageIndex - 1) / (int)10) * 10));
                    stringBuilder.Append("&SearchField=");
                    stringBuilder.Append(SearchField);
                    stringBuilder.Append("&SearchQuery=");
                    stringBuilder.Append(SearchQuery);
                    stringBuilder.Append("\">◀ </a></li>");
                }
            }
            else
            {
                stringBuilder.Append("<li class='disabled'><a>◁ </a></li>");
            }

            int startPage = ((PageIndex - 1) / (int)10) * 10 + 1;
            int endPage   = (((PageIndex - 1) / (int)10) + 1) * 10;

            for(i = startPage; i <= endPage; i++)
            {
                if(i > PageCount)
                {
                    break;
                }

                if(i == PageIndex)
                {
                    stringBuilder.Append("<li class='active'><a href='#'><b>");
                    stringBuilder.Append(i.ToString());
                    stringBuilder.Append("</b></a></li>");
                }
                else
                {
                    if(!SearchMode)
                    {
                        stringBuilder.Append("<li><a href=\"");
                        stringBuilder.Append(URL);
                        stringBuilder.Append("?Page=");
                        stringBuilder.Append(i.ToString());
                        stringBuilder.Append("\">");
                        stringBuilder.Append(i.ToString());
                        stringBuilder.Append("</a></li>");
                    }
                    else
                    {
                        stringBuilder.Append("<li><a href=\"");
                        stringBuilder.Append(URL);
                        stringBuilder.Append("?Page=");
                        stringBuilder.Append(i.ToString());
                        stringBuilder.Append("&SearchField=");
                        stringBuilder.Append(SearchField);
                        stringBuilder.Append("&SearchQuery=");
                        stringBuilder.Append(SearchQuery);
                        stringBuilder.Append("\">");
                        stringBuilder.Append(i.ToString());
                        stringBuilder.Append("</a></li>");
                    }
                }

                if(i < endPage)
                {
                    stringBuilder.Append("&nbsp;");
                }
            }

            if(i < PageCount)
            {
                if(!SearchMode)
                {
                    stringBuilder.Append("<li><a href=\"");
                    stringBuilder.Append(URL);
                    stringBuilder.Append("?Page=");
                    stringBuilder.Append(Convert.ToString(((PageIndex - 1) / (int)10) * 10 + 11));
                    stringBuilder.Append("\"> ▶</a></li>");
                }
                else
                {
                    stringBuilder.Append("<li><a href=\"");
                    stringBuilder.Append(URL);
                    stringBuilder.Append("?Page=");
                    stringBuilder.Append(Convert.ToString(((PageIndex - 1) / (int)10) * 10 + 11));
                    stringBuilder.Append("&SearchField=");
                    stringBuilder.Append(SearchField);
                    stringBuilder.Append("&SearchQuery=");
                    stringBuilder.Append(SearchQuery);
                    stringBuilder.Append("\"> ▶</a></li>");
                }
            }
            else
            {
                stringBuilder.Append("<li class='disabled'><a> ▷</a></li>");
            }

            output.Content.AppendHtml(stringBuilder.ToString());
        }

        #endregion
    }
}