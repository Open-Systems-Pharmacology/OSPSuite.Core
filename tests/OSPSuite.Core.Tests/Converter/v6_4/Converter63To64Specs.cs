using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v6_4;

namespace OSPSuite.Converter.v6_4
{
   public abstract class concern_for_Converter63To64 : ContextSpecification<Converter63To64>
   {
      protected override void Context()
      {
         sut = new Converter63To64();
      }
   }

   public class When_converting_a_curve_chart_template_element : concern_for_Converter63To64
   {
      private XElement _chartTemplateElement;

      protected override void Context()
      {
         base.Context();
         _chartTemplateElement = new XElement("CurveChartTemplate",
            new XElement("Curves",
               new XElement("CurveTemplate",
                  new XAttribute("xDataPath", "Time"),
                  new XAttribute("yDataPath", "Organism|PeripheralVenousBlood|Drug1|Conc"),
                  new XAttribute("xQuantityType", "Time"),
                  new XAttribute("yQuantityType", "Drug")
                  ),
               new XElement("CurveTemplate",
                  new XAttribute("xDataPath", "Time"),
                  new XAttribute("yDataPath", "Organism|PeripheralVenousBlood|Drug2|Conc"),
                  new XAttribute("xQuantityType", "Time"),
                  new XAttribute("yQuantityType", "Drug")
                  )
               ));
      }

      protected override void Because()
      {
         sut.ConvertXml(_chartTemplateElement);
      }

      [Observation]
      public void should_add_the_sub_node_to_the_cuve_template_element()
      {
         _chartTemplateElement.Descendants("xData").Count().ShouldBeEqualTo(2);
         _chartTemplateElement.Descendants("yData").Count().ShouldBeEqualTo(2);
      }
   }
}