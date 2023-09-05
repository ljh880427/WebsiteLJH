using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using WebsiteLJH.Models;

namespace WebsiteLJH.Controllers
{
    /// <summary>
    /// 게시판 컨트롤러
    /// </summary>
    public class NoticeBoardController : Controller
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 환경 변수
        /// </summary>
        private IWebHostEnvironment environment;

        /// <summary>
        /// 게시판 저장소
        /// </summary>
        private INoticeRepository noticeRepository;

        /// <summary>
        /// 댓글 저장소
        /// </summary>
        private ICommentRepository commentRepository;

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
        public int PageIndex { get; set; } = 0;

        #endregion
        #region 레코드 카운트 - RecordCount

        /// <summary>
        /// 레코드 카운트
        /// </summary>
        public int RecordCount { get; set; } = 0;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - NoticeBoardController(environment, noticeRepository, commentRepository)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="environment">환경</param>
        /// <param name="noticeRepository">게시판 저장소</param>
        /// <param name="commentRepository">댓글 저장소</param>
        public NoticeBoardController(IWebHostEnvironment environment, INoticeRepository noticeRepository, ICommentRepository commentRepository)
        {
            this.environment = environment;
            this.noticeRepository = noticeRepository;
            this.commentRepository = commentRepository;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 인덱스 페이지 처리하기 - Index()

        /// <summary>
        /// 인덱스 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult Index()
        {            
            
            SearchMode = (!string.IsNullOrEmpty(Request.Query["SearchField"]) && !string.IsNullOrEmpty(Request.Query["SearchQuery"]));

            if(SearchMode)
            {
                SearchField = Request.Query["SearchField"].ToString();
                SearchQuery = Request.Query["SearchQuery"].ToString();
            }

            if(!string.IsNullOrEmpty(Request.Query["Page"].ToString()))
            {
                PageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
            }

            if(!string.IsNullOrEmpty(Request.Cookies["WebsiteLJHPageNumber"]))
            {
                if(!String.IsNullOrEmpty(Request.Cookies["WebsiteLJHPageNumber"]))
                {
                    PageIndex = Convert.ToInt32(Request.Cookies["WebsiteLJHPageNumber"]);
                }
                else
                {
                    PageIndex = 0;
                }
            }

            IEnumerable<NoticeModel> noticeEnumerable;

            if(!SearchMode)
            {
                RecordCount = this.noticeRepository.GetCount();

                noticeEnumerable = this.noticeRepository.GetNoticeList(PageIndex);
            }
            else
            {
                RecordCount = this.noticeRepository.GetCount(SearchField, SearchQuery);

                noticeEnumerable = this.noticeRepository.GetNoticeList(PageIndex, SearchField, SearchQuery);
            }

            ViewBag.RecordCount = RecordCount;
            ViewBag.SearchMode  = SearchMode;
            ViewBag.SearchField = SearchField;
            ViewBag.SearchQuery = SearchQuery;

            return View(noticeEnumerable);
        }

        #endregion
        #region 생성 페이지 처리하기 - Create()

        /// <summary>
        /// 생성 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.FormType         = BoardWriteFormType.Write;
            ViewBag.TitleDescription = "게시글 쓰기 - 다음 항목들을 입력해 주시기 바랍니다.";
            ViewBag.SaveButtonText   = "저장";

            return View();
        }

        #endregion
        #region 생성 페이지 처리하기 - Create(sourceNotice, formFileCollection)

