using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Starter.Tasks
{
   public class ClassificationTypeToRootNodeTypeMapper : IClassificationTypeToRootNodeTypeMapper
   {
      public RootNodeType MapFrom(ClassificationType input)
      {
         return new RootNodeType(input.ToString(),ApplicationIcons.ActiveInflux,ClassificationType.Unknown);
      }
   }
}