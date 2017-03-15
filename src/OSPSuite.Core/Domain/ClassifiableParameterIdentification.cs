using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public class ClassifiableParameterIdentification : Classifiable<ParameterIdentification>
   {
      public ClassifiableParameterIdentification() : base(ClassificationType.ParameterIdentification)
      {
      }

      public ParameterIdentification ParameterIdentification => Subject;
   }
}