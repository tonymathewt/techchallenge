using Microsoft.AspNetCore.Mvc;
using BankAccountManagement.Models;
using BankAccountManagement.Services.Interfaces;

namespace BankAccountManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("{userName}")]
    public async Task<User> GetByUserNameAsync(string userName)
    {
        return await _userService.GetUserAsync(userName);
    }

    [HttpGet("/all")]
    public async Task<IList<User>> GetAllAsync()
    {
        return await _userService.GetAllAsync();
    }
}
