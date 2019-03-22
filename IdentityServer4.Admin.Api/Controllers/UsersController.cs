using IdentityServer4.Admin.Api.Mappers;
using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Api.Policy;
using IdentityServer4.Admin.Logic.Constants;
using IdentityServer4.Admin.Logic.Entities;
using IdentityServer4.Admin.Logic.Entities.Services;
using IdentityServer4.Admin.Logic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Admin.Api.Controllers
{
    [Authorize("user")]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IGrantService grantService;
        private readonly ILogger<UsersController> logger;
        private readonly IPolicyService policyService;
        private readonly IHttpContextAccessor httpContext;

        public UsersController(IUserService userService, IGrantService grantService, 
            ILogger<UsersController> logger, IPolicyService policyService, 
            IHttpContextAccessor httpContext)
        {
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));
            if (grantService == null)
                throw new ArgumentNullException(nameof(grantService));
            this.userService = userService;
            this.grantService = grantService;
            this.logger = logger;
            IPolicyService policyService1 = policyService;
            if (policyService1 == null)
                throw new ArgumentNullException(nameof(policyService));
            this.policyService = policyService1;
            this.httpContext = httpContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto user)
        {
            if (user == null)
            {
                logger.LogError(4004, MessageConstants.UserIsNullCannotCreate, Array.Empty<object>());

                return BadRequest(MessageConstants.UserCannotBeNull);
            }
            logger.LogInformation(1003, MessageConstants.CreatingUser ?? "", Array.Empty<object>());
            User serviceUser = user.ToService();
            IdentityResult identityResult = await this.userService.Create(serviceUser, user.Password);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                if (result.Errors.Any(x => x.Code == "420"))
                    return new ObjectResult(result.Errors.First(error => error.Code == "420")?.Description) { StatusCode = new int?(420) };
                logger.LogError(4005, MessageConstants.CreatingUser + " " + MessageConstants.FailedBadRequest, Array.Empty<object>());

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            PagedResult<User> pagedResult = await userService.Get(null, new UserQueryBy
            {
                Username = user.Username
            });

            logger.LogInformation(1003, MessageConstants.SuccessfullyCreatedUser ?? "", new object[1]
            {
         user
            });

            return this.CreatedAtAction("GetBySubject", "Users", (object)new
            {
                subject = pagedResult.Results.First().Subject
            }, (object)MessageConstants.UserSuccessfullyCreated);
        }

        [HttpDelete("{subject}")]
        public async Task<IActionResult> Delete([FromRoute] string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                logger.LogError(4008, MessageConstants.SubjectIsNullCannotDelete, Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectCannotBeNull);
            }
            User user1 = await userService.GetBySubject(subject);
            User user2 = user1;
            user1 = (User)null;
            if (user2 == null)
            {
                logger.LogError(4006, MessageConstants.UserIsNullCannotDelete, Array.Empty<object>());

                return NoContent();
            }
            logger.LogInformation(1005, MessageConstants.DeletingUserSubject, new object[1]
            {
         subject
            });
            IdentityResult identityResult = await userService.Delete(user2);
            IdentityResult result = identityResult;
            identityResult = null;
            if (result.Succeeded)
            {
                logger.LogInformation(1005, MessageConstants.DeletedUserSubject, new object[1]
                {
           subject
                });

                return NoContent();
            }
            logger.LogInformation(40010, MessageConstants.DeletingUserFailedBadRequest, new object[1]
            {
         subject
            });

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpGet("{subject}")]
        public async Task<IActionResult> GetBySubject([FromRoute] string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                logger.LogError(4001, MessageConstants.SubjectIsNullCannotGet, Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectCannotBeNull);
            }
            logger.LogInformation(1002, MessageConstants.FindingUserBySubjectAndVariable, new object[1]
            {
         subject
            });
            User user1 = await userService.GetBySubject(subject);
            User user2 = user1;
            user1 = (User)null;
            if (user2 != null)
            {
                logger.LogInformation(1002, MessageConstants.RetrievedItemBySubject, new object[1]
                {
           subject
                });

                return Ok((object)user2.ToDto());
            }
            logger.LogError(4002, MessageConstants.UserIsNullCannotGetBySubject, new object[1]
            {
         subject
            });

            return NotFound();
        }

        [HttpGet("lightweight")]
        public async Task<IActionResult> GetLightWeight([FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string state = null, [FromQuery] string q = null)
        {
            try
            {
                int pageToGet = page ?? 0;
                int pageSizeToGet = pageSize ?? 25;
                PagedResult<User> result = await userService.FindFast(new Pagination(pageToGet, pageSizeToGet), q, new UserState(state));

                return new OkObjectResult((object)new PagedResult<UserLightWeightDto>
                {
                    PageCount = result.PageCount,
                    TotalCount = result.TotalCount,
                    CurrentPage = result.CurrentPage,
                    PageSize = result.PageSize,
                    Results = (from x in result.Results
                               select x.ToLightWeightDto()).ToList(),
                    IsSorted = result.IsSorted
                });
            }
            catch (Exception x2)
            {
                LoggerExtensions.LogError(logger, 1001, "Unhandled exception in GetLightweight {0}", new object[1]
                {
                    x2
                });
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string sort = null, [FromQuery] string username = null, [FromQuery] string email = null, [FromQuery] string name = null, [FromQuery] string id = null, [FromQuery] string state = null, [FromQuery] string q = null)
        {
            PagedResult<User> pagedResult = await GetPagedUsers(page, pageSize, sort, username, email, name, id, state, q, true);
            PagedResult<User> result = pagedResult;
            pagedResult = null;

            return new OkObjectResult(new PagedResult<UserDto>() { PageCount = result.PageCount, TotalCount = result.TotalCount, CurrentPage = result.CurrentPage, PageSize = result.PageSize, Results = result.Results.Select((Func<User, UserDto>)(x => x.ToDto())).ToList(), IsSorted = result.IsSorted });
        }

        [HttpPut("{subject}")]
        public async Task<IActionResult> Update([FromRoute] string subject, [FromBody] UserDto user)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                logger.LogError(4012, MessageConstants.CannotUpdateUserSubjectIsNull ?? "", Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectCannotBeNull);
            }
            if (user == null)
            {
                logger.LogError(4011, MessageConstants.CannotUpdateUserIsNull, new object[1]
                {
           subject
                });

                return BadRequest(MessageConstants.UserCannotBeNull);
            }
            User user1 = await FindUser(subject);
            User foundUser = user1;
            user1 = (User)null;
            if (foundUser == null)
            {
                logger.LogError(4011, MessageConstants.CannotUpdateUserIsNull, new object[1]
                {
           subject
                });

                return NotFound();
            }
            ICollection<ClaimDto> changedClaims = new List<ClaimDto>();
            IList<ClaimDto> foundUserDtoClaims = foundUser.ToDto().Claims;
            IList<ClaimDto> updatedUserClaims = user.Claims ?? new List<ClaimDto>();
            foreach (ClaimDto claimDto in foundUserDtoClaims)
            {
                ClaimDto foundClientClaim = claimDto;
                if (updatedUserClaims.FirstOrDefault(x =>
                {
                    if (x.Type == foundClientClaim.Type)
                        return x.Value == foundClientClaim.Value;
                    return false;
                }) == null)
                    changedClaims.Add(foundClientClaim);
            }
            foreach (ClaimDto claimDto in updatedUserClaims)
            {
                ClaimDto updatedClientClaim = claimDto;
                if (foundUserDtoClaims.FirstOrDefault(x =>
                {
                    if (x.Type == updatedClientClaim.Type)
                        return x.Value == updatedClientClaim.Value;
                    return false;
                }) == null)
                    changedClaims.Add(updatedClientClaim);
            }
            foreach (ClaimDto claimToModify in changedClaims)
            {
                bool flag = await policyService.CanUserModifyClaim(httpContext.HttpContext?.User?.Claims, claimToModify);
                bool canModifyClaims = flag;
                if (!canModifyClaims)
                {

                    return Unauthorized();
                }
            }
            logger.LogInformation(1004, MessageConstants.UpdatingUserBySubjectAndVariable, new object[1]
            {
         subject
            });
            IdentityResult identityResult = await userService.Update(user.ToService());
            IdentityResult result = identityResult;
            identityResult = null;
            if (result.Succeeded)
            {
                logger.LogInformation(1004, MessageConstants.SuccessfullyUpdatedUserBySubject, new object[1]
                {
           subject
                });

                return NoContent();
            }
            logger.LogError(4013, MessageConstants.UpdatingUserForSubjectFailedBadRequest, new object[1]
            {
         subject
            });

            return BadRequest(result.Errors.FirstOrDefault()?.Description);
        }

        [HttpPost("{subject}/roles")]
        public async Task<IActionResult> AddRoles([FromRoute] string subject, [FromBody] IList<string> roles)
        {
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogError(4015, MessageConstants.CannotCreateRoleUserIsNull, new object[1]
                {
           subject
                });

                return NotFound(MessageConstants.CannotFindUserWithSubject + "'" + subject + "'");
            }
            if (roles == null || !roles.Any())
            {
                logger.LogError(4014, MessageConstants.NoRolesFoundRolesRequired, Array.Empty<object>());

                return BadRequest("A list of roles is required");
            }
            foreach (string role in roles)
            {
                bool flag = await policyService.CanUserModifyRole(httpContext.HttpContext?.User?.Claims, role);
                bool canModifyRoles = flag;
                if (!canModifyRoles)
                {

                    return Unauthorized();
                }
            }
            logger.LogInformation(1003, MessageConstants.CreatingRoles, new object[1]
            {
         roles
            });
            IdentityResult identityResult = await userService.AddRoles(foundUser, roles);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4005, MessageConstants.CreatingRolesFailedBadRequest, new object[1]
                {
           roles
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1003, MessageConstants.SuccessfullyCreatedRoles, new object[1]
            {
         roles
            });

            return Ok();
        }

        [HttpDelete("{subject}/roles")]
        public async Task<IActionResult> DeleteRoles([FromRoute] string subject, [FromBody] IList<string> roles)
        {
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogError(4006, MessageConstants.CannotDeleteRoleUserIsNull, new object[1]
                {
           subject
                });

                return NotFound(MessageConstants.CannotFindUserWithSubject + "'" + subject + "'");
            }
            if (roles == null || !roles.Any())
            {
                logger.LogError(4007, MessageConstants.NoRolesFoundRolesRequired, Array.Empty<object>());

                return BadRequest("A list of roles is required");
            }
            foreach (string role in roles)
            {
                bool flag = await policyService.CanUserModifyRole(httpContext.HttpContext?.User?.Claims, role);
                bool canModifyRoles = flag;
                if (!canModifyRoles)
                {

                    return Unauthorized();
                }
            }
            logger.LogInformation(1005, MessageConstants.DeletingRoles, new object[1]
            {
         roles
            });
            IdentityResult identityResult = await userService.DeleteRoles(foundUser, roles);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(40010, MessageConstants.DeletingRolesFailedBadRequest, new object[1]
                {
           roles
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1005, MessageConstants.SuccessfullyDeletedRoles, new object[1]
            {
         roles
            });

            return Ok();
        }

        [HttpPost("{subject}/claims")]
        public async Task<IActionResult> AddClaim([FromRoute] string subject, [FromBody] ClaimDto claim)
        {
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogError(4015, MessageConstants.CannotCreateClaimUserIsNull, new object[1]
                {
           subject
                });

                return NotFound(MessageConstants.CannotFindUserWithSubject + "'" + subject + "'");
            }
            if (claim == null)
            {
                logger.LogError(4016, MessageConstants.CannotCreateClaimIsNull, Array.Empty<object>());

                return BadRequest("A Claim is required");
            }
            bool flag = await policyService.CanUserModifyClaim(httpContext.HttpContext?.User?.Claims, claim);
            bool canModifyClaims = flag;
            if (!canModifyClaims)
            {

                return Unauthorized();
            }
            logger.LogInformation(1003, MessageConstants.AddingClaim, new object[1]
            {
        (object) claim
            });
            IdentityResult identityResult = await userService.AddClaim(foundUser, claim.ToService());
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4005, MessageConstants.AddingClaimFailedBadRequest, new object[1]
                {
          (object) claim
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1003, MessageConstants.SuccessfullyAddedClaim, new object[1]
            {
        (object) claim
            });

            return Ok();
        }

        [HttpDelete("{subject}/claims")]
        public async Task<IActionResult> DeleteClaim([FromRoute] string subject, [FromBody] ClaimDto claim)
        {
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogError(4006, MessageConstants.CannotDeleteClaimUserIsNull, new object[1]
                {
           subject
                });

                return NotFound(MessageConstants.CannotFindUserWithSubject + "'" + subject + "'");
            }
            if (claim == null)
            {
                logger.LogError(4009, MessageConstants.CannotDeleteClaimIsNull, Array.Empty<object>());

                return BadRequest("A list of roles is required");
            }
            bool flag = await policyService.CanUserModifyClaim(httpContext.HttpContext?.User?.Claims, claim);
            bool canModifyClaims = flag;
            if (!canModifyClaims)
            {

                return Unauthorized();
            }
            logger.LogInformation(1005, MessageConstants.DeletingClaim, new object[1]
            {
        (object) claim
            });
            IdentityResult identityResult = await userService.DeleteClaim(foundUser, claim.ToService());
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(40010, MessageConstants.DeletingClaimFailedBadRequest, new object[1]
                {
          (object) claim
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1005, MessageConstants.SuccessfullyDeletedClaim, new object[1]
            {
        (object) claim
            });

            return Ok();
        }

        [HttpPut("{subject}/claims")]
        public async Task<IActionResult> EditClaim([FromRoute] string subject, [FromBody] EditClaimDto dto)
        {
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {

                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(dto.OldClaimType))
            {
                logger.LogError(4017, MessageConstants.CannotDeleteOldClaimTypeIsNull, Array.Empty<object>());

                return BadRequest("Old claim type cannot be null");
            }
            if (string.IsNullOrWhiteSpace(dto.OldClaimValue))
            {
                logger.LogError(4018, MessageConstants.CannotDeleteOldClaimValueIsNull, Array.Empty<object>());

                return BadRequest("Old claim value cannot be null");
            }
            if (string.IsNullOrWhiteSpace(dto.NewClaimType))
            {
                logger.LogError(4019, MessageConstants.CannotDeleteNewClaimTypeIsNull, Array.Empty<object>());

                return BadRequest("New claim type cannot be null");
            }
            if (string.IsNullOrWhiteSpace(dto.NewClaimValue))
            {
                logger.LogError(4020, MessageConstants.CannotDeleteNewClaimValueIsNull, Array.Empty<object>());

                return BadRequest("New claim value cannot be null");
            }
            bool flag1 = await policyService.CanUserModifyClaim(httpContext.HttpContext?.User?.Claims, new ClaimDto(dto.OldClaimType, dto.OldClaimValue));
            bool canModifyOldClaims = flag1;
            if (!canModifyOldClaims)
            {

                return Unauthorized();
            }
            bool flag2 = await policyService.CanUserModifyClaim(httpContext.HttpContext?.User?.Claims, new ClaimDto(dto.NewClaimType, dto.NewClaimValue));
            bool canModifyNewClaims = flag2;
            if (!canModifyNewClaims)
            {

                return Unauthorized();
            }
            logger.LogInformation(1004, MessageConstants.UpdatingClaim, Array.Empty<object>());
            IdentityResult identityResult = await userService.EditClaim(foundUser, new Claim(dto.OldClaimType, dto.OldClaimValue), new Claim(dto.NewClaimType, dto.NewClaimValue));
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4013, MessageConstants.UpdatingClaimFailedBadRequest, new object[1]
                {
           dto
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1004, MessageConstants.SuccessfullyUpdatedClaim, new object[1]
            {
         dto
            });

            return Ok();
        }

        [HttpGet("{subject}/apps")]
        public async Task<IActionResult> GetApps([FromRoute] string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                logger.LogError(4001, MessageConstants.CannotGetAppsSubjectIsNull, Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectCannotBeNull);
            }
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {

                return NotFound();
            }
            logger.LogInformation(1002, MessageConstants.GettingApps, Array.Empty<object>());
            IList<Consent> consentList = await grantService.GetConsentForSubject(subject);
            IList<Consent> consents = consentList;
            consentList = null;
            logger.LogInformation(1002, MessageConstants.SuccessfullyRetrievedAppsForSubject, new object[1]
            {
         subject
            });

            return Ok(consents.Select(x => x.ToDto()).ToList());
        }

        [HttpDelete("{subject}/apps")]
        public async Task<IActionResult> RevokeUser([FromRoute] string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {

                return CannotUpdateItemSubjectIsNull(subject);
            }
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {

                return NotFound();
            }
            logger.LogInformation(1004, MessageConstants.RevokingUserBySubject, new object[1]
            {
         subject
            });
            IdentityResult identityResult = await grantService.RevokeAll(subject);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4013, MessageConstants.RevokingUserBySubjectFailedBadRequest, new object[1]
                {
           subject
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1004, MessageConstants.SuccessfullyRevokedUserForSubject, new object[1]
            {
         subject
            });

            return NoContent();
        }

        [HttpDelete("{subject}/apps/{clientId}")]
        public async Task<IActionResult> RevokeUserClient([FromRoute] string subject, [FromRoute] string clientId)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {

                return CannotUpdateItemSubjectIsNull(subject);
            }
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {

                return NotFound();
            }
            logger.LogInformation(1004, MessageConstants.RevokingUserClientBySubject, new object[1]
            {
         subject
            });
            IdentityResult identityResult = await grantService.RevokeClient(subject, clientId);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4013, MessageConstants.RevokingUserClientBySubjectFailedBadRequest, new object[2]
                {
           subject,
           clientId
                });

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1004, MessageConstants.SuccessfullyRevokedUserClientForSubject, new object[1]
            {
         subject
            });

            return NoContent();
        }

        [HttpGet("{subject}/password/reset")]
        public async Task<IActionResult> ResetPassword([FromRoute] string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                logger.LogError(1000, MessageConstants.SubjectIsNull, Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectIsNull);
            }
            logger.LogInformation("Finding User", Array.Empty<object>());
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogInformation("User not found", Array.Empty<object>());

                return NotFound();
            }
            logger.LogInformation("Generating password reset token", Array.Empty<object>());
            IdentityResult identityResult = await userService.ResetPassword(foundUser);
            IdentityResult result = identityResult;
            identityResult = null;
            if (!result.Succeeded)
            {
                logger.LogError(4021, "Generating password reset token failed", Array.Empty<object>());

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            logger.LogInformation(1000, "Generate password reset token successful", new object[1]
            {
         subject
            });

            return NoContent();
        }

        [HttpGet("{subject}/logins")]
        public async Task<IActionResult> GetUserLogins([FromRoute] string subject)
        {
            if (string.IsNullOrEmpty(subject))
            {
                logger.LogError(MessageConstants.SubjectIsNull, Array.Empty<object>());

                return BadRequest(MessageConstants.SubjectIsNull);
            }
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogInformation("User not found", Array.Empty<object>());

                return NotFound();
            }
            IList<UserLoginInfo> userLoginInfoList = await userService.GetUserLogins(subject);
            IList<UserLoginInfo> userLogins = userLoginInfoList;
            userLoginInfoList = null;

            return Ok(userLogins);
        }

        [HttpDelete("{subject}/logins")]
        public async Task<IActionResult> DeleteUserLogin([FromRoute] string subject, [FromBody] UserLoginDto userLogin)
        {
            if (string.IsNullOrEmpty(subject))
            {

                logger.LogError(MessageConstants.SubjectIsNull, Array.Empty<object>());
                return BadRequest(MessageConstants.SubjectIsNull);
            }
            User user = await FindUser(subject);
            User foundUser = user;
            user = (User)null;
            if (foundUser == null)
            {
                logger.LogInformation("User not found", Array.Empty<object>());

                return NotFound();
            }
            if (userLogin == null)
            {

                return BadRequest(MessageConstants.SubjectIsNull);
            }
            IdentityResult identityResult = await userService.DeleteUserLogin(subject, userLogin.LoginProvider, userLogin.ProviderKey);

            return NoContent();
        }

        private IActionResult CannotUpdateItemSubjectIsNull(string subject)
        {
            logger.LogError(4012, MessageConstants.CannotRevokeUserSubjectIsNull, new object[1]
            {
         subject
            });
            return BadRequest(MessageConstants.SubjectCannotBeNull);
        }

        private async Task<User> FindUser(string subject)
        {
            logger.LogInformation(1002, MessageConstants.FindingUserBySubjectAndVariable, new object[1]
            {
         subject
            });
            User bySubject = await userService.GetBySubject(subject);
            return bySubject;
        }

        private async Task<PagedResult<User>> GetPagedUsers(int? page, int? pageSize, string sort, string username, string email, string name, string id, string state, string q, bool includeRelationships = true)
        {
            Pagination pagination = null;

            if (page.HasValue)
                pagination = new Pagination(page.Value, pageSize ?? 25);

            UserQueryBy query = new UserQueryBy() { FullSearch = q, Username = username, Email = email, Name = name, Id = id };
            List<UserOrderBy> userOrderBy = new List<UserOrderBy>();

            if (!string.IsNullOrWhiteSpace(sort))
                userOrderBy.AddRange(sort.Split(',', StringSplitOptions.None).Select(x => x.ToUserOrder()));

            UserState userState = new UserState(state);

            logger.LogInformation(1002, "Getting users", Array.Empty<object>());

            PagedResult<User> pagedResult = await userService.Get(pagination, query, userOrderBy, userState, includeRelationships);
            PagedResult<User> result = pagedResult;
            pagedResult = null;
            logger.LogInformation(1002, MessageConstants.SuccessfullyRetrievedUsers, Array.Empty<object>());

            return result;
        }
    }
}
