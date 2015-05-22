# Introduction #

The index (.idx) file is reference table that maps the file names, directories, sizes and offsets within the data (.dat) file. The file is encrypted which must be decrtyped before it can be modified.

# File Structure #

| **Alias** | **Size** | **Notes** |
|:----------|:---------|:----------|
| **Main Header** |
| token     | `Int32`  | Must be 0x6867696E ('nigh') |
| structCount | `Int32`  | Number of Structs in Index(?) - count tokens |
| fileCount | `Int32`  | File count |
| **String Block** |
| token     | `Int32`  | Must be 0x68677073 ('spgh') |
| stringCount | `Int32`  | String count. |
| blockSize | `Int32`  | Number of bytes in following block. |
| stringBytes | `blockSize` | The strings (each one is \0) lumped together as one big block. |
| **String Data** |
| token     | `Int32`  | Must be 0x68677073 ('spgh') |
| for (stringCount) |
| {         |
| stringSize | `Int16`  | Count of chars in string (not including \0) |
| unknown   | `Int32`  | CRC perhaps?  -  Not required for valid game loading. |
| }         |
| **File Block** |
| token     | `Int32`  | Must be 0x68677073 ('spgh') |
| for (fileCount) |
| {         |
| token     | `Int32`  | Must be 0x6867696F ('oigh') |
| unknown   | `Int32`  | Not required for valid game loading (can be null). |
| unknown   | `Int32`  | REQUIRED for valid game loading! // Must be a specific value... What? |
| dataOffset | `Int32`  | Offset in bytes within accompanying .dat file. |
| null      | `Int32`  |           |
| uncompressedSize | `Int32`  |           |
| compressedSize | `Int32`  |           |
| null      | `Int32`  |           |
| directoryArrayPosition | `Int32`  |           |
| filenameArrayPosition | `Int32`  |           |
| unknown   | `Int32`  | REQUIRED for valid game loading! // Game clears .idx and .dat if null (can be anything but null). |
| unknown   | `Int32`  | Not required for valid game loading (can be null). |
| unknown   | `Int32`  | Not required for valid game loading (can be null). |
| unknown   | `Int32`  | Not required for valid game loading (can be null). |
| null      | `Int32`  |           |
| null      | `Int32`  |           |
| null      | `Int32`  |           |
| startOfFile | `8 Bytes` |First 8 bytes of said file.  -  Not required for valid game loading (can be null). |
| token     | `Int32`  | Must be 0x6867696F ('oigh'). |
| }         |

# Encryption #

The algorithm used the encrypt the index.