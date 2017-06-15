using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public interface ISimulation : IObjectBase, ILazyLoadable, IAnalysable, IWithCreationMetaData, IWithModel, IUsesObservedData
   {
      IEnumerable<CurveChart> Charts { get; }

      OutputSelections OutputSelections { get; }

      ISimulationSettings SimulationSettings { get; }

      /// <summary>
      /// Name of all compounds used in the simulation
      /// </summary>
      IReadOnlyList<string> CompoundNames { get; }

      IEnumerable<T> All<T>() where T : class, IEntity;

      /// <summary>
      ///    Returns the endtime of the simulation in kernel unit
      /// </summary>
      double? EndTime { get; }

      /// <summary>
      ///   Returns the total drug mass per body weight for the given <paramref name="compoundName"/> in [umol/kg BW or null if not found
      /// </summary>
      double? TotalDrugMassPerBodyWeightFor(string compoundName);

      /// <summary>
      ///    The reaction building block used to create the simulation. This is only use as meta information
      ///    on model creation for now. Adding <see cref="Reaction" /> to the building block will not change the model structure
      /// </summary>
      IReactionBuildingBlock Reactions { get; set; }
   }
}