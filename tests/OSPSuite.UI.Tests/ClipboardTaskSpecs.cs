using System.Data;
using System.Windows.Forms;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Mappers;
using OSPSuite.UI.Mappers;
using OSPSuite.UI.Services;

namespace OSPSuite.UI
{
   public abstract class concern_for_ClipboardCopyTask : ContextSpecification<ClipboardTask>
   {
      protected IDataTableToHtmlClipboardFormatMapper _dataTableToHtmlClipboardFormatMapper;
      protected IDataTableToTextMapper _dataTableToTextMapper;

      protected override void Context()
      {
         _dataTableToHtmlClipboardFormatMapper = A.Fake<IDataTableToHtmlClipboardFormatMapper>();
         _dataTableToTextMapper = A.Fake<IDataTableToTextMapper>();

         sut = new ClipboardTask(_dataTableToHtmlClipboardFormatMapper, _dataTableToTextMapper);
      }
   }

   public class When_mapping_data_table_to_clipboard_format : concern_for_ClipboardCopyTask
   {
      private DataTable _dataTable;
      private DataObject _result;

      protected override void Context()
      {
         base.Context();
         _dataTable = new DataTable();
      }

      protected override void Because()
      {
         _result = sut.CreateDataObject(_dataTable);
      }

      [Observation]
      public void must_have_mapped_html_format_for_data_table()
      {
         A.CallTo(() => _dataTableToHtmlClipboardFormatMapper.MapFrom(_dataTable, true)).MustHaveHappened();
      }

      [Observation]
      public void must_have_mapped_plain_text_format_for_data_table()
      {
         A.CallTo(() => _dataTableToTextMapper.MapFrom(_dataTable, true)).MustHaveHappened();
      }

      [Observation]
      public void must_contain_all_data_types_supported()
      {
         _result.GetDataPresent(Captions.DataTable).ShouldBeTrue();
         _result.GetDataPresent(DataFormats.Html).ShouldBeTrue();
         _result.GetDataPresent(DataFormats.UnicodeText).ShouldBeTrue();
         _result.GetDataPresent(DataFormats.Text).ShouldBeTrue();
      }
   }
}
