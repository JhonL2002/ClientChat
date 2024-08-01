using System.ComponentModel.DataAnnotations;

namespace ClientChat.Models
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Nickname { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        //Calculated property
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }
}
