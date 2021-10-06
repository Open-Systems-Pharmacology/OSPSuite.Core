using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class EmptyNamingConventionsException : AbstractImporterException
   {
      public EmptyNamingConventionsException() : base(Error.NamingConventionEmpty)
      {
      }
   }

   public class NullNamingConventionsException : AbstractImporterException
   {
      public NullNamingConventionsException() : base(Error.NamingConventionNull)
      {
      }
   }

   public class InconsistentMoleculeAndMolWeightException : AbstractImporterException
   {
      public InconsistentMoleculeAndMolWeightException() : base(Error.InconsistentMoleculeAndMolWeightException)
      {
      }
   }

   public class ColumnNotFoundException : AbstractImporterException
   {
      public ColumnNotFoundException(string columnName) : base(Error.ColumnNotFound(columnName))
      {
      }
   }
   public class BaseGridColumnNotFoundException : AbstractImporterException
   {
      public BaseGridColumnNotFoundException(string columnName) : base(Error.BaseGridColumnNotFoundException(columnName))
      {
      }
   }
}