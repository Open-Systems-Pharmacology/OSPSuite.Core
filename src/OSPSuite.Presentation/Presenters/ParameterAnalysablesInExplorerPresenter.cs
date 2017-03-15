using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters
{
   public interface IParameterAnalysablesInExplorerPresenter :
      IListener<ParameterIdentificationDeletedEvent>,
      IListener<ParameterIdentificationCreatedEvent>,
      IListener<SensitivityAnalysisDeletedEvent>,
      IListener<SensitivityAnalysisCreatedEvent>

   {
      bool CanDrag(ITreeNode node);
      bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode);
      void AddParameterAnalysablesToTree(IProject project);
      void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter);
   }

   public class ParameterAnalysablesInExplorerPresenter : IParameterAnalysablesInExplorerPresenter
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ITreeNodeFactory _treeNodeFactory;
      private IClassificationPresenter _classificationPresenter;
      private IExplorerPresenter _explorerPresenter;

      public ParameterAnalysablesInExplorerPresenter(IParameterIdentificationTask parameterIdentificationTask, ISensitivityAnalysisTask sensitivityAnalysisTask, ITreeNodeFactory treeNodeFactory)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _treeNodeFactory = treeNodeFactory;
      }

      public void InitializeWith(IExplorerPresenter explorerPresenter, IClassificationPresenter classificationPresenter)
      {
         _classificationPresenter = classificationPresenter;
         _explorerPresenter = explorerPresenter;
      }

      public void Handle(ParameterIdentificationCreatedEvent eventToHandle)
      {
         var node = addParameterIdentificationToTree(eventToHandle.ParameterIdentification);
         _explorerPresenter.EnsureNodeVisible(node);
      }

      public void Handle(SensitivityAnalysisCreatedEvent eventToHandle)
      {
         var node = addSensitivityAnalysisToTree(eventToHandle.SensitivityAnalysis);
         _explorerPresenter.EnsureNodeVisible(node);
      }

      public void Handle(ParameterIdentificationDeletedEvent eventToHandle)
      {
         _explorerPresenter.RemoveNodeFor(eventToHandle.ParameterIdentification);
      }

      public void Handle(SensitivityAnalysisDeletedEvent eventToHandle)
      {
         _explorerPresenter.RemoveNodeFor(eventToHandle.SensitivityAnalysis);
      }

      public bool CanDrag(ITreeNode node)
      {
         return node.IsAnImplementationOf<ParameterIdentificationNode>() ||
                node.IsAnImplementationOf<SensitivityAnalysisNode>();
      }

      public bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode)
      {
         if (classificationNode.Tag.ClassificationType == ClassificationType.ParameterIdentification)
         {
            IReadOnlyList<ParameterIdentification> allParameterIdentifications = classificationNode.AllNodes<ParameterIdentificationNode>().Select(x => x.Tag.ParameterIdentification).ToList();
            return _parameterIdentificationTask.Delete(allParameterIdentifications);
         }

         if (classificationNode.Tag.ClassificationType == ClassificationType.SensitiviyAnalysis)
         {
            IReadOnlyList<SensitivityAnalysis> allSensitivityAnalyses = classificationNode.AllNodes<SensitivityAnalysisNode>().Select(x => x.Tag.SensitivityAnalysis).ToList();
            return _sensitivityAnalysisTask.Delete(allSensitivityAnalyses);
         }

         return false;
      }

      public void AddParameterAnalysablesToTree(IProject project)
      {
         _classificationPresenter.AddClassificationsToTree(project.AllClassificationsByType(ClassificationType.ParameterIdentification));
         project.AllClassifiablesByType<ClassifiableParameterIdentification>().Each(x => addClassifiableParameterIdentificationToParameterIdentificationRootFolder(x));

         _classificationPresenter.AddClassificationsToTree(project.AllClassificationsByType(ClassificationType.SensitiviyAnalysis));
         project.AllClassifiablesByType<ClassifiableSensitivityAnalysis>().Each(x => addClassifiableSensitivityAnalysisToSensitivityAnalysisRootFolder(x));
      }

      private ITreeNode addClassifiableParameterIdentificationToParameterIdentificationRootFolder(ClassifiableParameterIdentification classifiableParameterIdentification)
      {
         return _explorerPresenter.AddClassifiableToTree(classifiableParameterIdentification, RootNodeTypes.ParameterIdentificationFolder, addClassifiableParameterIdentificationToTree);
      }

      private ITreeNode addClassifiableSensitivityAnalysisToSensitivityAnalysisRootFolder(ClassifiableSensitivityAnalysis classifiableSensitivityAnalysis)
      {
         return _explorerPresenter.AddClassifiableToTree(classifiableSensitivityAnalysis, RootNodeTypes.SensitivityAnalysisFolder, addClassifiableSensitivityAnalysisToTree);
      }

      private ITreeNode addClassifiableSensitivityAnalysisToTree(ITreeNode<IClassification> classificationNode, ClassifiableSensitivityAnalysis classifiableSensitivityAnalysis)
      {
         var sensitivityAnalysisNode = _treeNodeFactory.CreateFor(classifiableSensitivityAnalysis)
            .WithIcon(ApplicationIcons.SensitivityAnalysis);

         _explorerPresenter.AddClassifiableNodeToView(sensitivityAnalysisNode, classificationNode);
         return sensitivityAnalysisNode;
      }

      private ITreeNode addClassifiableParameterIdentificationToTree(ITreeNode<IClassification> classificationNode, ClassifiableParameterIdentification classifiableParameterIdentification)
      {
         var parameterIdentificationNode = _treeNodeFactory.CreateFor(classifiableParameterIdentification)
            .WithIcon(ApplicationIcons.ParameterIdentification);

         _explorerPresenter.AddClassifiableNodeToView(parameterIdentificationNode, classificationNode);
         return parameterIdentificationNode;
      }

      private ITreeNode addParameterIdentificationToTree(ParameterIdentification parameterIdentification)
      {
         return _explorerPresenter.AddSubjectToClassifyToTree<ParameterIdentification, ClassifiableParameterIdentification>(parameterIdentification, addClassifiableParameterIdentificationToParameterIdentificationRootFolder);
      }

      private ITreeNode addSensitivityAnalysisToTree(SensitivityAnalysis sensitivityAnalysis)
      {
         return _explorerPresenter.AddSubjectToClassifyToTree<SensitivityAnalysis, ClassifiableSensitivityAnalysis>(sensitivityAnalysis, addClassifiableSensitivityAnalysisToSensitivityAnalysisRootFolder);
      }
   }
}