using Microsoft.AspNetCore.Mvc;
using Revo.Application.DTOs;
using Revo.Application.Interfaces;
using Revo.Infrastructure.Cache;

namespace Revo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICacheService _cacheService;

    public ProjectsController(
        IProjectRepository projectRepository,
        ICacheService cacheService)
    {
        _projectRepository = projectRepository;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var cacheKey = "projects:all";
        var cached = await _cacheService.GetAsync<IEnumerable<ProjectDto>>(cacheKey);
        
        if (cached != null)
        {
            return Ok(cached);
        }

        var projects = await _projectRepository.GetAllAsync();
        var dtos = projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Color = p.Color,
            IsArchived = p.IsArchived,
            Order = p.Order,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            TaskCount = p.Tasks.Count,
            CompletedTaskCount = p.Tasks.Count(t => t.Status == Domain.Entities.TaskStatus.Done)
        });

        await _cacheService.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(5));
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        var dto = new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Color = project.Color,
            IsArchived = project.IsArchived,
            Order = project.Order,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            TaskCount = project.Tasks.Count,
            CompletedTaskCount = project.Tasks.Count(t => t.Status == Domain.Entities.TaskStatus.Done)
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto dto)
    {
        var project = new Domain.Entities.Project(
            dto.Name,
            dto.Description,
            dto.Color,
            "system");

        await _projectRepository.CreateAsync(project);
        await _cacheService.RemoveByPatternAsync("projects:*");

        var response = new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Color = project.Color,
            IsArchived = project.IsArchived,
            Order = project.Order,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            TaskCount = 0,
            CompletedTaskCount = 0
        };

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        project.Update(dto.Name, dto.Description, dto.Color, "system");
        await _projectRepository.UpdateAsync(project);
        await _cacheService.RemoveByPatternAsync("projects:*");

        var response = new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Color = project.Color,
            IsArchived = project.IsArchived,
            Order = project.Order,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            TaskCount = project.Tasks.Count,
            CompletedTaskCount = project.Tasks.Count(t => t.Status == Domain.Entities.TaskStatus.Done)
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        await _projectRepository.DeleteAsync(id);
        await _cacheService.RemoveByPatternAsync("projects:*");

        return NoContent();
    }
}
