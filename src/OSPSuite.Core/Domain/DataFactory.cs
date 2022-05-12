using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.SimModel;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    The Contract for a Factory creating a <see cref="DataRepository" /> from the results using SimModel
   /// </summary>
   public interface IDataFactory
   {
      /// <summary>
      ///    Map the results from the simmodel simulation and the core simulation and returns a new data repository
      /// </summary>
      /// <param name="simulation">the core simulation for which results should be retrieved</param>
      /// <param name="simModelSimulation">the simmodel simulation containing the results</param>
      /// <param name="repositoryName">
      ///    The optional name for the repository. If not provided, the standard name will be used
      /// </param>
      /// <returns></returns>
      DataRepository CreateRepository(IModelCoreSimulation simulation, Simulation simModelSimulation, string repositoryName = null);
   }

   public class DataFactory : IDataFactory
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly IDataRepositoryTask _dataRepositoryTask;
      private readonly IDimensionFactory _dimensionFactory;

      public DataFactory(
         IDimensionFactory dimensionFactory,
         IObjectPathFactory objectPathFactory,
         IDisplayUnitRetriever displayUnitRetriever,
         IDataRepositoryTask dataRepositoryTask)
      {
         _dimensionFactory = dimensionFactory;
         _objectPathFactory = objectPathFactory;
         _displayUnitRetriever = displayUnitRetriever;
         _dataRepositoryTask = dataRepositoryTask;
      }

      public DataRepository CreateRepository(IModelCoreSimulation simulation, Simulation simModelSimulation, string repositoryName = Constants.DEFAULT_SIMULATION_RESULTS_NAME)
      {
         var repository = new DataRepository().WithName(repositoryName);
         var allPersitableQuantities = new Cache<string, IQuantity>(q => _objectPathFactory.CreateAbsoluteObjectPath(q).ToString(), x => null);
         allPersitableQuantities.AddRange(simulation.Model.Root.GetAllChildren<IQuantity>(x => x.Persistable));

         var time = createTimeGrid(simModelSimulation.SimulationTimes);
         foreach (var quantityValue in simModelSimulation.AllValues)
         {
            var quantity = allPersitableQuantities[quantityValue.Path];

            if (quantity == null) continue;

            repository.Add(createColumn(time, quantity, quantityValue, quantityValue.Path.ToPathArray(), simulation));
         }

         return repository;
      }

      private DataColumn createColumn(BaseGrid baseGrid, IQuantity quantity, VariableValues quantityValues, IEnumerable<string> quantityPath, IModelCoreSimulation simulation)
      {
         var column = new DataColumn(quantity.Name, quantity.Dimension, baseGrid)
         {
            DataInfo =
            {
               Origin = ColumnOrigins.Calculation,
               ComparisonThreshold = (float) quantityValues.ComparisonThreshold
            },
            Values = new List<float>(quantityValues.Values.ToFloatArray()),
            QuantityInfo = new QuantityInfo(quantityPath, quantity.QuantityType),
            DisplayUnit = _displayUnitRetriever.PreferredUnitFor(quantity)
         };

         _dataRepositoryTask.UpdateMolWeight(column, quantity, simulation.Model);

         return column;
      }

      /// <summary>
      ///    Creates the time grid as <see cref="BaseGrid" /> from given values. <see cref="IDimension" /> is taken from
      ///    <see
      ///       cref="IDimensionFactory" />
      ///    given in constructor
      /// </summary>
      /// <param name="values">The x values as double array</param>
      /// <returns>
      ///    <see cref="BaseGrid" /> used for the whole <see cref="DataRepository" />
      /// </returns>
      private BaseGrid createTimeGrid(double[] values)
      {
         return new BaseGrid(Constants.TIME, _dimensionFactory.Dimension(Constants.Dimension.TIME))
         {
            QuantityInfo = new QuantityInfo(new List<string> {Constants.TIME}, QuantityType.BaseGrid),
            Values = new List<float>(values.ToFloatArray())
         };
      }
   }
}