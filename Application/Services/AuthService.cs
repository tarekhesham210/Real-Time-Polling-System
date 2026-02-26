using Application.DTOs;
using Application.Interfaces.JWT;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<AuthResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<AuthResponseDTO>.Failure("Invalid email or pasword", HttpStatusCode.BadRequest);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return Result<AuthResponseDTO>.Failure("Invalid email or pasword", HttpStatusCode.BadRequest);

            var response = await GenerateAuthResponse(user);

            return Result<AuthResponseDTO>.Success(response);
        }

        public async Task<Result<AuthResponseDTO>> RegisterAsync(RegisterRequestDTO request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return Result<AuthResponseDTO>.Failure("Email is registered", HttpStatusCode.Conflict);
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName= request.Email,
                PasswordHash = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.First().Description;
                return Result<AuthResponseDTO>.Failure(error, HttpStatusCode.BadRequest);
            }
            var response = await GenerateAuthResponse(user);

            return Result<AuthResponseDTO>.Success(response);
        }

        public async Task<Result<bool>> RevokeTokenAsync(RefreshTokenRequest request)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken));

            if (user == null)
                return Result<bool>.Failure("Invalid Token", HttpStatusCode.BadRequest);

            var refreshToken = user.RefreshTokens.Single(x => x.Token == request.RefreshToken);

            if (refreshToken.RevokedOn != null)
                return Result<bool>.Failure("Token already expired ", HttpStatusCode.BadRequest);

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Result<bool>.Success();
        }
        public async Task<Result<AuthResponseDTO>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken));

            if (user == null)
                return Result<AuthResponseDTO>.Failure("Invalid Refresh Token", HttpStatusCode.BadRequest);

            var existingToken = user.RefreshTokens.Single(x => x.Token == request.RefreshToken);
            if (existingToken.ExpiresOn < DateTime.UtcNow ||existingToken.RevokedOn!=null )
                return Result<AuthResponseDTO>.Failure("Refresh Token Expired or Revoked", HttpStatusCode.BadRequest);

            existingToken.RevokedOn = DateTime.UtcNow;

            var response = await GenerateAuthResponse(user);

            return Result<AuthResponseDTO>.Success(response);
        }

        private async Task<AuthResponseDTO> GenerateAuthResponse(ApplicationUser user)
        {
            var (token, _) = _jwtProvider.GenerateToken(user);

            var refreshToken = _jwtProvider.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = DateTime.UtcNow.AddDays(7), 
                CreatedOn = DateTime.UtcNow,
                UserId = user.Id
            };

            user.RefreshTokens.Add(refreshTokenEntity);
            await _userManager.UpdateAsync(user);

            return new AuthResponseDTO(token, refreshToken, refreshTokenEntity.ExpiresOn);
        }
    }
}
