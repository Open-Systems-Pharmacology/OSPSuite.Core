using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Container;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class concern_for_Importer : ContextSpecification<Services.Importer>
   {
      protected IUnformattedData basicFormat;
      protected IContainer IoC;
      protected IDataSourceFileParser parser;

      public override void GlobalContext()
      {
         base.GlobalContext();
         basicFormat = A.Fake<UnformattedData>();
         IoC = A.Fake<IContainer>();
         var dataFormat = A.Fake<IDataFormat>();
         A.CallTo(() => dataFormat.CheckFile(basicFormat)).Returns(true);
         A.CallTo(() => IoC.Resolve<IDataFormat>(A<string>.Ignored)).Returns(dataFormat);
         parser = A.Fake<IDataSourceFileParser>();
         A.CallTo(() => IoC.Resolve<IDataSourceFileParser>()).Returns(parser);
         sut = new Services.Importer(IoC);
      }
   }

   public class when_checking_DataFormat : concern_for_Importer
   {
      [TestCase]
      public void identify_basic_format()
      {
         var formats = sut.AvailableFormats(basicFormat);
         formats.Count.ShouldBeEqualTo(1);
      }
   }
}
