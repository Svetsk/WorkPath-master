using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkPath.Server.EditModels;
using WorkPath.Server.Services;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    private JobService Service;
    private IMapper Mapper;

    public JobController(JobService service, IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    // GET: api/Job
    [HttpGet]
    public async Task<ActionResult<List<JobViewModel>>> GetAll()
    {
        var datas = Service.GetAll().Select(x => Mapper.Map<JobViewModel>(x)).ToList();
        return datas;
    }

    [HttpGet("full")]
    public async Task<ActionResult<List<JobViewModel>>> GetFull()
    {
        var datas = Service.GetAllFull().Select(x => Mapper.Map<JobViewModel>(x)).ToList();
        return Ok(datas);
    }

    // GET: api/Job?id=5
    [HttpGet("{id}")]
    public async Task<ActionResult<JobViewModel>> GetByID(Guid id)
    {
        var data = await Service.GetByID(id);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<JobViewModel>(data));
    }

    // POST: api/Job
    [HttpPost]
    public async Task<ActionResult<JobViewModel>> Create([FromBody] JobEditModel editModel)
    {
        var data = await Service.Create(editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<JobViewModel>(data));
    }

    // PUT: api/Job/5
    [HttpPut("{id}")]
    public async Task<ActionResult<JobViewModel>> Edit(Guid id, [FromBody] JobEditModel editModel)
    {
        var data = await Service.Update(id, editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<JobViewModel>(data));
    }

    // DELETE: api/Job/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var data = await Service.Delete(id);
        return data ? NoContent() : NotFound("Сущность не найдена");
    }
}