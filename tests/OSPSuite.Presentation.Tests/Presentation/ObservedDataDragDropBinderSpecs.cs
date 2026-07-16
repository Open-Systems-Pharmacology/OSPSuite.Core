using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Binders;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ObservedDataDragDropBinder : ContextSpecification<ObservedDataDragDropBinder>
   {
      protected DragDropInfo _data;
      protected IDragEvent _dragEventArgs;

      protected override void Context()
      {
         _dragEventArgs = A.Fake<IDragEvent>();
         sut = new ObservedDataDragDropBinder();
         A.CallTo(() => _dragEventArgs.TypeBeingDraggedIs(typeof(IEnumerable<ITreeNode>))).Returns(true);
         A.CallTo(() => _dragEventArgs.Data<DragDropInfo>()).Returns(_data);
         A.CallTo(() => _dragEventArgs.Data<IEnumerable<ITreeNode>>()).Returns(_data.Subject.DowncastTo<IEnumerable<ITreeNode>>());
      }
   }

   public class When_preparing_the_drag_effect_for_an_observed_data_node : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new ObservedDataNode(new ClassifiableObservedData { Subject = new DataRepository() })
            });
         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_move()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.Move);
      }
   }

   public class When_preparing_the_drag_effect_for_a_classification_node_representing_observed_data : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(new List<ITreeNode>
         {
            new ClassificationNode(new Classification { ClassificationType = ClassificationType.ObservedData })
         });

         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_move()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.Move);
      }
   }

   public class When_preparing_the_drag_effect_for_a_classification_node_representing_simulations : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new ClassificationNode(new Classification { ClassificationType = ClassificationType.Simulation })
            });

         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_none()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.None);
      }
   }

   public class When_preparing_the_drag_effect_for_the_observed_data_folder_node : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new RootNode(new RootNodeType("ObservedDataFolder", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData))
            });
         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_move()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.Move);
      }
   }

   public class When_preparing_the_drag_effect_for_any_other_root_folder_node : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new RootNode(new RootNodeType("IndividualFolder", ApplicationIcons.IndividualFolder))
            });
         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_none()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.None);
      }
   }

   public class When_preparing_the_drag_effect_for_any_other_node : concern_for_ObservedDataDragDropBinder
   {
      protected override void Context()
      {
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new ObjectWithIdAndNameNode<Group>(new Group { Name = "test" })
            });
         base.Context();
      }

      protected override void Because()
      {
         sut.PrepareDrag(_dragEventArgs);
      }

      [Observation]
      public void should_set_the_drag_effect_to_none()
      {
         _dragEventArgs.Effect.ShouldBeEqualTo(DragEffect.None);
      }
   }

   public class When_retrieving_the_dropped_observed_data_for_an_observed_data_node : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository;
      private IReadOnlyList<DataRepository> _result;

      protected override void Context()
      {
         _repository = A.Fake<DataRepository>();
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               new ObservedDataNode(new ClassifiableObservedData { Subject = _repository })
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_the_underlying_observed_data()
      {
         _result.ShouldOnlyContainInOrder(_repository);
      }
   }

   public class When_retrieving_the_dropped_observed_data_for_a_classification_node : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private IReadOnlyList<DataRepository> _result;

      protected override void Context()
      {
         _repository1 = new DataRepository();
         _repository2 = new DataRepository();
         var classificationNode = new ClassificationNode(new Classification { ClassificationType = ClassificationType.ObservedData });
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository1 }));
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository2 }));

         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               classificationNode
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_all_underlying_observed_data()
      {
         _result.ShouldOnlyContainInOrder(_repository1, _repository2);
      }
   }

   public class When_retrieving_the_dropped_observed_data_for_the_observed_data_folder : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private IReadOnlyList<DataRepository> _result;

      protected override void Context()
      {
         _repository1 = A.Fake<DataRepository>();
         _repository2 = A.Fake<DataRepository>();
         var rootNode = new RootNode(new RootNodeType("ObservedDataFolder", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData));
         var classificationNode = new ClassificationNode(new Classification());
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository1 }));
         rootNode.AddChild(classificationNode);
         rootNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository2 }));
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               rootNode
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_all_underlying_observed_data()
      {
         _result.ShouldOnlyContainInOrder(_repository1, _repository2);
      }
   }

   public class When_retrieving_the_dropped_observed_data_color_grouped_for_an_observed_data_node_directly_under_the_root : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository;
      private List<List<DataRepository>> _result;

      protected override void Context()
      {
         _repository = A.Fake<DataRepository>();
         var rootNode = new RootNode(new RootNodeType("ObservedDataFolder", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData));
         var observedDataNode = new ObservedDataNode(new ClassifiableObservedData { Subject = _repository });
         rootNode.AddChild(observedDataNode);

         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               observedDataNode
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataWithFolderPathFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_the_underlying_observed_data_with_its_ID_as_key()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].ShouldOnlyContain(_repository);
      }
   }

   public class When_retrieving_the_dropped_observed_data_color_grouped_for_a_classification_node : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private List<List<DataRepository>> _result;

      protected override void Context()
      {
         _repository1 = new DataRepository();
         _repository2 = new DataRepository();
         var classificationNode = new ClassificationNode(new Classification { ClassificationType = ClassificationType.ObservedData });
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository1 }));
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository2 }));

         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               classificationNode
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataWithFolderPathFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_all_underlying_observed_data()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].ShouldOnlyContain(_repository1, _repository2);
      }
   }

   public class When_retrieving_the_dropped_observed_data_color_grouped_for_the_observed_data_folder : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private List<List<DataRepository>> _result;

      protected override void Context()
      {
         _repository1 = A.Fake<DataRepository>();
         _repository2 = A.Fake<DataRepository>();
         var rootNode = new RootNode(new RootNodeType("ObservedDataFolder", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData));
         var classificationNode = new ClassificationNode(new Classification());
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository2 }));
         rootNode.AddChild(classificationNode);
         rootNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _repository1 }));
         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               rootNode
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataWithFolderPathFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_all_underlying_observed_data()
      {
         _result.Count.ShouldBeEqualTo(2);
         _result[1].ShouldOnlyContain(_repository1);
         _result[0].ShouldOnlyContain(_repository2);
      }
   }

   public class When_retrieving_the_dropped_observed_data_color_grouped_for_a_subset_of_observed_data_nodes_in_the_same_folder : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repository1;
      private DataRepository _repository2;
      private DataRepository _ignoredRepository;
      private List<List<DataRepository>> _result;

      protected override void Context()
      {
         _repository1 = new DataRepository();
         _repository2 = new DataRepository();
         _ignoredRepository = new DataRepository();
         var classificationNode = new ClassificationNode(new Classification { ClassificationType = ClassificationType.ObservedData });
         var pickedNode1 = new ObservedDataNode(new ClassifiableObservedData { Subject = _repository1 });
         var pickedNode2 = new ObservedDataNode(new ClassifiableObservedData { Subject = _repository2 });
         classificationNode.AddChild(pickedNode1);
         classificationNode.AddChild(pickedNode2);
         classificationNode.AddChild(new ObservedDataNode(new ClassifiableObservedData { Subject = _ignoredRepository }));

         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               pickedNode1,
               pickedNode2
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataWithFolderPathFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_a_single_color_group_containing_only_the_selected_observed_data()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].ShouldOnlyContain(_repository1, _repository2);
      }
   }

   public class When_retrieving_the_dropped_observed_data_color_grouped_for_observed_data_nodes_from_different_folders : concern_for_ObservedDataDragDropBinder
   {
      private DataRepository _repositoryUnderRoot;
      private DataRepository _repositoryInClassification1;
      private DataRepository _repositoryInClassification2;
      private List<List<DataRepository>> _result;

      protected override void Context()
      {
         _repositoryUnderRoot = new DataRepository();
         _repositoryInClassification1 = new DataRepository();
         _repositoryInClassification2 = new DataRepository();

         var rootNode = new RootNode(new RootNodeType("ObservedDataFolder", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData));
         var classificationNode = new ClassificationNode(new Classification { ClassificationType = ClassificationType.ObservedData });
         rootNode.AddChild(classificationNode);

         var nodeUnderRoot = new ObservedDataNode(new ClassifiableObservedData { Subject = _repositoryUnderRoot });
         var nodeInClassification1 = new ObservedDataNode(new ClassifiableObservedData { Subject = _repositoryInClassification1 });
         var nodeInClassification2 = new ObservedDataNode(new ClassifiableObservedData { Subject = _repositoryInClassification2 });
         rootNode.AddChild(nodeUnderRoot);
         classificationNode.AddChild(nodeInClassification1);
         classificationNode.AddChild(nodeInClassification2);

         _data = new DragDropInfo(
            new List<ITreeNode>
            {
               nodeUnderRoot,
               nodeInClassification1,
               nodeInClassification2
            });
         base.Context();
      }

      protected override void Because()
      {
         _result = sut.DroppedObservedDataWithFolderPathFrom(_dragEventArgs);
      }

      [Observation]
      public void should_return_one_color_group_per_parent_folder()
      {
         _result.Count.ShouldBeEqualTo(2);
         _result.First(group => group.Contains(_repositoryUnderRoot)).ShouldOnlyContain(_repositoryUnderRoot);
         _result.First(group => group.Contains(_repositoryInClassification1)).ShouldOnlyContain(_repositoryInClassification1, _repositoryInClassification2);
      }
   }
}