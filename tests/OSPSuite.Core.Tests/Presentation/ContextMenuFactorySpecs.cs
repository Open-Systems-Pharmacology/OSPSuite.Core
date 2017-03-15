using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using FakeItEasy;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ContextMenuFactory : ContextSpecification<IContextMenuFactory<ITreeNode>>
   {
      protected IRepository<IContextMenuSpecificationFactory<ITreeNode>> _repository;
      protected IPresenterWithContextMenu<ITreeNode> _presenter;
      protected IList<IContextMenuSpecificationFactory<ITreeNode>> _allContextMenus;

      protected override void Context()
      {
         _repository = A.Fake<IRepository<IContextMenuSpecificationFactory<ITreeNode>>>();
         _presenter = A.Fake<IPresenterWithContextMenu<ITreeNode>>();
         _allContextMenus = new List<IContextMenuSpecificationFactory<ITreeNode>>();
         A.CallTo(() => _repository.All()).Returns(_allContextMenus);
      }
   }

   public class When_creating_a_context_menu_for_a_given_type : concern_for_ContextMenuFactory
   {
      private ITreeNode _node;
      private IContextMenuSpecificationFactory<ITreeNode> _nodeContextMenuFactorySatisfying;
      private IContextMenuSpecificationFactory<ITreeNode> _nodeContextMenuFactoryNotSatisfying;

      protected override void Context()
      {
         base.Context();
         _nodeContextMenuFactorySatisfying = A.Fake<IContextMenuSpecificationFactory<ITreeNode>>();
         _nodeContextMenuFactoryNotSatisfying = A.Fake<IContextMenuSpecificationFactory<ITreeNode>>();
         _node = A.Fake<ITreeNode>();
         _allContextMenus.Add(_nodeContextMenuFactorySatisfying);
         _allContextMenus.Add(_nodeContextMenuFactoryNotSatisfying);
         A.CallTo(() => _nodeContextMenuFactorySatisfying.IsSatisfiedBy(_node, _presenter)).Returns(true);
         A.CallTo(() => _nodeContextMenuFactoryNotSatisfying.IsSatisfiedBy(_node, _presenter)).Returns(false);
         sut = new ContextMenuFactory<ITreeNode>(_repository);
      }

      protected override void Because()
      {
         sut.CreateFor(_node, _presenter);
      }

      [Observation]
      public void should_iterate_through_all_the_factory_in_the_factory_repository()
      {
         A.CallTo(() => _repository.All()).MustHaveHappened();
      }

      [Observation]
      public void should_return_the_presenter_from_the_first_factory_that_satisfies_the_specifications()
      {
         A.CallTo(() => _nodeContextMenuFactorySatisfying.IsSatisfiedBy(_node, _presenter)).MustHaveHappened();
         A.CallTo(() => _nodeContextMenuFactorySatisfying.CreateFor(_node, _presenter)).MustHaveHappened();
         A.CallTo(() => _nodeContextMenuFactoryNotSatisfying.IsSatisfiedBy(_node, _presenter)).MustNotHaveHappened();
         A.CallTo(() => _nodeContextMenuFactoryNotSatisfying.CreateFor(_node, _presenter)).MustNotHaveHappened();
      }
   }

   public class When_creating_a_context_menu_for_a_type_that_was_not_registered : concern_for_ContextMenuFactory
   {
      private ITreeNode _node;
      private IContextMenuSpecificationFactory<ITreeNode> _nodeContextMenuFactory;
      private IContextMenu _result;

      protected override void Context()
      {
         base.Context();
         _nodeContextMenuFactory = A.Fake<IContextMenuSpecificationFactory<ITreeNode>>();
         _node = A.Fake<ITreeNode>();
         _allContextMenus.Add(_nodeContextMenuFactory);
         A.CallTo(() => _nodeContextMenuFactory.IsSatisfiedBy(_node, _presenter)).Returns(false);
         sut = new ContextMenuFactory<ITreeNode>(_repository);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_node, _presenter);
      }

      [Observation]
      public void should_return_an_empty_context_menu()
      {
         _result.ShouldBeAnInstanceOf<EmptyContextMenu>();
      }
   }
}