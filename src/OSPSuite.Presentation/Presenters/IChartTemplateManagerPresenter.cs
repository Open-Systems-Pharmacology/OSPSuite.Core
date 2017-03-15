using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IChartTemplateManagerPresenter : IPresenter<IChartTemplateManagerView>,  IDisposablePresenter
   {
      /// <summary>
      ///    Edits the chart templates <paramref name="chartTemplates" /> given as parameters and a brand new enumeration
      ///    containing the edited templates. Returns true if the edit was performed otherwise false
      /// </summary>
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates);

      /// <summary>
      ///    This method should be called to show the details of a <paramref name="chartTemplate" />
      /// </summary>
      void ShowTemplateDetails(CurveChartTemplate chartTemplate);

      /// <summary>
      ///    Add a new template to the list of managed templates
      /// </summary>
      void CloneTemplate(CurveChartTemplate templateToClone);

      /// <summary>
      ///    Removes the template <paramref name="chartTemplate" />
      /// </summary>
      void DeleteTemplate(CurveChartTemplate chartTemplate);

      /// <summary>
      ///    Returns the edited templates.
      /// </summary>
      IEnumerable<CurveChartTemplate> EditedTemplates { get; }

      /// <summary>
      ///    Saves the given <paramref name="template" /> to file
      /// </summary>
      /// <param name="template"></param>
      void SaveTemplateToFile(CurveChartTemplate template);

      /// <summary>
      /// Loads a template from a user selected file
      /// </summary>
      void LoadTemplateFromFile();

      bool HasChanged { get; }

      /// <summary>
      /// Sets the <paramref name="template"/> DefaultTemplate to <paramref name="isDefault"/>. 
      /// Also clears DefaultTemplate value for all other templates
      /// </summary>
      void SetDefaultTemplateValue(CurveChartTemplate template, bool isDefault);
   }
}