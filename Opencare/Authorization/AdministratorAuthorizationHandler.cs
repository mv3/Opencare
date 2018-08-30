using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Opencare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Opencare.Authorization
{
    public class AdministratorAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, Student>
    {
        protected override Task HandleRequirementAsync(
                                             AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                    Student resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.AdministratorsRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
