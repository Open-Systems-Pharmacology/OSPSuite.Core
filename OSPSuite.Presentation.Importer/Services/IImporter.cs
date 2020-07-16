
using Org.BouncyCastle.Math.EC.Rfc7748;
using OSPSuite.Presentation.Importer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Importer.Services
{
   public interface IImporter
   {
      IDataSourceFile LoadFile(string path);
      IDataSource ImportFromFile(IDataSourceFile file);
      IList<IDataFormat> AvailableFormats(IUnformattedData data);
   }

   public class Importer : IImporter
   {
      private readonly IoC container;

      public Importer(IoC container)
      {
         this.container = container;
      }

      public IList<IDataFormat> AvailableFormats(IUnformattedData data)
      {
         return GetType().Assembly.GetTypes()
            .Where(x => typeof(IDataFormat).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => container.Resolve<IDataFormat>(x.FullName))
            .Where(x => x.CheckFile(data))
            .ToList();
      }

      public IDataSource ImportFromFile(IDataSourceFile file)
      {
         throw new System.NotImplementedException();
      }

      public IDataSourceFile LoadFile(string path)
      {
         //Maybe also list supported file formats
         throw new System.NotImplementedException();
      }
   }
}
