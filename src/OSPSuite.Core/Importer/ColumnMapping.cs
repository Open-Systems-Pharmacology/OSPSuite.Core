using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Importer
{
   ///<summary>
   /// This is a class to store mapping information of columns in the excel file and specification of the data to be imported.
   ///</summary>
   public class ColumnMapping : IDisposable
   {
      /// <summary>
      /// This is the left part of the mapping.
      /// </summary>
      public string SourceColumn { get; set; }

      /// <summary>
      /// This is the right part of the mapping.
      /// </summary>
      public string Target { get; set; }

      /// <summary>
      /// This are MetaData to be used for all import data columns as default.
      /// </summary>
      public MetaDataTable MetaData { get; set; }

      /// <summary>
      /// Input parameter information to be used for all import data columns as default.
      /// </summary>
      public IList<InputParameter> InputParameters { get; set; }

      /// <summary>
      /// Unit to be used for all import data columns as default.
      /// </summary>
      public Unit SelectedUnit { get; set; }

      /// <summary>
      /// Property indicating whether the unit of the column has been set explicitly or not.
      /// </summary>
      public bool IsUnitExplicitlySet { get; set; }

      public ColumnMapping Clone()
      {
         return new ColumnMapping
                   {
                      SourceColumn = SourceColumn,
                      Target = Target,
                      InputParameters = (InputParameters == null) ? null : new List<InputParameter>(InputParameters),
                      MetaData = (MetaData == null) ? null : MetaData.Copy(),
                      SelectedUnit = SelectedUnit,
                      IsUnitExplicitlySet = IsUnitExplicitlySet
                   };
      }


      public void Dispose()
      {
         dispose(true);
         GC.SuppressFinalize(this);
      }

      private void dispose(bool disposing)
      {
         if (disposing)
         {
            if (MetaData != null) MetaData.Dispose();
         }

         MetaData = null;
         if (InputParameters != null) InputParameters.Clear();
      }

      ~ColumnMapping()
      {
         dispose (false);
      }

   }
}
