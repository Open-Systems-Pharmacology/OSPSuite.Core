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
   public class Module : ObjectBase, IEnumerable<IBuildingBlock>
   {
      private readonly List<IBuildingBlock> _buildingBlocks = new List<IBuildingBlock>();

      private T buildingBlockByType<T>() where T : IBuildingBlock => _buildingBlocks.OfType<T>().SingleOrDefault();
      private IReadOnlyList<T> buildingBlocksByType<T>() where T : IBuildingBlock => _buildingBlocks.OfType<T>().ToList();
      private readonly ExtendedPropertyStore<Module> _extendedPropertyStore;

      /// <summary>
      /// Module is a PKSim module if created in PKSim and the module version
      /// matches the version when it was first imported
      /// </summary>
      public bool IsPKSimModule =>
         !string.IsNullOrEmpty(PKSimVersion) && 
         Equals(Version, _extendedPropertyStore.Get(x => x.ModuleImportVersion));

      public string ModuleImportVersion
      {
         get { return _extendedPropertyStore.Get(x => x.ModuleImportVersion); }
         set { _extendedPropertyStore.Set(x => x.ModuleImportVersion, value); }
      }

      public string PKSimVersion
      {
         get { return _extendedPropertyStore.Get(x => x.PKSimVersion); }
         set { _extendedPropertyStore.Set(x => x.PKSimVersion, value); }
      }

      public EventGroupBuildingBlock EventGroups => buildingBlockByType<EventGroupBuildingBlock>();
      public MoleculeBuildingBlock Molecules => buildingBlockByType<MoleculeBuildingBlock>();
      public ObserverBuildingBlock Observers => buildingBlockByType<ObserverBuildingBlock>();
      public ReactionBuildingBlock Reactions => buildingBlockByType<ReactionBuildingBlock>();
      public PassiveTransportBuildingBlock PassiveTransports => buildingBlockByType<PassiveTransportBuildingBlock>();
      public SpatialStructure SpatialStructure => buildingBlockByType<SpatialStructure>();


      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => buildingBlocksByType<InitialConditionsBuildingBlock>();

      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollection => buildingBlocksByType<ParameterValuesBuildingBlock>();

      public virtual ExtendedProperties ExtendedProperties { get; } = new ExtendedProperties();

      public string Version => versionCalculation(BuildingBlocks);
      
      public Module()
      {
         _extendedPropertyStore = new ExtendedPropertyStore<Module>(ExtendedProperties);
         _extendedPropertyStore.ConfigureProperty(x => x.ModuleImportVersion);
         _extendedPropertyStore.ConfigureProperty(x => x.PKSimVersion);
      }

      public string VersionWith(ParameterValuesBuildingBlock selectedParameterValues, InitialConditionsBuildingBlock selectedInitialConditions)
      {
         var buildingBlocks = BuildingBlocks.Where(isSingle).ToList();
         
         if(selectedParameterValues != null)
            buildingBlocks.Add(selectedParameterValues);

         if (selectedInitialConditions != null)
            buildingBlocks.Add(selectedInitialConditions);

         return versionCalculation(buildingBlocks);
      }

      private string versionCalculation(IEnumerable<IBuildingBlock> buildingBlocks)
      {
         // Use OrderBy to ensure alphabetical ordering of the typed versions
         return string.Join(string.Empty, buildingBlocks.Select(typedVersionFor).OrderBy(x => x).ToArray()).GetHashCode().ToString();
      }

      private static string typedVersionFor(IBuildingBlock x)
      {
         return $"{x.GetType()}{x.Version}";
      }

      public override string Icon {
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
         ExtendedProperties.UpdateFrom(sourceModule.ExtendedProperties);
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
         buildingBlock.Module = null;
         _buildingBlocks.Remove(buildingBlock);
      }

      public IReadOnlyList<IBuildingBlock> BuildingBlocks => _buildingBlocks;

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         BuildingBlocks.Each(x => x.AcceptVisitor(visitor));
      }

      public void AddExtendedProperty<T>(string propertyName, T property)
      {
         AddExtendedProperty(new ExtendedProperty<T> { Name = propertyName, Value = property });
      }

      public void AddExtendedProperty(IExtendedProperty extendedProperty)
      { 
         ExtendedProperties[extendedProperty.Name] = extendedProperty;
      }

      /// <summary>
      /// </summary>
      /// <param name="propertyName"></param>
      /// <returns></returns>
      public string ExtendedPropertyValueFor(string propertyName) => ExtendedPropertyValueFor<string>(propertyName);

      public T ExtendedPropertyValueFor<T>(string propertyName)
      {
         return ExtendedProperties.Contains(propertyName) ? ExtendedProperties[propertyName].ValueAsObject.ConvertedTo<T>() : default(T);
      }

      public IEnumerator<IBuildingBlock> GetEnumerator()
      {
         return _buildingBlocks.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}