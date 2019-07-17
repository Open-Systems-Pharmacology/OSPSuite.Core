using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ObjectIdResetter : ContextSpecification<IObjectIdResetter>
   {
      protected override void Context()
      {
         sut = new ObjectIdResetter(new IdGenerator());
      }
   }

   public class When_reseting_the_id_for_a_given_object : concern_for_ObjectIdResetter
   {
      private IContainer _container;
      private string _individualId;
      private IEntity _parameter;
      private string _parmeterId;
      private IFormula _constantFormula;
      private SpatialStructure _spStructure;

      protected override void Context()
      {
         base.Context();
         _individualId = "_oldIndividualId";
         _parmeterId = "_oldParmeterId";
         _constantFormula = new ExplicitFormula().WithId("TOTO");
         _container = new Container().WithId(_individualId);
         _spStructure = new SpatialStructure().WithId("STRUCT");
         _spStructure.AddTopContainer(_container);
         _spStructure.AddFormula(_constantFormula);
         _parameter = new Parameter().WithId(_parmeterId).WithFormula(_constantFormula);
         _container.Add(_parameter);
      }

      protected override void Because()
      {
         sut.ResetIdFor(_spStructure);
      }

      [Observation]
      public void should_reset_the_id_of_the_object_to_a_newly_generated_one()
      {
         _container.Id.ShouldNotBeEqualTo(_individualId);
      }

      [Observation]
      public void should_reset_the_id_of_all_children_if_the_object_is_a_container_object()
      {
         _parameter.Id.ShouldNotBeEqualTo(_parmeterId);
      }

      [Observation]
      public void should_reset_the_id_of_all_formulas_being_used_in_the_object()
      {
         _constantFormula.Id.ShouldNotBeEqualTo("TOTO");
      }

      [Observation]
      public void should_have_refreshed_the_formula_cache()
      {
         _spStructure.FormulaCache.Contains(_constantFormula).ShouldBeTrue();
      }
   }


   public class When_resetting_the_id_of_a_simple_with_id_object : concern_for_ObjectIdResetter
   {
      private IWithId _objectWithId;

      protected override void Context()
      {
         base.Context();
         _objectWithId = A.Fake<IWithId>();
         _objectWithId.Id = "ABC";
      }
      
      protected override void Because()
      {
         sut.ResetIdFor(_objectWithId);
      }

      [Observation]
      public void should_reset_the_id_as_expected()
      {
         _objectWithId.Id.ShouldNotBeEqualTo("ABC");
      }
   }


   public class When_resetting_the_id_of_a_data_repository : concern_for_ObjectIdResetter
   {
      private DataRepository _dataRepository;
      private DataColumn _col1;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         _baseGrid= new BaseGrid("BaseGrid", "Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _col1 = new DataColumn("Col", "Toto", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid);
         _dataRepository = new DataRepository("ObsData1") {_col1};
      }

      protected override void Because()
      {
         sut.ResetIdFor(_dataRepository);
      }

      [Observation]
      public void should_also_reset_the_id_of_all_columns()
      {
         _dataRepository.Id.ShouldNotBeEqualTo("ObsData1");
         _col1.Id.ShouldNotBeEqualTo("Col");
         _baseGrid.Id.ShouldNotBeEqualTo("BaseGrid");
         _dataRepository.GetColumn(_col1.Id).ShouldBeEqualTo(_col1);
         _dataRepository.GetColumn(_baseGrid.Id).ShouldBeEqualTo(_baseGrid);
      }
   }
}