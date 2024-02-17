using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SheetIncision.Backend.Models;

namespace SheetIncision.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SheetIncisionController : Controller
{
    [HttpGet("GetNumberOfPieces")]
    public async Task<IActionResult> GetNumberOfPieces([FromQuery] string data)
    {
        var requestBody = JsonConvert.DeserializeObject<RequestBody>(data);

        var matrixOfIncision = new MatrixOfIncision(requestBody.Matrix, requestBody.AllowDiagonals);

        return Ok(await matrixOfIncision.GetNumberOfZones());
    }

}