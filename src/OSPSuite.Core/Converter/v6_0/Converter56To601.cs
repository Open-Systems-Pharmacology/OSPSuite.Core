using System.Xml.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml.Extensions;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converter.v6_0
{
   public class Converter56To601 : IObjectConverter,
      IVisitor<IMoleculeBuildingBlock>,
      IVisitor<IBuildConfiguration>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<SimulationTransfer>,
      IVisitor<IMoleculeBuilder>
   {
      private readonly IIdGenerator _idGenerator;
      private readonly IGroupRepository _groupRepository;
      private bool _converted;

      public Converter56To601(IIdGenerator idGenerator, IGroupRepository groupRepository)
      {
         _idGenerator = idGenerator;
         _groupRepository = groupRepository;
      }

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V5_6_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V6_0_1, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         _converted = false;
         element.DescendantsAndSelfNamed("Solver").Each(convertSolver);
         element.DescendantsAndSelfNamed("CurveChart").Each(convertCurveChart);
         return (PKMLVersion.V6_0_1, _converted);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         moleculeBuildingBlock.Each(Visit);
         _converted = true;
      }

      public void Visit(IBuildConfiguration buildConfiguration)
      {
         Visit(buildConfiguration.Molecules);
         _converted = true;
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         Visit(simulation.BuildConfiguration);
         _converted = true;
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
         _converted = true;
      }

      public void Visit(IMoleculeBuilder moleculeBuilder)
      {
         updateDefaultStartFormulaDimension(moleculeBuilder);
         updateIconIfNotSet(moleculeBuilder);
         _converted = true;
      }

      private void convertCurveChart(XElement curveChartElement)
      {
         var chartSettingsNode = new XElement("ChartSettings");
         curveChartElement.Add(chartSettingsNode);
         moveAttributes(curveChartElement, chartSettingsNode, "sideMarginsEnabled", "legendPosition", "backColor", "diagramBackColor");
         _converted = true;
      }

      private void moveAttributes(XElement curveChartElement, XElement chartSettingsNode, params string[] attributeNamesToMove)
      {
         attributeNamesToMove.Each(attr =>
         {
            var sourceAttribute = curveChartElement.Attribute(attr);
            if (sourceAttribute == null)
               return;

            chartSettingsNode.Add(sourceAttribute);
         });
      }

      private void convertSolver(XElement solverElement)
      {
         var solverName = solverElement.GetAttribute("solverName");
         var absTol = solverElement.GetAttribute("absTol");
         var relTol = solverElement.GetAttribute("relTol");
         var h0 = solverElement.GetAttribute("h0");
         var hMax = solverElement.GetAttribute("hMax");
         var hMin = solverElement.GetAttribute("hMin");
         var mxStep = solverElement.GetAttribute("mxStep");
         var useJacobian = solverElement.GetAttribute("useJacobian");
         solverElement.AddAttribute(Constants.Serialization.Attribute.NAME, solverName);
         var children = new XElement("Children");
         solverElement.Add(children);
         children.Add(createSolverParameterElement(Constants.Parameters.ABS_TOL, absTol));
         children.Add(createSolverParameterElement(Constants.Parameters.REL_TOL, relTol));
         children.Add(createSolverParameterElement(Constants.Parameters.H0, h0));
         children.Add(createSolverParameterElement(Constants.Parameters.H_MAX, hMax));
         children.Add(createSolverParameterElement(Constants.Parameters.H_MIN, hMin));
         children.Add(createSolverParameterElement(Constants.Parameters.MX_STEP, mxStep));
         children.Add(createSolverParameterElement(Constants.Parameters.USE_JACOBIAN, useJacobian));
         _converted = true;
      }

      private XElement createSolverParameterElement(string parameterName, string parameterValue)
      {
         var parameterElement = new XElement("Parameter");
         parameterElement.AddAttribute(Constants.Serialization.Attribute.ID, _idGenerator.NewId());
         parameterElement.AddAttribute(Constants.Serialization.Attribute.NAME, parameterName);
         parameterElement.AddAttribute("quantityType", QuantityType.Parameter.ToString());
         parameterElement.AddAttribute(Constants.Serialization.Attribute.VALUE, parameterValue);
         parameterElement.AddAttribute("icon", ApplicationIcons.Parameter.IconName);
         var parameterInfoElement = new XElement("Info");
         parameterInfoElement.AddAttribute("group", _groupRepository.GroupByName(Constants.Groups.SOLVER_SETTINGS).Id);
         parameterElement.Add(parameterInfoElement);
         return parameterElement;
      }

      private void updateDefaultStartFormulaDimension(IMoleculeBuilder moleculeBuilder)
      {
         if (dimensionIsAmountOrMolarConcentration(moleculeBuilder))
            return;

         moleculeBuilder.DefaultStartFormula.Dimension = moleculeBuilder.Dimension;
      }

      private static bool dimensionIsAmountOrMolarConcentration(IMoleculeBuilder moleculeBuilder)
      {
         var dimension = moleculeBuilder.DefaultStartFormula.Dimension;
         return dimension != null && dimension.Name.IsOneOf(Constants.Dimension.AMOUNT, Constants.Dimension.MOLAR_CONCENTRATION);
      }

      private void updateIconIfNotSet(IMoleculeBuilder moleculeBuilder)
      {
         if (string.IsNullOrEmpty(moleculeBuilder.Icon))
            moleculeBuilder.Icon = moleculeBuilder.QuantityType.ToString();
      }
   }
}