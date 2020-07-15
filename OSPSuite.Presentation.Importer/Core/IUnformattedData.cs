using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core
{
   //THIS IS NOT STRING, WE HAVE TO DO IT GENERIC
   public interface IUnformattedData
   {
      IList<string> GetRow(int index);
      IList<string> GetColumn(int columnIndex); 
      IList<string> GetColumn(string columnName);
      IList<string> getHeadersList();


      IList<IList<string>> GetRows(Func<List<string>, bool> filter);


      bool AddRow(List<string> row);
      void AddColumn(string columnName, int columnIndex); 
      //or even get a whole row and create all the columns
      // NO!!!!! - it makes no sense if we each column has a different datatype.
      //datatype either we do when we fill the headers - or alternatively when they are filled
      //we get a function CalculateDataTypeAndExistingValues (for teh ExistingValues we will have to do anyway)

   }

   public class UnformattedData : IUnformattedData
   {
      // WE COULD EVEN PROVIDE FOR A CONSTRUCTOR, THAT GETS A RANGE --- NAAAAH: A RANGE IS ACTUALLY A DATATABLE - 
      //otherwise we need to directly interface with npoi/excel


      // I believe this should be Dictionary<ColumnDescription, <List<string>>
      //private Dictionary<ColumnDescription, List<string>> rawDataTable;
      //after loading the rawDataTable we should not be able to access with index anymore -> this is the correct logic for the
      //representation of Data we want. Then headers we do not really want: it is simply rawDataTable.Keys
      //of course this opens the discussion whether we need UnformatedData...(MAYBE OPENS)

      //alternatively we could do an ordered dictionary - but the we should get the index in ICollection Keys, and ask for the value 
      //I Collection values - it seems way simpler to simply keep in ColumnDescription the row index for the list of lists, and simply add rows

      //we are back on the same problem: lists in the list are columns, NOT rows
      
      private List<List<string>> rawDataTable = new List<List<string>>();

      public Dictionary<string, ColumnDescription> headers = new Dictionary<string, ColumnDescription>(); //we have to ensure headers and RawData sizes match

      public void AddColumn(string columnName, int columnIndex) //it seems to me there is little sense in adding column after column
                                                //the list of headers is somehow the definition of the table
      {
         headers.Add(columnName, new ColumnDescription(columnIndex)); 
      }

      public bool AddRow(List<string> row)
      {                                   //the not empty row part we could check explicitly
         if (headers.Count == row.Count ) //I suppose row.Count != 0, so we do not add Data to a DataSheet without column names
         {
            rawDataTable.Add(row);
            return true;
         }

         return false;
      }

      public IList<string> GetColumn(int columnIndex)
      {
         var resultList = new List<string>();

         for ( int i = 0; i < rawDataTable.Count; i++ ) //should have at least one row
            resultList.Add(rawDataTable[i][columnIndex]); //make sure the 2 indexes are correctly positioned here

         return resultList;
      } 

      public IList<string> GetColumn(string columnName)
      {
         var columnIndex = headers[columnName].Index;

         var resultColumn = new List<string>();

         for (int i = 0; i < rawDataTable.Count; i++)
            resultColumn.Add(rawDataTable[i][columnIndex]);

         return resultColumn;
      }

      public IList<string> getHeadersList()
      {
         return headers.Keys.ToList();
      }

      public IList<string> GetRow(int index)
      {
         return rawDataTable[index];
      }

      public IList<IList<string>> GetRows(Func<List<string>, bool> filter)
      {
         throw new NotImplementedException();
      }
   }
}
