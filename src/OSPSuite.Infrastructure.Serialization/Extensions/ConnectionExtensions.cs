namespace OSPSuite.Infrastructure.Serialization.Extensions;

public static class ConnectionStringHelper
{
   public static string ConnectionStringFor(string dataSource)
   {
      return $"Data Source={dataSource};Foreign Keys=False;Pooling=False";
   }
}