using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_OutputResiduals : ContextSpecification<OutputResiduals>
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         _dataRepository = A.Fake<DataRepository>();
         sut = new OutputResiduals("the|full|path", _dataRepository, new List<Residual> { new Residual(0.0, 1.0, 1) });
      }
   }

   public class When_updating_the_full_path : concern_for_OutputResiduals
   {
      protected override void Because()
      {
         sut.UpdateFullOutputPath("the", "a");
      }

      [Observation]
      public void Observation()
      {
         sut.FullOutputPath.ShouldBeEqualTo("a|full|path");
      }
   }
}
