using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Services.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_HistoryBrowserPresenter : ContextSpecification<IHistoryBrowserPresenter>
   {
      protected IHistoryBrowserView _view;
      protected IHistoryManager _historyManager;
      protected IList<IHistoryItem> _historyList;
      protected ILabelTask _labelTask;
      protected IHistoryToHistoryDTOMapper _mapper;
      protected ICommentTask _commentTask;
      protected IHistoryBrowserProperties _historyBrowserProperties;
      protected IHistoryBrowserConfiguration _historyBrowserConfiguration;
      private IHistoryItemDTOEnumerableToHistoryItemDTOList _historyItemDTOListMapper;
      protected IHistoryItemDTOList _historyItemDTOList;
      protected IHistoryTask _historyTask;

      protected override void Context()
      {
         _view = A.Fake<IHistoryBrowserView>();
         _historyList = new List<IHistoryItem>();
         _historyManager = A.Fake<IHistoryManager>();
         _historyBrowserProperties = A.Fake<IHistoryBrowserProperties>();
         _labelTask = A.Fake<ILabelTask>();
         _mapper = A.Fake<IHistoryToHistoryDTOMapper>();
         _commentTask = A.Fake<ICommentTask>();
         _historyTask = A.Fake<IHistoryTask>();
         _historyItemDTOListMapper = A.Fake<IHistoryItemDTOEnumerableToHistoryItemDTOList>();
         _historyItemDTOList = A.Fake<IHistoryItemDTOList>();
         A.CallTo(_historyItemDTOListMapper).WithReturnType<IHistoryItemDTOList>().Returns(_historyItemDTOList);
         _historyBrowserConfiguration = new HistoryBrowserConfiguration();
         A.CallTo(() => _historyManager.History).Returns(_historyList);
         sut = new HistoryBrowserPresenter(_view, _labelTask, _commentTask, _mapper, _historyItemDTOListMapper, _historyTask);
         sut.Initialize();
         sut.HistoryManager = _historyManager;
      }
   }

   public class When_told_to_initialize : concern_for_HistoryBrowserPresenter
   {
      [Observation]
      public void should_attach_itself_to_the_view()
      {
         A.CallTo(() => _view.AttachPresenter(sut)).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_view()
      {
         A.CallTo(() => _view.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_available_columns_to_the_view()
      {
         A.CallTo(() => _view.AddColumn(HistoryColumns.CommandType)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.Description)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.Id)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.ObjectType)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.State)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.Time)).MustHaveHappened();
         A.CallTo(() => _view.AddColumn(HistoryColumns.User)).MustHaveHappened();
      }
   }

   public class When_the_browser_is_asked_to_update_the_history : concern_for_HistoryBrowserPresenter
   {
      protected override void Because()
      {
         sut.UpdateHistory();
      }

      [Observation]
      public void should_tell_the_view_to_bind_to_the_history()
      {
         A.CallTo(() => _view.BindTo(A<IHistoryItemDTOList>._)).MustHaveHappened();
      }
   }

   public class When_notified_that_the_history_was_cleared : concern_for_HistoryBrowserPresenter
   {
      protected override void Because()
      {
         sut.Handle(new HistoryClearedEvent());
      }

      [Observation]
      public void should_refresh_the_view()
      {
         A.CallTo(() => _view.BindTo(A<IHistoryItemDTOList>._)).MustHaveHappened();
      }
   }

   public class When_performing_a_rollback_command : concern_for_HistoryBrowserPresenter
   {
      private int _stateToRollBackTo;

      protected override void Context()
      {
         base.Context();
         sut.Initialize();
         sut.HistoryManager = _historyManager;
         _stateToRollBackTo = 2;
      }

      protected override void Because()
      {
         sut.RollBack(_stateToRollBackTo);
      }

      [Observation]
      public void should_leverage_the_underlying_history_manager_to_perform_a_rollback()
      {
         A.CallTo(() => _historyManager.RollBackTo(_stateToRollBackTo)).MustHaveHappened();
      }
   }

   public class When_asked_to_create_a_label : concern_for_HistoryBrowserPresenter
   {
      protected override void Because()
      {
         sut.AddLabel();
      }

      [Observation]
      public void should_leverage_the_label_task_to_create_a_new_label_history()
      {
         A.CallTo(() => _labelTask.AddLabelTo(_historyManager)).MustHaveHappened();
      }
   }

   public class When_the_user_triggers_a_clear_history_action_from_the_history_browser : concern_for_HistoryBrowserPresenter
   {
      protected override void Because()
      {
         sut.ClearHistory();
      }

      [Observation]
      public void should_leverage_the_history_task_to_clear_the_history()
      {
         A.CallTo(() => _historyTask.ClearHistory()).MustHaveHappened();
      }
   }

   public class When_retrieving_the_state_for_an_history_item_from_the_history_id : concern_for_HistoryBrowserPresenter
   {
      private string _historyItemId;
      private IHistoryItem _historyItem;
      private IHistoryItemDTO _historyItemDTO;

      protected override void Context()
      {
         base.Context();
         _historyItemId = "tutu";
         _historyItem = A.Fake<IHistoryItem>();
         _historyList.Add(_historyItem);
         _historyItemDTO = A.Fake<IHistoryItemDTO>();
         _historyItemDTO.Id = _historyItemId;
         _historyItemDTO.State = 5;
         A.CallTo(() => _mapper.MapFrom(_historyItem)).Returns(_historyItemDTO);
         A.CallTo(() => _historyItemDTOList.ItemById(_historyItemId)).Returns(_historyItemDTO);
         sut.UpdateHistory();
      }

      [Observation]
      public void should_return_the_state_of_that_history_item()
      {
         sut.StateFor(_historyItemId).ShouldBeEqualTo(_historyItemDTO.State);
      }
   }

   public class When_asked_if_an_history_item_is_a_label : concern_for_HistoryBrowserPresenter
   {
      private IHistoryItem _labelHistoryItem;
      private string _notALabelItemId;
      private string _aLabelItemId;
      private IHistoryItem _notALabelHistoryItem;
      private IHistoryItemDTO _labelHistoryItemDTO;
      private IHistoryItemDTO _notALabelHistoryItemDTO;

      protected override void Context()
      {
         base.Context();
         _notALabelItemId = "_notALabelItemId";
         _aLabelItemId = "_aLabelItemId";
         _labelHistoryItem = A.Fake<IHistoryItem>();
         A.CallTo(() => _labelHistoryItem.Command).Returns(A.Fake<ILabelCommand>());
         _notALabelHistoryItem = A.Fake<IHistoryItem>();
         A.CallTo(() => _notALabelHistoryItem.Command).Returns(A.Fake<ICommand>());
         _historyList.Add(_labelHistoryItem);
         _historyList.Add(_notALabelHistoryItem);

         _labelHistoryItemDTO = A.Fake<IHistoryItemDTO>().WithId(_aLabelItemId);
         A.CallTo(() => _labelHistoryItemDTO.Command).Returns(_labelHistoryItem.Command);
         _notALabelHistoryItemDTO = A.Fake<IHistoryItemDTO>().WithId(_notALabelItemId);
         A.CallTo(() => _notALabelHistoryItemDTO.Command).Returns(_notALabelHistoryItem.Command);
         A.CallTo(() => _mapper.MapFrom(_labelHistoryItem)).Returns(_labelHistoryItemDTO);
         A.CallTo(() => _mapper.MapFrom(_notALabelHistoryItem)).Returns(_notALabelHistoryItemDTO);
         A.CallTo(() => _historyItemDTOList.ItemById(_aLabelItemId)).Returns(_labelHistoryItemDTO);
         sut.UpdateHistory();
      }

      [Observation]
      public void should_return_true_for_a_label_history_item()
      {
         sut.IsLabel(_aLabelItemId).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_for_any_other_history()
      {
         sut.IsLabel(_notALabelItemId).ShouldBeFalse();
      }
   }

   public class When_addind_a_comment_for_a_command : concern_for_HistoryBrowserPresenter
   {
      private string _historyItemId;
      private IHistoryItem _historyItem;
      private IHistoryItemDTO _historyItemDTO;

      protected override void Context()
      {
         base.Context();
         _historyItem = A.Fake<IHistoryItem>();
         _historyList.Add(_historyItem);
         _historyItemId = "tutu";
         _historyItemDTO = A.Fake<IHistoryItemDTO>().WithId(_historyItemId);
         _historyItemDTO.State = 5;
         A.CallTo(() => _mapper.MapFrom(_historyItem)).Returns(_historyItemDTO);
         sut.HistoryManager = _historyManager;
         A.CallTo(() => _historyItemDTOList.ItemById(_historyItemId)).Returns(_historyItemDTO);

         sut.UpdateHistory();
      }

      protected override void Because()
      {
         sut.EditCommentFor(_historyItemId);
      }

      [Observation]
      public void should_leverage_the_add_comment_task_to_add_the_comment_to_the_given_history()
      {
         A.CallTo(() => _commentTask.EditCommentFor(_historyItemDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_view()
      {
         A.CallTo(() => _view.RefreshView()).MustHaveHappened();
      }
   }
}