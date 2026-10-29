using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization
{
   public abstract class concern_for_ObjectReferenceMapper : ContextSpecification<ObjectReferenceMapper>
   {
      protected SerializationContext _serializationContext;
      protected IParameter _parameter;
      protected ExplicitFormula _formula;

      protected override void Context()
      {
         _serializationContext = new SerializationContext(A.Fake<IDimensionFactory>(), A.Fake<IObjectBaseFactory>(), A.Fake<IWithIdRepository>(),
            new List<DataRepository>(), A.Fake<ICloneManagerForModel>(), A.Fake<Utility.Container.IContainer>());
         _parameter = new Parameter().WithId("P1");
         _formula = new ExplicitFormula().WithId("F1");
         sut = new ObjectReferenceMapper();
      }
   }

   public class When_mapping_references_without_a_reference_marker : concern_for_ObjectReferenceMapper
   {
      [Observation]
      public void should_return_the_id_of_the_referenced_object()
      {
         sut.ReferenceFor(_parameter, _serializationContext).ShouldBeEqualTo("P1");
      }

      [Observation]
      public void should_return_the_id_of_a_referenced_formula_and_add_the_formula_to_the_cache()
      {
         sut.ReferenceFor(_formula, _serializationContext).ShouldBeEqualTo("F1");
         _serializationContext.Formulas.ShouldOnlyContain(_formula);
      }

      [Observation]
      public void should_return_an_empty_reference_for_an_object_without_id_or_for_null()
      {
         sut.ReferenceFor(null, _serializationContext).ShouldBeEqualTo(string.Empty);
         sut.ReferenceFor("not an object with id", _serializationContext).ShouldBeEqualTo(string.Empty);
      }
   }

   public class When_mapping_references_with_a_reference_marker : concern_for_ObjectReferenceMapper
   {
      protected override void Context()
      {
         base.Context();
         _serializationContext.ReferenceMarker = "ref:";
      }

      [Observation]
      public void should_write_the_marker_in_front_of_the_id_of_the_referenced_object()
      {
         sut.ReferenceFor(_parameter, _serializationContext).ShouldBeEqualTo("ref:P1");
      }

      [Observation]
      public void should_write_the_marker_in_front_of_the_id_of_a_referenced_formula()
      {
         sut.ReferenceFor(_formula, _serializationContext).ShouldBeEqualTo("ref:F1");
      }

      [Observation]
      public void should_not_write_the_marker_for_an_object_without_id_or_for_null()
      {
         sut.ReferenceFor(null, _serializationContext).ShouldBeEqualTo(string.Empty);
         sut.ReferenceFor("not an object with id", _serializationContext).ShouldBeEqualTo(string.Empty);
      }
   }
}
