using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Extensions;

namespace App.Common.Security.Authentication
{
    static class AppAuthenticationTicketSerializer
    {
        public static AuthenticationTicket Deserialize(byte[] serializedTicket, int serializedTicketLength)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(serializedTicket))
                {
                    using (SerializingBinaryReader serializingBinaryReader = new SerializingBinaryReader((Stream)memoryStream))
                    {
                        if ((int)serializingBinaryReader.ReadByte() != 1)
                            return null;
                        int version = (int)serializingBinaryReader.ReadByte();
                        DateTime issueDateUtc = new DateTime(serializingBinaryReader.ReadInt64(), DateTimeKind.Utc);
                        if ((int)serializingBinaryReader.ReadByte() != 254)
                            return null;
                        DateTime expirationUtc = new DateTime(serializingBinaryReader.ReadInt64(), DateTimeKind.Utc);
                        string name = serializingBinaryReader.ReadBinaryString();
                        int userDataLength = serializingBinaryReader.ReadInt32();
                        byte[] userBinary = serializingBinaryReader.ReadBytes(userDataLength);
                        User user = null;
                        try
                        {
                            user = userBinary.BinaryDeserialize<User>();
                        }
                        catch
                        {
                            return null;
                        }

                        if ((int)serializingBinaryReader.ReadByte() != (int)byte.MaxValue || memoryStream.Position != (long)serializedTicketLength)
                            return null;
                        else
                            return new AuthenticationTicket(name, version, issueDateUtc, expirationUtc, user);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static byte[] Serialize(AuthenticationTicket ticket)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SerializingBinaryWriter serializingBinaryWriter = new SerializingBinaryWriter((Stream)memoryStream))
                {
                    var userBinary = ticket.UserData.BinarySerialize();
                    //var binaryString = CryptoUtils.BinaryToHex(userBinary);
                    serializingBinaryWriter.Write((byte)1);
                    serializingBinaryWriter.Write((byte)ticket.Version);
                    serializingBinaryWriter.Write(ticket.IssueDateUtc.Ticks);
                    serializingBinaryWriter.Write((byte)254);
                    serializingBinaryWriter.Write(ticket.ExpirationUtc.Ticks);
                    serializingBinaryWriter.WriteBinaryString(ticket.Name);
                    serializingBinaryWriter.Write((int)userBinary.Length);
                    serializingBinaryWriter.Write(userBinary);
                    serializingBinaryWriter.Write(byte.MaxValue);
                    return memoryStream.ToArray();
                }
            }
        }

        #region Serializing binary reader/writer
        private sealed class SerializingBinaryReader : BinaryReader
        {
            public SerializingBinaryReader(Stream input)
                : base(input)
            {
            }

            public string ReadBinaryString()
            {
                int length = this.Read7BitEncodedInt();
                byte[] numArray = this.ReadBytes(length * 2);
                char[] chArray = new char[length];
                for (int index = 0; index < chArray.Length; ++index)
                    chArray[index] = (char)((uint)numArray[2 * index] | (uint)numArray[2 * index + 1] << 8);
                return new string(chArray);
            }
        }

        private sealed class SerializingBinaryWriter : BinaryWriter
        {
            public SerializingBinaryWriter(Stream output)
                : base(output)
            {
            }

            public void WriteBinaryString(string value)
            {
                byte[] buffer = new byte[value.Length * 2];
                for (int index = 0; index < value.Length; ++index)
                {
                    char ch = value[index];
                    buffer[2 * index] = (byte)ch;
                    buffer[2 * index + 1] = (byte)((uint)ch >> 8);
                }
                this.Write7BitEncodedInt(value.Length);
                this.Write(buffer);
            }
        }

        #endregion
    }
}
