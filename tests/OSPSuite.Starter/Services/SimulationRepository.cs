using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Starter.Domain;

namespace OSPSuite.Starter.Services
{
   public class SimulationRepository : ISimulationRepository
   {
      private readonly IModelConstructor _modelConstructor;
      private readonly ModelHelperForSpecs _modelHelper;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IProjectRetriever _projectRetriever;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly TestSimulation _simulation;

      public SimulationRepository(IModelConstructor modelConstructor, ModelHelperForSpecs modelHelper, IEntityPathResolver entityPathResolver, IProjectRetriever projectRetriever, IDimensionFactory dimensionFactory)
      {
         _modelConstructor = modelConstructor;
         _modelHelper = modelHelper;
         _entityPathResolver = entityPathResolver;
         _projectRetriever = projectRetriever;
         _dimensionFactory = dimensionFactory;
         _simulation = createSimulation();
      }

      private TestSimulation createSimulation()
      {
         var buildConfiguration = _modelHelper.CreateBuildConfiguration();
         var result = _modelConstructor.CreateModelFrom(buildConfiguration, "MyModel");
         var simulation = new TestSimulation
         {
            BuildConfiguration = buildConfiguration,
            Model = result.Model,
            Name = "Test"
         };

         var allPersistable = simulation.All<IQuantity>().Where(x => x.Persistable);
         var first = allPersistable.First(x=>x.Dimension.Name==Constants.Dimension.AMOUNT);
         simulation.OutputSelections.AddOutput(new QuantitySelection(_entityPathResolver.PathFor(first), QuantityType.Molecule));

         _projectRetriever.CurrentProject.AddObservedData(observedDataFor(first));
         return simulation;
      }

      private DataRepository observedDataFor(IQuantity quantity)
      {
         var time = new BaseGrid("Time", _dimensionFactory.GetDimension(Constants.Dimension.TIME))
         {
            Values = new[] {1f, 2f, 3f}
         };

         var values = new DataColumn(quantity.Name, quantity.Dimension, time)
         {
            Values = new[] {10f, 20f, 30f},
            DataInfo = {Origin = ColumnOrigins.Observation},
         };


         var obsData = new DataRepository{Name = "ObsData" };
         obsData.Add(values);
         return obsData;
      }

      public IEnumerable<ISimulation> All()
      {
         return new[] {_simulation};
      }
   }
}
