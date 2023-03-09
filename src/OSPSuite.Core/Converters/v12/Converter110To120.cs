using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converters.v12
{
   public class Converter110To120 : IObjectConverter,
      IVisitor<ISpatialStructure>
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private bool _converted;

      public Converter110To120(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V11_0;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         performConversion(objectToUpdate);
         return (PKMLVersion.V12_0, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V12_0, false);
      }

      private void performConversion(object objectToUpdate) => this.Visit(objectToUpdate);

      public void Visit(ISpatialStructure spatialStructure)
      {
         spatialStructure.Neighborhoods.Each(updateNeighborsPathIn);
      }

      private void updateNeighborsPathIn(NeighborhoodBuilder neighborhoodBuilder)
      {
         neighborhoodBuilder.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.FirstNeighbor);
         neighborhoodBuilder.SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(neighborhoodBuilder.SecondNeighbor);
         _converted = true;
      }
   }
}