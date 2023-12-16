using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;
using WorkPath.Server.Extensions;
using WorkPath.Server.Models;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server.Services;

public class AuthService
{
    private UserService UserService;
    private IMapper Mapper;

    public AuthService(UserService userService, IMapper mapper)
    {
        UserService = userService;
        Mapper = mapper;
    }

    public async Task<ServiceResponse<AuthHelperModel<UserViewModel>>> CreateUser(UserEditModel model)
    {
        var res = new ServiceResponse<AuthHelperModel<UserViewModel>>();
        res.Name = "Проверьте введённые данные: номер телефона и/или пароль.";
        var checkUnique = UserService.GetByLogin(model.Login);
        if (checkUnique != null)
        {
            res.Name = "Проверьте оригинальность логина!";
            return res;
        }

        var user = await UserService.Create(model);
        if (user == null)
        {
            return res;
        }
        res = await Authenticate(new() { Login = model.Login, Password = model.Password });
        
        return res;
    }
    
    public async Task<ServiceResponse<AuthHelperModel<UserViewModel>>> Authenticate(UserAuthModel model)
    {
        var res = new ServiceResponse<AuthHelperModel<UserViewModel>>();
        res.Name = "Проверьте введённые данные: номер телефона и/или пароль.";

        var user = UserService.GetByLogin(model.Login);
        if (user == null)
        {
            return res;
        }

        if (user.PasswordHash != model.Password.ToHashSHA256())
        {
            return res;
        }

        res.Data = new AuthHelperModel<UserViewModel>
        {
            Entity = "User",
            EntityJSON = Mapper.Map<UserViewModel>(user),
            EntityHash = Mapper.Map<UserViewModel>(user).ToHashSHA256(),
            EntityID = user.ID.ToString(),
            PlatformID = null,
            Token = GetTokenByUser(user)
        };
        res.Status = true;
        res.Name = "Success!";
        
        return res;
    }


    #region ASPNET_IDENTIFY

    public const string ISSUER = "FraudCheck.Auth";
    public const string AUDIENCE = "FraudCheck.Service";

    /// <summary>
    /// Ключ шифрования.
    /// </summary>
    private static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTH_KEY") ?? "S9uj02iom2908hS927GUBINOJP@S()U*Y(HBIUHJNOMIK"));

    /// <summary>
    /// Get Bearer token by User entity.
    /// </summary>
    /// <param name="user">User entity.</param>
    /// <returns>Bearer token for User entity.</returns>
    static string GetTokenByUser(UserEntity user)
    {
        var time = TimeSpan.FromDays(365);

        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: ISSUER,
            audience: AUDIENCE,
            claims: GetIdentityByUser(user).Claims,
            expires: DateTime.UtcNow.Add(time),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    /// <summary>
    /// Generate Claims by User entity.
    /// </summary>
    /// <param name="user">User entity.</param>
    /// <returns>Claims.</returns>
    static ClaimsIdentity GetIdentityByUser(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new("ID", user.ID.ToString()),
            new("Entity", "User"),
            new(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new(ClaimsIdentity.DefaultNameClaimType, user.ID.ToString()),
        };

        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", "ID",
                "User");
        return claimsIdentity;
    }

    /// <summary>
    /// Builder paramethers.
    /// </summary>
    public static TokenValidationParameters BuilderParamethers => new TokenValidationParameters
    {
        // указывает, будет ли валидироваться издатель при валидации токена
        ValidateIssuer = true,
        // строка, представляющая издателя
        ValidIssuer = ISSUER,
        // будет ли валидироваться потребитель токена
        ValidateAudience = true,
        // установка потребителя токена
        ValidAudience = AUDIENCE,
        // будет ли валидироваться время существования
        ValidateLifetime = true,
        // установка ключа безопасности
        IssuerSigningKey = GetSymmetricSecurityKey(),
        // валидация ключа безопасности
        ValidateIssuerSigningKey = true,
    };

    #endregion
}