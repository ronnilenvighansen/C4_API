using DemoApp.Data;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Json;

namespace DemoApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ApiDbContext _context;

    public DriversController(
        ILogger<DriversController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
    {
        var results = await _context.Drivers.ToListAsync();

        using var activity = AppExtension.ActivitySource.StartActivity();
        AppExtension.Log.Information("Result of GetDrivers HttpGet request: {@results}", results);

        return results;
    }
    

    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> GetDriver(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);

        if (driver == null)
        {
            return NotFound();
        }

        using var activity = AppExtension.ActivitySource.StartActivity();
        AppExtension.Log.Information("Result of GetDriver HttpGet id request: {@driver}", driver);
        
        return driver;
    }
    
    [HttpPost]
    public async Task<ActionResult<Driver>> PostDriver(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        using var activity = AppExtension.ActivitySource.StartActivity();
        AppExtension.Log.Information("Created {@driver} in list of drivers with HttpPost request.", driver);
        
        return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriver(int id, Driver driver)
    {
        if (id != driver.Id)
        {
            return BadRequest();
        }
        
        _context.Entry(driver).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        using var activity = AppExtension.ActivitySource.StartActivity();
        AppExtension.Log.Information("Updated driver with id {@id} to {@driver} in list of drivers with HttpPut request.", id, driver);
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Driver>> DeleteDriver(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);

        if (driver == null)
        {
            return NotFound();
        }

        using var activity = AppExtension.ActivitySource.StartActivity();
        AppExtension.Log.Information("Deleted {@driver} from list of drivers with HttpDelete request.", driver);
        
        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}