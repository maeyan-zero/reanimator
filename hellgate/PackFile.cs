using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Revival.Common;

namespace Hellgate
{
    public abstract class PackFile : HellgateFile, IDisposable
    {
        #region ZLibImports
        /* Decompresses the source buffer into the destination buffer.  sourceLen is
           the byte length of the source buffer. Upon entry, destLen is the total
           size of the destination buffer, which must be large enough to hold the
           entire uncompressed data. (The size of the uncompressed data must have
           been saved previously by the compressor and transmitted to the decompressor
           by some mechanism outside the scope of this compression library.)
           Upon exit, destLen is the actual size of the compressed buffer.
             This function can be used to decompress a whole file at once if the
           input file is mmap'ed.

             uncompress returns Z_OK if success, Z_MEM_ERROR if there was not
           enough memory, Z_BUF_ERROR if there was not enough room in the output
           buffer, or Z_DATA_ERROR if the input data was corrupted or incomplete.
        */

        [DllImport("zlibwapi86.dll")]
        private static extern int uncompress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U4)]
            ref UInt32 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U4)]
            UInt32 sourceLength
        );

        [DllImport("zlibwapi64.dll")]
        private static extern int uncompress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U8)]
            ref UInt64 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U8)]
            UInt64 sourceLength
        );


        /* Compresses the source buffer into the destination buffer.  sourceLen is
           the byte length of the source buffer. Upon entry, destLen is the total
           size of the destination buffer, which must be at least the value returned
           by compressBound(sourceLen). Upon exit, destLen is the actual size of the
           compressed buffer.
             This function can be used to compress a whole file at once if the
           input file is mmap'ed.
             compress returns Z_OK if success, Z_MEM_ERROR if there was not
           enough memory, Z_BUF_ERROR if there was not enough room in the output
           buffer.
        */

        [DllImport("zlibwapi86.dll")]
        private static extern int compress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U4)]
            ref UInt32 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U4)]
            UInt32 sourceLength
        );

        [DllImport("zlibwapi64.dll")]
        private static extern int compress
        (
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] destinationBuffer,
            [MarshalAs(UnmanagedType.U8)]
            ref UInt64 destinationLength,
            [MarshalAs(UnmanagedType.LPArray)]
            Byte[] sourceBuffer,
            [MarshalAs(UnmanagedType.U8)]
            UInt64 sourceLength
        );
        #endregion

        protected FileStream DatFile;

        public String Path { get; private set; }
        public String Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
        public String Name { get { return System.IO.Path.GetFileName(Path); } }
        public String NameWithoutExtension { get { return System.IO.Path.GetFileNameWithoutExtension(Path); } }
        public abstract String DatExtension { get; }

        public List<PackFileEntry> Files { get; private set; }
        public int Count { get { return Files.Count; } }


        protected PackFile(String filePath)
        {
            if (String.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath", "File path cannot be empty!");

            Path = filePath;
            Files = new List<PackFileEntry>();
        }

        protected abstract void WriteDatHeader();
        public abstract bool AddFile(String directory, String fileName, byte[] bytes, DateTime? fileTime = null);

        /// <summary>
        /// Gets a file entry from a path.
        /// </summary>
        /// <param name="filePath">The relative path of the file</param>
        /// <returns>The found file entry or null if not found.</returns>
        public PackFileEntry GetFileEntry(String filePath)
        {
            return Files.FirstOrDefault(fileEntry => fileEntry.Path == filePath || filePath.EndsWith(fileEntry.Path));
        }

        /// <summary>
        /// Enable reading of dat file.
        /// </summary>
        public void BeginDatReading()
        {
            _OpenDat(FileAccess.Read);
        }

        /// <summary>
        /// Enable write to dat file.
        /// </summary>
        public void BeginDatWriting()
        {
            if (DatFile != null) DatFile.Close();
            _OpenDat(FileAccess.ReadWrite);
        }

        /// <summary>
        /// Opens the dat file with a specified access.
        /// </summary>
        /// <param name="fileAccess">The file access type requested.</param>
        private void _OpenDat(FileAccess fileAccess)
        {
            if (DatFile != null) return;

            String filePath = String.Format(@"{0}\{1}{2}", Directory, NameWithoutExtension, DatExtension);

            if (File.Exists(filePath) == false)
            {
                // create new dat and add header
                DatFile = new FileStream(filePath, FileMode.Create, fileAccess);
                WriteDatHeader();
            }
            else
            {
                DatFile = new FileStream(filePath, FileMode.Open, fileAccess);
            }

        }

        /// <summary>
        /// Close the .dat file if open.
        /// </summary>
        public void EndDatAccess()
        {
            if (DatFile == null) return;

            DatFile.Close();
            DatFile = null;
        }

        /// <summary>
        /// Adds file bytes to the accompanying .dat file.
        /// Does not remove old/duplicate file bytes from duplicate or new version additions.
        /// </summary>
        /// <param name="fileData">The file byte array to add.</param>
        /// <param name="fileEntry">The file entry details.</param>
        protected void _AddFileToDat(byte[] fileData, PackFileEntry fileEntry)
        {
            // ensure .dat file open
            BeginDatWriting();

            Debug.Assert(DatFile != null && fileData != null && fileEntry != null);

            DatFile.Seek(0, SeekOrigin.End);

            byte[] writeBuffer;
            int writeLength;
            if (fileEntry.SizeCompressed > 0)
            {
                writeBuffer = new byte[fileData.Length];

                if (IntPtr.Size == 4) // x86
                {
                    UInt32 destinationLength = (UInt32)writeBuffer.Length;
                    compress(writeBuffer, ref destinationLength, fileData, (UInt32)fileData.Length);
                    writeLength = (int)destinationLength;
                }
                else // x64
                {
                    UInt64 destinationLength = (UInt64)writeBuffer.Length;
                    compress(writeBuffer, ref destinationLength, fileData, (UInt64)fileData.Length);
                    writeLength = (int)destinationLength;
                }

                fileEntry.SizeCompressed = writeLength;
            }
            else
            {
                writeBuffer = fileData;
                fileEntry.SizeCompressed = 0;
                writeLength = fileData.Length;
            }

            fileEntry.Offset = (int)DatFile.Position;
            fileEntry.SizeUncompressed = fileData.Length;
            DatFile.Write(writeBuffer, 0, writeLength);

            EndDatAccess(); // Friday 25th March. There is a work around to a problem with the File Packing in Hellpack
            // Needs tracing back
        }

        /// <summary>
        /// Reads the accompanying .dat file for the file.
        /// </summary>
        /// <param name="file">The file to be read.</param>
        /// <returns>A byte array of the files bytes, or null on error.</returns>
        public byte[] GetFileBytes(PackFileEntry file)
        {
            return _GetFileBytes(file, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="decompress"></param>
        /// <returns></returns>
        private byte[] _GetFileBytes(PackFileEntry file, bool decompress)
        {
            Debug.Assert(DatFile != null);

            int result;
            // we shouldn't load huge files into memory like this
            // todo: write progressive extraction/copy progress (cinematic files aren't compressed anyways)
            byte[] destBuffer = new byte[file.SizeUncompressed];
            DatFile.Seek(file.Offset, SeekOrigin.Begin);

            if (file.SizeCompressed > 0 && decompress)
            {
                byte[] srcBuffer = new byte[file.SizeCompressed];

                DatFile.Read(srcBuffer, 0, srcBuffer.Length);
                if (IntPtr.Size == 4)
                {
                    uint len = (uint)file.SizeUncompressed;
                    result = uncompress(destBuffer, ref len, srcBuffer, (uint)file.SizeCompressed);
                }
                else
                {
                    ulong len = (uint)file.SizeUncompressed;
                    result = uncompress(destBuffer, ref len, srcBuffer, (uint)file.SizeCompressed);
                }

                if (result != 0)
                {
                    return null;
                }
            }
            else
            {
                // if NOT decompressing, and file IS compressed (CompressedSize > 0), then read the compressed size
                int readLength = !decompress && file.SizeCompressed > 0 ? file.SizeCompressed : file.SizeUncompressed;

                result = DatFile.Read(destBuffer, 0, readLength);
                if (result != file.SizeUncompressed) return null;
            }
            return destBuffer;
        }

        /// <summary>
        /// Override of ToString() function.
        /// </summary>
        /// <returns>The index filename without the index file extension.</returns>
        public override string ToString()
        {
            return NameWithoutExtension;
        }

        /// <summary>
        /// IDisposable implementation - Ensure dat file has been closed.
        /// </summary>
        public void Dispose()
        {
            if (DatFile == null) return;

            DatFile.Dispose();
            DatFile = null;
        }
    }

    public abstract class PackFileEntry
    {
        protected const String PatchPrefix = @"backup\";

        [Browsable(false)]
        public PackFile Pack { get; set; }

        public abstract String Directory { get; set; }
        public abstract UInt32 DirectoryHash { get; set; }
        public abstract Int64 FileTime { get; set; }
        public abstract bool IsPatchedOut { get; set; }
        public abstract String Name { get; set; }
        public abstract UInt32 NameHash { get; set; }
        public abstract Int64 Offset { get; set; }
        public abstract String Path { get; set; }
        public abstract Int32 SizeCompressed { get; set; }
        public abstract Int32 SizeUncompressed { get; set; }

        public UInt64 PathHash
        {
            get { return DirectoryHash | ((UInt64)NameHash << 32); }
        }

        public DateTime LastModified
        {
            get { return DateTime.FromFileTime(FileTime); }
            set { FileTime = value.ToFileTime(); }
        }

        [Browsable(false)]
        public List<PackFileEntry> Siblings;
    }
}