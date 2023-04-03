using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestMinApi.Data;
using TestMinApi.Dto;
using TestMinApi.Helpers;
using TestMinApi.Validation;

namespace TestMinApi.Commands
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly DbContextOptions<DemoContext> _options;
        private readonly IConfiguration _configuration;

        public LoginHandler(DbContextOptions<DemoContext> options, IConfiguration configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            using (var ctx = new DemoContext(_options))
            {
                var users = await ctx.Users.ToListAsync();
                if (users.Any(u => u.Email == request.Username && u.Password == request.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, request.Username == "admin.allen@gmail.com" 
                        ? "admin" : "default"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var token = CreateToken(authClaims);
                    return new LoginResponseDto
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    };
                }
            }
            throw new UnauthorizedAccessException();
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
