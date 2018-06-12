using System;

namespace OSPSuite.Core.Domain
{
   [Flags]
   public enum ParameterFlag
   {
      None = 0,
      CanBeVaried = 2 << 0,
      ReadOnly = 2 << 1,
      Visible = 2 << 2,
      MinIsAllowed = 2 << 3,
      MaxIsAllowed = 2 << 4,
      CanBeVariedInPopulation = 2 << 5,
   }

   public class ParameterInfo
   {
      public ParameterFlag ParameterFlag { get; private set; }

      public int ReferenceId { get; set; }
      public double? MinValue { get; set; }
      public double? MaxValue { get; set; }
      public string DefaultUnit { get; set; }
      public string GroupName { get; set; }
      public int Sequence { get; set; }

      public PKSimBuildingBlockType BuildingBlockType { get; set; }

      public ParameterInfo()
      {
         //Min value and Max value should be initialized to null to avoid serialization issues
         CanBeVaried = true;
         MinValue = null;
         MinIsAllowed = false;
         MaxValue = null;
         MaxIsAllowed = false;
         Visible = true;
         GroupName = Constants.Groups.UNDEFINED;
         Sequence = 1;
         ReferenceId = 0;
         BuildingBlockType=PKSimBuildingBlockType.Simulation;
      }

      public bool ReadOnly
      {
         get => (ParameterFlag & ParameterFlag.ReadOnly) == ParameterFlag.ReadOnly;
         set
         {
            if (ReadOnly != value)
            {
               ParameterFlag ^= ParameterFlag.ReadOnly;
            }
         }
      }

      public bool CanBeVaried
      {
         get => (ParameterFlag & ParameterFlag.CanBeVaried) == ParameterFlag.CanBeVaried;
         set
         {
            if (CanBeVaried != value)
            {
               ParameterFlag ^= ParameterFlag.CanBeVaried;
            }
         }
      }

      public bool MinIsAllowed
      {
         get => (ParameterFlag & ParameterFlag.MinIsAllowed) == ParameterFlag.MinIsAllowed;
         set
         {
            if (MinIsAllowed != value)
            {
               ParameterFlag ^= ParameterFlag.MinIsAllowed;
            }
         }
      }

      public bool MaxIsAllowed
      {
         get => (ParameterFlag & ParameterFlag.MaxIsAllowed) == ParameterFlag.MaxIsAllowed;
         set
         {
            if (MaxIsAllowed != value)
            {
               ParameterFlag ^= ParameterFlag.MaxIsAllowed;
            }
         }
      }

      public bool Visible
      {
         get => (ParameterFlag & ParameterFlag.Visible) == ParameterFlag.Visible;
         set
         {
            if (Visible != value)
            {
               ParameterFlag ^= ParameterFlag.Visible;
            }
         }
      }

      public bool CanBeVariedInPopulation
      {
         get => (ParameterFlag & ParameterFlag.CanBeVariedInPopulation) == ParameterFlag.CanBeVariedInPopulation;
         set
         {
            if (CanBeVariedInPopulation != value)
            {
               ParameterFlag ^= ParameterFlag.CanBeVariedInPopulation;
            }
         }
      }

      public ParameterInfo Clone()
      {
         var clone = new ParameterInfo();
         clone.UpdatePropertiesFrom(this);
         return clone;
      }

      public void UpdatePropertiesFrom(ParameterInfo parameterInfo)
      {
         if (parameterInfo == null) return;
         MinValue = parameterInfo.MinValue;
         MaxValue = parameterInfo.MaxValue;
         GroupName = parameterInfo.GroupName;
         ParameterFlag = parameterInfo.ParameterFlag;
         Sequence = parameterInfo.Sequence;
         DefaultUnit = parameterInfo.DefaultUnit;
         BuildingBlockType = parameterInfo.BuildingBlockType;
      }
   }
}