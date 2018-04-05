using System;
using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Starter.Services
{
   public class ParameterToParameterDTOMapper : IParameterToParameterDTOMapper
   {
      public IParameterDTO MapFrom(IParameter parameter)
      {
         return new ParameterDTO(parameter);
      }
   }

   public class ParameterDTO : ValidatableDTO<IParameter>, IParameterDTO
   {
      public ParameterDTO(IParameter parameter) : base(parameter)
      {
         Parameter = parameter;
         bindToParameter();
      }

      private void bindToParameter()
      {
         if (Parameter == null)
            return;

         Parameter.PropertyChanged += handlePropertyChanged;

         if (Parameter.Editable)
            Rules.Add(ParameterDTORules.ParameterIsValid());
      }

      private void handlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals("Value"))
         {
            ValueChanged(this, EventArgs.Empty);
         }

         RaisePropertyChanged(e.PropertyName);
      }

      public IDimension Dimension
      {
         get => Parameter.Dimension;
         set => Parameter.Dimension = value;
      }

      public Unit DisplayUnit
      {
         get => Parameter.DisplayUnit;
         set => Parameter.DisplayUnit = value;
      }

      public string Name
      {
         get => Parameter.Name;
         set => Parameter.Name = value;
      }

      public IEnumerable<Unit> AllUnits
      {
         get => Parameter.Dimension.Units;
         set { }
      }

      public double Value
      {
         get => Parameter.ValueInDisplayUnit;
         set => Parameter.ValueInDisplayUnit = value;
      }

      public PathElements PathElements { get; set; } = new PathElements();
      public PathElementDTO SimulationPathElement => PathElements[PathElement.Simulation];
      public PathElementDTO TopContainerPathElement => PathElements[PathElement.TopContainer];
      public PathElementDTO ContainerPathElement => PathElements[PathElement.Container];
      public PathElementDTO BottomCompartmentPathElement => PathElements[PathElement.BottomCompartment];
      public PathElementDTO MoleculePathElement => PathElements[PathElement.Molecule];
      public PathElementDTO NamePathElement => PathElements[PathElement.Name];
      public string Category { get; } = string.Empty;
      public string DisplayPathAsString => PathElements.ToString();
      public string Description { get; set; }
      public IParameter Parameter { get; }
      public double KernelValue => Parameter.Value;
      public bool IsFavorite { get; set; }
      public ValueOrigin ValueOrigin { get; set; } = new ValueOrigin();

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin) => Parameter.UpdateValueOriginFrom(sourceValueOrigin);

      public bool IsDiscrete => false;
      public ICache<double, string> ListOfValues { get; } = new Cache<double, string>();
      public event EventHandler ValueChanged = delegate { };
      public string DisplayName { get; set; }
      public FormulaType FormulaType { get; set; }
      public int Sequence { get; set; }
      public double Percentile { get; set; }
      public bool Editable => true;

      public void Release()
      {
         if (Parameter == null)
            return;

         Parameter.PropertyChanged -= handlePropertyChanged;
      }
   }
}