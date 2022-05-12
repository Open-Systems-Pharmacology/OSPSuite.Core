using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Visitor;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Starter
{
   internal class CreateDataForQuantityVisitor : IVisitor<IQuantity>
   {
      private readonly BaseGrid _baseGrid;
      private readonly DataRepository _dataRepository;
      private int _n;

      public CreateDataForQuantityVisitor(DataRepository dataRepository, BaseGrid baseGrid, DateTime date)
      {
         _dataRepository = dataRepository;
         _baseGrid = baseGrid;
         _n = 0;
      }

      public void Visit(IQuantity quantity)
      {
         try
         {
            _n++;

            var dimensionFactory = IoC.Resolve<IDimensionFactory>();
            if (quantity.Dimension == null) quantity.Dimension = dimensionFactory.Dimension("Mass");

            DataColumn dc = new DataColumn(quantity.Name, quantity.Dimension, _baseGrid);
            dc.DataInfo = new DataInfo(ColumnOrigins.Calculation, AuxiliaryType.Undefined, quantity.Dimension.DefaultUnitName, "", 320);

            dc.QuantityInfo = Helper.CreateQuantityInfo(quantity);

            dc.Values = new float[_baseGrid.Count];
            for (int i = 0; i < _baseGrid.Count; i++)
            {
               dc[i] = -4 + _n + (10 * i + 0.2F) * (10 * i + _n - 2); // / _baseGrid.Count ;
               int r;
               Math.DivRem(i, 2, out r);
               if (r == 1) dc[i] = 0;
            }

            _dataRepository.Add(dc);
         }
         catch (Exception ex)
         {
            throw new OSPSuiteException("QuantityVisitor n=" + _n + ", q=" + quantity.Name, ex);
         }
      }

      public void Run(IContainer root)
      {
         root.AcceptVisitor(this);
      }
   }
}