        /// <summary>
        /// 생성 페이지 처리하기
        /// </summary>
        /// <param name="sourceNotice">소스 게시글</param>
        /// <param name="formFileCollection">폼 파일 컬렉션</param>
        /// <returns>액션 결과</returns>
        [HttpPost]
        public async Task<IActionResult> Create(NoticeModel sourceNotice, ICollection<IFormFile> formFileCollection)
        {
            string fileName = string.Empty;
            int    fileSize = 0;

            string uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload");

            foreach(IFormFile formFile in formFileCollection)
            {
                if(formFile.Length > 0)
                {
                    fileSize = Convert.ToInt32(formFile.Length);

                    fileName = FileHelper.GetUniqueFileName
                    (
                        uploadDirectoryPath,
                        Path.GetFileName(ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"'))
                    );

                    using(FileStream fileStream = new FileStream(Path.Combine(uploadDirectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }

            NoticeModel targetNotice = new NoticeModel();

            targetNotice.Name        = sourceNotice.Name;
            targetNotice.MailAddress = HTMLHelper.Encode(sourceNotice.MailAddress);
            targetNotice.Homepage    = sourceNotice.Homepage;
            targetNotice.Title       = HTMLHelper.Encode(sourceNotice.Title);
            targetNotice.Content     = sourceNotice.Content;
            targetNotice.FileName    = fileName;
            targetNotice.FileSize    = fileSize;
            targetNotice.Password    = sourceNotice.Password;
            targetNotice.WriteIP     = HttpContext.Connection.RemoteIpAddress.ToString();
            targetNotice.Encoding    = sourceNotice.Encoding;

            this.noticeRepository.WriteNotice(targetNotice);

            TempData["Message"] = "게시글이 생성되었습니다.";

            return RedirectToAction("Index");
        }

        #endregion
        #region 수정 페이지 처리하기 - Update(id)

        /// <summary>
        /// 수정 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.FormType         = BoardWriteFormType.Modify;
            ViewBag.TitleDescription = "게시글 수정 - 다음 항목들을 수정해 주시기 바랍니다.";
            ViewBag.SaveButtonText   = "수정";

            NoticeModel notice = this.noticeRepository.GetNotice(id);

            if(notice.FileName.Length > 1)
            {
                ViewBag.FileName         = notice.FileName;
                ViewBag.FileSize         = notice.FileSize;
                ViewBag.FileNamePrevious = $"기존 업로드 파일명 : {notice.FileName}";
            }
            else
            {
                ViewBag.FileName = string.Empty;
                ViewBag.FileSize = 0;
            }

            return View(notice);
        }

        #endregion
        #region 수정 페이지 처리하기 - Update(sourceNotice, formFileCollection, id, previousFileName, previousFileSize)

        /// <summary>
        /// 수정 페이지 처리하기
        /// </summary>
        /// <param name="sourceNotice">소스 게시글</param>
        /// <param name="formFileCollection">폼 파일 컬렉션</param>
        /// <param name="id">ID</param>
        /// <param name="previousFileName">이전 파일명</param>
        /// <param name="previousFileSize">이전 파일 크기</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update
        (
            NoticeModel            sourceNotice,
            ICollection<IFormFile> formFileCollection,
            int                    id,
            string                 previousFileName = "",
            int                    previousFileSize = 0
        )
        {
            ViewBag.FormType         = BoardWriteFormType.Modify;
            ViewBag.TitleDescription = "게시글 수정 - 아래 항목을 수정해 주시기 바랍니다.";
            ViewBag.SaveButtonText   = "수정";

            string fileName = string.Empty;
            int    fileSize = 0;

            if(previousFileName != null)
            {
                fileName = previousFileName;
                fileSize = previousFileSize;
            }

            string uploadDirectoryPath = Path.Combine(this.environment.WebRootPath, "upload");

            foreach(IFormFile formFile in formFileCollection)
            {
                if(formFile.Length > 0)
                {
                    fileSize = Convert.ToInt32(formFile.Length);

                    fileName = FileHelper.GetUniqueFileName
                    (
                        uploadDirectoryPath,
                        Path.GetFileName(ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"'))
                    );

                    using(FileStream fileStream = new FileStream(Path.Combine(uploadDirectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }
            
            NoticeModel targetNotice = new NoticeModel();

            targetNotice.ID          = id;
            targetNotice.Name        = sourceNotice.Name;
            targetNotice.MailAddress = HTMLHelper.Encode(sourceNotice.MailAddress);
            targetNotice.Homepage    = sourceNotice.Homepage;
            targetNotice.Title       = HTMLHelper.Encode(sourceNotice.Title);
            targetNotice.Content     = sourceNotice.Content;
            targetNotice.FileName    = fileName;
            targetNotice.FileSize    = fileSize;
            targetNotice.Password    = sourceNotice.Password;
            targetNotice.UpdateIP    = HttpContext.Connection.RemoteIpAddress.ToString();
            targetNotice.Encoding    = sourceNotice.Encoding;

            int recordCount = this.noticeRepository.UpdateNotice(targetNotice);

            if(recordCount > 0)
            {
                TempData["Message"] = "게시글 수정이 완료되었습니다.";

                return RedirectToAction("Details", new { ID = id });
            }
            else
            {
                ViewBag.ErrorMessage = "게시글 수정을 실패했습니다. 패스워드를 확인해 주시기 바랍니다.";

                return View(targetNotice);
            }
        }

        #endregion
        #region 삭제 페이지 처리하기 - Delete(id)

        /// <summary>
        /// 삭제 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ViewBag.ID = id;

            return View();
        }

        #endregion
        #region 삭제 페이지 처리하기 - Delete(id, password)

        /// <summary>
        /// 삭제 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>액션 결과</returns>
        [HttpPost]
        public IActionResult Delete(int id, string password)
        {
            if(this.noticeRepository.DeleteNotice(id, password) > 0)
            {
                TempData["Message"] = "게시글이 삭제되었습니다.";

                //return RedirectToAction("DeleteCompleted");
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "게시글 삭제가 실패했습니다. 패스워드를 확인해 주시기 바랍니다.";
            }

            ViewBag.ID = id;

            return View();
        }

        #endregion
        #region 삭제 완료시 페이지 처리하기 - DeleteCompleted()

        /// <summary>
        /// 삭제 완료시 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult DeleteCompleted()
        {
            return View();
        }

        #endregion
        #region 상세 페이지 처리하기 - Details(id)

        /// <summary>
        /// 상세 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        public IActionResult Details(int id)
        {
            NoticeModel notice = this.noticeRepository.GetNotice(id);

            ContentEncodingType encodingType = (ContentEncodingType)Enum.Parse(typeof(ContentEncodingType), notice.Encoding);

            string targetContent = string.Empty;

            switch(encodingType)
            {
                case ContentEncodingType.Text :

                    targetContent = HTMLHelper.EncodeIncludingTabAndSpace(notice.Content);

                    break;

                case ContentEncodingType.HTML :

                    targetContent = notice.Content;

                    break;

                case ContentEncodingType.Mixed :

                    targetContent = notice.Content.Replace("\r\n", "<br />");

                    break;

                default :

                    targetContent = notice.Content;

                    break;
            }

            ViewBag.Content = targetContent;
            
            if(notice.FileName.Length > 1)
            {
                ViewBag.FileName = String.Format
                (
                    "<a href='/NoticeBoard/Download?id={0}'>{1} {2} / 전송 수 : {3}</a>",
                    notice.ID,
                    "<img src=\"/Image/FileExtension/ext_zip.gif\" border=\"0\">",
                    notice.FileName,
                    notice.DownloadCount
                );

                if(NoticeHelper.IsImageFile(notice.FileName))
                {
                    ViewBag.ImageDownloadURL = $"<img src=\'/NoticeBoard/DownloadImage/{notice.ID}\'><br />";
                }
            }
            else
            {
                ViewBag.FileName = "(업로드된 파일이 없습니다.)";
            }

            NoticeCommentModel noticeComment = new NoticeCommentModel();

            noticeComment.CommentList = this.commentRepository.GetCommentList(notice.ID);
            noticeComment.NoticeID    = notice.ID;

            ViewBag.NoticeComment = noticeComment;

            return View(notice);
        }

        #endregion
        #region 다운로드 페이지 처리하기 - Download(id)

        /// <summary>
        /// 다운로드 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>파일 결과</returns>
        public FileResult Download(int id)
        {
            string fileName = string.Empty;
            
            fileName = this.noticeRepository.GetFileName(id);

            if(fileName == null)
            {
                return null;
            }
            else
            {
                this.noticeRepository.UpdateDownloadCount(id);

                byte[] fileByteArray = System.IO.File.ReadAllBytes
                (
                    Path.Combine
                    (
                        this.environment.WebRootPath,
                        "upload",
                        fileName
                    )
                );

                return File(fileByteArray, "application/octet-stream", fileName);
            }
        }

        #endregion
        #region 이미지 다운로드 페이지 처리하기 - DownloadImage(id)

        /// <summary>
        /// 이미지 다운로드 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>파일 결과</returns>
        public FileResult DownloadImage(int id)
        {
            string fileName = string.Empty;

            fileName = this.noticeRepository.GetFileName(id);

            if(fileName == null)
            {
                return null;
            }
            else
            {
                string fileExtension = Path.GetExtension(fileName);
                string contentType   = string.Empty;

                if(fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                {
                    switch(fileExtension)
                    {
                        case ".gif"  : contentType = "image/gif";  break;
                        case ".jpg"  : contentType = "image/jpeg"; break;
                        case ".jpeg" : contentType = "image/jpeg"; break;
                        case ".png"  : contentType = "image/png";  break;
                    }
                }

                this.noticeRepository.UpdateDownloadCount(fileName);

                byte[] fileByteArray = System.IO.File.ReadAllBytes(Path.Combine(this.environment.WebRootPath, "upload") + "\\" + fileName);

                return File(fileByteArray, contentType, fileName);
            }
        }

        #endregion
        #region 답변 페이지 처리하기 - Reply(id)

        /// <summary>
        /// 답변 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpGet]
        public IActionResult Reply(int id)
        {
            ViewBag.FormType         = BoardWriteFormType.Reply;
            ViewBag.TitleDescription = "글 답변 - 다음 항목을 입력해 주시기 바랍니다.";
            ViewBag.SaveButtonText   = "답변";

            NoticeModel notice = this.noticeRepository.GetNotice(id);

            NoticeModel newNotice = new NoticeModel();

            newNotice.Title   = $"Re : {notice.Title}";
            newNotice.Content = $"\n\nOn {notice.WriteDate}, '{notice.Name}' wrote:\n----------\n>" +
                                $"{notice.Content.Replace("\n", "\n>")}\n---------";

            return View(newNotice);
        }

        #endregion
        #region 답변 페이지 처리하기 - Reply(sourceNotice, formFileCollection, id)

        /// <summary>
        /// 답변 페이지 처리하기
        /// </summary>
        /// <param name="sourceNotice">게시글</param>
        /// <param name="formFileCollection">폼 파일 컬렉션</param>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpPost]
        public async Task<IActionResult> Reply(NoticeModel sourceNotice, ICollection<IFormFile> formFileCollection, int id)
        {
            string fileName = string.Empty;
            int    fileSize = 0;

            string uploadDicrectoryPath = Path.Combine(this.environment.WebRootPath, "upload");

            foreach(IFormFile formFile in formFileCollection)
            {
                if(formFile.Length > 0)
                {
                    fileSize = Convert.ToInt32(formFile.Length);

                    fileName = FileHelper.GetUniqueFileName
                    (
                        uploadDicrectoryPath,
                        Path.GetFileName(ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"'))
                    );

                    using(FileStream fileStream = new FileStream(Path.Combine(uploadDicrectoryPath, fileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
            }

            NoticeModel targetNotice = new NoticeModel();

            targetNotice.ID          = targetNotice.ParentID = Convert.ToInt32(id);
            targetNotice.Name        = sourceNotice.Name;
            targetNotice.MailAddress = HTMLHelper.Encode(sourceNotice.MailAddress);
            targetNotice.Homepage    = sourceNotice.Homepage;
            targetNotice.Title       = HTMLHelper.Encode(sourceNotice.Title);
            targetNotice.Content     = sourceNotice.Content;
            targetNotice.FileName    = fileName;
            targetNotice.FileSize    = fileSize;
            targetNotice.Password    = sourceNotice.Password;
            targetNotice.WriteIP     = HttpContext.Connection.RemoteIpAddress.ToString();
            targetNotice.Encoding    = sourceNotice.Encoding;

            this.noticeRepository.ReplyNotice(targetNotice);

            TempData["Message"] = "답변 글이 저장되었습니다.";

            return RedirectToAction("Index");
        }

        #endregion
        #region 댓글 추가 페이지 처리하기 - AddComment(noticeID, name, password, comment)

        /// <summary>
        /// 댓글 추가 페이지 처리하기
        /// </summary>
        /// <param name="noticeID">게시글 ID</param>
        /// <param name="name">성명</param>
        /// <param name="password">패스워드</param>
        /// <param name="comment">댓글</param>
        /// <returns>액션 결과</returns>
        [HttpPost]
        public IActionResult AddComment(int noticeID, string name, string password, string comment)
        {
            CommentModel newComment = new CommentModel();

            newComment.NoticeID = noticeID;
            newComment.Name     = name;
            newComment.Password = password;
            newComment.Comment  = comment;

            this.commentRepository.WriteComment(newComment);
            
            return RedirectToAction("Details", new { ID = noticeID });
        }

        #endregion
        #region 댓글 삭제 페이지 처리하기 - DeleteComment(noticeID, id)

        /// <summary>
        /// 댓글 삭제 페이지 처리하기
        /// </summary>
        /// <param name="noticeID">게시글 ID</param>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpGet]
        public IActionResult DeleteComment(string noticeID, string id)
        {
            ViewBag.NoticeID = noticeID;
            ViewBag.ID       = id;

            return View();
        }

        #endregion
        #region 댓글 삭제 페이지 처리하기 - DeleteComment(noticeID, id, password)

        /// <summary>
        /// 댓글 삭제 페이지 처리하기
        /// </summary>
        /// <param name="noticeID">게시글 ID</param>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>액션 결과</returns>
        [HttpPost]
        public IActionResult DeleteComment(string noticeID, string id, string password)
        {
            if(this.commentRepository.GetCount(Convert.ToInt32(noticeID), Convert.ToInt32(id), password) > 0)
            {
                this.commentRepository.DeleteComment(Convert.ToInt32(noticeID), Convert.ToInt32(id), password);

                return RedirectToAction("Details", new { ID = noticeID });
            }

            ViewBag.NoticeID     = noticeID;
            ViewBag.ID           = id;
            ViewBag.ErrorMessage = "패스워드가 틀립니다. 다시 입력해 주시기 바랍니다.";

            return View();
        }

        #endregion
        #region 게시글 고정 페이지 처리하기 - Pin(id)

        /// <summary>
        /// 게시글 고정 페이지 처리하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>액션 결과</returns>
        [HttpGet]
        [Authorize("Administrators")]
        public IActionResult Pin(int id)
        {
            this.noticeRepository.PinNotice(id); 

            return RedirectToAction("Details", new { ID = id });
        }

        #endregion
        #region 최근 게시글 리스트 페이지 처리하기 - ListRecentNotice()

        /// <summary>
        /// 최근 게시글 리스트 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult ListRecentNotice()
        {
            return View();
        }

        #endregion
        #region 최근 댓글 리스트 페이지 처리하기 - ListRecentComment()

        /// <summary>
        /// 최근 댓글 리스트 페이지 처리하기
        /// </summary>
        /// <returns>액션 결과</returns>
        public IActionResult ListRecentComment()
        {
            return View();
        }

        #endregion
    }
}