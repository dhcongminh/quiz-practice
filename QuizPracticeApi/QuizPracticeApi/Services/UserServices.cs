using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizPracticeApi.Helpers;
using QuizPracticeApi.Models;
using System.Data.SqlTypes;

namespace QuizPracticeApi.Services {
    public class UserServices {
        private readonly QuizPracticeContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserServices(QuizPracticeContext context, IMapper mapper, IConfiguration configuration) {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public dynamic Register(UserDto userDto) {
            Response response = new Response();
            UserDto? usernameCheck = GetByUsername(userDto.Username);
            if (usernameCheck != null) {
                response.Errors.Add("Username already exists in the system.");
            }
            UserDto? emailCheck = GetByEmail(userDto.Email);
            if (emailCheck != null) {
                response.Errors.Add("Email already exists in the system.");
            }
            if (response.Errors.Count == 0) {
                User user = _mapper.Map<User>(userDto);
                user.Id = 0;
                user.CreatedAt = DateTime.Now;
                user.Password = EncryptionHelper.Base64Encode(userDto.Password);
                if (string.IsNullOrEmpty(user.UserDetail.Avatar)) {
                    user.UserDetail.Avatar = string.Format("https://ui-avatars.com/api/?name={0}+{1}", userDto.FirstName, userDto.LastName);
                }
                user.Status = 0;
                try {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    response.User = _mapper.Map<UserDto>(user);
                    response.Token = EncryptionHelper.GenerateActivateToken(userDto.Username, _configuration);
                } catch (Exception ex) {
                    response.Errors.Add($"{ex}");
                }
            }
            return response;
        }

        public Response Login(string username, string password) {
            var response = new Response();
            User? user = _context.Users
                .Include(u => u.UserDetail)
                .FirstOrDefault(u => u.Username == username && u.Password == EncryptionHelper.Base64Encode(password));
            if (user == null) {
                response.Errors.Add("Username or password is incorrect.");
            } else {
                response.User = _mapper.Map<UserDto>(user);
                response.Token = GenerateToken(user);
            }
            return response;
        }
        public Response Activate(string token, string username) {
            var response = new Response();
            User? user = _context.Users
                .Include(u => u.UserDetail)
                .FirstOrDefault(u => u.Username == username);
            if (user == null) {
                response.Errors.Add("Username does not exists in the system.");
            } else {
                var isValid = EncryptionHelper.ValidateActivateToken(token, username, _configuration);
                if (isValid) {
                    user.Status = 1;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                } else {
                    response.Errors.Add("Token is invalid or expired.");
                }
            }
            return response;
        }

        public UserDto? GetByUsername(string username) {
            User? user = _context.Users
                .Include(x => x.UserDetail)
                .FirstOrDefault(x => x.Username == username);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public UserDto? GetByEmail(string email) {
            User? user = _context.Users
                .Include(x => x.UserDetail)
                .FirstOrDefault(x => x.UserDetail.Email == email);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public List<UserDto> GetAll() {
            List<User> users = _context.Users
                .Include(u => u.UserDetail)
                .ToList();
            return _mapper.Map<List<UserDto>>(users);
        }

        private string GenerateToken(User user) {
            return JwtTokenHelper.GenerateToken(user, _configuration);
        }

    }

    public class Response {
        public Response() {
        }
        public UserDto? User { get; set; }
        public string? Token { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
    public class UserDto {
        public UserDto() { }
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? Avatar { get; set; }
    }
}
