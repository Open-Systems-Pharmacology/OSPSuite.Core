using System.Collections.Generic;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IWithProhibitedNames : IWithName
   {
      bool IsNameUnique(string name);
   }
   
   public interface IWithDimension
   {
      IDimension Dimension { get; set; }
   }

   public interface IWithDisplayUnit : IWithDimension
   {
      Unit DisplayUnit { get; set; }
   }

   public interface IWithNullableValue
   {
      double? Value { get; set; }
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

   public interface IWithHasChanged
   {
      bool HasChanged { get; set; }
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
      ///    Chart templates available in the simulation. Chart templates are identified by their name which should be unique
      /// </summary>
      IEnumerable<CurveChartTemplate> ChartTemplates { get; }

      void AddChartTemplate(CurveChartTemplate chartTemplate);

      void RemoveChartTemplate(string chartTemplateName);

      /// <summary>
      ///    Returns the default curve chart template if one is defined, otherwise the first chart template ordered
      ///    alphabetically. If not template is defined, returns null
      /// </summary>
      CurveChartTemplate DefaultChartTemplate { get; }

      CurveChartTemplate ChartTemplateByName(string templateName);

      void RemoveAllChartTemplates();
   }

   public interface IWithModel
   {
      IModel Model { set; get; }
   }

   public interface IMolWeightFinder
   {
      /// <summary>
      ///    Returns the value of the molweight <see cref="IParameter" /> defined in the simulation. If the parameter is not
      ///    found for
      ///    the given <paramref name="quantity" />, returns <c>null</c>.
      ///    We use the following logic:
      ///    For a <see cref="MoleculeAmount" /> a MolWeight parameter will be searched directly in the global container named
      ///    after the molecule.
      ///    For all other quantities (e.g. <see cref="Observer" />,  <see cref="IParameter" />) a MolWeight parameter will be
      ///    searched in the global container named after the parent.
      /// </summary>
      /// <param name="quantity">Quantity for which the molweight parameter should be retrieved</param>
      double? MolWeightFor(IQuantity quantity);

      /// <summary>
      ///    Returns the value of the molweight <see cref="IParameter" /> defined in the simulation. If the parameter is not
      ///    found for
      ///    the quantity with path <paramref name="quantityPath" />, returns <c>null</c>.
      ///    For implementation details, see MolWeightFor(IQuantity quantity)
      /// </summary>
      /// <param name="quantityPath">PAth of quantity for which the molecule weight should be retrieved</param>
      double? MolWeightFor(string quantityPath);
   }

   public interface IWithCreationMetaData
   {
      /// <summary>
      ///    Information describing how the object was created
      /// </summary>
      CreationMetaData Creation { get; set; }
   }
}