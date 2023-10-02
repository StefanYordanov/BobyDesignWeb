namespace BobyDesignWeb.Models
{
    public class RoleItem
    {
        public string Role { get; set; } = string.Empty;

        public bool Active { get; set; }
    }

    public class UserRolesModel
    {
        public string UserId { get; set; } = string.Empty;

        public ICollection<RoleItem> Roles { get; set; } = new List<RoleItem>();
    }

    public class UserPageViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }

    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
