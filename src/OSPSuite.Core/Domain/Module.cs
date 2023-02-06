using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class Module : ObjectBase
   {
      private readonly List<IMoleculeStartValuesBuildingBlock> _moleculeStartValuesBlockCollection = new List<IMoleculeStartValuesBuildingBlock>();
      private readonly List<IParameterStartValuesBuildingBlock> _parameterStartValuesBlockCollection = new List<IParameterStartValuesBuildingBlock>();

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
      public IReadOnlyList<IMoleculeStartValuesBuildingBlock> MoleculeStartValuesBlockCollection => _moleculeStartValuesBlockCollection;
      public IReadOnlyList<IParameterStartValuesBuildingBlock> ParameterStartValuesBlockCollection => _parameterStartValuesBlockCollection;

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

         sourceModule.MoleculeStartValuesBlockCollection.Each(x => _moleculeStartValuesBlockCollection.Add(cloneManager.Clone(x)));
         sourceModule.ParameterStartValuesBlockCollection.Each(x => _parameterStartValuesBlockCollection.Add(cloneManager.Clone(x)));
      }

      public void AddParameterStartValueBlock(IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
      {
         _parameterStartValuesBlockCollection.Add(parameterStartValuesBuildingBlock);
      }

      public void AddMoleculeStartValueBlock(IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         _moleculeStartValuesBlockCollection.Add(moleculeStartValuesBuildingBlock);
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