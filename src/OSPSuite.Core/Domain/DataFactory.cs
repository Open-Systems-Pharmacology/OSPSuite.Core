//TODO SIMMODEL


//using System.Collections.Generic;
//using OSPSuite.Core.Domain.Data;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Domain.UnitSystem;
//using OSPSuite.Core.Extensions;
//using OSPSuite.Core.Services;
//using OSPSuite.Utility.Collections;
//
//namespace OSPSuite.Core.Domain
//{
//   /// <summary>
//   ///    The Contract for a Factory creating a <see cref="DataRepository" /> from the results using SimModel
//   /// </summary>
//   public interface IDataFactory
//   {
//      /// <summary>
//      ///    Map the results from the simmodel simulation and the core simulation and returns a new data repository
//      /// </summary>
//      /// <param name="simulation">the core simulation for which results should be retrieved</param>
//      /// <param name="simModelSimulation">the simmodel simulation containing the results</param>
//      /// <param name="repositoryName">
//      ///    The optional name for the repository. If not provided, the standard name retrieved from
//      ///    the <see cref="IDataNamingService" /> will be used
//      /// </param>
//      /// <returns></returns>
//      DataRepository CreateRepository(IModelCoreSimulation simulation, SimModelNET.ISimulation simModelSimulation, string repositoryName = null);
//
//   }
//
//   /// <summary>
//   ///    An basic Implementation of <see cref="IDataFactory" /> using an
//   ///    <see cref="IDataNamingService" /> to create display information at the
//   ///    columns, an <see cref="IObjectBaseFactory" /> to create mapping
//   ///    Information between model<see cref="IEntity" />s and the
//   ///    <see cref="DataRepository" /> and a <see cref="IDimensionFactory" /> to get
//   ///    a Time <see cref="IDimension" /> for x values and to retrieve default
//   ///    units for all y values
//   /// </summary>
//   public class DataFactory : IDataFactory
//   {
//      private readonly IDataNamingService _dataNamingService;
//      private readonly IObjectPathFactory _objectPathFactory;
//      private readonly IDisplayUnitRetriever _displayUnitRetriever;
//      private readonly IDataRepositoryTask _dataRepositoryTask;
//      private readonly IDimensionFactory _dimensionFactory;
//
//      public DataFactory(
//         IDimensionFactory dimensionFactory, 
//         IDataNamingService dataNamingService, 
//         IObjectPathFactory objectPathFactory, 
//         IDisplayUnitRetriever displayUnitRetriever, 
//         IDataRepositoryTask dataRepositoryTask)
//      {
//         _dimensionFactory = dimensionFactory;
//         _dataNamingService = dataNamingService;
//         _objectPathFactory = objectPathFactory;
//         _displayUnitRetriever = displayUnitRetriever;
//         _dataRepositoryTask = dataRepositoryTask;
//      }
//
//      public DataRepository CreateRepository(IModelCoreSimulation simulation, SimModelNET.ISimulation simModelSimulation, string repositoryName = null)
//      {
//         var repository = new DataRepository().WithName(repositoryName ?? _dataNamingService.GetNewRepositoryName());
//         var allPersitableQuantities = new Cache<string, IQuantity>(q => _objectPathFactory.CreateAbsoluteObjectPath(q).ToString(), x => null);
//         allPersitableQuantities.AddRange(simulation.Model.Root.GetAllChildren<IQuantity>(x => x.Persistable));
//
//         var time = createTimeGrid(simModelSimulation.SimulationTimes);
//         foreach (var quantityValue in simModelSimulation.AllValues)
//         {
//            var quantity = allPersitableQuantities[quantityValue.Path];
//
//            if (quantity == null) continue;
//
//            repository.Add(createColumn(time, quantity, quantityValue, quantityValue.Path.ToPathArray(), simulation));
//         }
//
//         return repository;
//      }
//
//      private DataColumn createColumn(BaseGrid xValues, IQuantity quantity, IValues yValues, IEnumerable<string> quantityPath, IModelCoreSimulation simulation)
//      {
//         var column = new DataColumn(_dataNamingService.GetEntityName(yValues.EntityId), quantity.Dimension, xValues)
//         {
//            DataInfo =
//            {
//               Origin = ColumnOrigins.Calculation,
//               ComparisonThreshold = (float) yValues.ComparisonThreshold
//            },
//            Values = new List<float>(yValues.Values.ToFloatArray()),
//            QuantityInfo = new QuantityInfo(yValues.Name, quantityPath, quantity.QuantityType),
//            DisplayUnit = _displayUnitRetriever.PreferredUnitFor(quantity)
//         };
//
//         _dataRepositoryTask.UpdateMolWeight(column, quantity, simulation.Model);
//
//         return column;
//      }
//      
//
//      /// <summary>
//      ///    Creates the time grid as <see cref="BaseGrid" /> from given values. <see cref="IDimension" /> is taken from
//      ///    <see
//      ///       cref="IDimensionFactory" />
//      ///    given in constructor
//      /// </summary>
//      /// <param name="xValues">The x values as double array</param>
//      /// <returns>
//      ///    <see cref="BaseGrid" /> used for the whole <see cref="DataRepository" />
//      /// </returns>
//      private BaseGrid createTimeGrid(double[] xValues)
//      {
//         return new BaseGrid(_dataNamingService.GetTimeName(), _dimensionFactory.Dimension(Constants.Dimension.TIME))
//         {
//            QuantityInfo = new QuantityInfo(Constants.TIME, new List<string> {Constants.TIME}, QuantityType.BaseGrid),
//            Values = new List<float>(xValues.ToFloatArray())
//         };
//      }
//   }
//}