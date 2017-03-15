using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ObservedDataInExplorerPresenter : ContextSpecification<IObservedDataInExplorerPresenter>
   {
      private IProjectRetriever _projectRetriever;
      protected ITreeNodeFactory _treeNodeFactory;
      protected IExplorerPresenter _explorerPresenter;
      protected IClassificationPresenter _classificationPresenter;
      protected IExplorerView _explorerView;
      protected RootNodeType _rootNodeObservedDataFolder;
      protected RootNode _observationRootNode;
      protected IProject _project;
      private RootNodeType _rootNodeIndividualFolder;
      protected RootNode _individualRootNode;
      protected IObservedDataTask _observedDataTask;

      protected override void Context()
      {
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _observedDataTask = A.Fake<IObservedDataTask>();
         sut = new ObservedDataInExplorerPresenter(_projectRetriever, _treeNodeFactory, _observedDataTask);

         _explorerPresenter = A.Fake<IExplorerPresenter>();
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         _explorerView = A.Fake<IExplorerView>();

         A.CallTo(() => _explorerPresenter.BaseView).Returns(_explorerView);

         _rootNodeObservedDataFolder = new RootNodeType("ObservedData", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData);
         _rootNodeIndividualFolder = new RootNodeType("Individual", ApplicationIcons.IndividualFolder);
         _observationRootNode = new RootNode(_rootNodeObservedDataFolder);
         _individualRootNode = new RootNode(_rootNodeIndividualFolder);

         sut.InitializeWith(_explorerPresenter, _classificationPresenter, _rootNodeObservedDataFolder);
         _project = A.Fake<IProject>();

         A.CallTo(() => _explorerPresenter.NodeByType(_rootNodeObservedDataFolder)).Returns(_observationRootNode);
         A.CallTo(() => _explorerView.TreeView.NodeById(_rootNodeObservedDataFolder.Id)).Returns(_observationRootNode);
         A.CallTo(() => _explorerView.AddNode(A<ITreeNode>._)).ReturnsLazily(s => s.Arguments[0].DowncastTo<ITreeNode>());
      }
   }

   public class When_adding_observed_data_with_classification : concern_for_ObservedDataInExplorerPresenter
   {
      private ClassificationNode _tip;

      protected override void Context()
      {
         base.Context();
         _tip = new ClassificationNode(new Classification());

         A.CallTo(() => _treeNodeFactory.CreateFor(A<ClassifiableObservedData>._)).Returns(_tip);
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataAddedEvent(A.Fake<DataRepository>(), _project));
      }

      [Observation]
      public void root_should_be_under_folder()
      {
         _observationRootNode.Children.ShouldContain(_tip);
      }

      [Observation]
      public void should_add_root_node_to_view()
      {
         A.CallTo(() => _explorerView.AddNode(_observationRootNode)).MustHaveHappened();
      }
   }

   public class When_adding_observed_data_with_a_root_node_not_defined : concern_for_ObservedDataInExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _explorerPresenter.NodeByType(_rootNodeObservedDataFolder)).Returns(null);
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataAddedEvent(A.Fake<DataRepository>(), _project));
      }

      [Observation]
      public void should_not_add_any_node()
      {
         A.CallTo(() => _explorerView.AddNode(A<ITreeNode>._)).MustNotHaveHappened();
      }
   }

   public class When_adding_observed_data_without_classification : concern_for_ObservedDataInExplorerPresenter
   {
      private DataRepository _fakeRepository;

      protected override void Context()
      {
         base.Context();
         _fakeRepository = A.Fake<DataRepository>();
      }

      protected override void Because()
      {
         sut.Handle(new ObservedDataAddedEvent(_fakeRepository, _project));
      }

      [Observation]
      public void should_add_node_under_folder_node()
      {
         A.CallTo(() => _explorerView.AddNode(A<ITreeNode<IClassification>>._)).MustHaveHappened();
      }
   }

   public class When_asked_if_a_node_can_be_dragged : concern_for_ObservedDataInExplorerPresenter
   {
      [Observation]
      public void should_return_true_if_the_node_represents_an_observed_data_node()
      {
         sut.CanDrag(A.Fake<ObservedDataNode>()).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_node_represents_the_observed_data_folder()
      {
         sut.CanDrag(_observationRootNode).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_node_represents_a_root_node_that_is_note_the_observed_data_folder()
      {
         sut.CanDrag(_individualRootNode).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_node_does_not_represent_an_observed_data_node()
      {
         sut.CanDrag(A.Fake<ITreeNode>()).ShouldBeFalse();
      }
   }

   public class When_removing_all_observed_data_defined_under_a_classifiaction_node : concern_for_ObservedDataInExplorerPresenter
   {
      private ITreeNode<IClassification> _observedDataFolder;
      private bool _result;
      private ITreeNode _anotherNode;
      private List<DataRepository> _allObservedData;
      private DataRepository _repo1;
      private DataRepository _repo2;

      protected override void Context()
      {
         base.Context();
         _observedDataFolder = new ClassificationNode(new Classification());
         _anotherNode = A.Fake<ITreeNode>();
         _repo1 = new DataRepository().WithId("1");
         _repo2 = new DataRepository().WithId("2");
         _observedDataFolder.AddChild(new ObservedDataNode(new ClassifiableObservedData {Subject = _repo1}));
         _observedDataFolder.AddChild(new ObservedDataNode(new ClassifiableObservedData {Subject = _repo2}));
         _observedDataFolder.AddChild(_anotherNode);

         A.CallTo(() => _observedDataTask.Delete(A<IEnumerable<DataRepository>>._))
            .Invokes(x => _allObservedData = x.GetArgument<IEnumerable<DataRepository>>(0).ToList())
            .Returns(true);
      }

      protected override void Because()
      {
         _result = sut.RemoveObservedDataUnder(_observedDataFolder);
      }

      [Observation]
      public void should_delete_all_observed_data_defined_unter_the_node()
      {
         _allObservedData.ShouldOnlyContain(_repo1, _repo2);
      }

      [Observation]
      public void should_return_true_if_the_deletion_was_successful()
      {
         _result.ShouldBeTrue();
      }
   }
}