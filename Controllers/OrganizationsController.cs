using Ecotrack_Api.Domain;
using Ecotrack_Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization; // Añadido

namespace Ecotrack_Api.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/organizations
[Authorize] // <-- Añadido: todos los endpoints requieren JWT
public class OrganizationsController : ControllerBase
{
    private readonly IMongoContext _ctx;
    public OrganizationsController(IMongoContext ctx) => _ctx = ctx;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Organization org)
    {
        await _ctx.Organizations.InsertOneAsync(org);
        return Created($"/api/organizations/{org.Id}", org);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var list = await _ctx.Organizations.Find(_ => true).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var item = await _ctx.Organizations.Find(o => o.Id == id).FirstOrDefaultAsync();
        return item is null ? NotFound() : Ok(item);
    }
}

