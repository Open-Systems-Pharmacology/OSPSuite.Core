using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation
{
    public class When_adding_a_node_under_another_node : StaticContextSpecification
    {
        private ITreeNode _parentNode;
        private ITreeNode _childNode;

        protected override void Context()
        {
            _parentNode = A.Fake<ITreeNode>();
            _childNode = A.Fake<ITreeNode>();
        }

        protected override void Because()
        {
            _childNode.Under(_parentNode);
        }

        [Observation]
        public void the_added_node_should_be_a_child_node_of_the_parent_node()
        {
           A.CallTo(()=>_parentNode.AddChild(_childNode)).MustHaveHappened();
        }
    }

    public class When_adding_a_node_under_a_null_node : StaticContextSpecification
   {
      [Observation]
      public void should_not_crash()
      {
         var node = A.Fake<ITreeNode>();
         node.Under(null);
      }
   }
}