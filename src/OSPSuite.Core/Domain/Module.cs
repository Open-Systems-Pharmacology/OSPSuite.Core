using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class Module : ObjectBase
   {
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
      public IList<IMoleculeStartValuesBuildingBlock> MoleculeStartValueBlockCollection { set; get; } = new List<IMoleculeStartValuesBuildingBlock>();
      public IList<IParameterStartValuesBuildingBlock> ParametersStartValueBlockCollection { set; get; } = new List<IParameterStartValuesBuildingBlock>();

      public bool UserEditable { get; }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         
         if (!(source is Module sourceModule))
            return;

         MoleculeBlock = cloneManager.Clone(sourceModule.MoleculeBlock);
         ReactionBlock = cloneManager.Clone(sourceModule.ReactionBlock);
         PassiveTransport = cloneManager.Clone(sourceModule.PassiveTransport);
         SpatialStructure = cloneManager.Clone(sourceModule.SpatialStructure);
         ObserverBlock = cloneManager.Clone(sourceModule.ObserverBlock);
         EventBlock = cloneManager.Clone(sourceModule.EventBlock);

         sourceModule.MoleculeStartValueBlockCollection.Each(x => MoleculeStartValueBlockCollection.Add(cloneManager.Clone<IMoleculeStartValuesBuildingBlock>(x)));
         sourceModule.ParametersStartValueBlockCollection.Each(x => ParametersStartValueBlockCollection.Add(cloneManager.Clone(x)));
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