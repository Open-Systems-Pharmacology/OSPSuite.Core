using System.Linq;
using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;

namespace OSPSuite.Core.Converter.v5_6
{
   public class Converter551To561 : IObjectConverter, IVisitor<IPassiveTransportBuildingBlock>
   {
      public bool IsSatisfiedBy(int version)
      {
         return version == PKMLVersion.V5_5_1;
      }

      public int Convert(object objectToUpdate)
      {
         this.Visit(objectToUpdate);
         return PKMLVersion.V5_6_1;
      }

      public int ConvertXml(XElement element)
      {
         return PKMLVersion.V5_6_1;
      }

      public void Visit(IPassiveTransportBuildingBlock passiveTransportBuildingBlockToConvert)
      {
         // We need to set all parameters build mode to local if not already local, this was wrong in older versions of the ospsuite
         var allNonLocalParameters =
            passiveTransportBuildingBlockToConvert
               .SelectMany(ptb => ptb.GetAllChildren<IParameter>(p => p.BuildMode != ParameterBuildMode.Local))
               .ToList();

         allNonLocalParameters.Each(p => p.BuildMode = ParameterBuildMode.Local);
      }
   }
}