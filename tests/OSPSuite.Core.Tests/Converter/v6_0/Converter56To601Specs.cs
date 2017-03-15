using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Converter.v6_0;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Helpers;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Converter.v6_0
{
   public abstract class concern_for_Converter561To601 : ContextForModelConstructorIntegration
   {
      protected IModelCoreSimulation _simulation;
      protected IBuildConfiguration _simulationConfiguration;
      protected Converter56To601 _converter;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _converter = IoC.Resolve<Converter56To601>();
      }
   }

   public class When_converting_the_simple_IV_56_simulation_to_a_60_project : concern_for_Converter561To601
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("simple_IV_56").Simulation;
         _simulationConfiguration = _simulation.BuildConfiguration;
      }

      [Observation]
      public void should_have_converted_the_solver_parameters_in_the_simulation_settings_building_block()
      {
         _simulationConfiguration.SimulationSettings.Solver.AbsTol.ShouldBeEqualTo(1e-9, 1e-2);
      }

      [Observation]
      public void should_have_set_the_dimension_of_all_molecule_builder_amount_to_amount_if_not_the_case_already()
      {
         _simulationConfiguration.Molecules.Each(x => x.DefaultStartFormula.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.AMOUNT));
      }

      [Observation]
      public void should_have_set_the_icon_name_of_all_molecule_builder_if_not_set_already()
      {
         _simulationConfiguration.Molecules.Each(x => string.IsNullOrEmpty(x.Icon).ShouldBeFalse());
      }
   }

   public class When_converting_a_curve_chart_element_from_56_to_60   : concern_for_Converter561To601
   {
      private XElement _curveChartElement;

      protected override void Context()
      {
         base.Context();
         _curveChartElement = XElement.Parse(
            @"<CurveChart id='knZpOqIwh0qaCGEgKPBB0w' name='Analysis' sideMarginsEnabled='1' legendPosition='RightInside' backColor='Transparent' diagramBackColor='White'>
                <Axes>
                    <Axis axisType='X' dimension='Time' unitName='h' scaling='Linear' numberMode='Normal' gridLines='0'/>
                    <Axis axisType='Y' dimension='Concentration (molar)' unitName='µmol/l' scaling='Log' numberMode='Normal' gridLines='0'/>
                </Axes>
                <Curves>
                    <Curve name='C-Peripheral Venous Blood-Plasma-Concentration' xData='cfbde418-3fa0-40a6-aec3-4d9655933f36' yData='c5a1a464-5176-4d5b-94a7-b76b941f6f65'>
                        <CurveOptions yAxisType='Y' interpolationMode='xLinear' visible='1' color='Red' lineStyle='Solid' symbol='None' lineThickness='2'/>
                    </Curve>
                </Curves>
            </CurveChart>"
            );
      }

      protected override void Because()
      {
         _converter.ConvertXml(_curveChartElement);
      }

      [Observation]
      public void should_move_the_settings_attribute_to_a_new_sub_node()
      {
         var chartSettings = _curveChartElement.Element("ChartSettings");
         chartSettings.ShouldNotBeNull();
         chartSettings.GetAttribute("sideMarginsEnabled").ShouldBeEqualTo("1");
         chartSettings.GetAttribute("legendPosition").ShouldBeEqualTo("RightInside");
         chartSettings.GetAttribute("backColor").ShouldBeEqualTo("Transparent");
         chartSettings.GetAttribute("diagramBackColor").ShouldBeEqualTo("White");
      }
   }
}