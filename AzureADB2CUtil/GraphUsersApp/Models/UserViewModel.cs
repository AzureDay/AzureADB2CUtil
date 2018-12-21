using System.Linq;
using AADB2C.GraphService;

namespace GraphUsersApp.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserViewModel(GraphAccountModel account)
        {
            Id = account.Id;
            Email = account.OtherMails.FirstOrDefault();
            FirstName = account.GivenName;
            LastName = account.Surname;
        }
    }
}