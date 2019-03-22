using IdentityServer4.Admin.Api.Models;
using IdentityServer4.Admin.Logic.Entities.Services;
using Microsoft.AspNetCore.Identity;
using NewLife.Reflection;
using System.Security.Claims;

namespace IdentityServer4.Admin.Api.Mappers
{
    public static class DtoMappers
    {
        private static IReflect Mapper;

        static DtoMappers()
        {
            Mapper = new DefaultReflect();
        }

        public static TTarget Map<TSrc, TTarget>(this IReflect mapper, TSrc src, TTarget target = null)
            where TTarget : class, new()
        {
            if (target == null)
            {
                target = new TTarget();
            }
            mapper.Copy(target, src);
            return target;
        }

        public static Role ToService(this CreateRoleDto dto)
        {
            return Mapper.Map<CreateRoleDto, Role>(dto);
        }

        public static Role ToService(this RoleDto dto)
        {
            return Mapper.Map<RoleDto, Role>(dto);
        }

        public static RoleDto ToDto(this Role role)
        {
            return Mapper.Map<Role, RoleDto>(role);
        }

        public static User ToService(this CreateUserDto dto)
        {
            return Mapper.Map<CreateUserDto, User>(dto);
        }

        public static User ToService(this UserDto dto)
        {
            return Mapper.Map<UserDto, User>(dto);
        }

        public static UserDto ToDto(this User user)
        {
            return Mapper.Map<User, UserDto>(user);
        }

        public static UserLightWeightDto ToLightWeightDto(this User user)
        {
            return Mapper.Map<User, UserLightWeightDto>(user);
        }

        public static Claim ToService(this ClaimDto dto)
        {
            var claim = new Claim(dto.Type, dto.Value);
            return claim;
        }

        public static ClaimDto ToDto(this Claim claim)
        {
            return Mapper.Map<Claim, ClaimDto>(claim);
        }

        public static ClaimType ToService(this CreateClaimTypeDto dto)
        {
            return Mapper.Map<CreateClaimTypeDto, ClaimType>(dto);
        }

        public static ClaimType ToService(this ClaimTypeDto dto)
        {
            return Mapper.Map<ClaimTypeDto, ClaimType>(dto);
        }

        public static ClaimTypeDto ToDto(this ClaimType claimType)
        {
            return Mapper.Map<ClaimType, ClaimTypeDto>(claimType);
        }

        public static ClientDto ToDto(this Client client)
        {
            return Mapper.Map<Client, ClientDto>(client);
        }

        public static Client ToService(this ClientDto dto)
        {
            return Mapper.Map<ClientDto, Client>(dto);
        }

        public static GenericClient ToService(this CreateClientDto client)
        {
            return Mapper.Map<CreateClientDto, GenericClient>(client);
        }

        public static ProtectedResourceDto ToDto(this ApiResource client)
        {
            return Mapper.Map<ApiResource, ProtectedResourceDto>(client);
        }

        public static ApiResource ToService(this ProtectedResourceDto dto)
        {
            return Mapper.Map<ProtectedResourceDto, ApiResource>(dto);
        }

        public static ApiResource ToService(this CreateProtectedResourceDto dto)
        {
            return Mapper.Map<CreateProtectedResourceDto, ApiResource>(dto);
        }

        public static IdentityResource ToService(this CreateIdentityResourceDto dto)
        {
            return Mapper.Map<CreateIdentityResourceDto, IdentityResource>(dto);
        }

        public static IdentityResourceDto ToDto(this IdentityResource dto)
        {
            return Mapper.Map<IdentityResource, IdentityResourceDto>(dto);
        }

        public static IdentityResource ToService(this IdentityResourceDto dto)
        {
            return Mapper.Map<IdentityResourceDto, IdentityResource>(dto);
        }

        public static SecretDto ToDto(this PlainTextSecret dto)
        {
            return Mapper.Map<PlainTextSecret, SecretDto>(dto);
        }

        public static PlainTextSecret ToService(this SecretDto dto)
        {
            return Mapper.Map<SecretDto, PlainTextSecret>(dto);
        }

        public static PlainTextSecret ToService(this CreateSecretDto dto)
        {
            return Mapper.Map<CreateSecretDto, PlainTextSecret>(dto);
        }

        public static ScopeDto ToDto(this Scope dto)
        {
            return Mapper.Map<Scope, ScopeDto>(dto);
        }

        public static Scope ToService(this ScopeDto dto)
        {
            return Mapper.Map<ScopeDto, Scope>(dto);
        }

        public static ConsentDto ToDto(this Consent dto)
        {
            return Mapper.Map<Consent, ConsentDto>(dto);
        }

        public static UserClaimDto ToDto(this UserClaim dto)
        {
            return Mapper.Map<UserClaim, UserClaimDto>(dto);
        }

        public static UserClaim ToService(this UserClaimDto dto)
        {
            return Mapper.Map<UserClaimDto, UserClaim>(dto);
        }

        public static UserLoginDto ToDto(this UserLoginInfo dto)
        {
            return Mapper.Map<UserLoginInfo, UserLoginDto>(dto);
        }
    }
}
