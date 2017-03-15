using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Nodes
{
   public interface ITreeNodeFactory
   {
      ITreeNode CreateFor(ISimulation simulation);
      ITreeNode CreateFor(ClassifiableParameterIdentification classifiableParameterIdentification);
      ITreeNode CreateFor(ClassifiableSensitivityAnalysis classifiableSensitivityAnalysis);
      ITreeNode CreateFor(ClassifiableObservedData observedData);
      ITreeNode<TObjectBase> CreateFor<TObjectBase>(TObjectBase entity) where TObjectBase : class, IObjectBase;
      ITreeNode CreateFor(string nodeText, string id, string iconName);
      ITreeNode CreateFor(string nodeText, string id);
      ITreeNode CreateFor(string nodeText);
      ITreeNode<IGroup> CreateFor(IGroup group);
   }

   public class TreeNodeFactory : ITreeNodeFactory
   {
      private readonly IObservedDataRepository _observedDataRepository;
      protected readonly IToolTipPartCreator _toolTipPartCreator;

      public TreeNodeFactory(IObservedDataRepository observedDataRepository, IToolTipPartCreator toolTipPartCreator)
      {
         _observedDataRepository = observedDataRepository;
         _toolTipPartCreator = toolTipPartCreator;
      }

      public ITreeNode CreateFor(ISimulation simulation)
      {
         var simulationNode = CreateObjectBaseNode(simulation)
            .WithIcon(ApplicationIcons.Simulation);

         _observedDataRepository.All().Where(simulation.UsesObservedData).Each(observedData =>
         {
            var node = CreateFor(observedData).WithIcon(ApplicationIcons.ObservedData);
            simulationNode.AddChild(node);
         });
         return simulationNode;
      }

      public ITreeNode CreateFor(ClassifiableParameterIdentification classifiableParameterIdentification)
      {
         return new ParameterIdentificationNode(classifiableParameterIdentification);
      }

      public ITreeNode CreateFor(ClassifiableSensitivityAnalysis classifiableSensitivityAnalysis)
      {
         return new SensitivityAnalysisNode(classifiableSensitivityAnalysis);
      }
      
      public ITreeNode<TObjectBase> CreateFor<TObjectBase>(TObjectBase entity) where TObjectBase : class, IObjectBase
      {
         var node = CreateObjectBaseNode(entity);
         node.ToolTip = _toolTipPartCreator.ToolTipFor(entity);
         return node;
      }

      public ITreeNode CreateFor(UsedObservedData usedObservedData)
      {
         var observedData = _observedDataRepository.FindFor(usedObservedData);
         return new UsedObservedDataNode(usedObservedData, observedData)
            .WithIcon(ApplicationIcons.ObservedData);
      }

      public ITreeNode CreateFor(string nodeText)
      {
         return CreateFor(nodeText, ShortGuid.NewGuid());
      }

      public ITreeNode CreateFor(string nodeText, string id)
      {
         return CreateFor(nodeText, id, string.Empty);
      }

      public ITreeNode CreateFor(string nodeText, string id, string iconName)
      {
         return new TextNode(nodeText, id ?? ShortGuid.NewGuid()).WithIcon(ApplicationIcons.IconByName(iconName));
      }

      public ITreeNode CreateFor(ClassifiableObservedData observedData)
      {
         return new ObservedDataNode(observedData) {ToolTip = _toolTipPartCreator.ToolTipFor(observedData.Repository)};
      }

      protected ITreeNode<TObjectBase> CreateObjectBaseNode<TObjectBase>(TObjectBase objectBase) where TObjectBase : class, IObjectBase
      {
         return new ObjectWithIdAndNameNode<TObjectBase>(objectBase);
      }

      public ITreeNode<IGroup> CreateFor(IGroup group)
      {
         return new GroupNode(group) {ToolTip = _toolTipPartCreator.ToolTipFor(group.Description)};
      }
   }
}