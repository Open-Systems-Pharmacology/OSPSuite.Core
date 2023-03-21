using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    A set of Building Blocks used to create a Model
   /// </summary>
   public interface IBuildConfiguration : IVisitable<IVisitor>
   {
      MoleculeBuildingBlock Molecules { set; get; }
      IReactionBuildingBlock Reactions { set; get; }
      IPassiveTransportBuildingBlock PassiveTransports { set; get; }
      ISpatialStructure SpatialStructure { set; get; }
      MoleculeStartValuesBuildingBlock MoleculeStartValues { set; get; }
      ParameterStartValuesBuildingBlock ParameterStartValues { set; get; }
      IObserverBuildingBlock Observers { get; set; }
      IEventGroupBuildingBlock EventGroups { get; set; }
      SimulationSettings SimulationSettings { get; set; }

      /// <summary>
      ///    Returns the calculation that will be used to create the model structure
      /// </summary>
      IReadOnlyList<ICoreCalculationMethod> AllCalculationMethods();

      /// <summary>
      ///    Add a calculation method to the list of used calculation method for the given configuration
      /// </summary>
      void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd);

      /// <summary>
      ///    If the flag is true, the validation will be performed in the model construction. Otherwise, validation will be
      ///    skipped.
      ///    Default is <c>true</c>
      /// </summary>
      bool ShouldValidate { get; set; }

      /// <summary>
      ///    Set to true, the progress events will be triggered while creating the model. Default is true
      /// </summary>
      bool ShowProgress { get; set; }

      /// <summary>
      ///    Set to <c>true</c> (default), the circular reference check will be performed when creating a simulation. This
      ///    should only be set to <c>false</c> for very massive model.
      /// </summary>
      bool PerformCircularReferenceCheck { get; set; }

      /// <summary>
      ///    Returns the building block defined in the configuration
      /// </summary>
      IEnumerable<IBuildingBlock> AllBuildingBlocks { get; }
   }

   /// <summary>
   ///    A set of Building Blocks used to create a Model
   /// </summary>
   public class BuildConfiguration : IBuildConfiguration
   {
      private readonly List<ICoreCalculationMethod> _allCalculationMethods;
      public virtual ISpatialStructure SpatialStructure { get; set; }
      public virtual MoleculeStartValuesBuildingBlock MoleculeStartValues { get; set; }
      public virtual ParameterStartValuesBuildingBlock ParameterStartValues { get; set; }
      public virtual IObserverBuildingBlock Observers { get; set; }
      public virtual MoleculeBuildingBlock Molecules { get; set; }
      public virtual IReactionBuildingBlock Reactions { get; set; }
      public virtual IPassiveTransportBuildingBlock PassiveTransports { get; set; }
      public virtual IEventGroupBuildingBlock EventGroups { get; set; }
      public virtual SimulationSettings SimulationSettings { get; set; }

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;

      public bool ShouldValidate { get; set; } = true;
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;

      public BuildConfiguration()
      {
         _allCalculationMethods = new List<ICoreCalculationMethod>();
      }

      public virtual IReadOnlyList<ICoreCalculationMethod> AllCalculationMethods()
      {
         return _allCalculationMethods;
      }

      public virtual void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd)
      {
         _allCalculationMethods.Add(calculationMethodToAdd);
      }

      public virtual IEnumerable<IBuildingBlock> AllBuildingBlocks
      {
         get
         {
            return new IBuildingBlock[]
            {
               Molecules, Reactions, SpatialStructure,
               PassiveTransports, Observers, EventGroups,
               MoleculeStartValues, ParameterStartValues, SimulationSettings
            }.Where(bb => bb != null);
         }
      }

      public virtual void AcceptVisitor(IVisitor visitor)
      {
         AllBuildingBlocks.Each(x => x.AcceptVisitor(visitor));
      }
   }
}