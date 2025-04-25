namespace ESC_Agendamento.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] SenhaHash{ get; set; }
        public byte[] SenhaSalt{ get; set; }
        public string Role { get; set; } = "cliente"; // ou profissional/admin
    }
}
