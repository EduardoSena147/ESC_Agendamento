using ESC_Agendamento.Data;
using ESC_Agendamento.DTOs;
using ESC_Agendamento.Interfaces;
using ESC_Agendamento.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ESC_Agendamento.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UsuarioResponseDto> RegistrarAsync(UsuarioRegisterDto dto)
        {
            if(await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
            {
                throw new Exception("Email já cadastrado.");
            }

            CreatePasswordHash(dto.Senha, out byte[] hash, out byte[] salt);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email.ToLower(),
                SenhaHash = hash,
                SenhaSalt = salt,
                Role = "cliente"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Token = GenerateJwtToken(usuario)
            };
        }

        public async Task<UsuarioResponseDto> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());

            if (usuario == null || !VerifyPasswordHash(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                throw new Exception("Credenciais inválidas.");

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                Token = GenerateJwtToken(usuario)
            };
        }

        // Helpers

        private void CreatePasswordHash(string senha, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        private bool VerifyPasswordHash(string senha, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return computedHash.SequenceEqual(hash);
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Role)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
