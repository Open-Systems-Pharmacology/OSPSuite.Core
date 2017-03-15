using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class ImporterTestView : BaseUserControl, IImporterTestView
   {
      private IImporterTestPresenter _presenter;

      public ImporterTestView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         pkSimStartButton.Click += (o, e) => OnEvent(_presenter.StartWithPKSimSettings);
         mobiStartButton.Click += (o, e) => OnEvent(_presenter.StartWithMoBiSettings);
         pkSimSingleStartButton.Click += (o, e) => OnEvent(_presenter.StartPKSimSingleMode);
         testStartButton.Click += (o, e) => OnEvent(_presenter.StartWithTestSettings);
         ontogenyStartButton.Click += (o, e) => OnEvent(_presenter.StartWithOntogenySettings);
         groupByStartButton.Click += (o, e) => OnEvent(_presenter.StartWithTestForGroupBySettings);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         pkSimStartButton.Text = "Start with PK-Sim settings";
         mobiStartButton.Text = "Start with MoBi settings";
         pkSimSingleStartButton.Text = "Start with PK-Sim settings (single)";
         testStartButton.Text = "Start with test settings";
         ontogenyStartButton.Text = "Start with ontogeny settings";
         groupByStartButton.Text = "Start with group-by settings";
      }

      public void AttachPresenter(IImporterTestPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
