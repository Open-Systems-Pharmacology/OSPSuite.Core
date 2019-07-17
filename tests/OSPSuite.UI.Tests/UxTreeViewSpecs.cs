using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Helpers;

namespace OSPSuite.UI
{
   public abstract class concern_for_UxTreeView : ContextSpecification<UxTreeView>
   {
      protected override void Context()
      {
         sut = new UxTreeView();
      }
   }

   public class When_adding_a_node_to_the_dx_tree_view : concern_for_UxTreeView
   {
      private ITreeNode _nodeToAdd;
      private ITreeNode _childNode;

      protected override void Context()
      {
         base.Context();
         _nodeToAdd = new TestNode("toto", "toto");
         _childNode = new TestNode("tutu", "tutu");
         _nodeToAdd.AddChildren(_childNode);
      }

      protected override void Because()
      {
         sut.AddNode(_nodeToAdd);
      }

      [Observation]
      public void should_reference_this_node_with_its_node_name_in_the_underlying_list_of_nodes()
      {
         sut.NodeById(_nodeToAdd.Id).ShouldBeEqualTo(_nodeToAdd);
      }

      [Observation]
      public void should_add_its_sub_node_to_the_tree_view_as_well()
      {
         sut.NodeById(_childNode.Id).ShouldBeEqualTo(_childNode);
      }
   }

   public class When_adding_a_node_to_the_dx_tree_view_that_already_exists : concern_for_UxTreeView
   {
      private ITreeNode _nodeToAdd;

      protected override void Context()
      {
         base.Context();
         _nodeToAdd = new TestNode("toto", "toto");
         sut.AddNode(_nodeToAdd);
      }

      [Observation]
      public void should_not_crash()
      {
         sut.AddNode(_nodeToAdd);
      }
   }

   public class When_retrieving_a_node_from_the_dx_tree_view_by_id_that_does_not_exist : concern_for_UxTreeView
   {
      private ITreeNode _result;

      protected override void Because()
      {
         _result = sut.NodeById("tutu");
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_retrieving_a_node_from_the_dx_tree_view_by_id_that_does_exist : concern_for_UxTreeView
   {
      private ITreeNode _result;
      private ITreeNode _treeNode;
      private string _id;

      protected override void Context()
      {
         base.Context();
         _id = "toto";
         _treeNode = new TestNode(_id, "tutu");
         sut.AddNode(_treeNode);
      }

      protected override void Because()
      {
         _result = sut.NodeById(_id);
      }

      [Observation]
      public void should_return_a_valid_node()
      {
         _result.ShouldBeEqualTo(_treeNode);
      }
   }

   public class When_removing_a_node_from_the_dx_tree_view : concern_for_UxTreeView
   {
      private ITreeNode _parentNode;
      private ITreeNode _childNode1;
      private ITreeNode _childNode2;
      private ITreeNode _childWithChildrenNode1;
      private ITreeNode _childNode3;

      protected override void Context()
      {
         base.Context();
         _parentNode = new TestNode("Parent");
         _childNode1 = new TestNode("Child1");
         _childNode2 = new TestNode("Child2");
         _childWithChildrenNode1 = new TestNode("ChildWithChildren1");
         _childNode3 = new TestNode("Child3");
         _parentNode.AddChildren(_childNode1, _childNode2, _childWithChildrenNode1);
         _childWithChildrenNode1.AddChildren(_childNode3);
         sut.AddNode(_parentNode);
      }

      protected override void Because()
      {
         sut.DestroyNode(_parentNode);
      }

      [Observation]
      public void should_remove_the_node_and_all_its_children_from_the_tree_view()
      {
         sut.Nodes.Count.ShouldBeEqualTo(0);
      }
   }

   internal class When_adding_a_node_to_tree_uses_lazy_loading : concern_for_UxTreeView
   {
      private ITreeNode _parent;
      private ITreeNode _child;
      private ITreeNode _grandChild;

      protected override void Context()
      {
         base.Context();
         sut.ShouldExpandAddedNode = false;
         sut.UseLazyLoading = true;
         _parent = new TestNode("Parent", "Parent");
         _child = new TestNode("Child", "Child");
         _grandChild = new TestNode("GrandChild", "GrandChild");
         _child.AddChild(_grandChild);
         _parent.AddChild(_child);
      }

      protected override void Because()
      {
         sut.AddNode(_parent);
      }

      [Observation]
      public void Should_add_dispaly_node_for_parent()
      {
         sut.NodeFrom(_parent).ShouldNotBeNull();
      }

      [Observation]
      public void Should_add_dispaly_node_for_Child()
      {
         sut.NodeFrom(_child).ShouldNotBeNull();
      }

      [Observation]
      public void Should_not_add_dispaly_node_for_grand_child()
      {
         sut.NodeFrom(_grandChild).ShouldBeNull();
      }
   }

   internal class When_expending_a_node_at_tree_uses_lazy_loading : concern_for_UxTreeView
   {
      private ITreeNode _parent;
      private ITreeNode _child;
      private ITreeNode _grandChild;

      protected override void Context()
      {
         base.Context();
         sut.ShouldExpandAddedNode = false;
         sut.UseLazyLoading = true;
         _parent = new TestNode("Parent", "Parent");
         _child = new TestNode("Child", "Child");
         _grandChild = new TestNode("GrandChild", "GrandChild");
         _child.AddChild(_grandChild);
         _parent.AddChild(_child);
         sut.AddNode(_parent);
      }

      protected override void Because()
      {
         sut.ExpandNode(_parent);
      }

      [Observation]
      public void Should_add_dispaly_node_for_grand_child()
      {
         sut.NodeFrom(_grandChild).ShouldNotBeNull();
      }
   }
}