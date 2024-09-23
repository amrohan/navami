using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;
using BC = BCrypt.Net.BCrypt;

namespace navami.Services
{
    public class UserService
    {
        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;

        public UserService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public ApiResponse<UserMaster> RegisterUser(UserMaster model)
        {
            try
            {
                // Check if the username already exists
                var existingUser = dbContext.UserMasters.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    return new ApiResponse<UserMaster>("Username already exists.");
                }

                // Check if the email already exists
                existingUser = dbContext.UserMasters.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<UserMaster>("Email already exists.");
                }

                // Hash the password using BCrypt
                model.Password = BC.HashPassword(model.Password);

                // Add the user to the database
                dbContext.UserMasters.Add(model);
                dbContext.SaveChanges();

                return new ApiResponse<UserMaster>(model); // Return the registered user
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserMaster>(ex.Message); // Return any exception message
            }
        }

        /*        public ApiResponse<User> LoginUser(LoginModel model)
                {
                    try
                    {
                        // Find the user by email
                        var user = dbContext.Users.FirstOrDefault(u => u.Username == model.Username);
                        if (user == null)
                        {
                            return new ApiResponse<User>("Invalid email or password.");
                        }

                        // Verify the password
                        if (!BC.Verify(model.Password, user.Password))
                        {
                            return new ApiResponse<User>("Invalid email or password.");
                        }

                        return new ApiResponse<User>(user);
                    }
                    catch (Exception ex)
                    {
                        return new ApiResponse<User>(ex.Message); // Return any exception message
                    }
                }
        */
        public async Task<ApiResponse<UserMaster>> LoginUserAsync(LoginModel model)
        {
            try
            {
                // Find the user by username asynchronously
                var user = await dbContext.UserMasters
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null)
                {
                    return new ApiResponse<UserMaster>("Invalid username or password.");
                }

                // Verify the password
                if (!BC.Verify(model.Password, user.Password))
                {
                    return new ApiResponse<UserMaster>("Invalid username or password.");
                }

                return new ApiResponse<UserMaster>(user);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserMaster>(ex.Message); // Return any exception message
            }
        }
        public async Task<ApiResponse<List<Role>>> GetAllUserRolesAsync()
        {
            try
            {
                // Fetch the list of roles asynchronously
                var roles = await dbContext.Roles.ToListAsync();

                // Check if roles are found
                if (roles == null || roles.Count == 0)
                {
                    return new ApiResponse<List<Role>>("No roles found.");
                }
                var role = roles.Select(r => r.RoleName);

                // Return the list of roles
                return new ApiResponse<List<Role>>(roles);
            }
            catch (Exception ex)
            {
                // Log the exception here (if you have a logging mechanism)
                return new ApiResponse<List<Role>>($"An error occurred while fetching roles: {ex.Message}");
            }
        }


        public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                // Fetch the list of users asynchronously
                var users = await dbContext.UserMasters.ToListAsync();

                // Check if users are found
                if (users == null || users.Count == 0)
                {
                    return new ApiResponse<List<UserDto>>("No users found.");
                }

                // Return the list of users
                return new ApiResponse<List<UserDto>>(_mapper.Map<List<UserDto>>(users));
            }
            catch (Exception ex)
            {
                // Log the exception here (if you have a logging mechanism)
                return new ApiResponse<List<UserDto>>($"An error occurred while fetching users: {ex.Message}");
            }
        }

        //  update user
        public async Task<ApiResponse<UserMaster>> UpdateUserAsync(UserMaster model)
        {
            try
            {
                // Find the user by ID
                var user = await dbContext.UserMasters.FirstOrDefaultAsync(u => u.UserId == model.UserId);
                if (user == null)
                {
                    return new ApiResponse<UserMaster>("User not found.");
                }
                // check if password is hash or not
                if (!model.Password.StartsWith("$2a$"))
                {
                    // Hash the password using BCrypt
                    model.Password = BC.HashPassword(model.Password);
                }

                // validate username and email 
                var existingUser = dbContext.UserMasters.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null && existingUser.UserId != model.UserId)
                {
                    return new ApiResponse<UserMaster>("Username already exists.");
                }
                existingUser = dbContext.UserMasters.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null && existingUser.UserId != model.UserId)
                {
                    return new ApiResponse<UserMaster>("Email already exists.");
                }
                user.Username = model.Username;
                user.Password = model.Password;
                user.Email = model.Email;
                user.Role = model.Role;
                user.IsDeactivated = model.IsDeactivated;


                // Save the changes
                await dbContext.SaveChangesAsync();

                return new ApiResponse<UserMaster>(user);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserMaster>($"An error occurred while updating the user: {ex.Message}");
            }
        }
        public async Task<ApiResponse<UserMaster>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                // Find the user by ID
                var user = await dbContext.UserMasters.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    return new ApiResponse<UserMaster>("User not found.");
                }

                return new ApiResponse<UserMaster>(user);
            }
            catch (Exception ex)
            {
                // Log the exception here (if you have a logging mechanism)
                return new ApiResponse<UserMaster>($"An error occurred while fetching the user: {ex.Message}");
            }
        }

        // delete user 
        public async Task<ApiResponse<UserDto>> DeleteUserAsync(Guid userId)
        {
            try
            {
                // Find the user by ID
                var user = await dbContext.UserMasters.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null)
                {
                    return new ApiResponse<UserDto>("User not found.");
                }

                // Remove the user
                dbContext.UserMasters.Remove(user);
                await dbContext.SaveChangesAsync();
                var userDto = _mapper.Map<UserDto>(user);
                return new ApiResponse<UserDto>(userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDto>($"An error occurred while deleting the user: {ex.Message}");
            }
        }
    }
}