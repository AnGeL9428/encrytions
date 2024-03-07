# Chinese Encoder

The `ChineseEncoder` class in C# provides methods for encoding and decoding byte arrays containing Chinese characters. The encoding scheme used here is a custom representation designed to efficiently store Chinese characters in a byte array.

## Methods

### ChineseEncode

```csharp
public static string ChineseEncode(byte[] data)
```
byte -> string (Chinese)
Encodes a byte array containing Chinese characters into a string representation.


### ChineseDecode

```csharp
public static byte[] ChineseDecode(string encoded)
```
string(Chinese) -> string
Decodes an encoded string back into a byte array containing Chinese characters.

## Usage Example
```csharp
 static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // text with bytes
            byte[] originalData = Encoding.UTF8.GetBytes("Example");
            
            // (encode)
            string encodedData = ChineseEncoder.ChineseEncode(originalData);

            Console.WriteLine($"Encoded Data: {encodedData}");

            // (decode)
            byte[] decodedData = ChineseEncoder.ChineseDecode(encodedData);
            string originalString = Encoding.UTF8.GetString(decodedData);

            Console.WriteLine($"Decoded Data: {originalString}");


            Console.ReadKey();
        }
```
### Output
```csharp
Encoded Data: 彞吖菁穥7
Decoded Data: Example
```
