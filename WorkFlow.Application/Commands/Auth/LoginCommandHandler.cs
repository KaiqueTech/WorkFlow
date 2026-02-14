using Microsoft.AspNetCore.Identity;
using WorkFlow.Application.Interfaces;
using WorkFlow.Domain.Models;

namespace WorkFlow.Application.Commands.Auth
{
    public class LoginCommandHandler
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(UserManager<ApplicationUserModel> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseCommand> HandleAsync(LoginRequestCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                 return new LoginResponseCommand(false, null, "E-mail ou senha inválidos.");

            var token = await _jwtService.GenerateToken(user);

            return new LoginResponseCommand(true, "Login realizado com sucesso.",token);
        }
    }
}
