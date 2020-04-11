using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(12,MinimumLength=4,ErrorMessage="Your Password Should between 4 and 12 characters.")]
        public string Password { get; set; }
    }
}