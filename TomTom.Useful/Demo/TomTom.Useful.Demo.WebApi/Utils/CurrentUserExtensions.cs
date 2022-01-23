using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TomTom.Useful.Demo.Application;
using TomTom.Useful.Demo.Domain.Identities;

namespace TomTom.Useful.Demo.WebApi
{
    public static class CurrentUserExtensions
    {
        private static Guid CurrentUserId = Guid.NewGuid();
        public static DemoRequestContext ToDemoAppContext(this ControllerContext controllerContext)
        {
            return new DemoRequestContext(
                correlationId: controllerContext.HttpContext.TraceIdentifier,
                requestId: controllerContext.HttpContext.TraceIdentifier,
                currentUserId: CurrentUserId); // todo: extract from claims
        }
    }
}
