using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPath.Server.EditModels;
using WorkPath.Server.Models;
using WorkPath.Server.Services;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private CompanyService Service;
    private IMapper Mapper;

    public CompanyController(CompanyService service, IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    // GET: api/Company
    [HttpGet]
    public async Task<ActionResult<List<CompanyViewModel>>> GetAll()
    {
        var datas = Service.GetAll().Select(x => Mapper.Map<CompanyViewModel>(x)).ToList();
        return datas;
    }

    /// <summary>
    /// Get Full dates and add includes.
    /// </summary>
    /// <returns></returns>
    [HttpGet("full")]
    public async Task<ActionResult<List<CompanyViewModel>>> GetFull()
    {
        var datas = Service.GetAllFull().Select(x => Mapper.Map<CompanyViewModel>(x)).ToList();
        return Ok(datas);
    }

    // GET: api/Company?id=5
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyViewModel>> GetByID(Guid id)
    {
        var data = await Service.GetByID(id);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<CompanyViewModel>(data));
    }

    // POST: api/Company
    [HttpPost]
    public async Task<ActionResult<CompanyViewModel>> Create([FromBody] CompanyEditModel editModel)
    {
        var data = await Service.Create(editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<CompanyViewModel>(data));
    }

    // PUT: api/Company/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CompanyViewModel>> Edit(Guid id, [FromBody] CompanyEditModel editModel)
    {
        var data = await Service.Update(id, editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<CompanyViewModel>(data));
    }

    // DELETE: api/Company/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var data = await Service.Delete(id);
        return data ? NoContent() : NotFound("Сущность не найдена");
    }
}