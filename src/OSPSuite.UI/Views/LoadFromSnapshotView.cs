using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class LoadFromSnapshotView : BaseModalView, ILoadFromSnapshotView
   {
      private ILoadFromSnapshotPresenter _presenter;
      private readonly ScreenBinder<LoadFromSnapshotDTO> _screenBinder = new ScreenBinder<LoadFromSnapshotDTO>();

      //only for design time
      public LoadFromSnapshotView() : this(null)
      {
      }

      public LoadFromSnapshotView(IShell shell) : base(shell)
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.SnapshotFile)
            .To(buttonEditSelectSnapshot);

         _screenBinder.Bind(x => x.RunSimulations)
            .To(chkRunSimulations)
            .WithCaption(Captions.RunSimulations);

         buttonEditSelectSnapshot.ButtonClick += (o, e) => OnEvent(() => _presenter.SelectFile());
         buttonStart.Click += (o, e) => OnEvent(() => _presenter.StartAsync());

         RegisterValidationFor(_screenBinder);
      }

      public void AttachPresenter(ILoadFromSnapshotPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddLogView(IView view)
      {
         logPanel.FillWith(view);
      }

      public void BindTo(LoadFromSnapshotDTO loadFromSnapshotDTO)
      {
         _screenBinder.BindToSource(loadFromSnapshotDTO);
      }

      public void EnableButtons(bool cancelEnabled, bool okEnabled = false, bool startEnabled = false)
      {
         OkEnabled = okEnabled;
         CancelEnabled = cancelEnabled;
         buttonStart.Enabled = startEnabled;
      }

      protected override void SetOkButtonEnable()
      {
         base.SetOkButtonEnable();
         buttonStart.Enabled = !HasError;
      }

      protected override bool IsOkButtonEnable => _presenter.ModelIsDefined;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemStartButton.AdjustLargeButtonSize(layoutControl);
         buttonStart.InitWithImage(ApplicationIcons.Run, Captions.StartImport);
         layoutItemButtonSelectSnapshot.Text = Captions.SnapshotFile.FormatForLabel();
         ApplicationIcon = ApplicationIcons.Snapshot;
      }

      public override bool HasError => _screenBinder.HasError;

      protected override bool ShouldClose => _presenter.ShouldClose;
   }
}
