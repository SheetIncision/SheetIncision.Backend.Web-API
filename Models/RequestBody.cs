namespace SheetIncision.Backend.Models;

public class RequestBody
{
    public required List<List<int>> Matrix { get; set; }

    public bool AllowDiagonals { get; set; }
}