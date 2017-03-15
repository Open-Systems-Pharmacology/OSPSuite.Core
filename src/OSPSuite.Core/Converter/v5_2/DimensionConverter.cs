using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IDimensionConverter
   {
      void ConvertDimensionIn(XElement element);
      void ConvertDimensionIn(IContainer container);
      void ConvertDimensionIn(DataRepository dataRepository);
      void ConvertDimensionIn(IEnumerable<IParameter> parameters, bool convertFormulasAtUsingFormula);
      void ConvertDimensionIn<T>(IBuildingBlock<T> buildingBlock) where T : class, IObjectBase;
      void ConvertDimensionIn(IModelCoreSimulation simulation);
      void ConvertDimensionIn(IUsingFormula usingFormula);
   }

   internal class DimensionConverter : IDimensionConverter
   {
      private readonly IDimensionMapper _dimensionMapper;
      private readonly IParameterConverter _parameterConverter;
      private readonly IUsingFormulaConverter _usingFormulaConverter;
      private readonly IDataRepositoryConverter _dataRepositoryConverter;

      public DimensionConverter(IDimensionMapper dimensionMapper, IParameterConverter parameterConverter, IUsingFormulaConverter usingFormulaConverter, IDataRepositoryConverter dataRepositoryConverter)
      {
         _dimensionMapper = dimensionMapper;
         _parameterConverter = parameterConverter;
         _usingFormulaConverter = usingFormulaConverter;
         _dataRepositoryConverter = dataRepositoryConverter;
      }

      public void ConvertDimensionIn(XElement element)
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
            int id;
            if (int.TryParse(attribute.Value, out id))
               allMappedDimensionIds.Add(attributeValue);
            else
               attribute.SetValue(_dimensionMapper.DimensionNameFor(attributeValue));
         }


         var allMapAttributes = from child in element.Descendants(Constants.Serialization.STRING_MAP)
            let id = child.Attribute(Constants.Serialization.Attribute.ID)
            where allMappedDimensionIds.Contains(id.Value)
            select child.Attribute(Constants.Serialization.Attribute.STRING);

         foreach (var attribute in allMapAttributes)
         {
            attribute.SetValue(_dimensionMapper.DimensionNameFor(attribute.Value));
         }
      }

      private void convertDimensionIn(IContainer container, bool convertFormulasAtParameter)
      {
         ConvertDimensionIn(container.GetAllChildren<IParameter>(), convertFormulasAtParameter);

         var allUsingFormulas = container.GetAllChildren<IUsingFormula>(x => !x.IsAnImplementationOf<IParameter>());
         allUsingFormulas.Each(usingFormula => convertUsingFormula(usingFormula, convertFormulasAtParameter));

         var selfUsingFormula = container as IUsingFormula;
         if (selfUsingFormula != null)
            convertUsingFormula(selfUsingFormula, convertFormulasAtParameter);
      }

      public void ConvertDimensionIn(DataRepository dataRepository)
      {
         _dataRepositoryConverter.Convert(dataRepository);
      }

      public void ConvertDimensionIn(IEnumerable<IParameter> parameters, bool convertFormulasAtUsingFormula)
      {
         parameters.Each(parameter => _parameterConverter.Convert(parameter, convertFormulasAtUsingFormula));
      }

      private void convertUsingFormula(IUsingFormula usingFormula, bool convertFormulasAtParameter)
      {
         _usingFormulaConverter.Convert(usingFormula, convertFormulasAtParameter);
      }

      public void ConvertDimensionIn<T>(IBuildingBlock<T> buildingBlock) where T : class, IObjectBase
      {
         foreach (var objectBase in buildingBlock)
         {
            var container = objectBase as IContainer;
            //Formulas Converted In FormulaCache
            if (container != null)
               convertDimensionIn(container, convertFormulasAtParameter: false);
            else
            {
               // Convert StartValue before changeing It's using Formula properties otherwise, the Dimension may be chenged befoer Converting the Value
               convertPSV(objectBase as IParameterStartValue); 
               convertUsingFormula(objectBase as IUsingFormula, convertFormulasAtParameter: false);
            }
         }

         _usingFormulaConverter.Convert(buildingBlock.FormulaCache);
      }

      private void convertPSV(IParameterStartValue parameterStartValue)
      {
         if (parameterStartValue == null) return;
         var conversionFactor = _dimensionMapper.ConversionFactor(parameterStartValue);
         if (conversionFactor == 1)
            return;

         var value = parameterStartValue.StartValue.GetValueOrDefault();
         if (double.IsNaN(value))
            return;

         parameterStartValue.StartValue = parameterStartValue.StartValue * conversionFactor;
      }

      public void ConvertDimensionIn(IContainer container)
      {
         convertDimensionIn(container, convertFormulasAtParameter: true);
      }

      public void ConvertDimensionIn(IModelCoreSimulation simulation)
      {
         convertDimensionIn(simulation.Model.Root, convertFormulasAtParameter: true);

         if (simulation.BuildConfiguration.SimulationSettings == null) return;
         convertDimensionIn(simulation.BuildConfiguration.SimulationSettings.OutputSchema, convertFormulasAtParameter: true);
      }

      public void ConvertDimensionIn(IUsingFormula usingFormula)
      {
         convertUsingFormula(usingFormula,convertFormulasAtParameter:true);
      }
   }
}