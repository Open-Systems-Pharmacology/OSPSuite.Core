using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IModelCoreSimulation : IObjectBase, IWithCreationMetaData, IWithModel
   {
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
   }

   public class ModelCoreSimulation : ObjectBase, IModelCoreSimulation
   {
      public IModel Model { get; set; }
      public IBuildConfiguration BuildConfiguration { get; set; }
      public virtual DataRepository Results { get; set; }

      public double? EndTime => SimulationSettings?.OutputSchema?.EndTime;

      public OutputSelections OutputSelections => SimulationSettings?.OutputSelections;

      public ISimulationSettings SimulationSettings => BuildConfiguration?.SimulationSettings;

      public IReactionBuildingBlock Reactions => BuildConfiguration?.Reactions;

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

         if (!Results.IsNull())
            Results.AcceptVisitor(visitor);
      }
   }
}