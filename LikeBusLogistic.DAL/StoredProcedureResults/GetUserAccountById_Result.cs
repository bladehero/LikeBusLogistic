namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetUserAccountById_Result
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
