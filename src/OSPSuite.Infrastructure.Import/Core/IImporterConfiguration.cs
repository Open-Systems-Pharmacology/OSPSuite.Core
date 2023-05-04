namespace OSPSuite.Infrastructure.Import.Core
{
   public class ImporterConfiguration
   {
      public string FileName { get; set; }
      public IDataFormat Format { get; set; }
      public string NamingConventions { get; set; }
   }
}