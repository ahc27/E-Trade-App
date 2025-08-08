using AutoMapper;
using classLib;
using classLib.LogDtos;
using UserMicroservice.Data;
using UserMicroservice.Data.Repositories;
using UserMicroservice.Infrastructures.Messaging;
using classLib;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;
        private readonly RabbitMqProducer _rabbitMqProducer;

        public UserService(UserRepository userRepository, IMapper mapper, RabbitMqProducer rabbitMqProducer)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _rabbitMqProducer = rabbitMqProducer;
        }

        public async Task<IEnumerable<GetUserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<GetUserDto>>(users);
        }
            

        public async Task<GetUserDto> GetByIdAsync(int id)
        { 

        return _mapper.Map<GetUserDto>(await _userRepository.GetById(id));
        }

        public async Task<AuthorizationDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            return _mapper.Map<AuthorizationDto>(user);
        }


        public async Task<User> AddAsync(CreateUserdto userDto)
        {
            userDto.password = BCrypt.Net.BCrypt.HashPassword(userDto.password);
            var userEntity = _mapper.Map<User>(userDto);
            var addedUser = await _userRepository.Add(userEntity);
            if (addedUser == null)
            {
                await LogUser(null, false, "Add User", "User could not be added", new Exception("Failed to add user"));
                throw new Exception("User could not be added.");
            }

            await LogUser(addedUser.Id.ToString(), true, "Add User", "User added successfully", null);
            return addedUser;
        }


        public async Task<bool> UpdateAsync(int id, UpdateUserdto userDto)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                return false;
            }
            userDto.password = BCrypt.Net.BCrypt.HashPassword(userDto.password);
            _mapper.Map(userDto, existingUser);
            
            var updatedUser = await _userRepository.Update(existingUser);

            if (updatedUser == null)
            {
                await LogUser(existingUser.Id.ToString(), false, "Update User", "User update failed", new Exception("Failed to update user"));
            }

            await LogUser(existingUser.Id.ToString(), true, "Update User", "User updated successfully", null);

            return true;
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshTokenDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);

            if (user == null) return false;

            user.refreshToken = dto.refreshToken;
            var isSuccess = await _userRepository.UpdateRefreshTokenAsync(user);
            
            if (!isSuccess)
            {
                await LogUser(user.Id.ToString(), false, "Update Refresh Token", "Refresh token update failed", new Exception("Failed to update refresh token"));
                return false;
            }
            await LogUser(user.Id.ToString(), true, "Update Refresh Token", "Refresh token updated successfully", null);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
        return await _userRepository.Delete(id);
        }

        public async Task<bool> LogUser(string? entityId, bool success, string action, string message, Exception? exception)
        {
            try
            {

                var userLog = new Log
                {
                    IsSuccess = success,
                    Action = action,
                    Message = message,
                    Timestamp = DateTime.UtcNow,
                    EntityId = entityId,
                    ServiceName = "AuthAPI",
                    Level = success ? "Information" : "Error",
                    Exception = exception
                };

                await _rabbitMqProducer.SendLogAsync(userLog);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
                return false;
            }
        }

    }
}
