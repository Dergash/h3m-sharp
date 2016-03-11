using System;

/*  Based on potmdehex's h3m_player.h 
    https://github.com/potmdehex/homm3tools */

public class PlayersReader
{
    byte[] Source;
    
    BinaryReader Reader;
    
    Int32 StartOffset;

    const Int32 AllowedForHumanOffest = 0x00000000;
    const Int32 AllowedForComputerOffset = 0x0000001;
    const Int32 BehaviorOffset = 0x00000002;
    const Int32 TownsAvailableOffset = 0x00000003;
    const Int32 HasMainTownOffset = 0x00000005;
    
    TownAvailability AvailableTowns;
    Boolean HasMainTown;
    public PlayersReader(Byte[] Source, Int32 StartOffset)
    {
        this.Source = Source;
        this.Reader = new BinaryReader();
        this.StartOffset = StartOffset;
    }
    
    public Player ReadPlayer(Color Color, Int32 Offset)
    {
        var Result = new Player();
        this.StartOffset += Offset;
        
        //Console.WriteLine("Section start address: " + (StartOffset).ToString("X2"));
        //Console.WriteLine("Section start byte: " + Source[StartOffset].ToString("X2"));
        Result.Color = Color;
        Result.AllowedForHuman = Reader.ReadBoolean(StartOffset + AllowedForHumanOffest, Source);
        Result.AllowedForComputer = Reader.ReadBoolean(StartOffset + AllowedForComputerOffset, Source);
        Result.Behavior = (Behavior)Source[StartOffset + BehaviorOffset];
        Result.TownsAvailable = (TownAvailability)Source[StartOffset + TownsAvailableOffset];
        Result.Unknown = Source[StartOffset + TownsAvailableOffset + 1];
        Result.HasMainTown = Reader.ReadBoolean(StartOffset + HasMainTownOffset, Source);
        if(Result.HasMainTown)
        {
            Result.MainTownLocation = GetMainTownLocation();
        }
        Result.SectionType = GetSectionType(Result.HasMainTown);
        Result.StartingHero = GetStartingHero(Result.SectionType);
        
        Int32 Size;
        switch(Result.SectionType)
        {
            case PlayerSectionType.Default:
            {
                Result.Size = 8;
                break;
            }
            case PlayerSectionType.OnlyHero:
            {
                Result.Size = 14 + Result.StartingHero.NameSize;
                break;
            }
            case PlayerSectionType.OnlyTown:
            {
                Result.Size = 11;
                break;
            }
            case PlayerSectionType.TownAndHero:
            {
                Result.Size = 16 + Result.StartingHero.NameSize;
                break;
            }
        }
        return Result;
    }
    
    private Boolean IsNoTownAndHero()
    {
        if(Source[StartOffset + 6] == 0x00 || Source[StartOffset + 6] == 0x01)
        {
            if(Source[StartOffset + 7] == 0xFF)
            {
                return true;
            }
        }
        return false;
    }
    
    private Boolean IsOnlyTown()
    {
        if(Source[StartOffset + 8] == 0x00 || Source[StartOffset + 8] == 0x01)
        {
            if(Source[StartOffset + 10] == 0xFF)
            {
                return true;
            }
        }
        return false;
    }

    private Boolean IsOnlyHero()
    {
        if(Source[StartOffset + 6] == 0x00 || Source[StartOffset + 6] == 0x01)
        {
            if(Source[StartOffset + 7] != 0xFF)
            {
                return true;
            }
        }
        return false;
    }
    
    private Boolean IsTownAndHero()
    {
        if(Source[StartOffset + 8] == 0x00 || Source[StartOffset + 8] == 0x01)
        {
            if(Source[StartOffset + 10] != 0xFF)
            {
                return true;
            }
        }
        return false;
    }
    
    private Coordinate GetMainTownLocation()
    {
        Int32 X = Source[StartOffset + 6];
        Int32 Y = Source[StartOffset + 7];
        Boolean Z = Reader.ReadBoolean(StartOffset + 8, Source);
        return new Coordinate(X,Y,Z);
    }
    
    private Hero GetStartingHero(PlayerSectionType SectionType)
    {
        var Result = new Hero();
        
        if(SectionType == PlayerSectionType.Default)
        {
            Result.IsRandom = Reader.ReadBoolean(StartOffset + 0x00000006, Source);
            Result.Id = Source[StartOffset + 0x00000007];
        }
        if(SectionType == PlayerSectionType.OnlyTown)
        {
            Result.IsRandom = Reader.ReadBoolean(StartOffset + 0x00000008, Source);
            Result.Id = Source[StartOffset + 0x000000009]; 
        }
        if(SectionType == PlayerSectionType.OnlyHero)
        {
            Result.IsRandom = Reader.ReadBoolean(StartOffset + 0x00000006, Source);
            Result.Id = Source[StartOffset + 0x00000007];
            Result.Portrait = Source[StartOffset + 0x00000008];
            Result.NameSize = Reader.ReadInt32(StartOffset + 0x00000009, Source);
            if(Result.NameSize > 0)
            {
                Result.Name = Reader.ReadString(StartOffset + 0x0000000A, Result.NameSize, Source);
            }
        }
        if(SectionType == PlayerSectionType.TownAndHero)
        {
            Result.IsRandom = Reader.ReadBoolean(StartOffset + 0x00000009, Source);
            Result.Id = Source[StartOffset + 0x0000000A];
            Result.Portrait = Source[StartOffset + 0x0000000B];
            Result.NameSize = Reader.ReadInt32(StartOffset + 0x0000000C, Source);
            if(Result.NameSize > 0)
            {
                Result.Name = Reader.ReadString(StartOffset + 0x0000000D, Result.NameSize, Source);
            } 
        }
        return Result;
    }
    
    private PlayerSectionType GetSectionType(Boolean HasMainTown)
    {
        if(HasMainTown)
        {
            GetMainTownLocation();  
            if(IsOnlyTown())
            {
                return PlayerSectionType.OnlyTown;
            }
            if(IsTownAndHero())
            {
                return PlayerSectionType.TownAndHero;
            }
        }
        else
        {
            if(IsNoTownAndHero())
            {
               return PlayerSectionType.Default;
            }
            if(IsOnlyHero())
            {
                return PlayerSectionType.OnlyHero;
            }
        }
        throw new FormatException("Invalid player section type");
    }
}