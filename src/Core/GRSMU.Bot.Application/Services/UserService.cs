﻿using AutoMapper;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Documents;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Application.Services
{
    public class UserService : ITelegramUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<TelegramUser> CreateUserFromTelegramUpdateAsync(Update update)
        {
            var user = update.GetUser();

            var telegramId = user.Id.ToString();

            var userDocument = await _userRepository.GetByTelegramIdAsync(telegramId);

            if (userDocument == null)
            {
                userDocument = _mapper.Map<UserDocument>(user);

                userDocument.ChatId = update.GetChatId();

                await _userRepository.InsertAsync(userDocument);
            }

            var context = _mapper.Map<TelegramUser>(userDocument);

            return context;
        }

        public Task UpdateUserAsync(TelegramUser context)
        {
            var document = _mapper.Map<UserDocument>(context);

            return _userRepository.UpdateOneAsync(document);
        }

        public Task UpdateLastMessageBotIdAsync(TelegramUser context, int messageId)
        {
            context.LastBotMessageId = messageId;
            
            return UpdateUserAsync(context);
        }

        public Task DeleteLastMessageBotIdAsync(TelegramUser context)
        {
            context.LastBotMessageId = null;

            return UpdateUserAsync(context);
        }
    }
}
