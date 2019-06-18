using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Descriptors
{
   public interface IDescriptorCondition : ISpecification<EntityDescriptor>
   {
      IDescriptorCondition CloneCondition();
      void Replace(string keyword, string replacement);
   }

}