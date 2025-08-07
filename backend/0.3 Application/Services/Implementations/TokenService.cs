﻿using Domain.Entities;
using Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repository;
using Application.Schemas.Requests;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<bool> AddNotificationTokenAsync(NotificationTokenCreateDTO dto, int userId)
        {
            var existingToken = await _tokenRepository.GetAsQueryable()
                .FirstOrDefaultAsync(t => t.Token == dto.Token);

            if (existingToken != null)
            {
                existingToken.Platform = dto.Platform;
                existingToken.DeviceModel = dto.DeviceModel;
                existingToken.LastSeen = DateTime.UtcNow;
                existingToken.UserId = userId;

                return await _tokenRepository.UpdateAsync(existingToken);
            }

            var newToken = new NotificationToken
            {
                Token = dto.Token,
                Platform = dto.Platform,
                DeviceModel = dto.DeviceModel,
                UserId = userId,
                LastSeen = DateTime.UtcNow
            };

            var result = await _tokenRepository.AddAsync(newToken);

            if (result)
            {
                return true;
            }
            else
            {
                throw new Exception("Failed to add notification token.");
            }

        }
    }
}
