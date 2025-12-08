namespace SIGA_PET.DTOs
{
    /// <summary>
    /// DTO para retornar informações do usuário após login
    /// Compatível com tanto Tutor quanto Funcionário
    /// </summary>
    public class UserInfo
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public int? TutorId { get; set; }
        public int? FuncionarioId { get; set; }
    }
}
