using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IChartTemplateManagerPresenter : IPresenter<IChartTemplateManagerView>,  IDisposablePresenter
   {
      /// <summary>
      ///    Edits the chart templates <paramref name="chartTemplates" /> given as parameters and a brand-new enumeration
      ///    containing the edited templates. Returns true if the edit was performed otherwise false
      /// </summary>
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates);

      /// <summary>
      ///    Edits the chart templates <paramref name="chartTemplates" /> owned by <paramref name="templateOwner" />
      /// </summary>
      /// <param name="chartTemplates">The templates to edit</param>
      /// <param name="templateOwner">
      ///    The object owning the edited templates (e.g. the project or a simulation). Its templates are not offered when
      ///    copying templates from the project
      /// </param>
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates, IWithChartTemplates templateOwner);

      /// <summary>
      ///    Edits the chart templates <paramref name="chartTemplates" /> created for charts of type
      ///    <paramref name="managedChartType" />. The <paramref name="forbiddenNames" /> are taken into account when
      ///    validating template names. Loading or copying a template created for another chart type is rejected
      /// </summary>
      /// <param name="chartTemplates">The templates to edit</param>
      /// <param name="forbiddenNames">Names that cannot be used for the edited templates</param>
      /// <param name="managedChartType">The chart type of the edited templates</param>
      /// <param name="templateOwner">
      ///    The object owning the edited templates (e.g. the project or a simulation), if any. Its templates are not
      ///    offered when copying templates from the project
      /// </param>
      void EditTemplates(IEnumerable<CurveChartTemplate> chartTemplates, IReadOnlyList<string> forbiddenNames, CurveChartTypes managedChartType, IWithChartTemplates templateOwner = null);

      /// <summary>
      ///    Copies the templates selected by the user from the other template owners of the project (e.g. the project
      ///    itself or other simulations) into the edited templates. Owners keep their own templates, the copies are
      ///    independent
      /// </summary>
      void CopyTemplatesFromProject();

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
      /// Also clears DefaultTemplate value for all other templates of the same chart type
      /// </summary>
      void SetDefaultTemplateValue(CurveChartTemplate template, bool isDefault);
   }
}