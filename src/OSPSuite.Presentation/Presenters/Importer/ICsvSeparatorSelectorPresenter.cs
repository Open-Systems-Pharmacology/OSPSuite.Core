using OSPSuite.Assets;
using OSPSuite.Presentation.Views.Importer;
using System.Text;
using System.IO;
using System.Linq;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface ICsvSeparatorSelectorPresenter : IDisposablePresenter
   {
      void SetFileName(string fileName);
      char? GetCsvSeparator();
      bool Canceled();
      char SelectedSeparator { get; set; }
   }

   public class CsvSeparatorSelectorPresenter : AbstractDisposablePresenter<ICsvSeparatorSelectorView, ICsvSeparatorSelectorPresenter>,
      ICsvSeparatorSelectorPresenter
   {
      public char SelectedSeparator { get; set; }
      public CsvSeparatorSelectorPresenter(ICsvSeparatorSelectorView view) : base(view)
      {
      }

      public void SetFileName(string fileName)
      {
         _view.SetDescription(generateDescriptionFromFileName(fileName));
         _view.Display();
      }

      private string generateDescriptionFromFileName(string fileName)
      {
         var text = new StringBuilder();
         text.AppendLine(Captions.Importer.CsvSeparatorDescription(fileName));
         foreach (var line in File.ReadLines(fileName).Take(3))
         {
            text.AppendLine(line);
         }
         text.Append("...");
         return text.ToString();
      }

      public char? GetCsvSeparator()
      {
         return SelectedSeparator;
      }

      public bool Canceled()
      {
         return _view.Canceled;
      }
   }
}
