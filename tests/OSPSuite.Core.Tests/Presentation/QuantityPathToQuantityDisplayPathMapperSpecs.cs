using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_QuantityPathToQuantityDisplayPathMapper : ContextSpecification<IQuantityPathToQuantityDisplayPathMapper>
   {
      protected IObjectPathFactory _objectPathFactory;
      protected IPathToPathElementsMapper _pathToPathElementMapper;
      protected IDataColumnToPathElementsMapper _dataColumnToPathElementsMapper;

      protected override void Context()
      {
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _pathToPathElementMapper = A.Fake<IPathToPathElementsMapper>();
         _dataColumnToPathElementsMapper = A.Fake<IDataColumnToPathElementsMapper>();

         sut = new QuantityPathToQuantityDisplayPathMapper(_objectPathFactory, _pathToPathElementMapper, _dataColumnToPathElementsMapper);
      }
   }

   public abstract class When_retrieving_the_display_path_for_a_column : concern_for_QuantityPathToQuantityDisplayPathMapper
   {
      protected ISimulation _simulation;
      protected DataColumn _column;
      protected DataRepository _repository;
      protected string _displayPath;
      protected PathElements _pathElements;

      protected override void Context()
      {
         base.Context();
         _pathElements = new PathElements();
         _simulation = A.Fake<ISimulation>().WithName("Sim");
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _column = new DataColumn("A", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid);
         _repository = new DataRepository().WithName("Rep");
         A.CallTo(() => _dataColumnToPathElementsMapper.MapFrom(_column, A<IContainer>._)).Returns(_pathElements);

         _pathElements[PathElement.Simulation] = new PathElementDTO { DisplayName = "Sim" };
         _pathElements[PathElement.Container] = new PathElementDTO { DisplayName = "Cont" };
         _pathElements[PathElement.Molecule] = new PathElementDTO { DisplayName = "Drug" };
      }
   }

   public class When_retrieving_the_display_path_of_a_column_for_a_given_simulation_with_simulation_name_provided : When_retrieving_the_display_path_for_a_column
   {
      protected override void Because()
      {
         _displayPath = sut.DisplayPathAsStringFor(_simulation, _column, "SIM_NAME");
      }  

      [Observation]
      public void should_return_the_expected_display_path()
      {
         _displayPath.ShouldBeEqualTo("SIM_NAME-Drug-Cont");
      }
   }

   public class When_retrieving_the_display_path_of_a_column_for_a_given_simulation_with_simulation_name_not_provided : When_retrieving_the_display_path_for_a_column
   {
      protected override void Because()
      {
         _displayPath = sut.DisplayPathAsStringFor(_simulation, _column, "");
      }

      [Observation]
      public void should_return_the_expected_display_path()
      {
         _displayPath.ShouldBeEqualTo("Drug-Cont");
      }
   }


   public class When_retrieving_the_display_path_of_a_column_for_a_given_simulation_with_simulation_name_required : When_retrieving_the_display_path_for_a_column
   {
      protected override void Because()
      {
         _displayPath = sut.DisplayPathAsStringFor(_simulation, _column, addSimulationName: true);
      }

      [Observation]
      public void should_return_the_expected_display_path()
      {
         _displayPath.ShouldBeEqualTo("Sim-Drug-Cont");
      }
   }
}