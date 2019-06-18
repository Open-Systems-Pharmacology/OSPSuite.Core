using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalPresenter : IMainViewItemPresenter,
      IPresenterWithContextMenu<IViewItem>,
      IListener<JournalClosedEvent>,
      IListener<JournalLoadedEvent>,
      IListener<JournalPageAddedEvent>,
      IListener<JournalPageDeletedEvent>,
      IListener<EditJournalPageStartedEvent>,
      IListener<JournalPageUpdatedEvent>,
      IListener<JournalSearchPerformedEvent>,
      IListener<JournalClearSearchEvent>,
      IListener<JournalCloseSearchEvent>,
      IListener<ShowJournalEvent>

   {
      void Delete(JournalPageDTO journalPageDTO);

      /// <summary>
      ///    Starts the editing of the <paramref name="journalPageDTO" />
      /// </summary>
      void Edit(JournalPageDTO journalPageDTO);

      /// <summary>
      ///    Starts the editing of the <paramref name="journalPage" />
      /// </summary>
      void Edit(JournalPage journalPage);

      /// <summary>
      ///    When a <paramref name="journalPageDTO" /> is selected, a preview will be shown
      /// </summary>
      /// <param name="journalPageDTO"></param>
      void Select(JournalPageDTO journalPageDTO);

      /// <summary>
      ///    Shows the search form and ensure that working journal view is visible
      /// </summary>
      void ShowSearch();

      IReadOnlyList<string> AvailableTags { get; }

      /// <summary>
      ///    Returns <c>true</c> if all <see cref="JournalPage" /> have the same source otherwise <c>false</c>
      /// </summary>
      bool AllItemsHaveTheSameOrigin { get; }

      /// <summary>
      ///    Returns the currently visible pages from the view
      /// </summary>
      IEnumerable<JournalPage> VisibleJournalPages();
   }

   public abstract class JournalPresenter : AbstractCommandCollectorPresenter<IJournalView, IJournalPresenter>, IJournalPresenter
   {
      private readonly IJournalPageToJournalPageDTOMapper _mapper;
      private readonly IJournalTask _journalTask;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IJournalRetriever _journalRetriever;
      private readonly IJournalPagePreviewPresenter _previewPresenter;
      private readonly IJournalSearchPresenter _searchPresenter;
      private readonly IRegion _region;
      private readonly NotifyList<JournalPageDTO> _allJournalPageDTOs;
      private JournalSearch _journalSearch;

      protected JournalPresenter(
         IJournalView view, 
         IRegionResolver regionResolver, 
         IJournalPageToJournalPageDTOMapper mapper,
         IJournalTask journalTask, 
         IViewItemContextMenuFactory viewItemContextMenuFactory, 
         IJournalRetriever journalRetriever,
         IJournalPagePreviewPresenter previewPresenter, 
         IJournalSearchPresenter searchPresenter, RegionName regionName)
         : base(view)
      {
         _mapper = mapper;
         _journalTask = journalTask;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _journalRetriever = journalRetriever;
         _previewPresenter = previewPresenter;
         _searchPresenter = searchPresenter;
         _region = regionResolver.RegionWithName(regionName);
         _allJournalPageDTOs = new NotifyList<JournalPageDTO>();
         View.AddPreviewView(_previewPresenter.BaseView);
         View.AddSearchView(_searchPresenter.BaseView);
         AddSubPresenters(_previewPresenter, _searchPresenter);
         View.BindTo(_allJournalPageDTOs);
         View.SearchVisible = false;
         _journalSearch = null;
         _region.Add(View);
      }

      public void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      public IEnumerable<JournalPage> VisibleJournalPages()
      {
         return _view.VisibleJournalPages();
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         if (viewItem == null)
            viewItem = new JournalDTO();

         var contextMenu = _viewItemContextMenuFactory.CreateFor(viewItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(JournalClosedEvent eventToHandle)
      {
         _allJournalPageDTOs.Clear();
         _previewPresenter.ClearPreview();
      }

      public void Handle(JournalLoadedEvent eventToHandle)
      {
         loadJournal(eventToHandle.Journal);
      }

      private void loadJournal(OSPSuite.Core.Journal.Journal journal)
      {
         _allJournalPageDTOs.Clear();
         if (journal == null) return;
         journal.JournalPages.Each(x => addJournalPage(x));
         select(journal.Edited);
      }

      private JournalPageDTO addJournalPage(JournalPage journalPage, Func<JournalPage, string> descriptionFunc = null)
      {
         var dto = _mapper.MapFrom(journalPage, descriptionFunc);
         _allJournalPageDTOs.Add(dto);
         return dto;
      }

      public void Handle(JournalPageAddedEvent eventToHandle)
      {
         var dto = addJournalPage(eventToHandle.JournalPage);
         preview(dto);
      }

      public void Delete(JournalPageDTO journalPageDTO)
      {
         _journalTask.DeleteJournalPage(journalPageDTO.JournalPage);
      }

      public void Edit(JournalPageDTO journalPageDTO)
      {
         if (journalPageDTO == null) return;
         Edit(journalPageDTO.JournalPage);
      }

      public void Edit(JournalPage journalPage)
      {
         _journalTask.Edit(journalPage, showEditor: true, journalSearch: _journalSearch);
      }

      public void Select(JournalPageDTO journalPageDTO)
      {
         select(journalPageDTO.JournalPage);
      }

      private void select(JournalPage journalPage)
      {
         if (journalPage == null) return;
         _journalTask.Edit(journalPage, showEditor: false);
      }

      public void ShowSearch()
      {
         _searchPresenter.Activate();
         _view.SearchVisible = true;
         showView();
      }

      public IReadOnlyList<string> AvailableTags
      {
         get { return _allJournalPageDTOs.SelectMany(x => x.Tags).Distinct().OrderBy(x => x).ToList(); }
      }

      public bool AllItemsHaveTheSameOrigin
      {
         get { return _allJournalPageDTOs.Select(x => x.Origin).Distinct().Count() == 1; }
      }

      private void clearSearch()
      {
         loadJournal(_journalRetriever.Current);
      }

      public void Handle(JournalPageDeletedEvent eventToHandle)
      {
         var dto = journalPageDTOFor(eventToHandle.JournalPage);
         if (dto == null) return;
         _allJournalPageDTOs.Remove(dto);
         previewSelectedWorkingJournalItem();
      }

      private void previewSelectedWorkingJournalItem()
      {
         preview(_view.SelectedJournalPage);
      }

      private void preview(JournalPageDTO journalPageDTO)
      {
         _view.SelectedJournalPage = journalPageDTO;
         _previewPresenter.Preview(journalPageDTO);
      }

      private JournalPageDTO journalPageDTOFor(JournalPage journalPage)
      {
         return _allJournalPageDTOs.FirstOrDefault(x => Equals(x.JournalPage, journalPage));
      }

      private void updateCurrentEditedItem(JournalPage journalPage, bool edited)
      {
         _allJournalPageDTOs.Each(x => x.Edited = false);
         var dto = journalPageDTOFor(journalPage);
         dto.Edited = edited;

         if (edited)
         {
            _journalRetriever.Current.Edited = journalPage;
            preview(dto);
         }
         else
            _journalRetriever.Current.Edited = null;

         _view.UpdateLayout();
      }

      public void Handle(EditJournalPageStartedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         updateCurrentEditedItem(eventToHandle.JournalPage, edited: true);
      }

      public void Handle(JournalPageUpdatedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         var journalPage = eventToHandle.JournalPage;
         var journalPageDTO = journalPageDTOFor(journalPage);
         _mapper.Update(journalPageDTO, journalPage);
         previewSelectedWorkingJournalItem();
         _view.UpdateLayout();
      }

      private bool canHandle(JournalPageEvent eventToHandle)
      {
         return journalPageDTOFor(eventToHandle.JournalPage) != null;
      }

      public void Handle(JournalSearchPerformedEvent eventToHandle)
      {
         _journalSearch = eventToHandle.JournalSearch;
         _allJournalPageDTOs.Clear();
         eventToHandle.SearchResults.Each(x => { addJournalPage(x.JournalPage, wji => x.Context); });
      }

      public void Handle(JournalClearSearchEvent eventToHandle)
      {
         clearSearch();
      }

      public void Handle(JournalCloseSearchEvent eventToHandle)
      {
         _journalSearch = null;
         _view.SearchVisible = false;
      }

      public void Handle(ShowJournalEvent eventToHandle)
      {
         showView();
      }

      private void showView()
      {
         _region.Show();
      }
   }
}