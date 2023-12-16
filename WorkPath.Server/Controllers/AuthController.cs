using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPath.Server.EditModels;
using WorkPath.Server.Extensions;
using WorkPath.Server.Models;
using WorkPath.Server.Services;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private AuthService AuthService;
    private UserService UserService;
    private IMapper Mapper;

    public AuthController(AuthService authService, IMapper mapper, UserService userService)
    {
        AuthService = authService;
        Mapper = mapper;
        UserService = userService;
    }

    /// <summary>
    /// Auth User by auth data.
    /// </summary>
    /// <param name="model">Auth data.</param>
    /// <returns>Auth helper model.</returns>
    [HttpPost("user")]
    public async Task<ActionResult<ServiceResponse<AuthHelperModel<UserViewModel>>>> AuthUser(UserAuthModel model)
    {
        return await AuthService.Authenticate(model);
    }
    
    /// <summary>
    /// Create User by UserEditModel.
    /// </summary>
    /// <param name="model">User Edit Model.</param>
    /// <returns>Service responce of Auth Helper model of User.</returns>
    [HttpPost("user/create")]
    public async Task<ActionResult<ServiceResponse<AuthHelperModel<UserViewModel>>>> CreateUser(UserEditModel model)
    {
        return await AuthService.CreateUser(model);
    }
    
    /// <summary>
    /// Check User token.
    /// Required: Header Authorize: Bearer TOKEN 
    /// </summary>
    /// <returns>Service response of User view model.</returns>
    [HttpGet("user/check")]
    [Authorize]
    public async Task<ActionResult<ServiceResponse<UserViewModel>>> CheckToken()
    {
        var res = new ServiceResponse<UserViewModel>();
        var userId = User.GetId();
        var user = Mapper.Map<UserViewModel>(await UserService.GetByIDFull(userId));
        res.Data = user ?? null;
        if (user != null)
        {
            res.Status = true;
            return res;
        }
        else
        {
         	res.Name = "Пользователь по ID в токене не найден.";
            return res;
        }
    }


    /// <summary>
    /// Edit User by Bearer Token.
    /// Bearer token REQUIRED.
    /// Required: Header Authorize: Bearer TOKEN
    /// </summary>
    /// <param name="editModel">Edit Model of User.</param>
    /// <returns>Service Response of User View Model.</returns>
    [HttpPut("edit")]
    [Authorize]
    public async Task<ActionResult<ServiceResponse<UserViewModel>>> EditMy(UserEditModel editModel)
    {
        var res = new ServiceResponse<UserViewModel>();

        var id = User.GetId();
        if (id == null)
        {
            res.Name = "Пользователь по ID в токене не найден.";
            return res;
        }

        var result = await UserService.Update(id, editModel);
        if (result != null)
        {
            res.Data = Mapper.Map<UserViewModel>(result);
            res.Status = true;
        }
        else
        {
            res.Name = "При изменении не произошло ошибок.";
            return res;
        }

        return res;
    }
}