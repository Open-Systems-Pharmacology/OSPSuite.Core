using System.Linq;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml.Extensions;

namespace OSPSuite.Core.Converter.v5_5
{
   public class Converter541To551 : IObjectConverter,
      IVisitor<SimulationTransfer>,
      IVisitor<IModelCoreSimulation>,
      IVisitor<IMoleculeStartValuesBuildingBlock>,
      IVisitor<IParameterStartValuesBuildingBlock>,
      IVisitor<IMoleculeBuildingBlock>,
      IVisitor<IBuildConfiguration>
   {
      private readonly IDimension _amountDimension;

      public Converter541To551(IDimensionFactory dimensionFactory)
      {
         _amountDimension = dimensionFactory.GetDimension(Constants.Dimension.AMOUNT);
      }

      public bool IsSatisfiedBy(int version)
      {
         return PKMLVersion.V5_4_1.Equals(version);
      }

      public int Convert(object objectToUpdate)
      {
         this.Visit(objectToUpdate);
         return PKMLVersion.V5_5_1;
      }

      public int ConvertXml(XElement element)
      {
         element.DescendantsAndSelfNamed("ParameterStartValues", "ParameterStartValuesBuildingBlock").Each(ConvertParameterStartValuesElement);
         element.DescendantsAndSelfNamed("MoleculeStartValues", "MoleculeStartValuesBuildingBlock").Each(ConvertMoleculeStartValueElement);

         //"ModelCoreSimulation" required for older project
         element.DescendantsAndSelfNamed("Simulation", "ModelCoreSimulation").Each(ConvertSimuationElement);

         return PKMLVersion.V5_5_1;
      }

      public void ConvertSimuationElement(XElement element)
      {
         var simulationConfiguration = element.Element("SimulationConfiguration");
         var buildConfiguration = element.Element("BuildConfiguration");
         if (simulationConfiguration == null || buildConfiguration == null) return;

         simulationConfiguration.Remove();
         simulationConfiguration.Name = "SimulationSettings";
         simulationConfiguration.AddAttribute(Constants.Serialization.Attribute.NAME, element.GetAttribute(Constants.Serialization.Attribute.NAME));
         //node is always created for all building block and is required for deserialization
         var formulaCache = new XElement("FormulaCache", new XElement(Constants.Serialization.STRING_MAP_LIST));
         simulationConfiguration.Add(formulaCache);
         buildConfiguration.Add(simulationConfiguration);
      }

      public void ConvertMoleculeStartValueElement(XElement element)
      {
         //make sure the name is set to the molecule name
         foreach (var moleculeStartValue in element.Descendants("MoleculeStartValue"))
         {
            var moleculeName = moleculeStartValue.GetAttribute("moleculeName");
            moleculeStartValue.AddAttribute(Constants.Serialization.Attribute.NAME, moleculeName);
         }
      }

      public void ConvertParameterStartValuesElement(XElement element)
      {
         //rename parmaeter path to container path. Real conversion will be done in visitor
         foreach (var parameterPath in element.Descendants("ParameterPath"))
         {
            parameterPath.Name = "ContainerPath";
         }
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         Visit(simulation.BuildConfiguration);
         UpdateQuantityTypeForObservers(simulation);
         UpdateQuantityTypeForDataRepository(simulation.Results);
      }

      public void Visit(IBuildConfiguration buildConfiguration)
      {
         Visit(buildConfiguration.ParameterStartValues);
         Visit(buildConfiguration.MoleculeStartValues);
         Visit(buildConfiguration.Molecules);
      }
      public void UpdateQuantityTypeForDataRepository(DataRepository dataRepository)
      {
         if (dataRepository == null)
            return;

         foreach (var column in dataRepository.AllButBaseGrid().Where(x=>x.QuantityInfo.Type==QuantityType.Observer))
         {
            column.QuantityInfo.Type |= QuantityType.Drug;
         }
      }

      public void UpdateQuantityTypeForObservers(IModelCoreSimulation simulation)
      {
         foreach (var observer in simulation.Model.Root.GetAllChildren<IObserver>())
         {
            //only changes the one defined with the type observer. Otherwise, this was set already 
            if (observer.QuantityType == QuantityType.Observer)
               observer.QuantityType |= QuantityType.Drug;
         }
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
      }

      public void Visit(IMoleculeStartValuesBuildingBlock moleculeStartValuesBuilding)
      {
         foreach (var moleculeStartValue in moleculeStartValuesBuilding)
         {
            moleculeStartValue.ScaleDivisor = Constants.DEFAULT_SCALE_DIVISOR;
            updateToAmountDimension(moleculeStartValue);
         }
      }

      public void Visit(IParameterStartValuesBuildingBlock parameterStartValuesBuilding)
      {
         foreach (var parameterStartValue in parameterStartValuesBuilding.OfType<ParameterStartValue>().ToList())
         {
            //psv stored as a cache using path as key
            parameterStartValuesBuilding.Remove(parameterStartValue);
            parameterStartValue.Path = parameterStartValue.ContainerPath;
            parameterStartValuesBuilding.Add(parameterStartValue);
         }
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         moleculeBuildingBlock.Each(updateToAmountDimension);
      }

      private void updateToAmountDimension(IWithDisplayUnit molecule)
      {
         molecule.Dimension = _amountDimension;
         molecule.DisplayUnit = _amountDimension.DefaultUnit;
      }

  
   }
}