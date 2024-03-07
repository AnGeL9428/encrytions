using System;
using System.Text;

public class ChineseEncoder
{
    public static int ChineseGetEncodedSize(int originalSize)
    {
        return 1 + (int)Math.Ceiling((double)originalSize / 7) * 4;
    }

    public static string ChineseEncode(byte[] data)
    {
        StringBuilder result = new StringBuilder();
        int lastCharCount = 0;
        byte[] buffer = new byte[8];

        for (int i = 0; i < data.Length; i += 7)
        {
            byte[] originalChunk = new byte[Math.Min(7, data.Length - i)];
            Array.Copy(data, i, originalChunk, 0, originalChunk.Length);
            int charCount = originalChunk.Length;

            Array.Clear(buffer, 0, buffer.Length);
            Array.Copy(originalChunk, buffer, originalChunk.Length);

            int[] chars = new int[]
            {
                ((buffer[0] << 6) | (buffer[1] >> 2)),
                (((buffer[1] & 0b11) << 12) | (buffer[2] << 4) | (buffer[3] >> 4)),
                (((buffer[3] & 0b1111) << 10) | (buffer[4] << 2) | (buffer[5] >> 6)),
                (((buffer[5] & 0b111111) << 8) | buffer[6]),
            };

            for (int j = 0; j < chars.Length; j++)
            {
                Array.Copy(BitConverter.GetBytes((ushort)(0x4E00 + chars[j])), 0, buffer, j * 2, 2);
            }

            result.Append(Encoding.Unicode.GetString(buffer));
            lastCharCount = charCount;
        }

        result.Append(lastCharCount);
        return result.ToString();
    }

    public static int ChineseGetDecodedSize(string encoded)
    {
        int size = (((encoded.Length - 1) / 4) * 7);
        if (size % 1 != 0)
        {
            throw new Exception("Invalid data");
        }
        char padding = encoded[encoded.Length - 1];
        size -= 7 - int.Parse(padding.ToString());
        return size;
    }

    public static byte[] ChineseDecode(string encoded)
    {
        int decodedSize = ChineseGetDecodedSize(encoded);
        byte[] buffer = new byte[decodedSize];
        int outputOffset = 0;

        for (int i = 0; i < encoded.Length; i += 4)
        {
            int outputIndex = (i / 4) * 7;
            if (outputIndex >= buffer.Length)
            {
                break;
            }

            byte[] chunk = Encoding.Unicode.GetBytes(encoded.Substring(i, 4));
            int[] chars = new int[]
            {
            BitConverter.ToUInt16(chunk, 0) - 0x4E00,
            BitConverter.ToUInt16(chunk, 2) - 0x4E00,
            BitConverter.ToUInt16(chunk, 4) - 0x4E00,
            BitConverter.ToUInt16(chunk, 6) - 0x4E00,
            };

            byte[] decoded = new byte[]
            {
            (byte)(chars[0] >> 6),
            (byte)(((chars[0] & 0b111111) << 2) | (chars[1] >> 12)),
            (byte)((chars[1] >> 4) & 0b11111111),
            (byte)(((chars[1] & 0b1111) << 4) | (chars[2] >> 10)),
            (byte)((chars[2] >> 2) & 0b11111111),
            (byte)(((chars[2] & 0b11) << 6) | (chars[3] >> 8)),
            (byte)(chars[3] & 0b11111111)
            };

            int copyLength = Math.Min(decoded.Length, buffer.Length - outputIndex);
            Array.Copy(decoded, 0, buffer, outputIndex, copyLength);
            outputOffset += copyLength;
        }
        return buffer;
    }

}
