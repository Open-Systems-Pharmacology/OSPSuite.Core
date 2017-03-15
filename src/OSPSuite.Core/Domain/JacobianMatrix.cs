using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class JacobianMatrix
   {
      public List<string> ParameterNames { get; }
      private readonly List<JacobianRow> _rows = new List<JacobianRow>();
      private readonly Cache<string, PartialDerivatives> _partialDerivatives = new Cache<string, PartialDerivatives>(x=>x.FullOutputPath);
      public IReadOnlyList<JacobianRow> Rows => _rows;
      public int ColumnCount => ParameterNames.Count;
      public int RowCount => Rows.Count;
      public JacobianRow this[int rowIndex] => _rows[rowIndex];
      public IReadOnlyCollection<PartialDerivatives> AllPartialDerivatives => _partialDerivatives;

      [Obsolete("For serialization")]
      public JacobianMatrix()
      {
      }

      public JacobianMatrix(IEnumerable<string> parameterNames)
      {
         ParameterNames = parameterNames.ToList();
      }

      public void AddRows(IEnumerable<JacobianRow> rows)
      {
         rows.Each(AddRow);
      }

      public void AddRow(JacobianRow row)
      {
         if (row.Count != ParameterNames.Count)
            throw new InvalidArgumentException($"No the expected number of values ({row.Count} vs {ColumnCount})");

         _rows.Add(row);
         row.Matrix = this;
      }

      public int ColumnIndexFor(string parameterName)
      {
         return ParameterNames.IndexOf(parameterName);
      }

      public void AddPartialDerivatives(PartialDerivatives partialDerivatives)
      {
         _partialDerivatives.Add(partialDerivatives);
      }

      public virtual PartialDerivatives PartialDerivativesFor(string fullOutputPath)
      {
         return _partialDerivatives[fullOutputPath];
      }

   }

   public class JacobianRow
   {
      public string FullOutputPath { get; }
      public double Time { get; }
      public double[] Values { get; }
      public int Count => Values.Length;
      public JacobianMatrix Matrix { get; set; }

      public double this[int columnIndex] => Values[columnIndex];
      public double this[string parameterName] => this[Matrix.ColumnIndexFor(parameterName)];

      [Obsolete("For serialization")]
      public JacobianRow()
      {
      }

      public JacobianRow(string fullOutputPath, double time, double[] values)
      {
         FullOutputPath = fullOutputPath;
         Time = time;
         Values = values;
      }
   }
}