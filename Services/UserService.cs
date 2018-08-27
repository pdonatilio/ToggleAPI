using ToggleAPI.Models;
using ToggleAPI.Dtos;
using ToggleAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;


namespace ToggleAPI.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> Get();
        UserDto Get(int id);
        void Post(UserDto userDto);
        UserDto Post(string username, string password);
        void Put(int id, UserDto userDto);
        void Delete(int id);
    }   

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;

        private ToogleAPIContext _context;
       
        public UserService(
            ToogleAPIContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Get all and get by ID using expression Body
        public IEnumerable<UserDto> Get() => _mapper.Map<IList<UserDto>>(_context.Users);
        public UserDto Get(int id) => _mapper.Map<UserDto>(_context.Users.Find(id));

        //Creating a New User
        public void Post(UserDto userDto)
        {
            //Only Create a new user if they is not null, if they not already taken, and password is not null
            if (!string.IsNullOrWhiteSpace(userDto.Username) && 
                !_context.Users.Any(x => x.Username == userDto.Username) &&
                !string.IsNullOrWhiteSpace(userDto.Password))
            {
                // including the DTO itens to user model
                var user = _mapper.Map<User>(userDto);

                //creating the hash password
                byte[] passwordSalt, passwordHash;
                CreatePasswordHash(userDto.Password, out passwordSalt, out passwordHash);

                user.PasswordSalt = passwordSalt; 
                user.PasswordHash = passwordHash;

                _context.Users.Add(user);
                _context.SaveChanges();
            }                    
            else
                throw new Exception();
        }

        //User Authentication
        public UserDto Post(string username, string password)
        {
            UserDto userDto = new UserDto();           

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // getting the user by name
                User user = _context.Users.SingleOrDefault(u => u.Username == username);
                
                // checking password
                if (VerifyPassword( password, user.PasswordSalt, user.PasswordHash))
                {
                    userDto =  _mapper.Map<UserDto>(user);

                    // Getting the token
                    userDto.token = "123";
                }
                else
                    throw new Exception();
            }
            return userDto;
        }

        public void Put(int id, UserDto userDto)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;

                //Only Upddate the username if they is not null and if they not already taken
                if (!string.IsNullOrWhiteSpace(userDto.Username) && 
                    !_context.Users.Any(x => x.Username == userDto.Username))
                    user.Username = userDto.Username;
                else
                    throw new Exception();

                // Update Password if it was entered
                if (!string.IsNullOrWhiteSpace(userDto.Password))
                {
                    //creating the hash password
                    byte[] passwordSalt, passwordHash;
                    CreatePasswordHash(userDto.Password, out passwordSalt, out passwordHash);

                    user.PasswordSalt = passwordSalt; 
                    user.PasswordHash = passwordHash;
                }

                _context.Users.Update(user);
                _context.SaveChanges();
            }
            else
                throw new Exception();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception();
            }
        }

        // Thinking about migrate these itens below to helper's folder
        private static void CreatePasswordHash(string password,  out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPassword(string password,
                                            byte[] storedSalt,
                                            byte[] storedHash)
        {          
            if (password != null && storedSalt.Length == 128 && storedHash.Length == 64)
            {
                using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i]) return false;
                    }
                }
                return true;
            } else
                throw new Exception();
        }
    }
}