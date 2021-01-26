using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Importer
{
   public partial class NanView : BaseUserControl, INanView
   {
      private INanPresenter _presenter;
      public NanView()
      {
         InitializeComponent();
         fillNanOptions();
         actionImageComboBoxEdit.SelectedValueChanged += (s, a) => OnEvent(() =>
         {
            _presenter.Settings.Action = (NanSettings.ActionType) actionImageComboBoxEdit.SelectedIndex;
            _presenter.NewNaNSettings();
         });
         indicatorTextEdit.TextChanged += (s, a) => OnEvent(() =>
         {
            _presenter.Settings.Indicator = indicatorTextEdit.Text;
            _presenter.NewNaNSettings();
         });
      }

      private void fillNanOptions()
      {
         actionImageComboBoxEdit.Properties.Items.Clear();
         actionImageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(NanSettings.ActionType.IgnoreRow)
         {
            Description = Captions.Importer.NanActionIgnoreRow,
            ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.UncheckAll)
         });
         actionImageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(NanSettings.ActionType.Throw)
         {
            Description = Captions.Importer.NanActionThrowsError,
            ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.Exit)
         });
         actionImageComboBoxEdit.SelectedIndex = 0;
      }

      public void AttachPresenter(INanPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         indicatorLayoutControlItem.Text = Captions.Importer.NanIndicator.FormatForLabel(false);
         actionLayoutControlItem.Text = Captions.Importer.NanAction.FormatForLabel(false);
      }
   }
}
