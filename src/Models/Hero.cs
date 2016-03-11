using System;
public class Hero
{
    public Boolean IsRandom;
    public Int32 Id;
    public HeroClass Class;
    public Int32 Portrait;
    public Int32 NameSize;
    public String _name;
    
    public String Name
    {
        get
        {
            if(NameSize == 0)
            {
                return "Default";
            }
            return _name;
        }
        set
        {
            if(value.Length > 12)
            {
                throw new ArgumentOutOfRangeException("Hero name limited to 12 chars");
            }
            else
            {
                _name = value;
                NameSize = _name.Length;
            }
        }
    }
}