using AutoMapper;
using Globe.Identity.AdministrativeDashboard.Shared.DTOs;
using Globe.Identity.Models;
using Globe.Identity.Servicess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Globe.Identity.AdministrativeDashboard.Server.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        readonly IAsyncRegisterService _registerService;
        private readonly IMapper _mapper;

        public RegisterController(IAsyncRegisterService registerService, IMapper mapper)
        {
            _registerService = registerService;
            _mapper = mapper;
        }

        [HttpPost]
        async public Task<RegistrationResultDTO> Post([FromBody] RegistrationDTO registration)
        {
            if (!ModelState.IsValid)
            {
                return new RegistrationResultDTO
                {
                    Successful = false,
                    Errors = new string[] { "Invalid registration" }
                };
            }

            var mappedRegistration = _mapper.Map<Registration>(registration);
            var result = await _registerService.RegisterAsync(mappedRegistration);

            return _mapper.Map<RegistrationResultDTO>(result);
        }

        [HttpPut]
        [Authorize]
        async public Task<RegistrationResultDTO> Put([FromBody] RegistrationNewPasswordDTO registration)
        {
            if (!ModelState.IsValid)
            {
                return new RegistrationResultDTO
                {
                    Successful = false,
                    Errors = new string[] { "Invalid registration" }
                };
            }

            var mappedRegistration = _mapper.Map<RegistrationNewPassword>(registration);
            mappedRegistration.UserName = User.Identity.Name;
            var result = await _registerService.ChangePasswordAsync(mappedRegistration);

            return _mapper.Map<RegistrationResultDTO>(result);
        }
    }
}
