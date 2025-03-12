using System;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;

namespace OSPSuite.R
{
   [IntegrationTests]
   public abstract class ContextForIntegration<T> : ContextSpecification<T>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         Api.InitializeOnce(new ApiConfig
         {
            DimensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.DIMENSIONS_FILE_NAME),
            PKParametersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.PK_PARAMETERS_FILE_NAME),
         });

         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
      }
   }
}