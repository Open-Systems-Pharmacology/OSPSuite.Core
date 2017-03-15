using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Compression;
using OSPSuite.Infrastructure.Services;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_SharpLibCompression : ContextSpecification<ICompression>
   {
      protected override void Context()
      {
         sut = new SharpLibCompression();
      }
   }

   public class When_decompressing_a_compressed_byte_array_with_the_sharp_lib_compression : concern_for_SharpLibCompression
   {
      private byte[] _bytesToCompress;
      private byte[] _compressedBytes;

      protected override void Context()
      {
         base.Context();
         _bytesToCompress = new byte[] {1, 0, 1, 0, 1, 0, 2, 1, 2, 4, 2, 4, 6, 7, 8, 9, 3, 5, 7, 8, 7};
         _compressedBytes = sut.Compress(_bytesToCompress);
      }

      [Observation]
      public void should_return_the_orginal_string()
      {
         sut.Decompress(_compressedBytes).ShouldBeEqualTo(_bytesToCompress);
      }
   }
}