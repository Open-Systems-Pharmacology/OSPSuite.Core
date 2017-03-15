using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using OSPSuite.Utility.Compression;

namespace OSPSuite.Infrastructure.Services
{
   public class SharpLibCompression : ICompression
   {
      public byte[] Compress(byte[] byteArrayToCompress)
      {
         using (var ms = new MemoryStream())
         using (var zipOutput = new ZipOutputStream(ms))
         {
            var entry = new ZipEntry("Name");
            zipOutput.SetLevel(9);
            zipOutput.PutNextEntry(entry);
            entry.Size = byteArrayToCompress.Length;
            zipOutput.Write(byteArrayToCompress, 0, byteArrayToCompress.Length);
            zipOutput.CloseEntry();
            return ms.ToArray();
         }
      }

      public byte[] Decompress(byte[] byteArrayToDecompress)
      {
         using (var ms = new MemoryStream(byteArrayToDecompress))
         using (var zip = new ZipInputStream(ms))
         using (var output = new MemoryStream())
         {
            zip.GetNextEntry();
            int size = 4096;
            var buffer = new byte[size];
            int n;
            while ((n = zip.Read(buffer, 0, buffer.Length)) != 0)
            {
               output.Write(buffer, 0, n);
            }

            return output.ToArray();
         }
      }
   }
}