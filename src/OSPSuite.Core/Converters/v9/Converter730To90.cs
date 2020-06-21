using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v9
{
   public class Converter730To90 : IObjectConverter,
      IVisitor<SensitivityAnalysis>,
      IVisitor<PopulationSimulationPKAnalyses>
   {
      private readonly IDimensionFactory _dimensionFactory;
      private static readonly ICache<string, string> _pkParameterNameMapping = createPKParameterNameMapping();
      private bool _converted;

      public Converter730To90(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V7_3_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V9_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         _converted = false;
         convertDimensionIn(element);
         return (PKMLVersion.V9_0, _converted);
      }

      private void convertDimensionIn(XElement element)
      {
         //retrieve all elements with an attribute dimension
         var allDimensionAttributes = from child in element.DescendantsAndSelf()
            where child.HasAttributes
            let attr = child.Attribute(Constants.Serialization.Attribute.Dimension) ?? child.Attribute("dimension")
            where attr != null
            select attr;


         var allMappedDimensionIds = new HashSet<string>();
         foreach (var attribute in allDimensionAttributes)
         {
            string attributeValue = attribute.Value;
            if (int.TryParse(attribute.Value, out _))
               allMappedDimensionIds.Add(attributeValue);
            else
            {
               var newDimensionName = mapDimensionName(attributeValue);
               if (!string.Equals(newDimensionName, attributeValue))
               {
                  attribute.SetValue(newDimensionName);
                  _converted = true;
               }
            }
         }


         var allMapAttributes = from child in element.Descendants(Constants.Serialization.STRING_MAP)
            let id = child.Attribute(Constants.Serialization.Attribute.ID)
            where allMappedDimensionIds.Contains(id.Value)
            select child.Attribute(Constants.Serialization.Attribute.STRING);

         foreach (var attribute in allMapAttributes)
         {
            var oldDimensionName = attribute.Value;
            var newDimensionName = mapDimensionName(attribute.Value);
            if (!string.Equals(oldDimensionName, newDimensionName))
            {
               attribute.SetValue(newDimensionName);
               _converted = true;
            }
         }
      }

      private string mapDimensionName(string dimensionName)
      {
         if (_dimensionFactory.Has(dimensionName))
            return dimensionName;

         if (dimensionName.IsOneOf($"{Constants.Dimension.DIMENSIONLESS} per Time", $"{Constants.Dimension.DIMENSIONLESS} per time",
            "Fraction per time", "Fraction per Time"))
            return "Inversed time";

         // Some old dimensions that were not converted properly and that require some TLC 
         if (dimensionName.IsOneOf("Rate"))
            return Constants.Dimension.AMOUNT_PER_TIME;

         return dimensionName;
      }

      public void Visit(SensitivityAnalysis sensitivityAnalysis)
      {
         if (!sensitivityAnalysis.HasResults)
            return;

         sensitivityAnalysis.Results.AllPKParameterSensitivities.Each(x => { x.PKParameterName = ConvertPKParameterName(x.PKParameterName); });

         _converted = true;
      }

      public void Visit(PopulationSimulationPKAnalyses populationSimulationPKAnalyses)
      {
         var allQuantityPKAnalysis = populationSimulationPKAnalyses.All().ToList();
         populationSimulationPKAnalyses.Clear();
         allQuantityPKAnalysis.Each(x =>
         {
            x.Name = ConvertPKParameterName(x.Name);
            populationSimulationPKAnalyses.AddPKAnalysis(x);
         });

         _converted = true;
      }

      public string ConvertPKParameterName(string pkParameterName)
      {
         return _pkParameterNameMapping.Contains(pkParameterName) ? _pkParameterNameMapping[pkParameterName] : pkParameterName;
      }

      private static Cache<string, string> createPKParameterNameMapping()
      {
         return new Cache<string, string>
         {
            {"C_max_t1_t2", "C_max_tD1_tD2"},
            {"C_max_t1_t2_norm", "C_max_tD1_tD2_norm"},
            {"C_max_tLast_tEnd", "C_max_tDLast_tEnd"},
            {"C_max_tLast_tEnd_norm", "C_max_tDLast_tEnd_norm"},
            {"t_max_t1_t2", "t_max_tD1_tD2"},
            {"t_max_tLast_tEnd", "t_max_tDLast_tEnd"},
            {"C_trough_t2", "C_trough_tD2"},
            {"C_trough_tLast", "C_trough_tDLast"},
            {"AUC", "AUC_tEnd"},
            {"AUC_norm", "AUC_tEnd_norm"},
            {"AUC_t1_t2", "AUC_tD1_tD2"},
            {"AUC_t1_t2_norm", "AUC_tD1_tD2_norm"},
            {"AUC_tLast_minus_1_tLast", "AUC_tDLast_minus_1_tDLast"},
            {"AUC_tLast_minus_1_tLast_norm", "AUC_tDLast_minus_1_tDLast_norm"},
            {"AUC_inf_t1", "AUC_inf_tD1"},
            {"AUC_inf_t1_norm", "AUC_inf_tD1_norm"},
            {"AUC_inf_tLast", "AUC_inf_tDLast"},
            {"AUC_inf_tLast_norm", "AUC_inf_tDLast_norm"},
            {"Thalf_tLast_tEnd", "Thalf_tDLast_tEnd"}
         };
      }

    
   }
}