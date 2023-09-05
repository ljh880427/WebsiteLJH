using System.Collections.Generic;

namespace WebsiteLJH.Models
{
    /// <summary>
    /// 게시판 저장소 인터페이스
    /// </summary>
    public interface INoticeRepository
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Method

        #region 게시판 저장하기 - SaveNotice(notice, formType)

        /// <summary>
        /// 게시판 저장하기
        /// </summary>
        /// <param name="notice">게시판</param>
        /// <param name="formType">폼 타입</param>
        /// <returns>처리 레코드 수</returns>
        int SaveNotice(NoticeModel notice, BoardWriteFormType formType);

        #endregion
        #region 게시판 쓰기 - WriteNotice(notice)

        /// <summary>
        /// 게시판 쓰기
        /// </summary>
        /// <param name="notice">게시판</param>
        void WriteNotice(NoticeModel notice);

        #endregion
        #region 게시판 수정하기 - UpdateNotice(notice)

        /// <summary>
        /// 게시판 수정하기
        /// </summary>
        /// <param name="notice">게시판</param>
        /// <returns>처리 레코드 수</returns>
        int UpdateNotice(NoticeModel notice);

        #endregion
        #region 게시판 답변하기 - ReplyNotice(notice)

        /// <summary>
        /// 게시판 답변하기
        /// </summary>
        /// <param name="notice">게시판</param>
        void ReplyNotice(NoticeModel notice);

        #endregion
        #region 게시판 삭제하기 - DeleteNotice(id, password)

        /// <summary>
        /// 게시판 삭제하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="password">패스워드</param>
        /// <returns>처리 레코드 수</returns>
        int DeleteNotice(int id, string password);

        #endregion
        #region 카운트 구하기 - GetCount()

        /// <summary>
        /// 카운트 구하기
        /// </summary>
        int GetCount();

        #endregion
        #region  카운트 구하기 - GetCount(searchField, searchQuery)

        /// <summary>
        /// 카운트 구하기
        /// </summary>
        /// <param name="searchField">검색 필드</param>
        /// <param name="searchQuery">검색 쿼리</param>
        /// <returns>카운트</returns>
        int GetCount(string searchField, string searchQuery);

        #endregion
        #region 게시판 리스트 구하기 - GetNoticeList(int pageIndex)

        /// <summary>
        /// 게시판 리스트 구하기
        /// </summary>
        /// <param name="pageIndex">페이지 인덱스</param>
        List<NoticeModel> GetNoticeList(int pageIndex);

        #endregion
        #region 게시판 리스트 구하기 - GetNoticeList(pageIndex, searchField, searchQuery)

        /// <summary>
        /// 게시판 리스트 구하기
        /// </summary>
        /// <param name="pageIndex">페이지 인덱스</param>
        /// <param name="searchField">검색 필드</param>
        /// <param name="searchQuery">검색 쿼리</param>
        /// <returns>게시판 리스트</returns>
        List<NoticeModel> GetNoticeList(int pageIndex, string searchField, string searchQuery);

        #endregion
        #region 게시판 보기 - GetNotice(id)

        /// <summary>
        /// 게시판 보기
        /// </summary>
        NoticeModel GetNotice(int id);

        #endregion
        #region 파일명 구하기 - GetFileName(id)

        /// <summary>
        /// 파일명 구하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>파일명</returns>
        string GetFileName(int id);

        #endregion
        #region 다운로드 카운트 수정하기 - UpdateDownloadCount(id)

        /// <summary>
        /// 다운로드 카운트 수정하기
        /// </summary>
        /// <param name="id">ID</param>
        void UpdateDownloadCount(int id);

        #endregion
        #region 다운로드 카운트 수정하기 - UpdateDownloadCount(fileName)

        /// <summary>
        /// 다운로드 카운트 수정하기
        /// </summary>
        /// <param name="fileName">파일명</param>
        void UpdateDownloadCount(string fileName);

        #endregion

        #region 사진이 있는 최근 게시판 리스트 구하기 - GetRecentPhotoNoticeList()

        /// <summary>
        /// 사진이 있는 최근 게시판 리스트 구하기
        /// </summary>
        /// <returns>사진이 있는 최근 게시판 리스트</returns>
        List<NoticeModel> GetRecentPhotoNoticeList();

        #endregion
        #region 특정 카테고리 최근 게시판 리스트 구하기 - GetRecentCategoryNoticeList(category)

        /// <summary>
        /// 특정 카테고리 최근 게시판 리스트 구하기
        /// </summary>
        /// <returns>특정 카테고리 최근 게시판 리스트</returns>
        List<NoticeModel> GetRecentCategoryNoticeList(string category);

        #endregion
        #region 최근 게시판 리스트 구하기 - GetRecentNoticeList()

        /// <summary>
        /// 최근 게시판 리스트 구하기
        /// </summary>
        /// <returns>최근 게시판 리스트</returns>
        List<NoticeModel> GetRecentNoticeList();

        #endregion
        #region 최근 게시판 리스트 구하기 - GetRecentNoticeList(noticeCount)

        /// <summary>
        /// 최근 게시판 리스트 구하기
        /// </summary>
        /// <param name="noticeCount">게시판 수</param>
        /// <returns>최근 게시판 리스트</returns>
        List<NoticeModel> GetRecentNoticeList(int noticeCount);

        #endregion
        #region 게시판 고정하기 - PinNotice(id)

        /// <summary>
        /// 게시판 고정하기
        /// </summary>
        /// <param name="id">ID</param>
        /// <remarks>특정 게시글을 공지사항으로 올리기</remarks>
        void PinNotice(int id);

        #endregion
    }
}