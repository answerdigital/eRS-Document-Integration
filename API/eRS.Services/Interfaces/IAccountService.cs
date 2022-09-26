using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Users;

namespace eRS.Services.Interfaces;

public interface IAccountService
{
    public Task<UserDto?> CreateUser(UserCreate userCreate);
    public Task<UserDto?> AuthenticateLogin(UserLogin userLogin);
    public Task<UserDto?> GetUser(string email);
}
