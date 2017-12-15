using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converter.v7_3;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Serializer.Xml.Extensions;

namespace OSPSuite.Converter.v7_3
{
   public abstract class concern_for_Converter710To730 : ContextSpecification<Converter710To730>
   {
      protected override void Context()
      {
         sut = new Converter710To730();
      }
   }

   public class When_converting_an_xml_element_with_children_having_value_description_attribute : concern_for_Converter710To730
   {
      private XElement _parameterListElement;
      private string _valueDescription;
      private int _version;
      private bool _converted;

      protected override void Context()
      {
         base.Context();
         _valueDescription = "a big description";
         _parameterListElement = new XElement("ParameterList",
            new XElement("Parameter",
               new XAttribute("name", "P1"),
               new XAttribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION, _valueDescription)
            ),
            new XElement("Parameter",
               new XAttribute("name", "P2"),
               new XAttribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION, _valueDescription)
            ),
            new XElement("Parameter",
               new XAttribute("name", "P3"),
               new XAttribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION, _valueDescription)
            ),
            new XElement("Parameter",
               new XAttribute("name", "P4")
            )
         );
      }

      protected override void Because()
      {
         (_version, _converted) = sut.ConvertXml(_parameterListElement);
      }

      [Observation]
      public void should_have_converted_the_value_description_if_presentt()
      {
         var allValueOrigins = _parameterListElement.Descendants(Constants.Serialization.VALUE_ORIGIN).ToList();
         allValueOrigins.Count.ShouldBeEqualTo(3);
         allValueOrigins[0].GetAttribute(Constants.Serialization.Attribute.DESCRIPTION).ShouldBeEqualTo(_valueDescription);
      }


      [Observation]
      public void should_have_removed_the_value_description_attributes()
      {
         //retrieve all elements with an attribute dimension
         var allValueDescriptionAttributes = from child in _parameterListElement.DescendantsAndSelf()
            where child.HasAttributes
            let attr = child.Attribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION)
            where attr != null
            select attr;

         allValueDescriptionAttributes.ShouldBeEmpty();
      }

      [Observation]
      public void should_return_the_expected_version_and_converted_flag()
      {
         _version.ShouldBeEqualTo(PKMLVersion.V7_3_0);
         _converted.ShouldBeTrue();
      }
   }

   public class When_converting_an_xml_element_without_any_children_having_value_description_attribute : concern_for_Converter710To730
   {
      private XElement _parameterListElement;
      private int _version;
      private bool _converted;

      protected override void Context()
      {
         base.Context();
         _parameterListElement = new XElement("ParameterList",
            new XElement("Parameter",
               new XAttribute("name", "P1")
            )
         );
      }

      protected override void Because()
      {
         (_version, _converted) = sut.ConvertXml(_parameterListElement);
      }

      [Observation]
      public void should_not_add_any_value_origin_node()
      {
         var allValueOrigins = _parameterListElement.Descendants(Constants.Serialization.VALUE_ORIGIN).ToList();
         allValueOrigins.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_the_expected_version_and_converted_flag()
      {
         _version.ShouldBeEqualTo(PKMLVersion.V7_3_0);
         _converted.ShouldBeFalse();
      }
   }

   public class When_converting_an_xml_element_having_a_value_description : concern_for_Converter710To730
   {
      private XElement _parameterElement;
      private string _valueDescription;

      protected override void Context()
      {
         base.Context();
         _valueDescription = "a big description";
         _parameterElement = new XElement("Parameter",
            new XAttribute("name", "P1"),
            new XAttribute(Constants.Serialization.Attribute.VALUE_DESCRIPTION, _valueDescription)
         );
      }

      protected override void Because()
      {
         sut.ConvertXml(_parameterElement);
      }

      [Observation]
      public void should_have_converted_the_value_description_if_presentt()
      {
         var allValueOrigins = _parameterElement.Descendants(Constants.Serialization.VALUE_ORIGIN).ToList();
         allValueOrigins.Count.ShouldBeEqualTo(1);
         allValueOrigins[0].GetAttribute(Constants.Serialization.Attribute.DESCRIPTION).ShouldBeEqualTo(_valueDescription);
      }
   }
}