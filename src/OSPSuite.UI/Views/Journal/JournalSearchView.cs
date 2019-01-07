using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO.Journal;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Journal
{
   public partial class JournalSearchView : BaseUserControl, IJournalSearchView
   {
      private IJournalSearchPresenter _presenter;
      private readonly ScreenBinder<JournalSearchDTO> _screenBinder;
      private readonly EditorButton _advancedOptionButton;
      private JournalSearchDTO _searchDTO;
      private readonly EditorButton _closeSearchButton;
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };

      public JournalSearchView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<JournalSearchDTO>();
         _advancedOptionButton = new EditorButton(ButtonPredefines.Right);
         _closeSearchButton = new EditorButton(ButtonPredefines.Close) {IsLeft = true};
         tbSearch.Properties.Buttons.Add(_advancedOptionButton);
         tbSearch.Properties.Buttons.Add(_closeSearchButton);
      }

      public void AttachPresenter(IJournalSearchPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.Search)
            .To(tbSearch);

         _screenBinder.Bind(x => x.MatchAny)
            .To(chkMatchAny)
            .WithCaption(Captions.Journal.SearchMatchAny);

         _screenBinder.Bind(x => x.MatchWholePhrase)
            .To(chkMatchWholeWord)
            .WithCaption(Captions.Journal.SearchMatchWholePhrase);

         _screenBinder.Bind(x => x.MatchCase)
            .To(chkCaseSensitive)
            .WithCaption(Captions.Journal.SearchMatchCase);

         tbSearch.ButtonClick += (o, e) => OnEvent(buttonClicked, e);
         btnFind.Click += (o, e) => OnEvent(startSearch);
         btnClear.Click += (o, e) => OnEvent(clearSearch);
      }

      private void buttonClicked(ButtonPressedEventArgs e)
      {
         if (e.Button == _advancedOptionButton)
         {
            _searchDTO.ShowAdvancedOptions = !_searchDTO.ShowAdvancedOptions;
            updateOptionVisibility();
         }
         else if (e.Button == _closeSearchButton)
            closeSearch();
      }

      public IEnumerable<string> AvailableSearchTerms
      {
         set { tbSearch.FillWith(value); }
      }

      private void closeSearch()
      {
         _presenter.CloseSearch();
      }

      private void clearSearch()
      {
         _presenter.ClearSearch();
      }

      private void startSearch()
      {
         _searchDTO.Search = tbSearch.Text;
         _presenter.StartSearch();
      }

      protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
      {
         if (keyData.IsPressed(Keys.Enter))
         {
            startSearch();
            return true;
         }

         if (keyData.IsPressed(Keys.Escape))
         {
            clearSearch();
            return true;
         }

         return base.ProcessCmdKey(ref msg, keyData);
      }

      public void BindTo(JournalSearchDTO searchDTO)
      {
         _searchDTO = searchDTO;
         _screenBinder.BindToSource(searchDTO);
         updateOptionVisibility();
      }

      public void Activate()
      {
         ActiveControl = tbSearch;
      }

      private void updateOptionVisibility()
      {
         layoutGroupOptions.Visibility = LayoutVisibilityConvertor.FromBoolean(_searchDTO.ShowAdvancedOptions);
         _advancedOptionButton.Kind = _searchDTO.ShowAdvancedOptions ? ButtonPredefines.Down : ButtonPredefines.Right;
         AdjustHeight();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemSearch.TextVisible = false;
         layoutControl.AutoScroll = false;
         btnFind.InitWithImage(ApplicationIcons.Search, Captions.Journal.Find);
         btnClear.Text = Captions.Journal.Clear;
         layoutItemButtonClear.AdjustControlWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH);
         layoutItemButtonSearch.AdjustControlWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH);
      }

      public void AdjustHeight()
      {
         HeightChanged(this, new ViewResizedEventArgs(OptimalHeight));
      }

      public void Repaint()
      {
         /*nothing to do here*/
      }

      public int OptimalHeight
      {
         get
         {
            var height = layoutItemSearch.Height;
            if (layoutGroupOptions.Visible)
               height += layoutGroupOptions.Height;
            else
               height += layoutItemSearch.Padding.Height;

            return height;
         }
      }
   }
}