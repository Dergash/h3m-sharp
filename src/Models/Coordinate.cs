using System;

namespace com.github.dergash.h3msharp
{
    public class Coordinate
    {
        public UInt32 X;
        public UInt32 Y;
        public Boolean Z;

        public Coordinate(Int32 X, Int32 Y, Boolean Z)
        {
            this.X = (UInt32)X;
            this.Y = (UInt32)Y;
            this.Z = Z;
        }
        public Coordinate(Int32 Size, Boolean HasSubterrain)
        {
            this.X = (UInt32)Size;
            this.Y = (UInt32)Size;
            this.Z = HasSubterrain;
        }

        public override String ToString()
        {
            String Result = X + "x" + Y + "x" + ((Z) ? "1" : "0");
            return Result;
        }
    }
}