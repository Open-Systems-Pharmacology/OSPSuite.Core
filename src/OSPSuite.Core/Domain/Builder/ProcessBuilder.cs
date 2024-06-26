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

   public interface IProcessBuilder : IContainer, IUsingFormula, IBuilder, IContainsParameters
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
   ///    Contains all information used for every kind of amount change
   /// </summary>
   public abstract class ProcessBuilder : Container, IProcessBuilder
   {
      private IFormula _formula;
      private bool _createProcessRateParameter;
      private bool _processRateParameterPersistable;
      public IDimension Dimension { get; set; }
      public IBuildingBlock BuildingBlock { get; set; }

      /// <summary>
      ///    If set to true, a parameter rate named ProcessRate will be generated in the simulation.Its formula
      ///    will be set to the rate of the created process. Default is false
      /// </summary>
      public bool CreateProcessRateParameter
      {
         get => _createProcessRateParameter;
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
         get => _processRateParameterPersistable;
         set
         {
            _processRateParameterPersistable = CreateProcessRateParameter && value;
            OnPropertyChanged(() => ProcessRateParameterPersistable);
         }
      }

      public IEnumerable<IParameter> Parameters => GetChildren<IParameter>();

      public void AddParameter(IParameter parameter) => Add(parameter);

      public void RemoveParameter(IParameter parameter) => RemoveChild(parameter);

      public IFormula Formula
      {
         get => _formula;
         set
         {
            _formula = value;
            OnPropertyChanged(() => Formula);
         }
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcProcessBuilder = source as ProcessBuilder;
         if (srcProcessBuilder == null) return;

         Dimension = srcProcessBuilder.Dimension;
         CreateProcessRateParameter = srcProcessBuilder.CreateProcessRateParameter;
         ProcessRateParameterPersistable = srcProcessBuilder.ProcessRateParameterPersistable;
      }
   }
}