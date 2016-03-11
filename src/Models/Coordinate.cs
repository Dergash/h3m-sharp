using System;
public class Coordinate
{
    public Int32 X;
    public Int32 Y;
    public Boolean Z;
    
    public Coordinate(Int32 X, Int32 Y, Boolean Z)
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    public Coordinate(Int32 Size, Boolean HasSubterrain)
    {
        this.X = Size;
        this.Y = Size;
        this.Z = HasSubterrain;
    }
    
    public override String ToString()
    {
        String Result = X + "x" + Y + "x" + ((Z) ? "1" : "0");
        return Result;
    }
}