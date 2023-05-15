using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
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

      public bool ReadOnly { get; set; } = false;

      public EventGroupBuildingBlock EventGroups => buildingBlockByType<EventGroupBuildingBlock>();
      public MoleculeBuildingBlock Molecules => buildingBlockByType<MoleculeBuildingBlock>();
      public ObserverBuildingBlock Observers => buildingBlockByType<ObserverBuildingBlock>();
      public ReactionBuildingBlock Reactions => buildingBlockByType<ReactionBuildingBlock>();
      public PassiveTransportBuildingBlock PassiveTransports => buildingBlockByType<PassiveTransportBuildingBlock>();
      public SpatialStructure SpatialStructure => buildingBlockByType<SpatialStructure>();

      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollection => buildingBlocksByType<InitialConditionsBuildingBlock>();

      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollection => buildingBlocksByType<ParameterValuesBuildingBlock>();

      public virtual ExtendedProperties ExtendedProperties { get; } = new ExtendedProperties();

      public Module()
      {
         Icon = IconNames.Module;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         if (!(source is Module sourceModule))
            return;

         ReadOnly = sourceModule.ReadOnly;
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
         ExtendedProperties[propertyName] = new ExtendedProperty<T> { Name = propertyName, Value = property };
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

      public IReadOnlyList<IBuildingBlock> ReferringStartValueBuildingBlocks(IBuildingBlock referencedBuildingBlock)
      {
         return referringStartValueBuildingBlocks(referencedBuildingBlock, InitialConditionsCollection).ToList();
      }
      private IEnumerable<IBuildingBlock> referringStartValueBuildingBlocks(IBuildingBlock buildingBlockToRemove, IReadOnlyList<InitialConditionsBuildingBlock> buildingBlockCollection)
      {
         return buildingBlockCollection
            .Where(bb => bb.Uses(buildingBlockToRemove))
            .OfType<IBuildingBlock>()
            .ToList();
      }
   }
}