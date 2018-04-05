using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    A set of Building Blocks used to create a Model
   /// </summary>
   public interface IBuildConfiguration : IVisitable<IVisitor>
   {
      IMoleculeBuildingBlock Molecules { set; get; }
      IReactionBuildingBlock Reactions { set; get; }
      IPassiveTransportBuildingBlock PassiveTransports { set; get; }
      ISpatialStructure SpatialStructure { set; get; }
      IMoleculeStartValuesBuildingBlock MoleculeStartValues { set; get; }
      IParameterStartValuesBuildingBlock ParameterStartValues { set; get; }
      IObserverBuildingBlock Observers { get; set; }
      IEventGroupBuildingBlock EventGroups { get; set; }
      ISimulationSettings SimulationSettings { get; set; }

      /// <summary>
      ///    Return the molecules defined in the configuration that will be present in the model
      /// </summary>
      IEnumerable<IMoleculeBuilder> AllPresentMolecules();

      /// <summary>
      ///    Return the molecules values defined in the configuration for which a molecule also exists in the Molecule building
      ///    block
      /// </summary>
      IEnumerable<IMoleculeStartValue> AllPresentMoleculeValues();

      /// <summary>
      ///    Return the names of all molecules defined in the configuration that will be present in the model
      /// </summary>
      IEnumerable<string> AllPresentMoleculeNames();

      /// <summary>
      ///    Return the names of all molecules defined in the configuration that will be present in the model and that
      ///    satisifies the given query
      /// </summary>
      IEnumerable<string> AllPresentMoleculeNames(Func<IMoleculeBuilder, bool> query);

      /// <summary>
      ///    Return the names of all molecules defined in the configuration
      ///    that will be present in the model and are floating
      /// </summary>
      IEnumerable<string> AllPresentFloatingMoleculeNames();

      /// <summary>
      ///    Return the names of all molecules defined in the configuration
      ///    that will be present in the model, are floating and whose molecule dependent properties
      ///    will be copied into the simulation
      /// </summary>
      IEnumerable<string> AllPresentXenobioticFloatingMoleculeNames();

      /// <summary>
      ///    Return the names of all molecules defined in the configuration
      ///    that will be present in the model, are floating and whose molecule dependent properties
      ///    will be copied into the simulation
      /// </summary>
      IEnumerable<string> AllPresentEndogenousStationaryMoleculeNames();

      /// <summary>
      ///    Returns the calculation that will be used to create the model structure
      /// </summary>
      IEnumerable<ICoreCalculationMethod> AllCalculationMethods();

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
      ///    Set to <c>true</c> (default), the circulare reference check will be performed when creating a simulation. This
      ///    should only be set to <c>false</c> for very massive model.
      /// </summary>
      bool PerformCircularReferenceCheck { get; set; }

      /// <summary>
      ///    Retrieve the builder used to create the <paramref name="modelObject" /> given as parameter
      /// </summary>
      /// <param name="modelObject">Object for which a builder should be retrieved</param>
      /// <returns></returns>
      IObjectBase BuilderFor(IObjectBase modelObject);

      /// <summary>
      ///    This should be called whenever the construction of the model was finished to clear the cache
      /// </summary>
      void ClearCache();

      /// <summary>
      ///    Add a reference to the cache which describes that the <paramref name="modelObject" /> was created based on the
      ///    <paramref
      ///       name="builder" />
      /// </summary>
      /// <param name="modelObject">object that was created using the builder</param>
      /// <param name="builder">Builder used to create the model object</param>
      void AddBuilderReference(IObjectBase modelObject, IObjectBase builder);

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
      private readonly IList<ICoreCalculationMethod> _allCalculationMethods;
      private readonly ICache<IObjectBase, IObjectBase> _builderCache;
      public virtual ISpatialStructure SpatialStructure { get; set; }
      public virtual IMoleculeStartValuesBuildingBlock MoleculeStartValues { get; set; }
      public virtual IParameterStartValuesBuildingBlock ParameterStartValues { get; set; }
      public virtual IObserverBuildingBlock Observers { get; set; }
      public virtual IMoleculeBuildingBlock Molecules { get; set; }
      public virtual IReactionBuildingBlock Reactions { get; set; }
      public virtual IPassiveTransportBuildingBlock PassiveTransports { get; set; }
      public virtual IEventGroupBuildingBlock EventGroups { get; set; }
      public virtual ISimulationSettings SimulationSettings { get; set; }

      public SimModelExportMode SimModelExportMode { get; set; } = SimModelExportMode.Full;

      public bool ShouldValidate { get; set; } = true;
      public bool ShowProgress { get; set; } = true;
      public bool PerformCircularReferenceCheck { get; set; } = true;

      public BuildConfiguration()
      {
         _allCalculationMethods = new List<ICoreCalculationMethod>();
         _builderCache = new Cache<IObjectBase, IObjectBase>(onMissingKey: x => null);
      }

      public virtual IEnumerable<ICoreCalculationMethod> AllCalculationMethods()
      {
         return _allCalculationMethods;
      }

      public virtual void AddCalculationMethod(ICoreCalculationMethod calculationMethodToAdd)
      {
         _allCalculationMethods.Add(calculationMethodToAdd);
      }

      public virtual IEnumerable<IMoleculeBuilder> AllPresentMolecules()
      {
         return Molecules.AllPresentFor(MoleculeStartValues);
      }

      public virtual IEnumerable<IMoleculeStartValue> AllPresentMoleculeValues()
      {
         var moleculeNames = Molecules.Select(x => x.Name);
         return MoleculeStartValues.Where(msv => moleculeNames.Contains(msv.MoleculeName))
            .Where(msv => msv.IsPresent);
      }

      public virtual IEnumerable<string> AllPresentMoleculeNames()
      {
         return AllPresentMoleculeNames(x => true);
      }

      public IEnumerable<string> AllPresentMoleculeNames(Func<IMoleculeBuilder, bool> query)
      {
         return AllPresentMolecules().Where(query).Select(x => x.Name);
      }

      public virtual IEnumerable<string> AllPresentFloatingMoleculeNames()
      {
         return AllPresentMoleculeNames(m => m.IsFloating);
      }

      public IEnumerable<string> AllPresentXenobioticFloatingMoleculeNames()
      {
         return AllPresentMoleculeNames(m => m.IsFloating && m.IsXenobiotic);
      }

      public IEnumerable<string> AllPresentEndogenousStationaryMoleculeNames()
      {
         return AllPresentMoleculeNames(m => !m.IsFloating && !m.IsXenobiotic);
      }

      public virtual IObjectBase BuilderFor(IObjectBase modelObject)
      {
         return _builderCache[modelObject];
      }

      public virtual void ClearCache()
      {
         _builderCache.Clear();
      }

      public virtual void AddBuilderReference(IObjectBase modelObject, IObjectBase builder)
      {
         _builderCache.Add(modelObject, builder);
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