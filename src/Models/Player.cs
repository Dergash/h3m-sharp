using System;

namespace com.github.dergash.h3msharp
{
    public class Player
    {
        public Color Color;
        public Boolean AllowedForHuman;
        public Boolean AllowedForComputer;
        public Behavior Behavior;
        public TownAvailability TownsAvailable;
        public Byte Unknown;
        public Boolean HasMainTown;
        public Coordinate MainTownLocation;
        public Hero StartingHero;
        public PlayerSectionType SectionType;
        public Int32 Size;

    }
}