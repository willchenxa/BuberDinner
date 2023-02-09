using BuberDinner.Contracts.Authentication;

using Swashbuckle.AspNetCore.Filters;

namespace BuberDinner.Api.Swagger;

public class AuthenticationResponseExample : IExamplesProvider<AuthenticationResponse>
{
    public AuthenticationResponse GetExamples()
    {
        return new AuthenticationResponse(Guid.NewGuid(), "William", "Chen", "William.Chen@email.address", "token");
    }
}