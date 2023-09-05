namespace WebsiteLJH.Models
{
    /// <summary>
    /// ���� �� ��
    /// </summary>
    public class ErrorViewModel
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region ��û ID - RequestID

        /// <summary>
        /// ��û ID
        /// </summary>
        public string RequestID { get; set; }

        #endregion
        #region ��û ID ǥ�� ���� - ShowRequestID

        /// <summary>
        /// ��û ID ǥ�� ����
        /// </summary>
        public bool ShowRequestID => !string.IsNullOrEmpty(RequestID);

        #endregion
    }
}