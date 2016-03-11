using System;

namespace com.github.dergash.h3msharp
{
    public class BinaryReader
    {
        public BinaryReader()
        {

            // var Provider = System.Text.CodePagesEncodingProvider.Instance;
            // System.Text.Encoding.RegisterProvider(Provider);
        }
        public class MapFormatException : System.FormatException
        {
            public Int32 ErrorOffset;
            public Byte[] ErrorValue;

            public MapFormatException(Int32 Offset, Byte[] Value)
            {
                this.ErrorOffset = Offset;
                this.ErrorValue = Value;
            }
            public MapFormatException(string message) : base(message) { }
            public MapFormatException(string message, System.Exception inner) : base(message, inner) { }
        }

        public Boolean ReadBoolean(Int32 Offset, Byte[] Source)
        {

            Byte Value = Source[Offset];
            if (Value == 0x00)
            {
                return false;
            }
            if (Value == 0x01)
            {
                return true;
            }
            throw new MapFormatException(Offset, new byte[] { Value });
        }

        public Int32 ReadInt32(Int32 Offset, Byte[] Source)
        {
            Byte[] Value = new Byte[4];
            for (int i = 0; i < 4; i++)
            {
                Value[i] = Source[Offset + i];
            }
            return BitConverter.ToInt32(Value, 0);
        }

        public String ReadString(Int32 Offset, Int32 Length, Byte[] Source)
        {
            return System.Text.Encoding.GetEncoding(1251).GetString(Source, Offset, Length);
        }
    }
}