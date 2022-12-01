namespace Day11;

public class Point
{
    public int EnergyLevel;
    public int X;
    public int Y;
    public bool ToFlash;
    public bool HasFlashed;
    public List<(int X, int Y)>? SurroundingPoints;
}
