using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;
using OSPSuite.SimModel;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   internal abstract class concern_for_PopulationDataSplitterSpecs : ContextForIntegration<PopulationDataSplitter>
   {
      protected DataTable _agingData;
      protected DataTable _populationData;
      protected DataTable _initialValuesData;
      protected const int _numberOfCores = 2;
      protected string _nonAgingPath = new[] {ConstantsForSpecs.Organism, ConstantsForSpecs.BW}.ToPathString();
      private IModelCoreSimulation _simulation;
      protected IObjectPathFactory _objectPathFactory;
      protected Simulation _simModelSimulation;
      protected IReadOnlyList<ParameterProperties> _variableParameters;
      protected IReadOnlyList<SpeciesProperties> _variableSpecies;
      protected SimModelManagerForSpecs _simModelManagerForSpecs;

      public override void GlobalContext()
      {
         base.GlobalContext();

         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         var simModelExporter = IoC.Resolve<ISimModelExporter>();
         var simModelSimulationFactory = IoC.Resolve<ISimModelSimulationFactory>();
         _simModelManagerForSpecs = new SimModelManagerForSpecs(simModelExporter, simModelSimulationFactory);
         _simModelSimulation = _simModelManagerForSpecs.CreateSimulation(_simulation);
         _populationData = createPopTableParameters();
         _agingData = createPopAgingParameters();
         _initialValuesData = createPopInitialValues();
         sut = new PopulationDataSplitter(_populationData, _agingData, _initialValuesData, _numberOfCores);

         _variableParameters = _simModelManagerForSpecs.SetVariableParameters(_simModelSimulation, sut.ParameterPathsToBeVaried());
         _variableSpecies = _simModelManagerForSpecs.SetVariableSpecies(_simModelSimulation, sut.InitialValuesPathsToBeVaried());
      }

      private DataTable createPopInitialValues()
      {
         var dt = new DataTable("InitialValues");

         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);

         var path = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.ArterialBlood,
            ConstantsForSpecs.Plasma,
            "A").PathAsString;
         dt.Columns.Add(path);

         path = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.VenousBlood,
            ConstantsForSpecs.Plasma,
            "A").PathAsString;
         dt.Columns.Add(path);


         dt.Rows.Add(0, 1.1, 2.2);
         dt.Rows.Add(1, 3.3, 4.4);
         dt.Rows.Add(2, 5.5, 6.6);

         return dt;
      }

      private DataTable createPopAgingParameters()
      {
         var dt = new DataTable("AgingParams");

         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
         dt.Columns.Add(Constants.Population.PARAMETER_PATH_COLUMN);
         dt.Columns.Add(Constants.Population.TIME_COLUMN);
         dt.Columns.Add(Constants.Population.VALUE_COLUMN);

         var path1 = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.TableParameter1).PathAsString;

         var path2 = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.TableParameter2).PathAsString;

         int individualId = 0;
         dt.Rows.Add(individualId, path1, 10, 0.1);
         dt.Rows.Add(individualId, path1, 110, 0.11);
         dt.Rows.Add(individualId, path1, 1110, 0.111);

         dt.Rows.Add(individualId, path2, 20, 0.2);
         dt.Rows.Add(individualId, path2, 220, 0.220);

         individualId = 1;
         dt.Rows.Add(individualId, path1, 11, 0.11);
         dt.Rows.Add(individualId, path1, 111, 0.111);

         dt.Rows.Add(individualId, path2, 21, 0.21);
         dt.Rows.Add(individualId, path2, 221, 0.221);

         individualId = 2;
         dt.Rows.Add(individualId, path1, 12, 0.12);
         dt.Rows.Add(individualId, path1, 112, 0.112);

         dt.Rows.Add(individualId, path2, 22, 0.22);
         dt.Rows.Add(individualId, path2, 222, 0.222);

         return dt;
      }

      private DataTable createPopTableParameters()
      {
         var dt = new DataTable("PopParams");

         var path = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.BW).PathAsString;
         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
         dt.Columns.Add(path);

         //test model has event, which sets "Organism|ArterialBlood|Plasma|A" += 10 at t=StartTime
         path = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.ArterialBlood,
            ConstantsForSpecs.Plasma,
            ConstantsForSpecs.BolusApplication,
            ConstantsForSpecs.Application,
            ConstantsForSpecs.StartTime).PathAsString;
         dt.Columns.Add(path);

         dt.Rows.Add(0, 24.0, 10);
         dt.Rows.Add(1, 25.0, 100);
         dt.Rows.Add(2, 26.0, 1000);

         return dt;
      }
   }

   internal class When_filling_parameters_and_initial_values_for_the_first_individual : concern_for_PopulationDataSplitterSpecs
   {
      protected override void Because()
      {
         sut.UpdateParametersAndInitialValuesForIndividual(0, _variableParameters, _variableSpecies);
      }

      [Observation]
      public void should_fill_correct_value_for_variable_initial_values()
      {
         _variableSpecies.Count.ShouldBeEqualTo(2);

         _variableSpecies[0].InitialValue.ShouldBeEqualTo(1.1);
         _variableSpecies[1].InitialValue.ShouldBeEqualTo(2.2);
      }

      [Observation]
      public void should_fill_correct_value_for_nonTableParameters()
      {
         var nonTableParameters = _variableParameters.Where(x => !x.TablePoints.Any()).ToList();
         nonTableParameters.Count.ShouldBeEqualTo(2);

         nonTableParameters[0].Value.ShouldBeEqualTo(24);
         nonTableParameters[1].Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_fill_correct_value_for_nonTableParameters_in_case_of_corresponding_parameter_in_basis_individual_is_table()
      {
         var nonAgingPath  = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.BW).PathAsString;
         var nonTableParameters = _variableParameters.Where(x => x.Path.Equals(nonAgingPath)).ToList();
         nonTableParameters.Count.ShouldBeEqualTo(1);

         nonTableParameters[0].TablePoints.Count().ShouldBeEqualTo(0);
         nonTableParameters[0].Value.ShouldBeEqualTo(24);
      }

      [Observation]
      public void should_fill_correct_values_for_table_parameters()
      {
         var agingPath1 = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.TableParameter1).PathAsString;

         var agingPath2 = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,  ConstantsForSpecs.TableParameter2).PathAsString;


         var tableParameter1 = _variableParameters.First(x => x.Path.Equals(agingPath1));
         var tablePoints1 = tableParameter1.TablePoints;

         tablePoints1.Count().ShouldBeEqualTo(3);
         tablePoints1.ElementAt(0).X.ShouldBeEqualTo(10);
         tablePoints1.ElementAt(0).Y.ShouldBeEqualTo(0.1);
         tablePoints1.ElementAt(1).X.ShouldBeEqualTo(110);
         tablePoints1.ElementAt(1).Y.ShouldBeEqualTo(0.110);

         var tableParameter2 = _variableParameters.First(x =>x.Path.Equals(agingPath2));
         var tablePoints2 = tableParameter2.TablePoints;

         tablePoints2.Count().ShouldBeEqualTo(2);
         tablePoints2.ElementAt(0).X.ShouldBeEqualTo(20);
         tablePoints2.ElementAt(0).Y.ShouldBeEqualTo(0.20);
         tablePoints2.ElementAt(1).X.ShouldBeEqualTo(220);
         tablePoints2.ElementAt(1).Y.ShouldBeEqualTo(0.22);

      }
   }

   internal class When_filling_initial_values_from_empty_table : concern_for_PopulationDataSplitterSpecs
   {
      protected override void Context()
      {
         base.Context();

         _initialValuesData = createEmptyInitialValuesData();
         sut = new PopulationDataSplitter(_populationData, _agingData, _initialValuesData, _numberOfCores);

         _variableParameters = _simModelManagerForSpecs.SetVariableParameters(_simModelSimulation, sut.ParameterPathsToBeVaried());
         _variableSpecies = _simModelManagerForSpecs.SetVariableSpecies(_simModelSimulation, sut.InitialValuesPathsToBeVaried());
      }

      protected override void Because()
      {
         sut.UpdateParametersAndInitialValuesForIndividual(0, new List<ParameterProperties>(), _variableSpecies);
      }

      private DataTable createEmptyInitialValuesData()
      {
         var dt = new DataTable("InitialValues");
         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
         dt.Columns.Add("Trululu");
         return dt;
      }

      [Observation]
      public void should_not_crash_with_empty_table()
      {
      }
   }

   internal class When_getting_Individual_id_s_for_first_core : concern_for_PopulationDataSplitterSpecs
   {
      private IEnumerable<int> _result;

      protected override void Because()
      {
         _result = sut.GetIndividualIdsFor(0);
      }

      [Observation]
      public void should_return_the_First_IDs()
      {
         _result.ShouldOnlyContain(0, 1);
      }
   }

   internal class When_getting_Individual_id_s_for_last_core : concern_for_PopulationDataSplitterSpecs
   {
      private IEnumerable<int> _result;

      protected override void Because()
      {
         _result = sut.GetIndividualIdsFor(1);
      }

      [Observation]
      public void should_return_the_Last_IDs()
      {
         _result.ShouldOnlyContain(2);
      }
   }
}