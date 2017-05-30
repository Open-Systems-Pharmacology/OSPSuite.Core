using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ObjectBaseNode : ContextSpecification<ITreeNode<IEntity>>
   {
      protected IEntity _entity;

      protected override void Context()
      {
         _entity = new Parameter().WithId("id").WithName("tutu");
         sut = new ObjectWithIdAndNameNode<IEntity>(_entity);
      }
   }

   public class When_retrieving_the_name_of_an_object_base_node : concern_for_ObjectBaseNode
   {
      [Observation]
      public void should_return_the_name_of_the_underlying_entity()
      {
         sut.Text.ShouldBeEqualTo(_entity.Name);
      }
   }

   public class When_retrieving_the_id_of_an_object_base_node : concern_for_ObjectBaseNode
   {
      [Observation]
      public void should_return_the_id_of_the_underlying_entity()
      {
         sut.Id.ShouldBeEqualTo(_entity.Id);
      }
   }

   public class When_the_underlying_object_base_name_has_been_changed : concern_for_ObjectBaseNode
   {
      private bool _eventReceived;

      protected override void Context()
      {
         base.Context();
         sut.TextChanged += node => _eventReceived = true;
      }

      protected override void Because()
      {
         _entity.Name = "tralala";
      }

      [Observation]
      public void should_notify_that_its_name_has_changed()
      {
         _eventReceived.ShouldBeTrue();
      }
   }

   public class When_removing_a_child_node : concern_for_ObjectBaseNode
   {
      private IEntity _anotherEntity;
      private ITreeNode<IEntity> _childNode;
      private ITreeNode<IEntity> _anotherChild;

      protected override void Context()
      {
         base.Context();
         _anotherEntity = new Parameter().WithId("tata").WithName("tutu");
         _childNode = new ObjectWithIdAndNameNode<IEntity>(_anotherEntity);
         _anotherChild = A.Fake<ITreeNode<IEntity>>();
         sut.AddChild(_childNode);
         sut.AddChild(_anotherChild);
      }

      protected override void Because()
      {
         sut.RemoveChild(_childNode);
         sut.RemoveChild(_anotherChild);
      }

      [Observation]
      public void should_remove_the_node_from_the_internal_list_of_children()
      {
         sut.Children.Contains(_childNode).ShouldBeFalse();
      }
   }
}