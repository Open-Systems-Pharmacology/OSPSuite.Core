using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Descriptors
{
   public interface IDescriptorCondition : ISpecification<Tags>
   {
      IDescriptorCondition CloneCondition();
      void Replace(string keyword, string replacement);
   }

   public interface ITagCondition : IDescriptorCondition
   {
      string Tag { get; }
   }
}