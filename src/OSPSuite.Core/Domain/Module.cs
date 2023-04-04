using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public class Module : ObjectBase
   {
      private readonly List<MoleculeStartValuesBuildingBlock> _moleculeStartValuesCollection = new List<MoleculeStartValuesBuildingBlock>();
      private readonly List<ParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<ParameterStartValuesBuildingBlock>();

      public bool ReadOnly { get; set; } = false;

      public MoleculeBuildingBlock Molecule { set; get; }
      public IReactionBuildingBlock Reaction { set; get; }
      public IPassiveTransportBuildingBlock PassiveTransport { set; get; }
      public ISpatialStructure SpatialStructure { set; get; }
      public IObserverBuildingBlock Observer { set; get; }
      public IEventGroupBuildingBlock EventGroup { set; get; }
      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;
      public virtual ExtendedProperties ExtendedProperties { get; } = new ExtendedProperties();

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         if (!(source is Module sourceModule))
            return;

         ReadOnly = sourceModule.ReadOnly;
         // Cloning these properties within the update for now. It could change based on specs
         Molecule = cloneManager.Clone(sourceModule.Molecule);
         Reaction = cloneManager.Clone(sourceModule.Reaction);
         PassiveTransport = cloneManager.Clone(sourceModule.PassiveTransport);
         SpatialStructure = cloneManager.Clone(sourceModule.SpatialStructure);
         Observer = cloneManager.Clone(sourceModule.Observer);
         EventGroup = cloneManager.Clone(sourceModule.EventGroup);

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

      public IReadOnlyList<IBuildingBlock> AllBuildingBlocks()
      {
         var buildingBlocks = new List<IBuildingBlock>
         {
            SpatialStructure,
            EventGroup,
            PassiveTransport,
            Molecule,
            Observer,
            Reaction,
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