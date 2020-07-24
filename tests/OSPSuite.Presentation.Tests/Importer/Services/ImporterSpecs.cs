using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Utility.Container;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Services 
{
   public abstract class ConcernForImporter : ContextSpecification<Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<UnformattedData>();
         _container = A.Fake<IContainer>();
         var dataFormat = A.Fake<IDataFormat>();

         A.CallTo(() => dataFormat.CheckFile(_basicFormat)).Returns(true);
         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>() {dataFormat});
         _parser = A.Fake<IDataSourceFileParser>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         sut = new Services.Importer(_container, _parser);
      }
   }

   public class When_checking_data_format : ConcernForImporter
   {
      [TestCase]
      public void identify_basic_format()
      {
         var formats = sut.AvailableFormats(_basicFormat);
         formats.Count().ShouldBeEqualTo(1);
      }
   }
}
