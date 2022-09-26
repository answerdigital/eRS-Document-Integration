using AutoMapper;
using eRS.Data;
using eRS.Data.Entities;
using eRS.Models.Dtos;
using eRS.Models.Models.Users;
using eRS.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eRS.Services.Services;

public class AccountService : IAccountService
{
    private readonly eRSContext context;
    private readonly IMapper mapper;

    public AccountService(eRSContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<UserDto?> CreateUser(UserCreate userCreate)
    {
        userCreate.UserPassword = EncryptPassword(userCreate.UserEmail, userCreate.UserPassword);
        var newUser = this.mapper.Map<User>(userCreate);

        await this.context.Users.AddAsync(newUser);
        await this.context.SaveChangesAsync();

        return this.mapper.Map<UserDto>(newUser);
    }

    public async Task<UserDto?> AuthenticateLogin(UserLogin userLogin)
    {
        var user = await this.context.Users.FirstOrDefaultAsync(o => o.UserEmail.ToLower().Equals(userLogin.Email.ToLower()));

        if (user is null)
        {
            return null;
        }

        var verifyResult = this.VerifyPassword(userLogin.Email, user.UserPassword, userLogin.Password);

        if (verifyResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.UserPassword = this.EncryptPassword(user.UserEmail, userLogin.Password);
            await this.context.SaveChangesAsync();
        }

        return this.mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUser(string email)
    {
        var user = await this.context.Users.FirstOrDefaultAsync(x => x.UserEmail == email);

        if (user is null)
        {
            return null;
        }

        return this.mapper.Map<UserDto>(user);
    }

    private string EncryptPassword(string email, string password)
    {
        var passwordHasher = new PasswordHasher<string>();
        return passwordHasher.HashPassword(email, password);
    }

    private PasswordVerificationResult VerifyPassword(string email, string hashedPassword, string providedPassword)
    {
        var passwordHasher = new PasswordHasher<string>();
        return passwordHasher.VerifyHashedPassword(email, hashedPassword, providedPassword);
    }
}
