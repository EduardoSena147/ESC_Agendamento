namespace ESC_Agendamento.DTOs
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string NomeUsuario { get; set; }
        public string Token { get; set; }
    }
}
