using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Importer;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Importer.Services 
{
   public abstract class ConcernForImporter : ContextSpecification<Importer>
   {
      protected IUnformattedData _basicFormat;
      protected IContainer _container;
      protected IDataSourceFileParser _parser;
      protected IDialogCreator _dialogCreator;
      protected IReadOnlyList<ColumnInfo> _columnInfos;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _basicFormat = A.Fake<UnformattedData>();
         _container = A.Fake<IContainer>();
         _dialogCreator = A.Fake<IDialogCreator>();
         var dataFormat = A.Fake<IDataFormat>();
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };

         A.CallTo(() => dataFormat.SetParameters(_basicFormat, _columnInfos)).Returns(true);
         A.CallTo(() => _container.ResolveAll<IDataFormat>()).Returns(new List<IDataFormat>() {dataFormat});
         _parser = A.Fake<IDataSourceFileParser>();
         A.CallTo(() => _container.Resolve<IDataSourceFileParser>()).Returns(_parser);
         sut = new Importer(_container, _parser);
      }
   }

   public class When_checking_data_format : ConcernForImporter
   {
      [TestCase]
      public void identify_basic_format()
      {
         var formats = sut.AvailableFormats(_basicFormat, _columnInfos);
         formats.Count().ShouldBeEqualTo(1);
      }
   }
}
