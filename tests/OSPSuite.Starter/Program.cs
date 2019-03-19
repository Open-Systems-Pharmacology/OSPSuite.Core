using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Starter.Bootstrapping;
using OSPSuite.Starter.Presenters;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter
{
   internal static class Program
   {
      /// <summary>
      ///    The main entry point for the application.
      /// </summary>
      [STAThread]
      private static void Main(string[] args)
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         ApplicationStartup.Initialize();

         Application.Run(IoC.Resolve<ITestPresenter>().View as Form);
      }
   }

   internal static class Helper
   {
      internal static QuantityInfo CreateQuantityInfo(IQuantity quantity)
      {
         var opf = IoC.Resolve<IObjectPathFactory>();
         var qi = new QuantityInfo(quantity.Name, opf.CreateAbsoluteObjectPath(quantity).ToList(), quantity.QuantityType) {OrderIndex = quantity.Name.Length};
         return qi;
      }

      internal static TestEnvironment TestEnvironment { get; private set; }

      public static string NameDefinition(DataColumn column)
      {
         var name = column.Name + "(" + column.Repository.Name + ")";
         var path = "";
         for (var i = 2; i < column.QuantityInfo.Path.Count() - 1; i++) path += column.QuantityInfo.Path.ElementAt(i) + "/";
         return path + name;
      }

      public static IList<string> DisplayQuantityPathDefinition(DataColumn column)
      {
         IList<string> path = new List<string>(column.QuantityInfo.Path);
         if (path.Count > 0) path.RemoveAt(0);
         if (path.Count > 0) path[0] += "X";

         if (column.DataInfo.Origin == ColumnOrigins.Observation) path.Add(column.Name);
         return path;
      }

      public static string IdentificationKeyDefintion(DataColumn col)
      {
         const int quantityPathLevelForIdentification = 1;

         var key = col.DataInfo.Origin + ObjectPath.PATH_DELIMITER + col.Dimension;
         if (col.DataInfo.Origin == ColumnOrigins.Calculation)
         {
            // use quantity path as key 
            IList<string> quantityPath = col.QuantityInfo.Path.ToList();
            // suppress first path elements for identification because they can depend on the repository         
            for (int i = quantityPathLevelForIdentification; i < quantityPath.Count; i++)
               key += ObjectPath.PATH_DELIMITER + quantityPath[i];
         }
         else if (col.DataInfo.Origin == ColumnOrigins.BaseGrid)
         {
            //Base Grids are identified via the "other" datacolumn of the curve
            return null;
         }
         else if (col.DataInfo.Origin == ColumnOrigins.Observation)
         {
            // use (category, source, date, name) as key 
            key += ObjectPath.PATH_DELIMITER + col.DataInfo.Category
                                             + ObjectPath.PATH_DELIMITER + col.DataInfo.Source
                                             + ObjectPath.PATH_DELIMITER + col.DataInfo.Date.ToShortDateString()
                                             + ObjectPath.PATH_DELIMITER + col.DataInfo.Date.ToShortTimeString()
                                             + ObjectPath.PATH_DELIMITER + col.Name;
         }

         return key;
      }
   }
}