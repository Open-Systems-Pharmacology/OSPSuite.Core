using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Domain.Services
{
   /// <summary>
   /// Replaces a keyword in an object path
   /// </summary>
   public interface IKeywordReplacer
   {
   }

   public interface IKeywordInObjectPathReplacer : IKeywordReplacer
   {
      /// <summary>
      /// Replaces the Keyword in the given <paramref name="objectPath"/>.
      /// returns true if a replacement was done, otherwise false
      /// </summary>
      /// <param name="objectPath">The object path.</param>
      void ReplaceIn(IObjectPath objectPath);
   }

   public interface IKeywordInTagsReplacer : IKeywordReplacer
   {
      void ReplaceIn(Tags tags);
      void ReplaceIn(IDescriptorCondition descriptorCondition);
   }
}