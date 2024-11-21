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

        public Response Register(UserDto userDto) {
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
                if (string.IsNullOrEmpty(user.UserDetail.Avatar)) {
                    user.UserDetail.Avatar = string.Format("https://ui-avatars.com/api/?name={0}+{1}", userDto.FirstName, userDto.LastName);
                }
                user.Status = 0;
                try {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    response.User = _mapper.Map<UserDto>(user);
                    response.Token = JwtTokenHelper.GenerateToken(_mapper.Map<User>(userDto), _configuration);
                } catch (Exception ex) {
                    response.Errors.Add($"{ex}");
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
