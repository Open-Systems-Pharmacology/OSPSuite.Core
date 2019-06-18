using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimModelNET;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Engine.Domain;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_PopulationRunner : ContextForIntegration<IPopulationRunner>
   {
      private ISimModelExporter _simModelExporter;
      private IWithIdRepository _withIdRepository;
      protected IModelCoreSimulation _simulation;
      protected IObjectPathFactory _objectPathFactory;
      protected DataTable _populationData;
      protected DataTable _agingData;
      protected DataTable _initialValuesData;
      protected PopulationRunResults _results;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simModelExporter = IoC.Resolve<ISimModelExporter>();
         _withIdRepository = IoC.Resolve<IWithIdRepository>();
         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();

         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         new RegisterTaskForSpecs(_withIdRepository).RegisterAllIn(_simulation.Model.Root);
         var schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.SimModel.xsd");
         XMLSchemaCache.InitializeFromFile(schemaPath);

         _populationData = createPopTableParameters();
         _agingData = createPopAgingParameters();
         _initialValuesData = createPopInitialValues();

         sut = new PopulationRunner(_simModelExporter, new SimModelSimulationFactory(), _objectPathFactory);

         _results = sut.RunPopulationAsync(_simulation, _populationData, _agingData).Result;
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

         return dt;
      }

      private DataTable createPopAgingParameters()
      {
         var dt = new DataTable("AgingParams");

         dt.Columns.Add(Constants.Population.INDIVIDUAL_ID_COLUMN);
         dt.Columns.Add(Constants.Population.PARAMETER_PATH_COLUMN);
         dt.Columns.Add(Constants.Population.TIME_COLUMN);
         dt.Columns.Add(Constants.Population.VALUE_COLUMN);

         var path = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.TableParameter1).PathAsString;

         var path2 = _objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism,
            ConstantsForSpecs.TableParameter2).PathAsString;

         int individualId = 0;
         dt.Rows.Add(individualId, path, 0, 1000);
         dt.Rows.Add(individualId, path, 10, 1000);
         dt.Rows.Add(individualId, path, 100, 0);

         dt.Rows.Add(individualId, path2, 0, 0);
         dt.Rows.Add(individualId, path2, 100, 1);

         individualId = 1;
         dt.Rows.Add(individualId, path, 0, 10);
         dt.Rows.Add(individualId, path, 10, 10);

         dt.Rows.Add(individualId, path2, 0, 0);
         dt.Rows.Add(individualId, path2, 100, 1);

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

         dt.Rows.Add(0, 24.0, 0);
         dt.Rows.Add(1, 25.0, 100);

         return dt;
      }
   }

   public class When_running_population : concern_for_PopulationRunner
   {
      protected float[] arterialPlasmaValuesFor(IndividualResults individualResults)
      {
         var path = _objectPathFactory.CreateObjectPathFrom(_simulation.Model.Root.Name,
            ConstantsForSpecs.Organism,
            ConstantsForSpecs.ArterialBlood,
            ConstantsForSpecs.Plasma, "A").PathAsString;
         return individualResults.AllValues.First(v => v.QuantityPath.Equals(path)).Values;
      }

      protected float[] venousPlasmaValuesFor(IndividualResults individualResults)
      {
         var path = _objectPathFactory.CreateObjectPathFrom(_simulation.Model.Root.Name,
            ConstantsForSpecs.Organism,
            ConstantsForSpecs.VenousBlood,
            ConstantsForSpecs.Plasma, "A").PathAsString;
         return individualResults.AllValues.First(v => v.QuantityPath.Equals(path)).Values;
      }

      [Observation]
      public void should_have_updated_the_time_reference_on_all_individuals()
      {
         foreach (var result in _results.Results)
         {
            result.Time.ShouldNotBeNull();
            result.Time.Length.ShouldBeGreaterThan(0);
         }
      }

      [Observation]
      public void should_have_updated_the_time_reference_on_all_results_as_a_reference_to_the_time_on_the_individual_results()
      {
         foreach (var result in _results.Results)
         {
            foreach (var value in result)
            {
               value.Time.ShouldBeEqualTo(result.Time);
            }
         }
      }

      [Observation]
      public void should_have_reordered_the_results_by_individual_ids()
      {
         int i = 0;
         foreach (var result in _results.Results)
         {
            result.IndividualId.ShouldBeEqualTo(i);
            i++;
         }
      }

      [Observation]
      public void should_return_results_for_all_individuals()
      {
         _results.Results.Count().ShouldBeEqualTo(_populationData.Rows.Count);
      }

      [Observation]
      public void should_change_value_of_arterial_blood_plasma_A_at_t_0_in_individual_1()
      {
         var simResults = _results.Results;

         var arterialBloodPlasmaValuesSim1 = arterialPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(0));
         var arterialBloodPlasmaValuesSim2 = arterialPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(1));

         arterialBloodPlasmaValuesSim1[1].ShouldBeGreaterThan(arterialBloodPlasmaValuesSim2[0] + 1);
      }

      [Observation]
      public async Task should_set_correct_initial_values_for_arterial_blood_plasma_A_and_venous_blood_plasma_A()
      {
         var results = await sut.RunPopulationAsync(_simulation, _populationData, _agingData, _initialValuesData);
         var simResults = results.Results;

         var arterialBloodPlasmaValuesSim1 = arterialPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(0));
         var arterialBloodPlasmaValuesSim2 = arterialPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(1));

         arterialBloodPlasmaValuesSim1[0].ShouldBeEqualTo(1.1f);
         arterialBloodPlasmaValuesSim2[0].ShouldBeEqualTo(3.3f);

         var venousBloodPlasmaValuesSim1 = venousPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(0));
         var venousBloodPlasmaValuesSim2 = venousPlasmaValuesFor(simResults.AllIndividualResults.ElementAt(1));

         venousBloodPlasmaValuesSim1[0].ShouldBeEqualTo(2.2f);
         venousBloodPlasmaValuesSim2[0].ShouldBeEqualTo(4.4f);
      }
   }
}