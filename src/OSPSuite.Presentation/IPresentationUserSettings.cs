using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Assets;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation
{
   /// <summary>
   ///    Standard interface defining common properties for user settings in all application in the suite
   /// </summary>
   public interface IPresentationUserSettings
   {
      /// <summary>
      ///    Name of the default editor layout that should be used when creating a chart editor
      /// </summary>
      string DefaultChartEditorLayout { get; set; }

      /// <summary>
      ///    Active skin (name of Skin)
      /// </summary>
      string ActiveSkin { get; set; }

      /// <summary>
      ///    Indicates the default axis scaling as Linear or Logarithmic
      /// </summary>
      Scalings DefaultChartYScaling { get; set; }

      /// <summary>
      ///    Icon size used for tree icons
      /// </summary>
      IconSize IconSizeTreeView { get; set; }

      /// <summary>
      ///    Icon size used for the tabs
      /// </summary>
      IconSize IconSizeTab { get; set; }

      /// <summary>
      ///    Icon size used for the context menu
      /// </summary>
      IconSize IconSizeContextMenu { get; set; }

      /// <summary>
      ///    Color used for a plot back color (everything but diagram)
      /// </summary>
      Color ChartBackColor { get; set; }

      /// <summary>
      ///    Color used for a diagram back color
      /// </summary>
      Color ChartDiagramBackColor { get; set; }

      /// <summary>
      ///    Default display units defined at the user level
      /// </summary>
      DisplayUnitsManager DisplayUnits { get; set; }

      /// <summary>
      ///    Returns set the list of recently used project files
      /// </summary>
      IList<string> ProjectFiles { get; set; }

      /// <summary>
      ///    Number of items to display in the list of recently used items
      /// </summary>
      uint MRUListItemCount { get; set; }

      /// <summary>
      ///    Last settings used during the comparison and saved in user profile
      /// </summary>
      ComparerSettings ComparerSettings { get; set; }

      /// <summary>
      ///    Diagram options
      /// </summary>
      IDiagramOptions DiagramOptions { get; set; }

      /// <summary>
      ///    Represents the editor layout that the user saved to be used instead of our default editor layout
      /// </summary>
      string ChartEditorLayout { get; set; }

      /// <summary>
      ///    Settings applied to the journal page editor
      /// </summary>
      JournalPageEditorSettings JournalPageEditorSettings { get; set; }

      /// <summary>
      ///    Settings applied to the parameter identification feedback view
      /// </summary>
      ParameterIdentificationFeedbackEditorSettings ParameterIdentificationFeedbackEditorSettings { get; set; }

      /// <summary>
      ///   Settings applied to sensitivity analysis feedback view
      /// </summary>
      SensitivityAnalysisFeedbackEditorSettings SensitivityAnalysisFeedbackEditorSettings { get; set; }
   }
}