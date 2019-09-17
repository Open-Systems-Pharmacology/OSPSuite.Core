using System;
using System.IO;

namespace OSPSuite.R
{
   public static class HelperForSpecs
   {
      public static string DataFile(string fileName)
      {
         return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", fileName);
      }
   }
}