using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Services.Commands;
using OSPSuite.Presentation.Views.Commands;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Commands
{
   public interface IHistoryBrowserPresenter : 
      IListener<HistoryClearedEvent>
   {
      /// <summary>
      ///    Refreshes the history in the view
      /// </summary>
      void UpdateHistory();

      /// <summary>
      ///    Adjust column width
      /// </summary>
      void BestFitColumns();

      /// <summary>
      ///    Performs a roll back to the given state
      /// </summary>
      void RollBack(int state);

      /// <summary>
      ///    Initializes the presenter (set the view, add dynamic columns etc..)
      /// </summary>
      void Initialize();

      /// <summary>
      ///    Returns whether the history item with the given id is a label command.
      /// </summary>
      bool IsLabel(string historyItemId);

      /// <summary>
      ///    Returns whether a rollback can be performd to the command in the history item with the given id
      /// </summary>
      bool CanRollBackTo(string historyItemId);

      /// <summary>
      ///    Adds a label to the history
      /// </summary>
      void AddLabel();


      /// <summary>
      /// Triggers the clear history command that will clear the history in the project
      /// </summary>
      void ClearHistory();

      /// <summary>
      ///    Returns the view associated with the history browser
      /// </summary>
      IHistoryBrowserView View { get; }

      /// <summary>
      ///    returns the state defined for the history item with the given id
      /// </summary>
      int StateFor(string historyItemId);

      /// <summary>
      ///    Allows the end user to edit the comments for the history with the given id
      /// </summary>
      void EditCommentFor(string historyItemId);

      /// <summary>
      ///    Gets or sets the history maanger
      /// </summary>
      IHistoryManager HistoryManager { get; set; }

      /// <summary>
      ///    Returns the DTO for the given history item id
      /// </summary>
      IHistoryItemDTO HistoryItemFrom(string historyItemId);

      /// <summary>
      ///    Defines if the filtering capability is enabled.
      /// </summary>
      bool EnableFiltering { set; }

      /// <summary>
      ///    Defines if the auto filtering is enabled. (Note: Filtering has to be enabled as well to use this feature)
      /// </summary>
      bool EnableAutoFilterRow { set; }

      /// <summary>
      ///    This should be called whenever a item is added to the history
      /// </summary>
      void HistoryItemAdded(IHistoryItem historyItem);

      /// <summary>
      ///    Starts the undo action
      /// </summary>
      void Undo();

      /// <summary>
      ///    Set to <c>true</c> (default), the list of command to display will be analyzed and simplified. This can be very time
      ///    consuming for big projects
      /// </summary>
      bool EnableHistoryPruning { set; }
   }

   public class HistoryBrowserPresenter : IHistoryBrowserPresenter
   {
      private IHistoryManager _historyManager;
      private readonly ILabelTask _labelTask;
      private readonly ICommentTask _commentTask;
      private readonly IHistoryToHistoryDTOMapper _mapper;
      private readonly IHistoryItemDTOEnumerableToHistoryItemDTOList _historyItemDTOListMapper;
      private readonly IHistoryTask _historyTask;
      private IHistoryItemDTOList _historyItemDtoList;

      public HistoryBrowserPresenter(IHistoryBrowserView view, ILabelTask labelTask, ICommentTask commentTask,
         IHistoryToHistoryDTOMapper mapper, IHistoryItemDTOEnumerableToHistoryItemDTOList historyItemDTOListMapper, IHistoryTask historyTask)
      {
         View = view;
         _labelTask = labelTask;
         _commentTask = commentTask;
         _mapper = mapper;
         _historyItemDTOListMapper = historyItemDTOListMapper;
         _historyTask = historyTask;
         EnableFiltering = true;
         EnableAutoFilterRow = true;
         EnableHistoryPruning = true;
         _historyItemDtoList = historyItemDTOListMapper.MapFrom(Enumerable.Empty<IHistoryItemDTO>());
      }

      public void Initialize()
      {
         View.AttachPresenter(this);
         View.Clear();
         HistoryColumns.All().Each(View.AddColumn);
         View.UpdateColumnPosition();
      }

      public bool EnableHistoryPruning
      {
         set => _mapper.EnableHistoryPruning = value;
      }

      public bool IsLabel(string historyItemId)
      {
         return HistoryItemFrom(historyItemId).Command.IsAnImplementationOf<ILabelCommand>();
      }

      public bool CanRollBackTo(string historyItemId)
      {
         return HistoryItemFrom(historyItemId).Command.Loaded;
      }

      public IHistoryItemDTO HistoryItemFrom(string historyItemId)
      {
         return _historyItemDtoList.ItemById(historyItemId);
      }

      public bool EnableFiltering
      {
         set => View.EnableFiltering = value;
      }

      public bool EnableAutoFilterRow
      {
         set => View.EnableAutoFilterRow = value;
      }

      public void AddLabel()
      {
         _labelTask.AddLabelTo(HistoryManager);
         UpdateHistory();
      }

      public void ClearHistory()
      {
         _historyTask.ClearHistory();
      }

      public IHistoryBrowserView View { get; }

      public int StateFor(string historyItemId)
      {
         return HistoryItemFrom(historyItemId).State;
      }

      public void EditCommentFor(string historyItemId)
      {
         var historyItem = HistoryItemFrom(historyItemId);
         _commentTask.EditCommentFor(historyItem);
         View.RefreshView();
      }

      public void HistoryItemAdded(IHistoryItem historyItem)
      {
         var historyItemDTO = _mapper.MapFrom(historyItem);
         if (historyItemDTO.IsAnImplementationOf<NullHistoryItemDTO>()) return;
         _historyItemDtoList.AddAtFront(historyItemDTO);
         View.BindTo(_historyItemDtoList);
      }

      public void Undo()
      {
         HistoryManager.Undo();
      }

      public void UpdateHistory()
      {
         //We display the history in the reversed chronological order
         var reversedHistory = HistoryManager.History.Reverse();
         var allhistoryItemsDTO = reversedHistory.MapAllUsing(_mapper).Where(dto => !dto.IsAnImplementationOf<NullHistoryItemDTO>());
         _historyItemDtoList = _historyItemDTOListMapper.MapFrom(allhistoryItemsDTO);
         View.BindTo(_historyItemDtoList);
      }

      public void BestFitColumns()
      {
         View.BestFitColumns();
      }

      public void RollBack(int state)
      {
         HistoryManager.RollBackTo(state);
      }

      public IHistoryManager HistoryManager
      {
         get => _historyManager ?? new NullHistoryManager();
         set
         {
            if (_historyManager != null)
               _historyManager.CommandAdded -= HistoryItemAdded;

            _historyManager = value;

            if (_historyManager != null)
               _historyManager.CommandAdded += HistoryItemAdded;
         }
      }

      public void Handle(HistoryClearedEvent eventToHandle)
      {
         UpdateHistory();
      }
   }
}