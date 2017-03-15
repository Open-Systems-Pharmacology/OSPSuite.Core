using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IWithDimension
   {
      IDimension Dimension { get; set; }
   }

   public interface IWithDisplayUnit : IWithDimension
   {
      Unit DisplayUnit { get; set; }
   }

   public interface IWithValue
   {
      double Value { get; set; }
   }

   public interface IWithId
   {
      string Id { get; set; }
   }

   public interface IWithName
   {
      string Name { get; set; }
   }

   public interface IWithDescription
   {
      string Description { get; set; }
   }

   public interface IWithScaleDivisor
   {
      /// <summary>
      ///    Scale divisor used to scale the corresponding amount when solving the system. Default is
      ///    <c>Constants.DEFAULT_SCALE_FACTOR</c>
      /// </summary>
      double ScaleDivisor { get; set; }
   }

   public interface IWithChartTemplates
   {
      /// <summary>
      /// Chart templates available in the simulation. Chart templates are idenfitied by their name which should be unique
      /// </summary>
      IEnumerable<CurveChartTemplate> ChartTemplates { get; }

      void AddChartTemplate(CurveChartTemplate chartTemplate);

      void RemoveChartTemplate(string chartTemplateName);

      /// <summary>
      /// Returns the default curve chart template if one is defined, otherwise the first chart template ordered alphabetically. If not template is defined, returns null
      /// </summary>
      CurveChartTemplate DefaultChartTemplate { get; }

      CurveChartTemplate ChartTemplateByName(string templateName);

      void RemoveAllChartTemplates();
   }

   public interface IWithModel
   {
      IModel Model { set; get; }
   }

   public interface IWithCreationMetaData
   {
      /// <summary>
      ///    Information describing how the object was created
      /// </summary>
      CreationMetaData Creation { get; set; }
   }
}