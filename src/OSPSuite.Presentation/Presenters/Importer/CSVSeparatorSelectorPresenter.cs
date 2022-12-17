using System;
using System.Linq;
using System.Text;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using static OSPSuite.Assets.Captions.Importer;
using File = System.IO.File;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ICSVSeparatorSelectorPresenter : IDisposablePresenter
   {
      void SetFileName(string fileName);
      (char? ColumnSeparator, char DecimalSeparator) GetCSVSeparators();
      bool Canceled();
      int PreviewLineCount { get; }
      void SetColumnSeparator(char selectedSeparator);
      void SetDecimalSeparator(char selectedSeparator);
   }

   public class CSVSeparatorSelectorPresenter : AbstractDisposablePresenter<ICSVSeparatorSelectorView, ICSVSeparatorSelectorPresenter>,
      ICSVSeparatorSelectorPresenter
   {
      private const int LineLength = 100;
      private char _columnSeparator;
      private char _decimalSeparator;
      public int PreviewLineCount => 5;

      public void SetColumnSeparator(char selectedSeparator)
      {
         _columnSeparator = selectedSeparator;
      }

      public void SetDecimalSeparator(char selectedSeparator)
      {
         _decimalSeparator = selectedSeparator;
      }

      public CSVSeparatorSelectorPresenter(ICSVSeparatorSelectorView view) : base(view)
      {
      }

      public void SetFileName(string fileName)
      {
         _view.SetInstructions(CsvSeparatorInstructions(fileName));
         _view.SetPreview(generatePreview(fileName));
         _view.Display();
      }

      private string generatePreview(string fileName)
      {
         var text = new StringBuilder();

         foreach (var line in File.ReadLines(fileName).SkipWhile(string.IsNullOrEmpty).Take(PreviewLineCount))
         {
            text.AppendLine(line.Substring(0, Math.Min(line.Length, LineLength)) + (line.Length > LineLength ? "..." : ""));
         }

         return text.ToString().WithEllipsis();
      }

      public (char? ColumnSeparator, char DecimalSeparator) GetCSVSeparators()
      {
         return (_columnSeparator, _decimalSeparator);
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }
   }
}