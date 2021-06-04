using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ColumnXmlSerializer : OSPSuiteXmlSerializer<Column>
   {
      public override void PerformMapping()
      {
         Map(x => x.ErrorStdDev);
         Map(x => x.LloqColumn);
         Map(x => x.Name);
         Map(x => x.Unit);
         Map(x => x.Dimension);
      }

      protected override void TypedDeserialize(Column objectToDeserialize, XElement outputToDeserialize, SerializationContext context)
      {
         base.TypedDeserialize(objectToDeserialize, outputToDeserialize, context);

         if (objectToDeserialize.ErrorStdDev == Constants.STD_DEV_GEOMETRIC)
         {
            objectToDeserialize.Unit = new UnitDescription();
            objectToDeserialize.Dimension = Constants.Dimension.NO_DIMENSION;
         }

         if (!objectToDeserialize.Unit.ColumnName.IsNullOrEmpty())
         {
            objectToDeserialize.Dimension = null;
         }
      }
   }
}
