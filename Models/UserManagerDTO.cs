namespace UCITMS.Models
{
    #region Get user manager info
    public class GetUserManagerDTO
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public int? ManagerID { get; set; }
        public string? ManagerName { get; set; }
        public string? PrimaryManagerName { get; set; }
        public string? SecondaryManagerName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
    }
    #endregion

    #region Post user manager info
    public class PostUserManagerDTO
    {
        public int UserID { get; set; }
        public int? PrimaryManagerID { get; set; }
        public int? SecondaryManagerID { get; set; }
        public int? ModUserID { get; set; }
    }
    #endregion
}