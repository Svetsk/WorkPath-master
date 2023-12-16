using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkPath.Server.EditModels;
using WorkPath.Server.Services;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationController : ControllerBase
{
    private EducationService Service;
    private IMapper Mapper;

    public EducationController(EducationService service, IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    // GET: api/Education
    [HttpGet]
    public async Task<ActionResult<List<EducationViewModel>>> GetAll()
    {
        var datas = Service.GetAll().Select(x => Mapper.Map<EducationViewModel>(x)).ToList();
        return datas;
    }

    [HttpGet("full")]
    public async Task<ActionResult<List<EducationViewModel>>> GetFull()
    {
        var datas = Service.GetAllFull().Select(x => Mapper.Map<EducationViewModel>(x)).ToList();
        return Ok(datas);
    }

    // GET: api/Education?id=5
    [HttpGet("{id}")]
    public async Task<ActionResult<EducationViewModel>> GetByID(Guid id)
    {
        var data = await Service.GetByID(id);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<EducationViewModel>(data));
    }

    // POST: api/Education
    [HttpPost]
    public async Task<ActionResult<EducationViewModel>> Create([FromBody] EducationEditModel editModel)
    {
        var data = await Service.Create(editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<EducationViewModel>(data));
    }

    // PUT: api/Education/5
    [HttpPut("{id}")]
    public async Task<ActionResult<EducationViewModel>> Edit(Guid id, [FromBody] EducationEditModel editModel)
    {
        var data = await Service.Update(id, editModel);
        if (data == null)
        {
            return NotFound();
        }

        return Ok(Mapper.Map<EducationViewModel>(data));
    }

    // DELETE: api/Education/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var data = await Service.Delete(id);
        return data ? NoContent() : NotFound("Сущность не найдена");
    }
}