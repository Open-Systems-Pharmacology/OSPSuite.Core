using System.Linq;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Converter.v5_6
{
   public class Converter551To561 : IObjectConverter, IVisitor<IPassiveTransportBuildingBlock>
   {
      private bool _converted;
      public bool IsSatisfiedBy(int version) => version == PKMLVersion.V5_5_1;

      public (int convertedToVersion, bool conversionHappened) Convert(object objectToUpdate)
      {
         _converted = false;
         this.Visit(objectToUpdate);
         return (PKMLVersion.V5_6_1, _converted);
      }

      public (int convertedToVersion, bool conversionHappened) ConvertXml(XElement element)
      {
         return (PKMLVersion.V5_6_1, false);
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlockToConvert)
      {
         // We need to set all parameters build mode to local if not already local, this was wrong in older versions of the ospsuite
         var allNonLocalParameters =
            passiveTransportBuildingBlockToConvert
               .SelectMany(ptb => ptb.GetAllChildren<IParameter>(p => p.BuildMode != ParameterBuildMode.Local))
               .ToList();

         allNonLocalParameters.Each(p => p.BuildMode = ParameterBuildMode.Local);
         _converted = true;
      }
   }
}