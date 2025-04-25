using ESC_Agendamento.DTOs;

namespace ESC_Agendamento.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioResponseDto> RegistrarAsync(UsuarioRegisterDto dto);
        Task<UsuarioResponseDto> LoginAsync(UsuarioLoginDto dto);
    }
}
