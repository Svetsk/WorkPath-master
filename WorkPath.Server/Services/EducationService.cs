using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;

namespace WorkPath.Server.Services;

public class EducationService
{
    private ServerContext Context;
    private IMapper Mapper;

    public EducationService(ServerContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public List<EducationEntity> GetAll()
    {
        return Context.Educations.AsNoTracking().ToList();
    }

    public List<EducationEntity> GetAllFull()
    {
        return Context.Educations.AsNoTracking()
            .ToList();
    }

    public async Task<EducationEntity?> GetByID(Guid id)
    {
        return await Context.Educations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<EducationEntity?> GetByIDFull(Guid id)
    {
        return await Context.Educations.AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.ID == id);
    }
    
    public async Task<EducationEntity?> Update(Guid id, EducationEditModel editModel)
    {
        var entity = Mapper.Map<EducationEntity>(editModel);
        entity.ID = id;

        // TODO проверки

        Context.Entry(entity).Property(x => x.UserID).IsModified = false;

        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<EducationEntity?> Create(EducationEditModel editModel)
    {
        var entity = Mapper.Map<EducationEntity>(editModel);
        // TODO проверки

        Context.Add(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await Context.Educations.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        Context.Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }
}