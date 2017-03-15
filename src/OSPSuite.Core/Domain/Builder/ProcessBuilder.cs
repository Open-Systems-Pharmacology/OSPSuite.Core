using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IContainsParameters : IObjectBase
   {
      IEnumerable<IParameter> Parameters { get; }
      void AddParameter(IParameter parameter);
      void RemoveParameter(IParameter parameter);
   }

   /// <summary>
   ///    Base builder interface for all builder creating amount changing objects
   ///    Contains all information used for every kind of amount change
   /// </summary>
   public interface IProcessBuilder : IContainer, IUsingFormula, IContainsParameters
   {
      /// <summary>
      ///    If set to true, a parameter rate named ProcessRate will be generated in the simulation.Its formula
      ///    will be set to the rate of the created process. Default is false
      /// </summary>
      bool CreateProcessRateParameter { get; set; }

      bool ProcessRateParameterPersistable { get; set; }
   }

   /// <summary>
   ///    Base builder interface for all builder creating amount changing objects
   ///    Contains all informations used for every kind of amount change
   /// </summary>
   public abstract class ProcessBuilder : Container, IProcessBuilder
   {
      private IFormula _formula;
      private bool _createProcessRateParameter;
      private bool _processRateParameterPersistable;
      public IDimension Dimension { get; set; }

      public bool CreateProcessRateParameter
      {
         get { return _createProcessRateParameter; }
         set
         {
            _createProcessRateParameter = value;
            if (!_createProcessRateParameter)
               ProcessRateParameterPersistable = false;

            OnPropertyChanged(() => CreateProcessRateParameter);
         }
      }

      public bool ProcessRateParameterPersistable
      {
         get { return _processRateParameterPersistable; }
         set
         {
            _processRateParameterPersistable = CreateProcessRateParameter && value;
            OnPropertyChanged(() => ProcessRateParameterPersistable);
         }
      }

      public IEnumerable<IParameter> Parameters
      {
         get { return GetChildren<IParameter>(); }
      }

      public void AddParameter(IParameter parameter)
      {
         Add(parameter);
      }

      public void RemoveParameter(IParameter parameter)
      {
         RemoveChild(parameter);
      }

      public IFormula Formula
      {
         get { return _formula; }
         set
         {
            _formula = value;
            OnPropertyChanged(() => Formula);
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcProcessBuilder = source as IProcessBuilder;
         if (srcProcessBuilder == null) return;

         Dimension = srcProcessBuilder.Dimension;
         CreateProcessRateParameter = srcProcessBuilder.CreateProcessRateParameter;
         ProcessRateParameterPersistable = srcProcessBuilder.ProcessRateParameterPersistable;
      }
   }
}