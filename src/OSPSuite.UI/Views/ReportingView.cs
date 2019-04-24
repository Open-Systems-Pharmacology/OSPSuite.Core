using System;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Reporting;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class ReportingView : BaseModalView, IReportingView
   {
      private IReportingPresenter _presenter;
      private readonly ScreenBinder<ReportConfiguration> _screenBinder;
      private ReportConfiguration _reportConfiguration;

      public ReportingView(IShell shell) : base(shell)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ReportConfiguration>();
      }

      public void AttachPresenter(IReportingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ReportConfiguration reportConfiguration)
      {
         _reportConfiguration = reportConfiguration;
         _screenBinder.BindToSource(reportConfiguration);
         rgColor.EditValue = reportConfiguration.ColorStyle;
         updateTemplateDescription(reportConfiguration.Template);
      }

      public bool IsDeveloperMode
      {
         set => layoutItemDeleteWorkingFolder.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.Template)
            .To(cbReportTemplates)
            .WithValues(dto => _presenter.AllTemplates())
            .AndDisplays(dto => dto.DisplayName)
            .OnValueUpdating += (o, e) => updateTemplateDescription(e.NewValue);

         _screenBinder.Bind(x => x.Verbose)
            .To(chkVerbose)
            .WithCaption(Captions.Reporting.Verbose);

         _screenBinder.Bind(x => x.OpenReportAfterCreation)
            .To(chkOpenReportAfterCreation)
            .WithCaption(Captions.Reporting.OpenReportAfterCreation);

         _screenBinder.Bind(x => x.Author)
            .To(tbAuthor);

         _screenBinder.Bind(x => x.Title)
            .To(tbTitle);

         _screenBinder.Bind(x => x.SubTitle)
            .To(tbSubtitle);

         _screenBinder.Bind(x => x.SaveArtifacts)
            .To(chkSaveReportArtifacts)
            .WithCaption(Captions.Reporting.SaveArtifacts);

         _screenBinder.Bind(x => x.DeleteWorkingDir)
            .To(chkDeleteWorkingFolder)
            .WithCaption(Captions.Reporting.DeleteWorkingFolder);

         _screenBinder.Bind(x => x.Draft)
            .To(chkDraft)
            .WithCaption(Captions.Reporting.Draft);

         _screenBinder.Bind(x => x.Font)
            .To(cbFont)
            .WithValues(dto => _presenter.AvailableFonts());

         rgColor.SelectedIndexChanged += colorChanged;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void colorChanged(object sender, EventArgs e)
      {
         var radioGroup = sender as RadioGroup;
         if (radioGroup == null) return;
         _reportConfiguration.ColorStyle = (ReportColorStyles) radioGroup.Properties.Items[radioGroup.SelectedIndex].Value;
      }

      private void updateTemplateDescription(ReportTemplate template)
      {
         lblTemplateDescription.Text = $"<I>{template.Description}</I>";
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutGroupFirstPage.Text = Captions.Reporting.FirstPageSettings;
         layoutGroupOptions.Text = Captions.Reporting.Options;
         layoutItemAuthor.Text = Captions.Reporting.Author.FormatForLabel();
         layoutItemTitle.Text = Captions.Reporting.Title.FormatForLabel();
         layoutItemSubtitle.Text = Captions.Reporting.Subtitle.FormatForLabel();
         layoutItemFont.Text = Captions.Reporting.Font.FormatForLabel();
         layoutItemColor.TextVisible = false;
         btnOk.Text = Captions.Reporting.CreateReport;
         Caption = Captions.Reporting.ReportToPDFTitle;
         Icon = ApplicationIcons.PDF.WithSize(IconSizes.Size16x16);
         layoutGroupTemplate.Text = Captions.Reporting.TemplateSelection;
         layoutItemTemplate.Text = Captions.Reporting.Template.FormatForLabel();
         lblTemplateDescription.AsDescription();

         rgColor.Properties.Items.AddRange(new[]
         {
            new RadioGroupItem(ReportColorStyles.Color, Captions.Reporting.Color),
            new RadioGroupItem(ReportColorStyles.GrayScale, Captions.Reporting.GrayScale),
         });
      }
   }
}