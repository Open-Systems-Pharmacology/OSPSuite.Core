using System;
using System.IO;
using System.Xml.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ChartTemplatePersistor : ContextSpecification<IChartTemplatePersistor>
   {
      private ICurveChartToCurveChartTemplateMapper _curveChartTemplateMapper;
      protected IOSPSuiteXmlSerializerRepository _xmlSerializerRepository;
      private IDimensionFactory _dimensionFactory;
      private IObjectBaseFactory _objectBaseFactory;
      private IWithIdRepository _withIdRepository;

      protected override void Context()
      {
         _curveChartTemplateMapper = A.Fake<ICurveChartToCurveChartTemplateMapper>();
         _xmlSerializerRepository = A.Fake<IOSPSuiteXmlSerializerRepository>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _withIdRepository = A.Fake<IWithIdRepository>();
         sut = new ChartTemplatePersistor(_curveChartTemplateMapper, _xmlSerializerRepository, _dimensionFactory, _objectBaseFactory, _withIdRepository);
      }
   }

   public class When_loading_a_template_from_an_empty_string : concern_for_ChartTemplatePersistor
   {
      [Observation]
      public void should_return_a_null_template()
      {
         sut.DeserializeFromString(string.Empty).ShouldBeNull();
         sut.DeserializeFromString(null).ShouldBeNull();
      }
   }

   public class When_loading_a_template_from_an_invalid_string : concern_for_ChartTemplatePersistor
   {
      [Observation]
      public void should_return_a_null_template()
      {
         sut.DeserializeFromString("AA").ShouldBeNull();
      }
   }

   public class When_loading_a_template_from_a_file_that_contains_xml_content_for_another_type : concern_for_ChartTemplatePersistor
   {
      private string _tmpFile;
      private IXmlSerializer<SerializationContext> _serializer;

      protected override void Context()
      {
         base.Context();
         _serializer = A.Fake<IXmlSerializer<SerializationContext>>();
         _tmpFile = FileHelper.GenerateTemporaryFileName();
         using (var fileWrite = new StreamWriter(_tmpFile))
            fileWrite.WriteLine("<Units></Units>");

         A.CallTo(_xmlSerializerRepository).WithReturnType<IXmlSerializer<SerializationContext>>().Returns(_serializer);
         A.CallTo(() => _serializer.Deserialize<CurveChartTemplate>(A<XElement>._, A<SerializationContext>._)).Throws<InvalidCastException>();
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.DeserializeFromFile(_tmpFile)).ShouldThrowAn<OSPSuiteException>();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_tmpFile);
      }
   }

   public class When_loading_a_template_from_a_file_that_does_not_contain_some_xml_file : concern_for_ChartTemplatePersistor
   {
      private string _tmpFile;

      protected override void Context()
      {
         base.Context();
         _tmpFile = FileHelper.GenerateTemporaryFileName();
         using (var fileWrite = new StreamWriter(_tmpFile))
            fileWrite.WriteLine("AAAA");
      }

      [Observation]
      public void should_return_a_null_template()
      {
         sut.DeserializeFromFile(_tmpFile).ShouldBeNull();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_tmpFile);
      }
   }

   public class When_serializing_an_undefined_chart : concern_for_ChartTemplatePersistor
   {
      protected override void Context()
      {
         base.Context();
         var chartSerializer = A.Fake<IXmlSerializer<SerializationContext>>();
         A.CallTo(_xmlSerializerRepository).WithReturnType<IXmlSerializer<SerializationContext>>().Returns(chartSerializer);
         A.CallTo(chartSerializer).WithReturnType<XElement>().Returns(new XElement("ChartTemplate"));
      }

      [Observation]
      public void should_not_crash()
      {
         sut.SerializeAsStringBasedOn(null).ShouldNotBeNull();
         sut.SerializeBasedOn(null).ShouldNotBeNull();
      }
   }
}