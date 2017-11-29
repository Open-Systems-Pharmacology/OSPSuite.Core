using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Starter.Tasks
{
   public class PresentationUserSettings : IPresentationUserSettings
   {
      public string DefaultChartEditorLayout { get; set; }
      public string ActiveSkin { get; set; }
      public Scalings DefaultChartYScaling { get; set; }
      public IconSize IconSizeTreeView { get; set; }
      public IconSize IconSizeTab { get; set; }
      public IconSize IconSizeContextMenu { get; set; }
      public Color ChartBackColor { get; set; }
      public Color ChartDiagramBackColor { get; set; }
      public DisplayUnitsManager DisplayUnits { get; set; }
      public IList<string> ProjectFiles { get; set; }
      public uint MRUListItemCount { get; set; }
      public ComparerSettings ComparerSettings { get; set; }
      public IDiagramOptions DiagramOptions { get; set; }
      public string ChartEditorLayout { get; set; }
      public JournalPageEditorSettings JournalPageEditorSettings { get; set; }
      public ParameterIdentificationFeedbackEditorSettings ParameterIdentificationFeedbackEditorSettings { get; set; }
      public SensitivityAnalysisFeedbackEditorSettings SensitivityAnalysisFeedbackEditorSettings { get; set; }

      public PresentationUserSettings()
      {
         DisplayUnits = new DisplayUnitsManager();
         ComparerSettings = new ComparerSettings();
         DiagramOptions = new DiagramOptions {SnapGridVisible = true};
         ProjectFiles = new List<string>();
         JournalPageEditorSettings = new JournalPageEditorSettings();
         DefaultChartYScaling = Scalings.Log;
         ParameterIdentificationFeedbackEditorSettings = new ParameterIdentificationFeedbackEditorSettings();
         SensitivityAnalysisFeedbackEditorSettings = new SensitivityAnalysisFeedbackEditorSettings();
      }
   }
}