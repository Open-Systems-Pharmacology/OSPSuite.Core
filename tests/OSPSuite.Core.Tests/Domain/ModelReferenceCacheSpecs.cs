using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Serializer;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ModelReferenceCache : ContextSpecification<ModelReferenceCache>
   {
      private ICloneManagerForModel _cloneManagerForModel;
      private IWithIdRepository _withIdRepository;

      protected override void Context()
      {
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _withIdRepository = A.Fake<IWithIdRepository>();
         sut = new ModelReferenceCache(_withIdRepository, _cloneManagerForModel);
      }
   }

   public class When_adding_a_property_map_and_resolving_the_reference_twice_for_an_object : concern_for_ModelReferenceCache
   {
      private object _anObject;
      private IPropertyMap _propertyMap;

      protected override void Context()
      {
         base.Context();
         _anObject = new object();
         _propertyMap = A.Fake<IPropertyMap>();
         A.CallTo(() => _propertyMap.ResolveValue(_anObject)).Returns(new object());
         sut.Add(_anObject, _propertyMap, "TOTO");
      }

      [Observation]
      public void should_not_set_the_reference_a_second_time()
      {
         A.CallTo(() => _propertyMap.SetValue(_anObject, A<object>._)).MustNotHaveHappened();
      }
   }
}