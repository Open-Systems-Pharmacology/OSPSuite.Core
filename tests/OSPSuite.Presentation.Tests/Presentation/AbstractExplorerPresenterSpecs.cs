using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_AbstractExplorerPresenter : ContextSpecification<ExplorerPresenterForSpecs>
   {
      protected IClassificationPresenter _classificationPresenter;
      protected ClassificationNode _folder1;
      protected ClassificationNode _subFolder;
      protected ClassificationNode _folder2;

      protected override void Context()
      {
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         sut = new ExplorerPresenterForSpecs(A.Fake<IExplorerViewForSpecs>(), A.Fake<IRegionResolver>(), _classificationPresenter, A.Fake<IToolTipPartCreator>(), A.Fake<IProjectRetriever>());

         _folder1 = simulationFolder();
         _subFolder = simulationFolder();
         _folder2 = simulationFolder();
         _folder1.AddChild(_subFolder);
      }

      private ClassificationNode simulationFolder() => new ClassificationNode(new Classification {ClassificationType = ClassificationType.Simulation});
   }

   public class When_removing_multiple_classifications_with_their_data : concern_for_AbstractExplorerPresenter
   {
      protected override void Because()
      {
         sut.RemoveClassifications(new[] {_folder1, _subFolder, _folder2}, removeData: true);
      }

      [Observation]
      public void should_remove_the_data_under_the_top_most_classifications_at_once()
      {
         sut.RemoveDataRequests.Count.ShouldBeEqualTo(1);
         sut.RemoveDataRequests[0].ShouldOnlyContainInOrder(_folder1, _folder2);
      }

      [Observation]
      public void should_remove_the_top_most_classifications_with_their_sub_classifications()
      {
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(_folder1)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(_folder1)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(_folder2)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(_folder2)).MustHaveHappened();
      }

      [Observation]
      public void should_not_remove_a_classification_again_once_removed_with_its_ancestor()
      {
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(_subFolder)).MustNotHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(_subFolder)).MustNotHaveHappened();
      }
   }

   public class When_removing_multiple_classifications_with_their_data_and_the_data_are_not_removed : concern_for_AbstractExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.DataRemoved = false;
      }

      protected override void Because()
      {
         sut.RemoveClassifications(new[] {_folder1, _folder2}, removeData: true);
      }

      [Observation]
      public void should_not_remove_any_classification()
      {
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(A<ITreeNode<IClassification>>._)).MustNotHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(A<ITreeNode<IClassification>>._)).MustNotHaveHappened();
      }
   }

   public class When_removing_multiple_classifications_and_keeping_their_data : concern_for_AbstractExplorerPresenter
   {
      protected override void Because()
      {
         sut.RemoveClassifications(new[] {_folder1, _folder2});
      }

      [Observation]
      public void should_not_remove_the_data()
      {
         sut.RemoveDataRequests.ShouldBeEmpty();
      }

      [Observation]
      public void should_remove_the_classifications_with_their_sub_classifications()
      {
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(_folder1)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(_folder1)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveChildrenClassifications(_folder2)).MustHaveHappened();
         A.CallTo(() => _classificationPresenter.RemoveClassification(_folder2)).MustHaveHappened();
      }
   }

   public class When_removing_no_classification_with_their_data : concern_for_AbstractExplorerPresenter
   {
      protected override void Because()
      {
         sut.RemoveClassifications(new List<ITreeNode<IClassification>>(), removeData: true);
      }

      [Observation]
      public void should_not_remove_any_data()
      {
         sut.RemoveDataRequests.ShouldBeEmpty();
      }
   }

   public class When_removing_one_classification_with_its_data : concern_for_AbstractExplorerPresenter
   {
      protected override void Because()
      {
         sut.RemoveChildrenClassifications(_folder2, removeParent: true, removeData: true);
      }

      [Observation]
      public void should_remove_the_data_under_the_classification()
      {
         sut.RemoveDataRequests.Count.ShouldBeEqualTo(1);
         sut.RemoveDataRequests[0].ShouldOnlyContain(_folder2);
      }

      [Observation]
      public void should_remove_the_classification()
      {
         A.CallTo(() => _classificationPresenter.RemoveClassification(_folder2)).MustHaveHappened();
      }
   }

   public interface IExplorerPresenterForSpecs : IExplorerPresenter, IPresenter<IExplorerViewForSpecs>
   {
   }

   public interface IExplorerViewForSpecs : IView<IExplorerPresenterForSpecs>, IExplorerView
   {
   }

   public class ExplorerPresenterForSpecs : AbstractExplorerPresenter<IExplorerViewForSpecs, IExplorerPresenterForSpecs>, IExplorerPresenterForSpecs
   {
      public List<IReadOnlyList<ITreeNode<IClassification>>> RemoveDataRequests { get; } = new List<IReadOnlyList<ITreeNode<IClassification>>>();
      public bool DataRemoved { get; set; } = true;

      public ExplorerPresenterForSpecs(IExplorerViewForSpecs view, IRegionResolver regionResolver, IClassificationPresenter classificationPresenter, IToolTipPartCreator toolTipPartCreator, IProjectRetriever projectRetriever)
         : base(view, regionResolver, classificationPresenter, toolTipPartCreator, new RegionName("Explorer", "Explorer", ApplicationIcons.EmptyIcon), projectRetriever)
      {
      }

      public override bool RemoveDataUnderClassifications(IReadOnlyList<ITreeNode<IClassification>> classificationNodes)
      {
         RemoveDataRequests.Add(classificationNodes);
         return DataRemoved;
      }

      public override bool CanDrag(ITreeNode node) => false;

      public override bool CopyAllowed() => false;

      public override IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode) => Enumerable.Empty<ClassificationTemplate>();

      public override void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category)
      {
      }

      protected override void AddProjectToTree(IProject project)
      {
      }

      public override void ShowContextMenu(ITreeNode node, Point popupLocation)
      {
      }

      public override void ShowContextMenu(IReadOnlyList<ITreeNode> treeNodes, Point popupLocation)
      {
      }
   }
}
