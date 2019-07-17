using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_TreeNodeFactory : ContextSpecification<ITreeNodeFactory>
   {
      protected IToolTipPartCreator _toolTipPartCreator;
      private IObservedDataRepository _observedDataRepository;

      protected override void Context()
      {
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();
         sut = new TreeNodeFactory(_observedDataRepository, _toolTipPartCreator);
      }
   }

   public class When_creating_a_node_for_a_data_repository : concern_for_TreeNodeFactory
   {
      private DataRepository _dataRepository;
      private ITreeNode _node;

      protected override void Context()
      {
         base.Context();
         _dataRepository = new DataRepository().WithId("id").WithName("toto");
      }

      protected override void Because()
      {
         _node = sut.CreateFor(new ClassifiableObservedData {Subject = _dataRepository});
      }

      [Observation]
      public void should_return_an_observed_data_node()
      {
         _node.ShouldBeAnInstanceOf<ObservedDataNode>();
      }

      [Observation]
      public void should_return_a_node_whose_id_was_set_to_the_repository_id()
      {
         _node.Id.ShouldBeEqualTo(_dataRepository.Id);
      }

      [Observation]
      public void should_return_a_node_whose_text_was_set_to_the_repository_name()
      {
         _node.Text.ShouldBeEqualTo(_dataRepository.Name);
      }
   }
}