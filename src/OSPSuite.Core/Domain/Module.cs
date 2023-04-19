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
   public class Module : ObjectBase
   {
      private readonly List<MoleculeStartValuesBuildingBlock> _moleculeStartValuesCollection = new List<MoleculeStartValuesBuildingBlock>();
      private readonly List<ParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterStartValuesBuildingBlock>();

      public bool ReadOnly { get; set; } = false;

      public EventGroupBuildingBlock EventGroups { set; get; }
      public MoleculeBuildingBlock Molecules { set; get; }
      public ObserverBuildingBlock Observers { set; get; }
      public ReactionBuildingBlock Reactions { set; get; }
      public PassiveTransportBuildingBlock PassiveTransports { set; get; }
      public SpatialStructure SpatialStructure { set; get; }
      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;
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
         // Cloning these properties within the update for now. It could change based on specs
         Molecules = cloneManager.Clone(sourceModule.Molecules);
         Reactions = cloneManager.Clone(sourceModule.Reactions);
         PassiveTransports = cloneManager.Clone(sourceModule.PassiveTransports);
         SpatialStructure = cloneManager.Clone(sourceModule.SpatialStructure);
         Observers = cloneManager.Clone(sourceModule.Observers);
         EventGroups = cloneManager.Clone(sourceModule.EventGroups);

         sourceModule.MoleculeStartValuesCollection.Each(x => _moleculeStartValuesCollection.Add(cloneManager.Clone(x)));
         sourceModule.ParameterStartValuesCollection.Each(x => _parameterStartValuesCollection.Add(cloneManager.Clone(x)));

         ExtendedProperties.UpdateFrom(sourceModule.ExtendedProperties);
      }

      public void AddParameterStartValueBlock(ParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         _parameterStartValuesCollection.Add(parameterStartValuesBuildingBlock);
      }

      public void AddMoleculeStartValueBlock(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         _moleculeStartValuesCollection.Add(moleculeStartValuesBuildingBlock);
      }

      public void RemoveParameterStartValueBlock(ParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         _parameterStartValuesCollection.Remove(parameterStartValuesBuildingBlock);
      }

      public void RemoveMoleculeStartValueBlock(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         _moleculeStartValuesCollection.Remove(moleculeStartValuesBuildingBlock);
      }

      public void AddBuildingBlock(IBuildingBlock buildingBlock)
      {
         switch (buildingBlock)
         {
            case MoleculeBuildingBlock molecule:
               Molecules = molecule;
               break;
            case ReactionBuildingBlock reaction:
               Reactions = reaction;
               break;
            case SpatialStructure spatialStructure:
               SpatialStructure = spatialStructure;
               break;
            case PassiveTransportBuildingBlock passiveTransport:
               PassiveTransports = passiveTransport;
               break;
            case EventGroupBuildingBlock eventGroup:
               EventGroups = eventGroup;
               break;
            case ObserverBuildingBlock observer:
               Observers = observer;
               break;
            case ParameterStartValuesBuildingBlock parameterStartValues:
               AddParameterStartValueBlock(parameterStartValues);
               break;
            case MoleculeStartValuesBuildingBlock moleculeStartValues:
               AddMoleculeStartValueBlock(moleculeStartValues);
               break;
            case null:
               return;
            default:
               throw new OSPSuiteException(Error.BuildingBlockTypeNotSupported(buildingBlock.Name));
         }
      }

      public void RemoveBuildingBlock(IBuildingBlock buildingBlock)
      {
         switch (buildingBlock)
         {
            case MoleculeBuildingBlock _:
               Molecules = null;
               break;
            case ReactionBuildingBlock _:
               Reactions = null;
               break;
            case SpatialStructure _:
               SpatialStructure = null;
               break;
            case PassiveTransportBuildingBlock _:
               PassiveTransports = null;
               break;
            case EventGroupBuildingBlock _:
               EventGroups = null;
               break;
            case ObserverBuildingBlock _:
               Observers = null;
               break;
            case ParameterStartValuesBuildingBlock parameterStartValues:
               RemoveParameterStartValueBlock(parameterStartValues);
               break;
            case MoleculeStartValuesBuildingBlock moleculeStartValues:
               RemoveMoleculeStartValueBlock(moleculeStartValues);
               break;
            case null:
               return;
            default:
               throw new OSPSuiteException(Error.BuildingBlockTypeNotSupported(buildingBlock.Name));
         }
      }

      public IReadOnlyList<IBuildingBlock> AllBuildingBlocks()
      {
         var buildingBlocks = new List<IBuildingBlock>
         {
            SpatialStructure,
            EventGroups,
            PassiveTransports,
            Molecules,
            Observers,
            Reactions,
         };

         buildingBlocks.AddRange(ParameterStartValuesCollection);
         buildingBlocks.AddRange(MoleculeStartValuesCollection);

         return buildingBlocks.Where(x => x != null).ToList();
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         AllBuildingBlocks().Each(x => x.AcceptVisitor(visitor));
      }

      public void AddExtendedProperty<T>(string propertyName, T property)
      {
         ExtendedProperties[propertyName] = new ExtendedProperty<T> {Name = propertyName, Value = property};
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
   }
}