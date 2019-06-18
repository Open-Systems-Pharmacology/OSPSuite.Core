using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;

namespace OSPSuite.Helpers
{
   public static class AssertForSpecs
   {
      public static void AreEqual<T>(T x1, T x2) where T : class
      {
         Assert.AreEqual(x1.GetType(), x2.GetType());

         if (typeof (T) == typeof (String))
         {
            AreEqualStrings(x1 as string, x2 as string);
            return;
         }

         if (x1.IsAnImplementationOf<OutputInterval>())
         {
            AreEqualOutputInterval(x1 as OutputInterval, x2 as OutputInterval);
            return;
         }
         if (x1.IsAnImplementationOf<DataColumn>())
         {
            AreEqualMcDataColumn(x1 as DataColumn, x2 as DataColumn);
            return;
         }
         // Types related to EventBuilder
         if (x1.IsAnImplementationOf<IApplicationBuilder>())
         {
            AreEqualApplicationBuilder((IApplicationBuilder) x1, (IApplicationBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEventGroupBuilder>())
         {
            AreEqualEventGroupBuilder((IEventGroupBuilder) x1, (IEventGroupBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEventBuilder>())
         {
            AreEqualEventBuilder((IEventBuilder) x1, (IEventBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEventAssignmentBuilder>())
         {
            AreEqualEventAssignmentBuilder((IEventAssignmentBuilder) x1, (IEventAssignmentBuilder) x2);
            return;
         }
         // Types derived from other Builder
         if (x1.IsAnImplementationOf<INeighborhoodBuilder>())
         {
            AreEqualNeighborhoodBuilder((INeighborhoodBuilder) x1, (INeighborhoodBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<TransporterMoleculeContainer>())
         {
            AreEqualTransporterMoleculeContainer(x1 as TransporterMoleculeContainer, x2 as TransporterMoleculeContainer);
         }
         if (x1.IsAnImplementationOf<InteractionContainer>())
         {
            AreEqualInteractionContainer(x1 as InteractionContainer, x2 as InteractionContainer);
         }
         if (x1.IsAnImplementationOf<IMoleculeBuilder>())
         {
            AreEqualMoleculeBuilder((IMoleculeBuilder) x1, (IMoleculeBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IParameter>())
         {
            AreEqualParameterBuilder((IParameter) x1, (IParameter) x2);
            return;
         }
         // Types derived from ObserverBuilder
         if (x1.IsAnImplementationOf<IContainerObserverBuilder>())
         {
            AreEqualObserverBuilder((IContainerObserverBuilder) x1, (IContainerObserverBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IAmountObserverBuilder>())
         {
            AreEqualAmountObserverBuilder((IAmountObserverBuilder) x1, (IAmountObserverBuilder) x2);
            return;
         }
         // Types derived from ProcessBuilder
         if (x1.IsAnImplementationOf<IReactionPartnerBuilder>())
         {
            AreEqualReactionPartnerBuilder((IReactionPartnerBuilder) x1, (IReactionPartnerBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IReactionBuilder>())
         {
            AreEqualReactionBuilder((IReactionBuilder) x1, (IReactionBuilder) x2);
            return;
         }
         if (x1.IsAnImplementationOf<ITransportBuilder>())
         {
            AreEqualTransportBuilder((ITransportBuilder) x1, (ITransportBuilder) x2);
            return;
         }

         // StartValue Types
         if (x1.IsAnImplementationOf<IMoleculeStartValue>())
         {
            AreEqualMoleculeStartValue((IMoleculeStartValue) x1, (IMoleculeStartValue) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IParameterStartValue>())
         {
            AreEqualParameterStartValue((IParameterStartValue) x1, (IParameterStartValue) x2);
            return;
         }
         // Types derived from Event
         if (x1.IsAnImplementationOf<IEventGroup>())
         {
            AreEqualEventGroup((IEventGroup) x1, (IEventGroup) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEvent>())
         {
            AreEqualEvent((IEvent) x1, (IEvent) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEventAssignment>())
         {
            AreEqualEventAssignment((IEventAssignment) x1, (IEventAssignment) x2);
            return;
         }
         // Types derived from Process
         if (x1.IsAnImplementationOf<IReaction>())
         {
            AreEqualReaction((IReaction) x1, (IReaction) x2);
            return;
         }
         if (x1.IsAnImplementationOf<ITransport>())
         {
            AreEqualTransport((ITransport) x1, (ITransport) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IProcess>())
         {
            AreEqualProcess((IProcess) x1, (IProcess) x2);
            return;
         }

         // Types derived from QuantityAndContainer
         if (x1.IsAnImplementationOf<IDistributedParameter>())
         {
            AreEqualDistributedParameter((IDistributedParameter) x1, (IDistributedParameter) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IMoleculeAmount>())
         {
            AreEqualMoleculeAmount((IMoleculeAmount) x1, (IMoleculeAmount) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IQuantityAndContainer>())
         {
            AreEqualQuantityAndContainer((IQuantityAndContainer) x1, (IQuantityAndContainer) x2);
            return;
         }

         // Types derived from Quantity
         if (x1.IsAnImplementationOf<IParameter>())
         {
            AreEqualParameter((IParameter) x1, (IParameter) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IObserver>())
         {
            AreEqualObserver((IObserver) x1, (IObserver) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IQuantity>())
         {
            AreEqualQuantity((IQuantity) x1, (IQuantity) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IFormulaUsable>())
         {
            AreEqualFormulaUsable((IFormulaUsable) x1, (IFormulaUsable) x2);
            return;
         }
         if (x1.IsAnImplementationOf<DataRepository>())
         {
            AreEqualMcDataRepository(x1 as DataRepository, x2 as DataRepository);
            return;
         }

         // Types derived from Formula
         if (x1.IsAnImplementationOf<ExplicitFormula>())
         {
            AreEqualExplicitFormula(x1.DowncastTo<ExplicitFormula>(), x2.DowncastTo<ExplicitFormula>());
            return;
         }
         if (x1.IsAnImplementationOf<ConstantFormula>())
         {
            AreEqualConstantFormula(x1.DowncastTo<ConstantFormula>(), x2.DowncastTo<ConstantFormula>());
            return;
         }
         if (x1.IsAnImplementationOf<TableFormula>())
         {
            AreEqualTableFormula(x1.DowncastTo<TableFormula>(), x2.DowncastTo<TableFormula>());
            return;
         }
         if (x1.IsAnImplementationOf<ValuePoint>())
         {
            AreEqualValuePoint(x1.DowncastTo<ValuePoint>(), x2.DowncastTo<ValuePoint>());
            return;
         }
         if (x1.IsAnImplementationOf<IFormula>())
         {
            AreEqualFormula((IFormula) x1, (IFormula) x2);
            return;
         }

         // Types derived from ObjectBase and Container
         if (x1.IsAnImplementationOf<INeighborhood>())
         {
            AreEqualNeighborhood((INeighborhood) x1, (INeighborhood) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IContainer>())
         {
            AreEqualContainer((IContainer) x1, (IContainer) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IEntity>())
         {
            AreEqualEntity((IEntity) x1, (IEntity) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IObjectBase>())
         {
            AreEqualObjectBase((IObjectBase) x1, (IObjectBase) x2);
            return;
         }

         // Types not derived from IObjectBase
         if (x1.IsAnImplementationOf<IReactionPartner>())
         {
            AreEqualReactionPartner((IReactionPartner) x1, (IReactionPartner) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IObjectReference>())
         {
            AreEqualObjectReference((IObjectReference) x1, (IObjectReference) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IFormulaUsablePath>())
         {
            AreEqualFormulaUsablePath((IFormulaUsablePath) x1, (IFormulaUsablePath) x2);
            return;
         }
         if (x1.IsAnImplementationOf<IObjectPath>())
         {
            AreEqualObjectPath((IObjectPath) x1, (IObjectPath) x2);
            return;
         }

         if (x1.IsAnImplementationOf<Axis>())
         {
            AreEqualAxis(x1 as Axis, x2 as Axis);
            return;
         }

         Assert.Fail("No McAssert.Equal available for Type " + x1.GetType().Name);
      }

      public static bool AssertBothNotNull(object x2, object x1)
      {
         if (x2 == null)
         {
            Assert.IsNull(x1);
            return false;
         }
         
         if (x1 == null)
         {
            Assert.IsNull(x2);
         }
         return true;
      }

      // checks whether both are null or else both are equal
      public static void AssertAreEqual(object x2, object x1)
      {
         if (AssertBothNotNull(x2, x1)) Assert.AreEqual(x2, x1);
      }

      // checks whether both are null or else both are equal
      public static void AreEqualStrings(string x1, string x2)
      {
         if (string.IsNullOrEmpty(x1) && string.IsNullOrEmpty(x2))
            return;
         Assert.AreEqual(x2, x1);
      }

      // checks whether both are null or else both are equal
      public static void AssertAreEqualDouble(double x1, double x2)
      {
         //if (double.IsNaN(x2)) Assert.IsNaN(x1);
         if (double.IsNaN(x1)) Assert.IsNaN(x2);
         Assert.AreEqual(x1, x2, 1e-10);
      }

      public static void AssertAreEqualNullableDouble(double? x1, double? x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         if (double.IsNaN(x1.Value))
            Assert.IsNaN(x2.Value);

         Assert.AreEqual(x1.Value, x2.Value, 1e-10);
      }

      // checks whether both are null or else both have the same ID
      public static void AssertAreEqualId(IWithId x2, IWithId x1)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x2.Id, x1.Id);
      }

      // checks whether both are null or else both are equal
      public static void AreEqualEnumerableOfNamedObjects<T>(IEnumerable<T> x2, IEnumerable<T> x1, Func<T, string> nameSelector) where T : class
      {
         if (!AssertBothNotNull(x1, x2)) return;
         var l1 = x2.ToDictionary(nameSelector);
         var l2 = x1.ToDictionary(nameSelector);
         Assert.AreEqual(l1.Count(), l2.Count());
         foreach (var name in l1.Keys)
         {
            Assert.IsTrue(l2.ContainsKey(name));
            AreEqual(l1[name], l2[name]);
         }
      }

      private static void AreEqualEnumerableWithSameOrder<T>(IEnumerable<T> x2, IEnumerable<T> x1) where T : class
      {
         if (!AssertBothNotNull(x1, x2)) return;
         var l1 = x2.ToArray();
         var l2 = x1.ToArray();
         Assert.AreEqual(l1.Length, l2.Length);
         for (int i = 0; i < l1.Length; i++)
         {
            AreEqual(l1[i], l2[i]);
         }
      }
      public static void AreEqualObjectPath(IObjectPath x1, IObjectPath x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqual(x1.PathAsString, x2.PathAsString);
      }

      public static void AreEqualFormulaUsablePath(IFormulaUsablePath x1, IFormulaUsablePath x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectPath(x1, x2);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AssertAreEqual(x1.Alias, x2.Alias);
      }

      public static void AreEqualObjectReference(IObjectReference x1, IObjectReference x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqual(x1.Alias, x2.Alias);
         AssertAreEqualId(x1.Object, x2.Object);
      }

      public static void AreEqualObjectBase(IObjectBase x2, IObjectBase x1)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Id, x2.Id);
         AssertAreEqual(x2.Name, x1.Name);
      }

      public static void AreEqualPKParameter(PKParameter x1, PKParameter x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqual(x1.Name, x2.Name);
         AssertAreEqual(x1.Description, x2.Description);
         AssertAreEqual(x1.DisplayName, x2.DisplayName);
         AssertAreEqual(x1.Dimension, x2.Dimension);
      }

      public static void AreEqualAxis(Axis x1, Axis x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqual(x1.AxisType, x2.AxisType);
         AssertAreEqual(x1.UnitName, x2.UnitName);
         AssertAreEqual(x1.Caption, x2.Caption);
         AssertAreEqual(x1.DefaultColor, x2.DefaultColor);
         AssertAreEqual(x1.DefaultLineStyle, x2.DefaultLineStyle);
         AssertAreEqual(x1.GridLines, x2.GridLines);
         AssertAreEqual(x1.Min, x2.Min);
         AssertAreEqual(x1.Max, x2.Max);
         AssertAreEqual(x1.NumberMode, x2.NumberMode);
         AssertAreEqual(x1.Scaling, x2.Scaling);
         AssertAreEqual(x1.Visible, x2.Visible);
      }

      public static void AreEqualEntity(IEntity x1, IEntity x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AssertAreEqualId(x2.ParentContainer, x1.ParentContainer);

         Assert.AreEqual(x1.Tags.Count(), x2.Tags.Count());
         foreach (var tag in x2.Tags)
         {
            Assert.IsTrue(x1.Tags.Contains(tag));
         }
      }

      public static void AreEqualContainer(IContainer x1, IContainer x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEntity(x1, x2);
         Assert.AreEqual(x1.Mode, x2.Mode);
         AreEqualEnumerableOfNamedObjects(x1.Children, x2.Children, x => x.Name);
      }

      public static void AreEqualNeighborhood(INeighborhood x1, INeighborhood x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
         AssertAreEqualId(x1.FirstNeighbor, x2.FirstNeighbor);
         AssertAreEqualId(x1.SecondNeighbor, x2.SecondNeighbor);
      }

      public static void AreEqualModel(IModel x1, IModel x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1.Root, x2.Root);
         AreEqualContainer(x1.Neighborhoods, x2.Neighborhoods);
      }

      public static void AreEqualValuePoint(ValuePoint x1, ValuePoint x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqualDouble(x1.X, x2.X);
         AssertAreEqualDouble(x1.Y, x2.Y);
         Assert.AreEqual(x1.RestartSolver, x2.RestartSolver);
      }

      public static void AreEqualFormula(IFormula x1, IFormula x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualEnumerableOfNamedObjects(x1.ObjectPaths, x2.ObjectPaths, x => x.Alias);
      }

      public static void AreEqualTableFormulaWithArgument(TableFormulaWithXArgument x1, TableFormulaWithXArgument x2)
      {
         AreEqualFormula(x1, x2);
         Assert.AreEqual(x1.TableObjectAlias, x2.TableObjectAlias);
         Assert.AreEqual(x1.XArgumentAlias, x2.XArgumentAlias);
      }

      public static void AreEqualConstantFormula(ConstantFormula x1, ConstantFormula x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualFormula(x1, x2);
         AssertAreEqualDouble(x1.Value, x2.Value);
      }

      public static void AreEqualJacobianMatrix(JacobianMatrix x1, JacobianMatrix x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEnumerableWithSameOrder(x1.ParameterNames, x2.ParameterNames);
         Assert.AreEqual(x1.RowCount, x2.RowCount);
         Assert.AreEqual(x1.ColumnCount, x2.ColumnCount);
         for (int rowIndex = 0; rowIndex < x1.RowCount; rowIndex++)
         {
            AreEqualDoubleArray(x1.Rows[rowIndex].Values, x2.Rows[rowIndex].Values);
         }
      }


      public static void AreEqualPartialDerivatives(PartialDerivatives x1, PartialDerivatives x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.FullOutputPath, x2.FullOutputPath);
         AreEqualEnumerableWithSameOrder(x1.ParameterNames, x2.ParameterNames);
           Assert.AreEqual(x1.AllPartialDerivatives.Count, x2.AllPartialDerivatives.Count);

         for (int i = 0; i < x1.AllPartialDerivatives.Count; i++)
         {
            AreEqualDoubleArray(x1.AllPartialDerivatives[i], x2.AllPartialDerivatives[i]);
         }
      }

      public static void AreEqualTableFormula(TableFormula x1, TableFormula x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualFormula(x1, x2);
         AreEqualDimension(x1.XDimension, x2.XDimension);
         AreEqualUnit(x1.XDisplayUnit, x2.XDisplayUnit);
         AreEqualUnit(x1.YDisplayUnit, x2.YDisplayUnit);
         Assert.AreEqual(x1.XName, x2.XName);
         Assert.AreEqual(x1.YName, x2.YName);
         AreEqualEnumerableWithSameOrder(x1.AllPoints(), x2.AllPoints());
      }

      public static void AreEqualExplicitFormula(ExplicitFormula x1, ExplicitFormula x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualFormula(x1, x2);
         Assert.AreEqual(x1.FormulaString, x2.FormulaString);
      }

      public static void AreEqualProcess(IProcess x1, IProcess x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualReactionPartner(IReactionPartner x1, IReactionPartner x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.StoichiometricCoefficient, x2.StoichiometricCoefficient);
         AssertAreEqualId(x1.Partner, x2.Partner);
      }

      public static void AreEqualReaction(IReaction x1, IReaction x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualProcess(x1, x2);
         AreEqualEnumerableOfNamedObjects(x1.Educts, x2.Educts, x => x.Partner.Name);
         AreEqualEnumerableOfNamedObjects(x1.Products, x2.Products, x => x.Partner.Name);
         x2.ModifierNames.ShouldOnlyContain(x1.ModifierNames.ToArray());
      }

      public static void AreEqualTransport(ITransport x1, ITransport x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualProcess(x1, x2);
         AssertAreEqualId(x1.SourceAmount, x2.SourceAmount);
         AssertAreEqualId(x1.TargetAmount, x2.TargetAmount);
      }

      public static void AreEqualDimension(IDimension x1, IDimension x2)
      {
         if (x1 == null && x2 == null) return;
         if (x1 == null)
         {
            Assert.AreEqual(x2.Name, Constants.Dimension.DIMENSIONLESS);
            return;
         }
         if (x2 == null)
         {
            Assert.AreEqual(x1.Name, Constants.Dimension.DIMENSIONLESS);
            return;
         }

         if (x1.Name != x2.Name)
            Assert.AreEqual(x1.Name, x2.Name);

         Assert.IsTrue(x1.BaseRepresentation.Equals(x2.BaseRepresentation));
         AreEqualUnit(x1.BaseUnit, x2.BaseUnit);
         AreEqualUnit(x1.DefaultUnit, x2.DefaultUnit);

         Assert.AreEqual(x1.GetUnitNames().Count(), x2.GetUnitNames().Count());
         foreach (var name1 in x2.GetUnitNames())
         {
            Unit unit1 = x2.Unit(name1);
            Unit unit2 = x1.Unit(name1);
            AreEqualUnit(unit2, unit1);
         }
      }

      public static void AreEqualUnit(Unit x1, Unit x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Name, x2.Name);

         AssertAreEqualDouble(x1.Factor, x2.Factor);
         AssertAreEqualDouble(x1.Offset, x2.Offset);
      }

      public static void AreEqualDimensionFactory(IDimensionFactory x1, DimensionFactory x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Dimensions.Count(), x2.Dimensions.Count());
         foreach (var dimension1 in x2.Dimensions)
         {
            IDimension dimension2 = x1.Dimension(dimension1.Name);
            AreEqualDimension(dimension2, dimension1);
         }
      }

      public static void AreEqualFormulaUsable(IFormulaUsable x1, IFormulaUsable x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEntity(x1, x2);
         AssertAreEqualDouble(x1.Value, x2.Value);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualUnit(x1.DisplayUnit, x1.DisplayUnit);
      }

      public static void AreEqualQuantity(IQuantity x1, IQuantity x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;

         AreEqualEntity(x1, x2);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         Assert.AreEqual(x1.IsFixedValue, x2.IsFixedValue);
         if (x1.IsFixedValue) 
            AssertAreEqualDouble(x1.Value, x2.Value);

         Assert.AreEqual(x1.Persistable, x2.Persistable);
         Assert.AreEqual(x1.QuantityType, x2.QuantityType);
         Assert.AreEqual(x1.NegativeValuesAllowed, x2.NegativeValuesAllowed);
         AreEqualValueOrigin(x1.ValueOrigin, x2.ValueOrigin);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualValueOrigin(ValueOrigin x1, ValueOrigin x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqual(x1.Id, x2.Id);
         AssertAreEqual(x1.Source, x2.Source);
         AssertAreEqual(x1.Method, x2.Method);
         AssertAreEqual(x1.Description, x2.Description);
      }

      public static void AreEqualObserver(IObserver x1, IObserver x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualQuantity(x1, x2);
      }

      public static void AreEqualParameter(IParameter x1, IParameter x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualQuantity(x1, x2);
         AreEqualFormula(x1.RHSFormula, x2.RHSFormula);
         Assert.AreEqual(x1.CanBeVaried, x2.CanBeVaried);
         Assert.AreEqual(x1.BuildingBlockType, x2.BuildingBlockType);
         Assert.AreEqual(x1.GroupName, x2.GroupName);
         Assert.AreEqual(x1.IsDefault, x2.IsDefault);
      }

      public static void AreEqualQuantityAndContainer(IQuantityAndContainer x1, IQuantityAndContainer x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualQuantity(x1, x2);
         AreEqualContainer(x1, x2);
      }

      public static void AreEqualMoleculeAmount(IMoleculeAmount x1, IMoleculeAmount x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualQuantityAndContainer(x1, x2);
         AssertAreEqualDouble(x1.ScaleDivisor, x2.ScaleDivisor);
         AreEqualUnit(x1.DisplayUnit, x2.DisplayUnit);
      }

      public static void AreEqualDistributedParameter(IDistributedParameter x1, IDistributedParameter x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualQuantityAndContainer(x1, x2);
         AssertAreEqualDouble(x1.Percentile, x2.Percentile);
         AreEqualFormula(x1.RHSFormula, x2.RHSFormula);
         Assert.AreEqual(x1.IsDefault, x2.IsDefault);
      }

      public static void AreEqualEventAssignment(IEventAssignment x1, IEventAssignment x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEntity(x1, x2);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualFormula(x1.Formula, x2.Formula);
         Assert.AreEqual(x1.UseAsValue, x2.UseAsValue);
         
         AssertAreEqualId(x1.ChangedEntity, x2.ChangedEntity);
      }

      public static void AreEqualEvent(IEvent x1, IEvent x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualFormula(x1.Formula, x2.Formula);
         Assert.AreEqual(x1.OneTime, x2.OneTime);
      }

      public static void AreEqualEventGroup(IEventGroup x1, IEventGroup x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
      }

      public static void AreEqualDescriptorCriteria(DescriptorCriteria x1, DescriptorCriteria x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Count, x2.Count);
         foreach (var dc1 in x2)
         {
            Assert.IsTrue(x1.Contains(dc1));
         }
         foreach (var dc2 in x1)
         {
            Assert.IsTrue(x2.Contains(dc2));
         }
      }

      public static void AreEqualParameterDescriptor(ParameterDescriptor x1, ParameterDescriptor x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;

         Assert.AreEqual(x1.ParameterName, x2.ParameterName);
         AreEqualDescriptorCriteria(x1.ContainerCriteria, x2.ContainerCriteria);
      }

      public static void AreEqualMatchTagCondition(MatchTagCondition x1, MatchTagCondition x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.Tag, x2.Tag);
         AreEqualStrings(x1.Condition, x2.Condition);
      }

      public static void AreEqualNotMatchTagCondition(NotMatchTagCondition x1, NotMatchTagCondition x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.Tag, x2.Tag);
         AreEqualStrings(x1.Condition, x2.Condition);
      }

      public static void AreEqualInContainerCondition(InContainerCondition x1, InContainerCondition x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.Tag, x2.Tag);
         AreEqualStrings(x1.Condition, x2.Condition);
      }

      public static void AreEqualNotInContainerCondition(NotInContainerCondition x1, NotInContainerCondition x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.Tag, x2.Tag);
         AreEqualStrings(x1.Condition, x2.Condition);
      }

      public static void AreEqualFormulaCache(IFormulaCache x1, IFormulaCache x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEnumerableOfNamedObjects(x1, x2, x => x.Id);
      }

      public static void AreEqualObserverBuilder(IObserverBuilder x1, IObserverBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualMoleculeList(x1.MoleculeList, x2.MoleculeList);
         AreEqualFormula(x1.Formula, x2.Formula);
         AreEqualDimension(x1.Dimension, x2.Dimension);
      }

      public static void AreEqualMoleculeList(MoleculeList x1, MoleculeList x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.MoleculeNames.Count(), x2.MoleculeNames.Count());
         foreach (var name in x2.MoleculeNames)
         {
            Assert.IsTrue(x1.MoleculeNames.Contains(name));
         }
         Assert.AreEqual(x1.MoleculeNamesToExclude.Count(), x2.MoleculeNamesToExclude.Count());
         foreach (var name in x2.MoleculeNamesToExclude)
         {
            Assert.IsTrue(x1.MoleculeNamesToExclude.Contains(name));
         }
         Assert.AreEqual(x1.ForAll, x2.ForAll);
      }

      public static void AreEqualComparerSettings(ComparerSettings x1, ComparerSettings x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.FormulaComparison, x2.FormulaComparison);
         Assert.AreEqual(x1.OnlyComputingRelevant, x2.OnlyComputingRelevant);
         Assert.AreEqual(x1.RelativeTolerance, x2.RelativeTolerance);
         Assert.AreEqual(x1.CompareHiddenEntities, x2.CompareHiddenEntities);
         Assert.AreEqual(x1.ShowValueOrigin, x2.ShowValueOrigin);
      }

      public static void AreEqualCalculationMethod(ICoreCalculationMethod x1, ICoreCalculationMethod x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualBuildingBlock(x1, x2);
         Assert.AreEqual(x1.Category, x2.Category);
         Assert.AreEqual(x1.AllOutputFormulas().Count(), x2.AllOutputFormulas().Count());
         Assert.AreEqual(x1.AllHelpParameters().Count(), x2.AllHelpParameters().Count());

         for (int i = 0; i < x1.AllOutputFormulas().Count(); i++)
         {
            var x1Formula = x1.AllOutputFormulas().ElementAt(i);
            var x2Formula = x2.AllOutputFormulas().ElementAt(i);
            AreEqualFormula(x1Formula, x2Formula);
            AreEqualParameterDescriptor(x1.DescriptorFor(x1Formula), x2.DescriptorFor(x2Formula));
         }


         for (int i = 0; i < x1.AllHelpParameters().Count(); i++)
         {
            var x1Parameter = x1.AllHelpParameters().ElementAt(i);
            var x2Parameter = x2.AllHelpParameters().ElementAt(i);
            AreEqualParameter(x1Parameter, x2Parameter);
            AreEqualDescriptorCriteria(x1.DescriptorFor(x1Parameter), x2.DescriptorFor(x2Parameter));
         }
      }

      public static void AreEqualAmountObserverBuilder(IAmountObserverBuilder x1, IAmountObserverBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObserverBuilder(x1, x2);
         AreEqualDescriptorCriteria(x1.ContainerCriteria, x2.ContainerCriteria);
      }

      public static void AreEqualParameterBuilder(IParameter x1, IParameter x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         Assert.AreEqual(x1.BuildMode, x2.BuildMode);
         Assert.AreEqual(x1.Visible, x2.Visible);
         Assert.AreEqual(x1.CanBeVaried, x2.CanBeVaried);
         Assert.AreEqual(x1.GroupName, x2.GroupName);
         AreEqualParameter(x1, x2);
      }

      public static void AreEqualMoleculeBuilder(IMoleculeBuilder x1, IMoleculeBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         Assert.AreEqual(x1.IsFloating, x2.IsFloating);
         Assert.AreEqual(x1.QuantityType, x2.QuantityType);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualUnit(x1.DisplayUnit, x2.DisplayUnit);
         AreEqualFormula(x1.DefaultStartFormula, x2.DefaultStartFormula);
         AreEqualEnumerableOfNamedObjects(x1.Parameters, x2.Parameters, x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.TransporterMoleculeContainerCollection, x2.TransporterMoleculeContainerCollection, x => x.Name);
         AreEqualEnumerableOfNamedObjects(x1.InteractionContainerCollection, x2.InteractionContainerCollection, x => x.Name);
      }

      public static void AreEqualInteractionContainer(InteractionContainer x1, InteractionContainer x2)
      {
         AreEqualContainer(x1, x2);
      }

      public static void AreEqualTransporterMoleculeContainer(TransporterMoleculeContainer x1, TransporterMoleculeContainer x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualStrings(x1.TransportName, x2.TransportName);
         AreEqualEnumerableOfNamedObjects(x1.Parameters, x2.Parameters, x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.ActiveTransportRealizations, x2.ActiveTransportRealizations, x => x.Name);
      }

      public static void AreEqualProcessBuilder(IProcessBuilder x1, IProcessBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         Assert.AreEqual(x1.CreateProcessRateParameter, x2.CreateProcessRateParameter);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualTransportBuilder(ITransportBuilder x1, ITransportBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualProcessBuilder(x1, x2);
         Assert.AreEqual(x1.TransportType, x2.TransportType);
         Assert.AreEqual(x1.CreateProcessRateParameter, x2.CreateProcessRateParameter);
         Assert.AreEqual(x1.ProcessRateParameterPersistable, x2.ProcessRateParameterPersistable);
         AreEqualDescriptorCriteria(x1.SourceCriteria, x2.SourceCriteria);
         AreEqualDescriptorCriteria(x1.TargetCriteria, x2.TargetCriteria);
         AreEqualEnumerableOfNamedObjects(x1.Parameters, x2.Parameters, x => x.Id);
         AreEqualMoleculeList(x1.MoleculeList, x2.MoleculeList);
      }

      public static void AreEqualReactionPartnerBuilder(IReactionPartnerBuilder x1, IReactionPartnerBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.MoleculeName, x2.MoleculeName);
         Assert.AreEqual(x1.StoichiometricCoefficient, x2.StoichiometricCoefficient);
      }

      public static void AreEqualReactionBuilder(IReactionBuilder x1, IReactionBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualProcessBuilder(x1, x2);
         AreEqualEnumerableOfNamedObjects(x1.Educts, x2.Educts, x => x.MoleculeName);
         AreEqualEnumerableOfNamedObjects(x1.Products, x2.Products, x => x.MoleculeName);
         AreEqualEnumerableOfNamedObjects(x1.Parameters, x2.Parameters, x => x.Id);
         AreEqualDescriptorCriteria(x1.ContainerCriteria, x2.ContainerCriteria);
         x2.ModifierNames.ShouldOnlyContain(x1.ModifierNames.ToArray());
      }

      public static void AreEqualNeighborhoodBuilder(INeighborhoodBuilder x1, INeighborhoodBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
         AreEqualContainer(x1.FirstNeighbor, x2.FirstNeighbor);
         AreEqualContainer(x1.SecondNeighbor, x2.SecondNeighbor);
         AreEqualContainer(x1.MoleculeProperties, x2.MoleculeProperties);
      }

      public static void AreEqualEventAssignmentBuilder(IEventAssignmentBuilder x1, IEventAssignmentBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         Assert.AreEqual(x1.UseAsValue, x2.UseAsValue);
         AreEqualObjectPath(x1.ObjectPath, x2.ObjectPath);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualEventBuilder(IEventBuilder x1, IEventBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         Assert.AreEqual(x1.OneTime, x2.OneTime);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AreEqualFormula(x1.Formula, x2.Formula);
         AreEqualEnumerableOfNamedObjects(x1.Parameters, x2.Parameters, x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.Assignments, x2.Assignments, x => x.Id);
      }

      public static void AreEqualEventGroupBuilder(IEventGroupBuilder x1, IEventGroupBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualContainer(x1, x2);
         AreEqualStrings(x1.EventGroupType, x2.EventGroupType);
         AreEqualDescriptorCriteria(x1.SourceCriteria, x2.SourceCriteria);
         AreEqualEnumerableOfNamedObjects(x1.GetChildren<IParameter>(), x2.GetChildren<IParameter>(), x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.Events, x2.Events, x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.GetChildren<IApplicationBuilder>(), x2.GetChildren<IApplicationBuilder>(), x => x.Id);
      }

      public static void AreEqualApplicationBuilder(IApplicationBuilder x1, IApplicationBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEventGroupBuilder(x1, x2);
         AreEqualEnumerableOfNamedObjects(x1.Molecules, x2.Molecules, x => x.Name);
         AreEqualEnumerableOfNamedObjects(x1.Transports, x2.Transports, x => x.Name);
      }

      public static void AreEqualMoleculeStartValue(IMoleculeStartValue x1, IMoleculeStartValue x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.IsPresent, x2.IsPresent);
         AreEqualObjectPath(x1.ContainerPath, x2.ContainerPath);
         AreEqualStrings(x1.MoleculeName, x2.MoleculeName);
         AssertAreEqualNullableDouble(x1.StartValue, x2.StartValue);
         Assert.AreEqual(x1.ScaleDivisor, x2.ScaleDivisor, 1e-10);
         AssertAreEqual(x1.ValueOrigin, x2.ValueOrigin);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualParameterStartValue(IParameterStartValue x1, IParameterStartValue x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectPath(x1.Path, x2.Path);
         AssertAreEqualNullableDouble(x1.StartValue, x2.StartValue);
         AreEqualUnit(x1.DisplayUnit, x2.DisplayUnit);
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AssertAreEqual(x1.ValueOrigin, x2.ValueOrigin);
         Assert.AreEqual(x1.IsDefault, x2.IsDefault);
         AreEqualFormula(x1.Formula, x2.Formula);
      }

      public static void AreEqualBuildingBlock(IBuildingBlock x1, IBuildingBlock x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualCreationMetaData(x1.Creation, x2.Creation);
         AreEqualEnumerableOfNamedObjects(x1.FormulaCache, x2.FormulaCache, x => x.Id);
      }

      public static void AreEqualCreationMetaData(CreationMetaData x1, CreationMetaData x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.ClonedFrom, x2.ClonedFrom);
         AreEqualStrings(x1.CreatedBy, x2.CreatedBy);
         AreEqualStrings(x1.Version, x2.Version);
         Assert.AreEqual(x1.CreatedBy, x2.CreatedBy);
         Assert.AreEqual(x1.Origin, x2.Origin);
         Assert.AreEqual(x1.CreationMode, x2.CreationMode);

      }

      public static void AreEqualBuildingBlock<TBuilder>(IBuildingBlock<TBuilder> x1, IBuildingBlock<TBuilder> x2) where TBuilder : class, IObjectBase
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualBuildingBlock((IBuildingBlock) x1, x2);
         AreEqualEnumerableOfNamedObjects(x1, x2, x => x.Id);
      }

      public static void AreEqualSpatialStructure(ISpatialStructure x1, ISpatialStructure x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualBuildingBlock((IBuildingBlock) x1, x2);
         AreEqualContainer(x1.GlobalMoleculeDependentProperties, x2.GlobalMoleculeDependentProperties);
         AreEqualEnumerableOfNamedObjects(x1.TopContainers, x2.TopContainers, x => x.Id);
         AreEqualEnumerableOfNamedObjects(x1.Neighborhoods, x2.Neighborhoods, x => x.Id);
      }

      public static void AreEqualMoleculeStartValuesBuildingBlock(IMoleculeStartValuesBuildingBlock x1, IMoleculeStartValuesBuildingBlock x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualEnumerableOfNamedObjects(x1, x2, x => x.Path.ToString()); //Justified, because MoleculePath must be unique to use index operator
      }

      public static void AreEqualParameterStartValuesBuildingBlock(IParameterStartValuesBuildingBlock x1, IParameterStartValuesBuildingBlock x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualEnumerableOfNamedObjects(x1, x2, x => x.Path.ToString());
      }

      public static void AreEqualBuildConfiguration(IBuildConfiguration x1, IBuildConfiguration x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualBuildingBlock(x1.Molecules, x2.Molecules);
         AreEqualBuildingBlock(x1.Observers, x2.Observers);
         AreEqualBuildingBlock(x1.EventGroups, x2.EventGroups);
         AreEqualBuildingBlock(x1.PassiveTransports, x2.PassiveTransports);
         AreEqualBuildingBlock(x1.Reactions, x2.Reactions);
         AreEqualSpatialStructure(x1.SpatialStructure, x2.SpatialStructure);
         AreEqualParameterStartValuesBuildingBlock(x1.ParameterStartValues, x2.ParameterStartValues);
         AreEqualMoleculeStartValuesBuildingBlock(x1.MoleculeStartValues, x2.MoleculeStartValues);
         AreEqualCalculationMethodLists(x1.AllCalculationMethods(), x2.AllCalculationMethods());
         AreEqualSimulationSettings(x1.SimulationSettings, x2.SimulationSettings);
      }

      public static void AreEqualCalculationMethodLists(IEnumerable<ICoreCalculationMethod> x1, IEnumerable<ICoreCalculationMethod> x2)
      {
         Assert.AreEqual(x1.Count(), x2.Count());
         for (int i = 0; i < x1.Count(); i++)
         {
            AreEqualCalculationMethod(x1.ElementAt(i), x2.ElementAt(i));
         }
      }

      public static void AreEqualSimulationSettings(ISimulationSettings x1, ISimulationSettings x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.RandomSeed, x2.RandomSeed);
         AreEqualSolverSettings(x1.Solver, x2.Solver);
         AreEqualOutputSchema(x1.OutputSchema, x2.OutputSchema);
         AreEqualQuantitySelections(x1.OutputSelections, x2.OutputSelections);
      }

  

      private static void AreEqualSolverSettings(SolverSettings x1, SolverSettings x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.AbsTol, x2.AbsTol);
         Assert.AreEqual(x1.H0, x2.H0);
         Assert.AreEqual(x1.HMax, x2.HMax);
         Assert.AreEqual(x1.HMin, x2.HMin);
         Assert.AreEqual(x1.MxStep, x2.MxStep);
         Assert.AreEqual(x1.RelTol, x2.RelTol);
         AreEqualStrings(x1.Name, x2.Name);
         Assert.AreEqual(x1.UseJacobian, x2.UseJacobian);
      }

      private static void AreEqualOutputInterval(OutputInterval x1, OutputInterval x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Resolution, x2.Resolution);
         Assert.AreEqual(x1.StartTime, x2.StartTime);
         Assert.AreEqual(x1.EndTime, x2.EndTime);
      }

      private static void AreEqualOutputSchema(OutputSchema x1, OutputSchema x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualEnumerableWithSameOrder(x1.Intervals, x2.Intervals);
      }

      private static void AreEqualQuantitySelections(OutputSelections x1, OutputSelections x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.AllOutputs.Count(), x2.AllOutputs.Count());
         foreach (var x1Quantity in x1.AllOutputs)
         {
            var x2Quantity = x2.AllOutputs.First(x => x.Path == x1Quantity.Path);
            x2Quantity.QuantityType.ShouldBeEqualTo(x1Quantity.QuantityType);
         }
      }

      public static void AreEqualSimulationTransfer(SimulationTransfer x1, SimulationTransfer x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualSimulation(x1.Simulation, x2.Simulation);
         AreEqualEnumerableWithSameOrder(x1.AllObservedData, x2.AllObservedData);
         AreEqualEnumerableWithSameOrder(x1.Favorites, x2.Favorites);
         AssertAreEqual(x1.PkmlVersion, x2.PkmlVersion);
      }

      public static void AreEqualSimulation(IModelCoreSimulation x1, IModelCoreSimulation x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualBuildConfiguration(x1.BuildConfiguration, x2.BuildConfiguration);
         AreEqualModel(x1.Model, x2.Model);
         AreEqualMcDataRepository(x1.Results, x2.Results);
      }

      public static void AreEqualFloatArray(IReadOnlyList<float> x1, IReadOnlyList<float> x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Count, x2.Count);
         for (int i = 0; i < x1.Count; i++)
         {
            AssertAreEqualDouble(x1[i], x2[i]);
         }
      }

      public static void AreEqualDoubleArray(IReadOnlyList<double> x1, IReadOnlyList<double> x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Count, x2.Count);
         for (int i = 0; i < x1.Count; i++)
         {
            AssertAreEqualDouble(x1[i], x2[i]);
         }
      }

      public static void AreEqualMcDataInfo(DataInfo x1, DataInfo x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         Assert.AreEqual(x1.Origin, x2.Origin);
         Assert.AreEqual(x1.AuxiliaryType, x2.AuxiliaryType);
         AreEqualStrings(x1.DisplayUnitName, x2.DisplayUnitName);
         Assert.AreEqual(x1.Date, x2.Date);
         AreEqualStrings(x1.Source, x2.Source);
         AreEqualStrings(x1.Category, x2.Category);

         if (x1.MolWeight.HasValue && x2.MolWeight.HasValue)
            AssertAreEqualDouble(x1.MolWeight.Value, x2.MolWeight.Value);
         else
         {
            Assert.IsNull(x1.MolWeight);
            Assert.IsNull(x2.MolWeight);
         }
      }

      public static void AreEqualMcQuantityInfo(QuantityInfo x1, QuantityInfo x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualStrings(x1.Name, x2.Name);
         Assert.AreEqual(x1.Type, x2.Type);
         Assert.AreEqual(x1.Path.Count(), x2.Path.Count());
         for (int i = 0; i < x1.Path.Count(); i++)
         {
            AreEqualStrings(x1.Path.ElementAt(i), x2.Path.ElementAt(i));
         }
      }

      public static void AreEqualMcDataColumn(DataColumn x1, DataColumn x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqualId(x1, x2);
         AreEqualStrings(x1.Name, x2.Name);
         Assert.AreEqual(x2.IsInternal, x1.IsInternal); 
         AreEqualDimension(x1.Dimension, x2.Dimension);
         AssertAreEqualId(x1.Repository, x2.Repository);
         AssertAreEqualId(x1.BaseGrid, x2.BaseGrid);
         AreEqualFloatArray(x1.Values, x2.Values);
         AreEqualMcQuantityInfo(x1.QuantityInfo, x2.QuantityInfo);
         AreEqualMcDataInfo(x1.DataInfo, x2.DataInfo);
         x1.DataInfo.LLOQ.ShouldBeEqualTo(x2.DataInfo.LLOQ);
         x1.DataInfo.ComparisonThreshold.ShouldBeEqualTo(x2.DataInfo.ComparisonThreshold);

         Assert.AreEqual(x1.RelatedColumns.Count(), x2.RelatedColumns.Count());
         foreach (var relCol1 in x1.RelatedColumns)
         {
            Assert.IsTrue(x2.ContainsRelatedColumn(relCol1.DataInfo.AuxiliaryType));
            AreEqual(relCol1, x2.GetRelatedColumn(relCol1.DataInfo.AuxiliaryType));
         }
       
      }

      public static void AreEqualMcDataRepository(DataRepository x1, DataRepository x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AssertAreEqualId(x1, x2);
         AreEqualStrings(x1.Name, x2.Name);
         AreEqualStrings(x1.Description, x2.Description);
         AreEqualEnumerableOfNamedObjects(x1, x2, x => x.Id);
      }

      public static void AreEqualApplicationMoleculeBuilder(IApplicationMoleculeBuilder x1, IApplicationMoleculeBuilder x2)
      {
         if (!AssertBothNotNull(x1, x2)) return;
         AreEqualObjectBase(x1, x2);
         AreEqualObjectPath(x1.RelativeContainerPath, x2.RelativeContainerPath);
         AreEqualFormula(x1.Formula, x2.Formula);
      }
   }
}