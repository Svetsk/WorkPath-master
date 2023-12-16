using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;

namespace WorkPath.Server.Services;

public class JobService
{
    private ServerContext Context;
    private IMapper Mapper;

    public JobService(ServerContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public List<JobEntity> GetAll()
    {
        return Context.Jobs.AsNoTracking().ToList();
    }

    public List<JobEntity> GetAllFull()
    {
        return Context.Jobs.AsNoTracking()
            .ToList();
    }

    public async Task<JobEntity?> GetByID(Guid id)
    {
        return await Context.Jobs.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<JobEntity?> GetByIDFull(Guid id)
    {
        return await Context.Jobs.AsNoTracking()
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.ID == id);
    }
    
    public async Task<JobEntity?> Update(Guid id, JobEditModel editModel)
    {
        var entity = Mapper.Map<JobEntity>(editModel);
        entity.ID = id;

        // TODO проверки

        Context.Entry(entity).Property(x => x.CompanyID).IsModified = false;

        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<JobEntity?> Create(JobEditModel editModel)
    {
        var entity = Mapper.Map<JobEntity>(editModel);
        // TODO проверки

        Context.Add(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await Context.Jobs.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        Context.Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }
}