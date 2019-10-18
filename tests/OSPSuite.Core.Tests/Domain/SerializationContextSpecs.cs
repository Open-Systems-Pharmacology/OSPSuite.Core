using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SerializationContext : ContextSpecification<SerializationContext>
   {
      protected ExplicitFormula _formula1;
      protected ExplicitFormula _formula2;
      private IDimensionFactory _dimensionFactory;
      private IObjectBaseFactory _objectBaseFactory;
      private IWithIdRepository _withIdRepository;
      private IEnumerable<DataRepository> _dataRepositories;
      private ICloneManagerForModel _cloneManagerForModel;
      protected Utility.Container.IContainer _container;

      protected override void Context()
      {
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _withIdRepository = A.Fake<IWithIdRepository>();
         _dataRepositories = new List<DataRepository>();
         _cloneManagerForModel = A.Fake<ICloneManagerForModel>();
         _container = A.Fake<Utility.Container.IContainer>();

         sut = new SerializationContext(_dimensionFactory, _objectBaseFactory, _withIdRepository, _dataRepositories, _cloneManagerForModel, _container);
         _formula1 = new ExplicitFormula().WithId("1");
         _formula2 = new ExplicitFormula().WithId("1");
      }
   }

   public class When_adding_two_formulas_with_the_same_id_in_the_cache : concern_for_SerializationContext
   {
      protected override void Because()
      {
         sut.AddFormulaToCache(_formula1);
         sut.AddFormulaToCache(_formula2);
      }

      [Observation]
      public void the_cache_should_only_contain_one_formula()
      {
         sut.Formulas.ShouldOnlyContain(_formula1);
      }
   }

   public class When_adding_two_formulas_with_the_different_id_in_the_cache_but_sharing_the_same_origin_id : concern_for_SerializationContext
   {
      private string _resultingId;

      protected override void Context()
      {
         base.Context();
         _formula1.WithOriginId("origin");
         _formula2.WithId("2").WithOriginId("origin");
      }

      protected override void Because()
      {
         sut.AddFormulaToCache(_formula1);
         _resultingId = sut.AddFormulaToCache(_formula2);
      }

      [Observation]
      public void the_cache_should_only_contain_one_formula()
      {
         sut.Formulas.ShouldOnlyContain(_formula1);
      }

      [Observation]
      public void the_resulting_id_for_the_second_formula_should_be_the_one_from_the_first_formula_having_the_same_origin()
      {
         _resultingId.ShouldBeEqualTo(_formula1.Id);
      }
   }

   public class When_adding_the_results_of_a_simulation_that_was_not_simulated : concern_for_SerializationContext
   {
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IModelCoreSimulation>();
         _simulation.Results = null;
      }

      protected override void Because()
      {
         sut.AddRepository(_simulation.Results);
      }

      [Observation]
      public void should_not_add_the_results()
      {
         sut.Repositories.ShouldBeEmpty();
      }
   }

   public class When_resolving_an_entity_during_deserialization : concern_for_SerializationContext
   {
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _container.Resolve<IModelCoreSimulation>()).Returns(_simulation);
      }

      [Observation]
      public void should_leverage_the_container_to_return_the_underlying_entity()
      {
         sut.Resolve<IModelCoreSimulation>().ShouldBeEqualTo(_simulation);
      }
   }
}