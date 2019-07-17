using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class QuantityAndContainerXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyQuantityAndContainer()
      {
         var x1 = CreateObject<MoleculeAmount>().WithName("Monica");
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantityAndContainer(x2, x1);
      }

      [Test]
      public void TestSerializationQuantityAndContainerWithFixedValue()
      {
         MoleculeAmount x1 = CreateObject<MoleculeAmount>().WithName("Monica").WithDimension(DimensionLength);
         x1.Persistable = true;
         x1.IsFixedValue = true;
         x1.Value = 2.3;
         x1.Mode = ContainerMode.Physical;
         Parameter p1 = CreateObject<Parameter>().WithName("Quentin").WithDimension(DimensionLength);
         x1.Add(p1);

         IQuantityAndContainer x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantityAndContainer(x2, x1);
      }

      [Test]
      public void TestSerializationQuantityAndContainerWithConstantFormula()
      {
         MoleculeAmount x1 = CreateObject<MoleculeAmount>().WithName("Monica").WithDimension(DimensionLength);
         x1.IsFixedValue = false;
         x1.Persistable = false;
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         x1.Mode = ContainerMode.Logical;
         Parameter p1 = CreateObject<Parameter>().WithName("Quentin").WithDimension(DimensionLength);
         x1.Add(p1);

         IQuantityAndContainer x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantityAndContainer(x2, x1);
      }
   }

   public class MoleculeAmountXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationMoleculeAmountWithFixedValue()
      {
         MoleculeAmount x1 = CreateObject<MoleculeAmount>().WithName("Monica").WithDimension(DimensionLength);
         x1.Persistable = true;
         x1.Value = 2.3;
         x1.ScaleDivisor = 0.345;

         IMoleculeAmount x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualMoleculeAmount(x2, x1);
      }
   }

   public class DistributedParameterXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      private DistributedParameter _dp;

      protected override void Context()
      {
         base.Context();
         _dp = CreateObject<DistributedParameter>().WithName("Diana").WithDimension(DimensionLength);
         ConstantFormula dpMeanFormula = CreateObject<ConstantFormula>().WithValue(3.0);
         IParameter dpMean = CreateObject<Parameter>().WithName("Mean").WithFormula(dpMeanFormula);
         ConstantFormula dpDeviationFormula = CreateObject<ConstantFormula>().WithValue(1.0);
         IParameter dpDeviation = CreateObject<Parameter>().WithName("Deviation").WithFormula(dpDeviationFormula);
         ConstantFormula dpPercentileFormula = CreateObject<ConstantFormula>().WithValue(0.5);
         IParameter dpPercentile = CreateObject<Parameter>().WithName("Percentile").WithFormula(dpPercentileFormula);

         _dp.Add(dpMean);
         _dp.Add(dpDeviation);
         _dp.Add(dpPercentile);
         IDistributionFormula noDiFo = CreateObject<NormalDistributionFormula>().WithDimension(DimensionLength);
         noDiFo.AddObjectPath(new FormulaUsablePath(new[] {Constants.Distribution.MEAN}) {Alias = Constants.Distribution.MEAN, Dimension = DimensionLength});
         noDiFo.AddObjectPath(new FormulaUsablePath(new[] {Constants.Distribution.DEVIATION}) {Alias = Constants.Distribution.DEVIATION, Dimension = DimensionLength});
         noDiFo.AddObjectPath(new FormulaUsablePath(new[] {Constants.Distribution.PERCENTILE}) {Alias = Constants.Distribution.PERCENTILE, Dimension = DimensionLength});
         _dp.Formula = noDiFo;
         _dp.Formula.ResolveObjectPathsFor(_dp);

         _dp.RHSFormula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
      }

      [Test]
      public void TestSerializationDistributedParameterWithPercentile()
      {
         _dp.Percentile = 0.8;
         _dp.IsDefault = true;

         var dp = SerializeAndDeserialize(_dp);
         dp.Formula.ResolveObjectPathsFor(dp);

         AssertForSpecs.AreEqualDistributedParameter(dp, _dp);
      }

      [Test]
      public void TestSerializationDistributedParameterWithValue()
      {
         _dp.Value = 4.0;

         var dp = SerializeAndDeserialize(_dp);
         dp.Formula.ResolveObjectPathsFor(dp);

         AssertForSpecs.AreEqualDistributedParameter(dp, _dp);
      }
   }
}