using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_EntityDescriptorMapList : ContextSpecification<EntityDescriptorMapList<IContainer>>
   {
      protected override void Context()
      {
         sut = new EntityDescriptorMapList<IContainer>();
      }
   }

   public class When_resolving_all_sub_containers_fulfilling_a_given_conditions : concern_for_EntityDescriptorMapList
   {
      private DescriptorCriteria _conditions;
      private IContainer _subContainerFulfilling;
      private IContainer _subContainerNotFulfilling;
      private IContainer _subSubContainerFulfilling;
      private IContainer _subSubContainerNotFulfilling;
      private IEnumerable<IContainer> _results;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _conditions = Create.Criteria(x => x.With("C1"));
         _subContainerFulfilling = new Container().WithName("C1");
         _subContainerNotFulfilling = new Container().WithName("C2");
         _subSubContainerFulfilling = new Container().WithName("C3");
         _subSubContainerFulfilling.AddTag("C1");
         _subSubContainerNotFulfilling = new Container().WithName("C4");
         _container.Add(_subContainerFulfilling);
         _container.Add(_subContainerNotFulfilling);
         _subContainerFulfilling.Add(_subSubContainerNotFulfilling);
         _subContainerNotFulfilling.Add(_subSubContainerFulfilling);

         sut = new[] {_container, _subContainerFulfilling, _subSubContainerFulfilling, _subSubContainerNotFulfilling}.ToEntityDescriptorMapList();
      }

      protected override void Because()
      {
         _results = sut.AllSatisfiedBy(_conditions);
      }

      [Observation]
      public void should_return_only_the_sub_containers_whose_descriptors_match_the_condition()
      {
         _results.ShouldOnlyContain(_subContainerFulfilling, _subSubContainerFulfilling);
      }

      [Observation]
      public void should_not_return_the_sub_containers_whose_descriptors_do_not_match_the_condition()
      {
         _results.Contains(_subContainerNotFulfilling).ShouldBeFalse();
         _results.Contains(_subSubContainerNotFulfilling).ShouldBeFalse();
      }
   }

   public class When_resolving_all_containers_fulfilling_a_given_conditions : concern_for_EntityDescriptorMapList
   {
      private DescriptorCriteria _conditions;
      private IContainer _subContainerFulfilling;
      private IContainer _subContainerNotFulfilling;
      private IEnumerable<IContainer> _results;
      private Container _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("C");

         _conditions = A.Fake<DescriptorCriteria>();
         _subContainerFulfilling = new Container().WithName("C1");
         _subContainerFulfilling.AddTag("C");
         _subContainerNotFulfilling = new Container().WithName("C2");
         _container.Add(_subContainerFulfilling);
         _container.Add(_subContainerNotFulfilling);
         _conditions = Create.Criteria(x => x.With("C"));
         sut = new[] {_container, _subContainerFulfilling, _subContainerNotFulfilling}.ToEntityDescriptorMapList();
      }

      protected override void Because()
      {
         _results = sut.AllSatisfiedBy(_conditions);
      }

      [Observation]
      public void should_return_only_containers_whose_descriptors_match_the_condition()
      {
         _results.ShouldOnlyContain(_subContainerFulfilling, _container);
      }
   }
}