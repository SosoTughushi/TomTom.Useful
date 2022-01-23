using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TomTom.Useful.Demo.Application;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.WebApi
{
    public static class CurrentUserExtensions
    {
        public static DemoRequestContext ToDemoAppContext(this ControllerContext controllerContext)
        {
            return new DemoRequestContext(
                correlationId: controllerContext.HttpContext.TraceIdentifier,
                requestId: controllerContext.HttpContext.TraceIdentifier,
                currentUserId: Guid.NewGuid()); // todo: extract from clains
        }
    }
}
