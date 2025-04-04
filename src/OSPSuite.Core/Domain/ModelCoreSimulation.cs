using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IModelCoreSimulation : IObjectBase, IWithCreationMetaData, IWithModel, IMolWeightFinder
   {
      /// <summary>
      ///    Simulation configuration used to create the simulation. May be null
      /// </summary>
      SimulationConfiguration Configuration { get; set; }

      OutputSelections OutputSelections { get; }

      /// <summary>
      ///    Returns the end time of the simulation in kernel unit
      /// </summary>
      double? EndTime { get; }

      SimulationSettings Settings { get; }

      /// <summary>
      ///    The reactions used to create the simulation. This is only use as meta information
      ///    on model creation for now. Adding <see cref="Reaction" /> to the building block will not change the model structure
      /// </summary>
      IReadOnlyList<ReactionBuildingBlock> Reactions { get; }

      /// <summary>
      ///    Name of all compounds used in the simulation
      /// </summary>
      IReadOnlyList<string> CompoundNames { get; }

      IEnumerable<T> All<T>() where T : class, IEntity;

      /// <summary>
      ///    Returns the Body weight <see cref="IParameter" /> if available in the model otherwise null.
      /// </summary>
      IParameter BodyWeight { get; }

      /// <summary>
      ///    Returns the Total drug mass parameter applied for a molecule <see cref="IParameter" /> if available in the model
      ///    otherwise null.
      /// </summary>
      IParameter TotalDrugMassFor(string moleculeName);

      /// <summary>
      ///    Tracks objects used to create the simulation. (may be null)
      /// </summary>
      EntitySources EntitySources { get; }
   }

   public class ModelCoreSimulation : ObjectBase, IModelCoreSimulation
   {
      public IModel Model { get; set; }

      public SimulationConfiguration Configuration { get; set; }

      public CreationMetaData Creation { get; set; }

      public EntitySources EntitySources { get; } = new EntitySources();

      public double? EndTime => Settings?.OutputSchema?.EndTime;

      public OutputSelections OutputSelections => Settings?.OutputSelections;

      public SimulationSettings Settings => Configuration?.SimulationSettings;

      public IReadOnlyList<ReactionBuildingBlock> Reactions => Configuration?.All<ReactionBuildingBlock>();

      public IReadOnlyList<string> CompoundNames => Model?.AllPresentMoleculeNames;

      public IEnumerable<T> All<T>() where T : class, IEntity => Model?.Root.GetAllChildren<T>().Union(allFromSettings<T>()) ?? Enumerable.Empty<T>();

      public IParameter BodyWeight => Model?.BodyWeight;

      public IParameter TotalDrugMassFor(string moleculeName) => Model?.TotalDrugMassFor(moleculeName);

      public double? MolWeightFor(IQuantity quantity) => Model?.MolWeightFor(quantity);

      public double? MolWeightFor(string quantityPath) => Model?.MolWeightFor(quantityPath);

      private IEnumerable<TEntity> allFromSettings<TEntity>() where TEntity : class, IEntity
      {
         if (Settings?.OutputSchema == null || Settings?.Solver == null)
            return Enumerable.Empty<TEntity>();

         return Settings.OutputSchema.GetAllChildren<TEntity>()
            .Union(Settings.Solver.GetAllChildren<TEntity>());
      }

      public ModelCoreSimulation()
      {
         Creation = new CreationMetaData();
         Icon = IconNames.SIMULATION;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);

         Settings?.AcceptVisitor(visitor);

         Model?.AcceptVisitor(visitor);

         Configuration?.AcceptVisitor(visitor);

         EntitySources?.AcceptVisitor(visitor);
      }
   }
}