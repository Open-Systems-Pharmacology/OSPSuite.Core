using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Helpers;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_AbstractNode : ContextSpecification<AbstractNode>
   {
      protected ITreeNode _node1;
      protected ITreeNode _node2;
      protected ITreeNode _leaf1;
      protected ITreeNode _node11;
      protected ITreeNode _leaf21;
      protected ITreeNode _leaf111;     
 
      protected override void Context()
      {
         base.Context();
         _node1 = new TestNode().WithText("node1");
         _node2 = new TestNode().WithText("node2");
         _leaf1 = new TestNode().WithText("leaf1");
         _node11 = new TestNode().WithText("node11");
         _leaf21 = new TestNode().WithText("leaf21");
         _leaf111 = new TestNode().WithText("leaf111");
         _node1.AddChild(_node11);
         _node11.AddChild(_leaf111);
         _node2.AddChild(_leaf21);
         sut = new TestNode { Text = "root" };
         sut.AddChild(_node1);
         sut.AddChild(_node2);
         sut.AddChild(_leaf1);

      }
   }

   
   public class When_retrieving_all_nodes_from_a_root_node : concern_for_AbstractNode
   {
    
      [Observation]
      public void should_return_all_the_nodes_defined_in_the_hiearchy_including_the_root_node()
      {
         sut.AllNodes.ShouldOnlyContain(_node1,_node2,_leaf1,_node11,_leaf21,_leaf111,sut);
      }
   }
    
   public class When_retrieving_all_leaf_nodes_from_a_root_node : concern_for_AbstractNode
   {
     
      [Observation]
      public void should_return_only_the_leaf_nodes_defined_in_the_hiearchy()
      {
         sut.AllLeafNodes.ShouldOnlyContain( _leaf1,  _leaf21, _leaf111);
      }

   }

   public class When_retrieving_the_full_path_for_a_given_node : concern_for_AbstractNode
   {
      [Observation]
      public void should_return_the_expected_path_for_a_root_node()
      {
         sut.FullPath("-").ShouldBeEqualTo(sut.Text);
         _node1.FullPath("-").ShouldBeEqualTo("root-node1");
      }

      [Observation]
      public void should_return_the_expected_path_for_a_leaf_node()
      {
         _leaf111.FullPath("**").ShouldBeEqualTo("root**node1**node11**leaf111");
      }
   }

   public class When_adding_the_same_node_twice : concern_for_AbstractNode
   {
      [Observation]
      public void should_add_the_node_only_once()
      {
         sut = new TestNode { Text = "root" };

         sut.AddChild(_node1);
         sut.AddChild(_node1);

         sut.Children.ShouldOnlyContain(_node1);
      }
   }
}	