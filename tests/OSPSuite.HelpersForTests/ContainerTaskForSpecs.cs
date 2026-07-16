using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Helpers
{
   public class ContainerTaskForSpecs : ContainerTask
   {
      public ContainerTaskForSpecs() : base(A.Fake<IObjectBaseFactory>(), new EntityPathResolverForSpecs(), new ObjectPathFactoryForSpecs())
      {
      }
   }
}