using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Descriptors
{
   public interface IDescriptorCondition : ISpecification<EntityDescriptor>
   {
      IDescriptorCondition CloneCondition();
      void Replace(string keyword, string replacement);
   }

   public interface ITagCondition : IDescriptorCondition
   {
      /// <summary>
      /// Returns the underlying tag associated with the condition
      /// </summary>
      string Tag { get; }

      /// <summary>
      /// Returns the semantic display of the condition for the tag
      /// </summary>
      string Condition { get; }
   }
}