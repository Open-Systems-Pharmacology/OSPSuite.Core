using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Nodes
{
   public class ParameterIdentificationNode : ObjectWithIdAndNameNode<ClassifiableParameterIdentification>, IViewItem
   {
      public ParameterIdentificationNode(ClassifiableParameterIdentification classifiableParameterIdentification)
         : base(classifiableParameterIdentification)
      {
      }

      public ParameterIdentification ParameterIdentification => Tag.ParameterIdentification;
   }
}