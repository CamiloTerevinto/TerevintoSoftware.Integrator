using Microsoft.AspNetCore.Mvc;

namespace SampleWebsite.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private readonly WeatherForecast _staticForecast = new(new DateOnly(2024, 1, 1), 50, "");

    [HttpGet] public IActionResult GetNull() => Ok();
    
    [HttpGet("ienumerable")] public ActionResult<IEnumerable<WeatherForecast>> GetIenumerable() => Ok(new[] { _staticForecast });
    [HttpGet("array")] public ActionResult<WeatherForecast[]> GetArray() => Ok(new[] { _staticForecast });
    [HttpGet("list")] public ActionResult<List<WeatherForecast>> GetList() => Ok(new[] { _staticForecast });

    [HttpGet("{id}")] public IActionResult GetEmpty(int id) => Ok();

    [HttpGet("{id}/sync")] public WeatherForecast Get(int id) => _staticForecast;
    [HttpGet("{id}/sync-wrapped")] public ActionResult<WeatherForecast> GetWrapped(int id) => _staticForecast;

    [HttpGet("{id}/async")] public async Task<WeatherForecast> GetAsync(int id) => _staticForecast;
    [HttpGet("{id}/async-wrapped")] public async Task<ActionResult<WeatherForecast>> GetWrappedAsync(int id) => _staticForecast;

    [HttpPost] public ActionResult<WeatherForecast> Create(WeatherForecast forecast) => ModelState.IsValid ? Ok(forecast) : BadRequest();

    [HttpPut("{id}")] public ActionResult<WeatherForecast> Update(int id, WeatherForecast forecast) => ModelState.IsValid ? Ok(forecast) : BadRequest();

    [HttpDelete("{id}")] public IActionResult Delete(int id) => NoContent();
}