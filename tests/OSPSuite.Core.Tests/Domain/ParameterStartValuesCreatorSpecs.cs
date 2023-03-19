using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterStartValuesCreator : ContextSpecification<IParameterStartValuesCreator>
   {
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectBaseFactory.Create<ParameterStartValuesBuildingBlock>()).Returns(new ParameterStartValuesBuildingBlock());
         sut = new ParameterStartValuesCreator(_objectBaseFactory,new ObjectPathFactory(new AliasCreator()), new IdGenerator());
      }
   }

}	