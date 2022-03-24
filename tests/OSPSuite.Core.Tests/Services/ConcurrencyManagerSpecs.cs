using System.Collections.Generic;
using System.Threading;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ConcurrencyManager : ContextSpecification<IConcurrencyManager>
   {
      protected IObjectTypeResolver _objectTypeResolver;

      protected override void Context()
      {
         _objectTypeResolver= A.Fake<IObjectTypeResolver>();
         sut = new ConcurrencyManager(_objectTypeResolver);
      }
   }

   public class When_running_concurrently_a_list_of_identical_object : concern_for_ConcurrencyManager
   {
      private readonly List<IParameter> _data = new List<IParameter>();

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectTypeResolver.TypeFor<IParameter>()).Returns("Parameter");
         _data.Add(DomainHelperForSpecs.ConstantParameterWithValue(10).WithId("ID1"));
         _data.Add(DomainHelperForSpecs.ConstantParameterWithValue(20).WithId("ID1"));
      }
      
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.RunAsync(_data, (x, t) => x.Id, CancellationToken.None, 4)).ShouldThrowAn<NotUniqueIdException>();
      }
   }
}