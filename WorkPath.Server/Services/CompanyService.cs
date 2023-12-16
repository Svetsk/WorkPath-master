using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;

namespace WorkPath.Server.Services;

public class CompanyService
{
    private ServerContext Context;
    private IMapper Mapper;

    public CompanyService(ServerContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }
    
    public List<CompanyEntity> GetAll()
    {
        return Context.Companies.AsNoTracking().ToList();
    }

    public List<CompanyEntity> GetAllFull()
    {
        return Context.Companies.AsNoTracking()
            .ToList();
    }

    public async Task<CompanyEntity?> GetByID(Guid id)
    {
        return await Context.Companies.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<CompanyEntity?> GetByIDFull(Guid id)
    {
        return await Context.Companies.AsNoTracking()
            .Include(x => x.Director)
            .Include(x => x.Jobs)
            .FirstOrDefaultAsync(x => x.ID == id);
    }
    
    public async Task<CompanyEntity?> Update(Guid id, CompanyEditModel editModel)
    {
        var entity = Mapper.Map<CompanyEntity>(editModel);
        entity.ID = id;

        // TODO проверки

        Context.Entry(entity).Property(x => x.INN).IsModified = false;
        Context.Entry(entity).Property(x => x.OGRN).IsModified = false;

        Context.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<CompanyEntity?> Create(CompanyEditModel editModel)
    {
        var entity = Mapper.Map<CompanyEntity>(editModel);
        // TODO проверки

        Context.Add(entity);

        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await Context.Companies.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        Context.Remove(entity);
        await Context.SaveChangesAsync();

        return true;
    }
}