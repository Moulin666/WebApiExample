using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Web.Endpoints
{
    public class AuthHandler : BaseAsyncEndpoint<AuthRequest, AuthResponse>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthHandler(SignInManager<AppUser> signInManager,
            ITokenClaimsService tokenClaimsService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
        }

        [HttpPost("api/authenticate")]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user",
            OperationId = "auth.authenticate",
            Tags = new[] { "AuthEndpoints" })]
        public override async Task<ActionResult<AuthResponse>> HandleAsync(AuthRequest request, CancellationToken cancellationToken = default)
        {
            var response = new AuthResponse(request.CorrelationId());

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

            response.Result = result.Succeeded;
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = request.Username;

            if (result.Succeeded)
                response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);

            return response;
        }
    }
}
