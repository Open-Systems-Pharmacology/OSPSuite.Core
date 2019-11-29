using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IModelCoreSimulation : IObjectBase, IWithCreationMetaData, IWithModel
   {
      /// <summary>
      ///    Build configuration used to create the simulation. May be null
      /// </summary>
      IBuildConfiguration BuildConfiguration { get; }

      OutputSelections OutputSelections { get; }

      /// <summary>
      ///    Returns the end time of the simulation in kernel unit
      /// </summary>
      double? EndTime { get; }

      ISimulationSettings SimulationSettings { get; }

      /// <summary>
      ///    The reaction building block used to create the simulation. This is only use as meta information
      ///    on model creation for now. Adding <see cref="Reaction" /> to the building block will not change the model structure
      /// </summary>
      IReactionBuildingBlock Reactions { get; }

      /// <summary>
      ///    Name of all compounds used in the simulation
      /// </summary>
      IReadOnlyList<string> CompoundNames { get; }

      IEnumerable<T> All<T>() where T : class, IEntity;
   }

   public class ModelCoreSimulation : ObjectBase, IModelCoreSimulation
   {
      public IModel Model { get; set; }

      public IBuildConfiguration BuildConfiguration { get; set; }

      public double? EndTime => SimulationSettings?.OutputSchema?.EndTime;

      public OutputSelections OutputSelections => SimulationSettings?.OutputSelections;

      public ISimulationSettings SimulationSettings => BuildConfiguration?.SimulationSettings;

      public IReactionBuildingBlock Reactions => BuildConfiguration?.Reactions;

      public IReadOnlyList<string> CompoundNames => BuildConfiguration?.AllPresentMolecules().AllNames();

      public IEnumerable<T> All<T>() where T : class, IEntity => Model?.Root.GetAllChildren<T>().Union(allFromSettings<T>());

      private IEnumerable<TEntity> allFromSettings<TEntity>() where TEntity : class, IEntity
      {
         if (SimulationSettings?.OutputSchema == null || SimulationSettings?.Solver == null)
            return Enumerable.Empty<TEntity>();

         return SimulationSettings.OutputSchema.GetAllChildren<TEntity>()
            .Union(SimulationSettings.Solver.GetAllChildren<TEntity>());
      }

      public CreationMetaData Creation { get; set; }

      public ModelCoreSimulation()
      {
         Creation = new CreationMetaData();
         Icon = IconNames.SIMULATION;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);

         Model?.AcceptVisitor(visitor);

         BuildConfiguration?.AcceptVisitor(visitor);
      }
   }
}