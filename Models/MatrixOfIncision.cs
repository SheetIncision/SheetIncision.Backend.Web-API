namespace SheetIncision.Backend.Models;

public class MatrixOfIncision
{
    private readonly IEnumerable<IEnumerable<int>> _matrix;
    private readonly int _rows;
    private readonly int _cols;
    private readonly bool[,] _visited;
    private readonly bool _allowDiagonals;
    private readonly List<List<Tuple<int, int>>> _zones = new List<List<Tuple<int, int>>>();
    public MatrixOfIncision(IEnumerable<IEnumerable<int>> inputMatrix, bool allowDiagonals)
    {
        _matrix = inputMatrix;
        _allowDiagonals = allowDiagonals;
        _rows = _matrix.Count();
        _cols = _matrix.First().Count();
        _visited = new bool[_rows, _cols];
    }

    public async Task<int> GetNumberOfZones()
    {
        await FindZones();

        return _zones.Count;
    }

    private async Task FindZones()
    {
        List<Tuple<int, int>> unVisited = new List<Tuple<int, int>>();

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                if (_matrix.ElementAt(i).ElementAt(j) == 1)
                {
                    _visited[i, j] = true;
                }
                else
                {
                    unVisited.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        foreach (var vertex in unVisited)
        {
            if (!_visited[vertex.Item1, vertex.Item2])
            {
                var zone = new List<Tuple<int, int>>();

                await Traversal(
                    vertex.Item1,
                    vertex.Item2,
                    zone);

                _zones.Add(zone);
            }
        }
    }

    private async Task Traversal(int row, int col, List<Tuple<int, int>> zone)
    {
        _visited[row, col] = true;

        var directions = GetDirections();

        for (int i = 0; i < directions.GetLength(1); i++)
        {
            int newRow = row + directions[0, i];
            int newCol = col + directions[1, i];

            if (IsValid(newRow, newCol))
            {
                await Traversal(newRow, newCol, zone);
                i = -1;
            }
        }

        zone.Add(new Tuple<int, int>(row, col));
    }

    private bool IsValid(int row, int col)
    {
        return row >= 0
               && row < _rows
               && col >= 0
               && col < _cols
               && !_visited[row, col];
    }

    private int[,] GetDirections() => _allowDiagonals switch
    {
        true => new int[,]
        {
            { -1, 1, 0, 0, -1, -1, 1, 1},
            { 0, 0, -1, 1, -1, 1, -1, 1 },
        },
        false => new int[,]
        {
            { -1, 1, 0, 0 },
            { 0, 0, -1, 1 },
        }
    };
}