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
      
      public IMoleculeBuildingBlock MoleculeBlock { set; get; }
      public IReactionBuildingBlock ReactionBlock { set; get; }
      public IPassiveTransportBuildingBlock PassiveTransport { set; get; }
      public ISpatialStructure SpatialStructure { set; get; }
      public IObserverBuildingBlock ObserverBlock { set; get; }
      public IEventGroupBuildingBlock EventBlock { set; get; }
      public IReadOnlyList<IMoleculeStartValuesBuildingBlock> MoleculeStartValuesCollection => _moleculeStartValuesCollection;
      public IReadOnlyList<IParameterStartValuesBuildingBlock> ParameterStartValuesCollection => _parameterStartValuesCollection;

      public bool UserEditable { get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         
         if (!(source is Module sourceModule))
            return;

         // Cloning these properties within the update for now. It could change based on specs
         MoleculeBlock = cloneManager.Clone(sourceModule.MoleculeBlock);
         ReactionBlock = cloneManager.Clone(sourceModule.ReactionBlock);
         PassiveTransport = cloneManager.Clone(sourceModule.PassiveTransport);
         SpatialStructure = cloneManager.Clone(sourceModule.SpatialStructure);
         ObserverBlock = cloneManager.Clone(sourceModule.ObserverBlock);
         EventBlock = cloneManager.Clone(sourceModule.EventBlock);

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