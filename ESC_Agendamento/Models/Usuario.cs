namespace ESC_Agendamento.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string NomeUsuario { get; set; }
        public byte[] SenhaHash{ get; set; }
        public byte[] SenhaSalt{ get; set; }
        public int Role { get; set; }

        //public enum RoleEnum
        //{
        //    Admin = 0,
        //    Profissional = 1,
        //    Cliente = 2
        //}
    }
}
