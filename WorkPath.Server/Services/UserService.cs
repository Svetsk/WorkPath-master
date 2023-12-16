using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;
using WorkPath.Server.Extensions;
using WorkPath.Server.Models;

namespace WorkPath.Server.Services;

public class UserService
{
    private ServerContext Context;
    private IMapper Mapper;

    public UserService(ServerContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public List<UserEntity> GetAll()
    {
        return Context.Users.AsNoTracking().ToList();
    }

    public List<UserEntity> GetAllFull()
    {
        return Context.Users.AsNoTracking()
            .ToList();
    }

    public async Task<UserEntity?> GetByID(Guid id)
    {
        return await Context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<UserEntity?> GetByIDFull(Guid id)
    {
        return await Context.Users.AsNoTracking()
            .Include(x => x.Companies)
            .Include(x => x.Educations)
            .FirstOrDefaultAsync(x => x.ID == id);
    }
    
    public UserEntity? GetByLogin(string login)
    {
        return Context.Users.AsNoTracking().AsEnumerable().FirstOrDefault(x =>
            (x.Login?.Equals(login, StringComparison.OrdinalIgnoreCase) ?? false));
    }

    public async Task<UserEntity?> Update(Guid id, UserEditModel editModel)
    {
        var entity = Mapper.Map<UserEntity>(editModel);
        entity.ID = id;

        // TODO проверки

        if (!string.IsNullOrWhiteSpace(editModel.Password) && editModel.Password.Equals(editModel.RepeatPassword))
        {
            entity.PasswordHash = editModel.Password.ToHashSHA256();
        }
        else
        {
            Context.Entry(entity).Property(x => x.PasswordHash).IsModified = false;
        }

        Context.Entry(entity).Property(x => x.CreatedAt).IsModified = false;

        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<UserEntity?> Create(UserEditModel editModel)
    {
        var entity = Mapper.Map<UserEntity>(editModel);
        // TODO проверки

        if (!string.IsNullOrWhiteSpace(editModel.Password))
        {
            entity.PasswordHash = editModel.Password.ToHashSHA256();
        }
        else
        {
            // TODO
        }

        Context.Add(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await Context.Users.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        Context.Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }
}