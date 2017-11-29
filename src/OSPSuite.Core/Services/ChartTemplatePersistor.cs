using System;
using System.Xml;
using System.Xml.Linq;
using OSPSuite.Assets;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Services
{
   public interface IChartTemplatePersistor
   {
      /// <summary>
      ///    Deserializes the given <paramref name="element" /> and returns the corresponding <see cref="CurveChartTemplate" />
      /// </summary>
      CurveChartTemplate Deserialize(XElement element);

      /// <summary>
      ///    Deserializes the content of the file with path <paramref name="fileFullPath" /> as a
      ///    <see cref="CurveChartTemplate" />. Returns null if the file does not contain a valid xml file.
      /// </summary>
      CurveChartTemplate DeserializeFromFile(string fileFullPath);

      /// <summary>
      ///    Deserializes the <paramref name="xmlString" /> as a  <see cref="CurveChartTemplate" />. Returns <c>null</c> if the
      ///    string is not defined or not a valid xml string
      /// </summary>
      CurveChartTemplate DeserializeFromString(string xmlString);

      /// <summary>
      ///    Saves the <paramref name="chartTemplate" /> as Xml and returns the <see cref="XElement" />.
      /// </summary>
      XElement Serialize(CurveChartTemplate chartTemplate);

      /// <summary>
      ///    Creates a <see cref="CurveChartTemplate" /> based on the given <paramref name="chart" /> and returns an
      ///    <see cref="XElement" />
      ///    corresponding to its serialization
      /// </summary>
      XElement SerializeBasedOn(CurveChart chart);

      /// <summary>
      ///    Creates a <see cref="CurveChartTemplate" /> based on the given <paramref name="chart" /> and returns an
      ///    xml string corresponding to its serialization
      /// </summary>
      string SerializeAsStringBasedOn(CurveChart chart);

      /// <summary>
      ///    Serializes the <paramref name="chartTemplate" /> to the file with the path <paramref name="fileFullPath" />
      /// </summary>
      void SerializeToFile(CurveChartTemplate chartTemplate, string fileFullPath);

      /// <summary>
      ///    Creates a <see cref="CurveChartTemplate" /> based on the given <paramref name="chart" /> and serializes it to the
      ///    file with path <paramref name="fileFullPath" />
      /// </summary>
      void SerializeToFileBasedOn(CurveChart chart, string fileFullPath);
   }

   public class ChartTemplatePersistor : IChartTemplatePersistor
   {
      private readonly ICurveChartToCurveChartTemplateMapper _curveChartTemplateMapper;
      private readonly IOSPSuiteXmlSerializerRepository _modellingXmlSerializerRepository;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IWithIdRepository _withIdRepository;

      public ChartTemplatePersistor(ICurveChartToCurveChartTemplateMapper curveChartTemplateMapper, IOSPSuiteXmlSerializerRepository modellingXmlSerializerRepository,
         IDimensionFactory dimensionFactory, IObjectBaseFactory objectBaseFactory, IWithIdRepository withIdRepository)
      {
         _curveChartTemplateMapper = curveChartTemplateMapper;
         _modellingXmlSerializerRepository = modellingXmlSerializerRepository;
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _withIdRepository = withIdRepository;
      }

      public CurveChartTemplate Deserialize(XElement element)
      {
         using (var serializationContext = SerializationTransaction.Create(_dimensionFactory, _objectBaseFactory, _withIdRepository))
         {
            var chartTemplateSerializer = _modellingXmlSerializerRepository.SerializerFor(element);
            return chartTemplateSerializer.Deserialize<CurveChartTemplate>(element, serializationContext);
         }
      }

      public CurveChartTemplate DeserializeFromFile(string fileFullPath)
      {
         try
         {
            return Deserialize(XElement.Load(fileFullPath));
         }
         catch (XmlException)
         {
            //happens when file is not a valid xml file
            return null;
         }
         catch (InvalidCastException)
         {
            throw new OSPSuiteException(Error.CouldNotLoadTemplateFromFile(fileFullPath));
         }
      }

      public CurveChartTemplate DeserializeFromString(string xmlString)
      {
         try
         {
            if (string.IsNullOrEmpty(xmlString))
               return null;

            return Deserialize(XmlHelper.ElementFromString(xmlString));
         }
         catch (XmlException)
         {
            return null;
         }
      }

      public XElement Serialize(CurveChartTemplate chartTemplate)
      {
         using (var serializationContext = SerializationTransaction.Create())
         {
            var chartSerializer = _modellingXmlSerializerRepository.SerializerFor(chartTemplate);
            return chartSerializer.Serialize(chartTemplate, serializationContext);
         }
      }

      public XElement SerializeBasedOn(CurveChart chart)
      {
         return Serialize(_curveChartTemplateMapper.MapFrom(chart));
      }

      public string SerializeAsStringBasedOn(CurveChart chart)
      {
         return XmlHelper.XmlContentToString(SerializeBasedOn(chart));
      }

      public void SerializeToFile(CurveChartTemplate chartTemplate, string fileFullPath)
      {
         var xel = Serialize(chartTemplate);
         saveTemplateToFile(xel, fileFullPath);
      }

      public void SerializeToFileBasedOn(CurveChart chart, string fileFullPath)
      {
         var xel = SerializeBasedOn(chart);
         saveTemplateToFile(xel, fileFullPath);
      }

      private void saveTemplateToFile(XElement element, string fileFullPath)
      {
         element.AddAttribute(Constants.Serialization.Attribute.VERSION, PKMLVersion.Current);
         element.Save(fileFullPath);
      }
   }
}