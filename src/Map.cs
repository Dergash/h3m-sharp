using System;
using System.Collections.Generic;

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
            this.Parse(Source);
        }
    
        private void Parse(byte[] Source)
        {
            try
            {
                GetHeader(Source);
                GetPlayers(Source,HeaderSize);
            }
            catch(BinaryReader.MapFormatException e)
            {
                Console.WriteLine("\nInvalid value occured while reading offset: " + e.ErrorOffset.ToString());
                Console.WriteLine("\n Value encountered: " + BitConverter.ToString(e.ErrorValue));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
           
        private void GetHeader(Byte[] Source)
        {
            var HReader = new HeaderReader(Source); 
            this.Version = HReader.GetVersion();
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