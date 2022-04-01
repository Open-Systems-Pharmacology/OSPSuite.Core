using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converters.v11;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converters
{
   public abstract class concern_for_Converter100To110 : ContextSpecification<Converter100To110>
   {

      protected override void Context()
      {
         sut = new Converter100To110();
      }
   }


   public class When_converting_quantity_pk_parameter_to_version_110 : concern_for_Converter100To110
   {
      private int _version;
      private bool _converted;
      private XElement _element;

      protected override void Context()
      {
         base.Context();
         var xDoc = XDocument.Parse("<QuantityPKParameter dimension = \"Concentration(mass)\" name = \"C_max\" quantityPath =\"A|B|C\">" + 
                                       "<Values>AAEAAAD /////AQAAAAAAAAAPAQAAAAMAAAALAACgQAAAgEAAAABACw==</Values>" + 
                                       "</QuantityPKParameter>");
         _element = xDoc.Root;
      }

      protected override void Because()
      {
         (_version, _converted) = sut.ConvertXml(_element);
      }

      [Observation]
      public void should_have_converted_the_pk_parameter_element_as_expected()
      {
         _version.ShouldBeEqualTo(PKMLVersion.V11_0);
         _converted.ShouldBeTrue();
         var valueCache = _element.Element("ValueCache");
         valueCache.ShouldNotBeNull();
         var valueElement = valueCache.Elements(Constants.Serialization.VALUES);
         valueElement.ShouldNotBeNull();
      }
   }
}