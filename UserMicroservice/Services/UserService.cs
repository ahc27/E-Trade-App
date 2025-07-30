using AutoMapper;
using UserMicroservice.Data;
using UserMicroservice.Data.Repositories;
using UserMicroservice.Services.Dtos;

namespace UserMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreateUserdto>> GetAllAsync()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<CreateUserdto>>(users);
        }
            

        public async Task<CreateUserdto> GetByIdAsync(int id)
        { 

        return _mapper.Map<CreateUserdto>(await _userRepository.GetById(id));
        }
            

        public async Task<User> AddAsync(CreateUserdto userDto)
        {
            userDto.password = BCrypt.Net.BCrypt.HashPassword(userDto.password);
            var userEntity = _mapper.Map<User>(userDto);
            var addedUser = await _userRepository.Add(userEntity);
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
            _mapper.Map(userDto, existingUser); // direkt olarak mevcut entity'ye map'le

            return await _userRepository.Update(existingUser);
        }


        public async Task<bool> DeleteAsync(int id)
        {
        return await _userRepository.Delete(id);
        }

 

    }
}
