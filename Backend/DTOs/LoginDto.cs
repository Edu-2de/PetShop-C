using System.ComponentModel.DataAnnotations;

namespace SIGA_PET.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserInfo Usuario { get; set; } = new UserInfo();
    }
}