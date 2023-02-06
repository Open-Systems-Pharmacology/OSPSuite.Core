using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class Module : ObjectBase
   {
      private readonly List<IMoleculeStartValuesBuildingBlock> _moleculeStartValuesCollection = new List<IMoleculeStartValuesBuildingBlock>();
      private readonly List<IParameterStartValuesBuildingBlock> _parameterStartValuesCollection = new List<IParameterStartValuesBuildingBlock>();

      public Module() : this(userEditable:true)
      {

      }

      protected Module(bool userEditable)
      {
         UserEditable = userEditable;
      }
      
      public IMoleculeBuildingBlock Molecule { set; get; }
      public IReactionBuildingBlock Reaction { set; get; }
      public IPassiveTransportBuildingBlock PassiveTransport { set; get; }
      public ISpatialStructure SpatialStructure { set; get; }
      public IObserverBuildingBlock Observer { set; get; }
      public IEventGroupBuildingBlock EventGroup { set; get; }
      public IReadOnlyList<IMoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<IParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;

      public bool UserEditable { get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         
         if (!(source is Module sourceModule))
            return;

         // Cloning these properties within the update for now. It could change based on specs
         Molecule = cloneManager.Clone(sourceModule.Molecule);
         Reaction = cloneManager.Clone(sourceModule.Reaction);
         PassiveTransport = cloneManager.Clone(sourceModule.PassiveTransport);
         SpatialStructure = cloneManager.Clone(sourceModule.SpatialStructure);
         Observer = cloneManager.Clone(sourceModule.Observer);
         EventGroup = cloneManager.Clone(sourceModule.EventGroup);

         sourceModule.MoleculeStartValuesCollection.Each(x => _moleculeStartValuesCollection.Add(cloneManager.Clone(x)));
         sourceModule.ParameterStartValuesCollection.Each(x => _parameterStartValuesCollection.Add(cloneManager.Clone(x)));
      }

      public void AddParameterStartValueBlock(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         _parameterStartValuesCollection.Add(parameterStartValuesBuildingBlock);
      }

      public void AddMoleculeStartValueBlock(IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         _moleculeStartValuesCollection.Add(moleculeStartValuesBuildingBlock);
      }
   }

   public class PKSimModule : Module
   {
      public PKSimModule() : base(userEditable:false)
      {
         
      }
      public string PKSimVersion { set; get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         if (!(source is PKSimModule sourcePKSimModule))
            return;

         PKSimVersion = sourcePKSimModule.PKSimVersion;
      }
   }
}