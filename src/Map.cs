using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace com.github.dergash.h3msharp
{ 
    public class Map
    {
        public Version Version;
        public Boolean IsHeroPresent;
        public Coordinate Size;
        public String Name;
        public String Description;
        public Difficulty Difficulty;
    
        private Int32 HeaderSize;
    
        List<Player> _players;
        public List<Player> Players
        {
            get
            {
                if(_players == null)
                {
                    _players = new List<Player>();
                }
                return _players;
            }
        }
        public Map(byte[] Source)
        {
            this.Version = GetVersion(Source[0]);
            Byte[] UnpackedSource;
            if(this.Version == Version.Unknown)
            {
                UnpackedSource = Unpack(Source);
            }
            else
            {
                UnpackedSource = Source;
            }
            GetHeader(UnpackedSource);
        }

        public Version GetVersion(Byte VersionCode)
        {
            switch (VersionCode)
            {
                case (Int32)Version.ROE: break;
                case (Int32)Version.AB: break;
                case (Int32)Version.SOD: break;
                case (Int32)Version.CHR: break;
                case (Int32)Version.HOTA: break;
                case (Int32)Version.WOG: break;
                default: return Version.Unknown;
            }
            return (Version)VersionCode;
        }

        public Byte[] Unpack(Byte[] PackedMap)
        {
            Byte[] Result;
            using (var InStream = new MemoryStream(PackedMap))
            {
                using (var OutStream = new MemoryStream())
                {
                    using (var GZipStream = new GZipStream(InStream, CompressionMode.Decompress))
                    {
                        int CurrentByte;
                        do
                        {
                            CurrentByte = GZipStream.ReadByte();
                            OutStream.WriteByte((Byte)CurrentByte);
                        }
                        while (CurrentByte != -1);
                    }
                    Result = OutStream.ToArray();
                }
            }
            return Result;
        } 

        public void Parse(byte[] Source)
        {
            GetPlayers(Source,HeaderSize);
        }
           
        private void GetHeader(Byte[] Source)
        {
            var HReader = new HeaderReader(Source); 
            this.IsHeroPresent = HReader.GetIsHeroPresent();
            this.Size = new Coordinate(HReader.GetMapSize(), HReader.GetHasSubterrain());
            this.Name = HReader.GetName();
            this.Description = HReader.GetDescription();
            this.Difficulty = HReader.GetDifficulty();
            this.HeaderSize = HReader.HeaderSize;
        }
    
        private void GetPlayers(Byte[] Source, Int32 Offset)
        {
            var PReader = new PlayersReader(Source, Offset);
            Int32 PlayerOffset = 0;
            for(Color i = Color.Red; i <= Color.Pink; i++)
            {
                var Player = PReader.ReadPlayer(i, PlayerOffset);
                PlayerOffset = Player.Size;
            
                Console.WriteLine("Player size " + Player.Size);
                Players.Add(Player);
            }
        }
    }
}