using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Merge behavior for merging spatial structures form different modules
   /// </summary>
   public enum MergeBehavior
   {
      /// <summary>
      ///    A container being merged will overwrite existing containers if found (by name) This is the default behavior
      /// </summary>
      Overwrite,

      /// <summary>
      ///    If a container with the same name is found, we try to merge the content of the container being merged into the
      ///    existing container according to the following logic
      ///    * We add non-existing children (by name) to the existing container
      ///    * We replace existing parameter by name
      ///    * We replace existing formula by name
      ///    * if a child container with the same name is found, we recursively merge the content of the child container
      /// </summary>
      Extend
   }

   public class Module : ObjectBase, IEnumerable<IBuildingBlock>
   {
      private readonly List<IBuildingBlock> _buildingBlocks = new List<IBuildingBlock>();
      private string _snapshot;
      private T buildingBlockByType<T>() where T : IBuildingBlock => _buildingBlocks.OfType<T>().SingleOrDefault();
      private IReadOnlyList<T> buildingBlocksByType<T>() where T : IBuildingBlock => _buildingBlocks.OfType<T>().ToList();

      /// <summary>
      ///    Module is a PKSim module if created in PKSim 
      /// </summary>
      public bool IsPKSimModule { get; set; }

      /// <summary>
      /// Holds a snapshot of everything required to recreate the module in PK-sim
      /// </summary>
      public string Snapshot
      {
         set => _snapshot = value;
         get => !IsPKSimModule ? string.Empty : _snapshot;
      }

      public string ModuleImportVersion { get; set; }

      public string PKSimVersion { get; set; }

      public MergeBehavior MergeBehavior { get; set; } = MergeBehavior.Overwrite;

      public EventGroupBuildingBlock EventGroups => buildingBlockByType<EventGroupBuildingBlock>();
      public MoleculeBuildingBlock Molecules => buildingBlockByType<MoleculeBuildingBlock>();
      public ObserverBuildingBlock Observers => buildingBlockByType<ObserverBuildingBlock>();
      public ReactionBuildingBlock Reactions => buildingBlockByType<ReactionBuildingBlock>();
      public PassiveTransportBuildingBlock PassiveTransports => buildingBlockByType<PassiveTransportBuildingBlock>();
      public SpatialStructure SpatialStructure => buildingBlockByType<SpatialStructure>();

      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => buildingBlocksByType<InitialConditionsBuildingBlock>();

      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollection => buildingBlocksByType<ParameterValuesBuildingBlock>();

      public string Version => versionCalculation(BuildingBlocks);

      public bool HasSnapshot => !string.IsNullOrEmpty(Snapshot);

      public string VersionWith(ParameterValuesBuildingBlock selectedParameterValues, InitialConditionsBuildingBlock selectedInitialConditions)
      {
         var buildingBlocks = BuildingBlocks.Where(isSingle).ToList();

         if (selectedParameterValues != null)
            buildingBlocks.Add(selectedParameterValues);

         if (selectedInitialConditions != null)
            buildingBlocks.Add(selectedInitialConditions);

         return versionCalculation(buildingBlocks);
      }

      private string versionCalculation(IEnumerable<IBuildingBlock> buildingBlocks)
      {
          return string.Concat(typedVersionConcat(buildingBlocks), (int)MergeBehavior)
              .GetHashCode().ToString();
      }
      
      private static string typedVersionConcat(IEnumerable<IBuildingBlock> buildingBlocks)
      {
          // Use OrderBy to ensure alphabetical ordering of the typed versions
          return string.Concat(buildingBlocks.Select(typedVersionFor).OrderBy(x => x).ToArray());
      }

      private static string typedVersionFor(IBuildingBlock x)
      {
         return $"{x.GetType()}{x.Version}";
      }

      public override string Icon
      {
         get => IsPKSimModule ? IconNames.PKSimModule : IconNames.Module;
         set
         {
            // Do not set from outside
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         if (!(source is Module sourceModule))
            return;

         sourceModule.BuildingBlocks.Select(cloneManager.Clone).Each(Add);
         PKSimVersion = sourceModule.PKSimVersion;
         ModuleImportVersion = sourceModule.ModuleImportVersion;
         MergeBehavior = sourceModule.MergeBehavior;
         IsPKSimModule = sourceModule.IsPKSimModule;
         Snapshot = sourceModule.Snapshot;
      }

      public void Add(IBuildingBlock buildingBlock)
      {
         if (isSingle(buildingBlock))
         {
            var type = buildingBlock.GetType();
            var existingBuildingBlock = _buildingBlocks.FirstOrDefault(x => x.IsAnImplementationOf(type));
            if (existingBuildingBlock != null)
               throw new OSPSuiteException(Error.BuildingBlockTypeAlreadyAddedToModule(buildingBlock.Name, type.Name));
         }

         buildingBlock.Module = this;
         _buildingBlocks.Add(buildingBlock);
      }

      private bool isSingle(IBuildingBlock buildingBlock) => !(buildingBlock.IsAnImplementationOf<InitialConditionsBuildingBlock>() || buildingBlock.IsAnImplementationOf<ParameterValuesBuildingBlock>());

      public void Remove(IBuildingBlock buildingBlock)
      {
         // If a PKSim module is cloned and some building blocks removed
         // it is no longer a PK-Sim module.
         IsPKSimModule = false;
         buildingBlock.Module = null;
         _buildingBlocks.Remove(buildingBlock);
      }

      public IReadOnlyList<IBuildingBlock> BuildingBlocks => _buildingBlocks;

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         BuildingBlocks.Each(x => x.AcceptVisitor(visitor));
      }

      public IEnumerator<IBuildingBlock> GetEnumerator()
      {
         return _buildingBlocks.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      ///    Checks if a building block can be added to a module.
      /// </summary>
      public bool CanAdd<T>(T buildingBlockToAdd) where T : IBuildingBlock
      {
         var forbiddenTypes = new HashSet<Type>
         {
            typeof(ExpressionProfileBuildingBlock),
            typeof(IndividualBuildingBlock)
         };

         var uniqueTypes = new HashSet<Type>
         {
            typeof(MoleculeBuildingBlock),
            typeof(PassiveTransportBuildingBlock),
            typeof(EventGroupBuildingBlock),
            typeof(ReactionBuildingBlock),
            typeof(SpatialStructure),
            typeof(ObserverBuildingBlock)
         };

         // Check if the type to be added is in the forbiddenTypes set
         if (buildingBlockToAdd.IsAnImplementationOfAny(forbiddenTypes))
            return false;

         // Check if the type to be added or any of its base types are in the uniqueTypes set
         if (!buildingBlockToAdd.IsAnImplementationOfAny(uniqueTypes))
            return true;

         return !buildingBlockToAdd.IsAnImplementationOfAny(BuildingBlocks.Select(block => block.GetType()));
      }
   }
}