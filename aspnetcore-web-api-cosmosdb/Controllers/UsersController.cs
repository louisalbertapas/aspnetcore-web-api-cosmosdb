using aspnetcore_web_api_cosmosdb.Models;
using aspnetcore_web_api_cosmosdb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_web_api_cosmosdb.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync().ConfigureAwait(false);

            return users;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            var createdUser = await _userRepository.CreateUserAsync(user).ConfigureAwait(false);

            return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<User>> UpdateUser(string userId, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (existingUser == null)
            {
                return NotFound();
            }

            user.Id = userId;

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(userId);

            return NoContent();
        }
    }
}
