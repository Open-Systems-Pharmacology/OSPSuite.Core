using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using IDialogCreator = OSPSuite.Core.Services.IDialogCreator;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationSimulationSelectionPresenter : IPresenter<IParameterIdentificationSimulationSelectionView>,
      IParameterIdentificationPresenter,
      IPresenterWithContextMenu<ITreeNode>,
      IPresenterWithContextMenu<IReadOnlyList<ITreeNode>>
   {
      void AddSimulation(ISimulation simulation);
      void AddSimulations(IEnumerable<ISimulation> simulations);
      void SelectSimulationsToAdd();
      event EventHandler<SimulationEventArgs> SimulationAdded;
      event EventHandler<SimulationEventArgs> SimulationRemoved;
      void Refresh();
      void RemoveSimulation(ISimulation simulation);
      void ReplaceSimulation(ISimulation simulation);
      bool CanUseAllSimulations(IReadOnlyList<ISimulation> simulations);
   }

   public class ParameterIdentificationSimulationSelectionPresenter : AbstractPresenter<IParameterIdentificationSimulationSelectionView, IParameterIdentificationSimulationSelectionPresenter>, IParameterIdentificationSimulationSelectionPresenter
   {
      private readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IApplicationController _applicationController;
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly ITreeNodeContextMenuFactory _treeNodeContextMenuFactory;
      private readonly IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private ParameterIdentification _parameterIdentification;
      public event EventHandler<SimulationEventArgs> SimulationAdded = delegate { };
      public event EventHandler<SimulationEventArgs> SimulationRemoved = delegate { };

      public ParameterIdentificationSimulationSelectionPresenter(IParameterIdentificationSimulationSelectionView view, ITreeNodeFactory treeNodeFactory,
         IApplicationController applicationController, ILazyLoadTask lazyLoadTask, ITreeNodeContextMenuFactory treeNodeContextMenuFactory,
         IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IDialogCreator dialogCreator, IParameterIdentificationTask parameterIdentificationTask) : base(view)
      {
         _treeNodeFactory = treeNodeFactory;
         _applicationController = applicationController;
         _lazyLoadTask = lazyLoadTask;
         _treeNodeContextMenuFactory = treeNodeContextMenuFactory;
         _multipleTreeNodeContextMenuFactory = multipleTreeNodeContextMenuFactory;
         _dialogCreator = dialogCreator;
         _parameterIdentificationTask = parameterIdentificationTask;
      }

      public void AddSimulation(ISimulation simulation)
      {
         if (_parameterIdentification.UsesSimulation(simulation))
            return;

         _parameterIdentificationTask.AddSimulationTo(_parameterIdentification, simulation);
         addSimulationToView(simulation);
         SimulationAdded(this, new SimulationEventArgs(simulation));
      }

      public void AddSimulations(IEnumerable<ISimulation> simulations)
      {
         simulations.Each(AddSimulation);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _view.DestroyNodes();
         _parameterIdentification = parameterIdentification;
         _parameterIdentification.AllSimulations.Each(addSimulationToView);
      }

      public void ReplaceSimulation(ISimulation simulation)
      {
         doWithSimulationSelectionPresenter(simulations => swapSimulations(simulation, simulations.FirstOrDefault()), allowMultipleSelections: false);
      }

      public bool CanUseAllSimulations(IReadOnlyList<ISimulation> simulations)
      {
         return simulations.All(_parameterIdentificationTask.SimulationCanBeUsedForIdentification);
      }

      private void swapSimulations(ISimulation oldSimulation, ISimulation newSimulation)
      {
         if (newSimulation == null)
            return;

         _parameterIdentificationTask.SwapSimulations(_parameterIdentification, oldSimulation, newSimulation);
      }

      public void SelectSimulationsToAdd()
      {
         doWithSimulationSelectionPresenter(AddSimulations, allowMultipleSelections: true);
      }

      private void doWithSimulationSelectionPresenter(Action<IEnumerable<ISimulation>> action, bool allowMultipleSelections)
      {
         using (var simulationSelectionPresenter = _applicationController.Start<ISelectionSimulationPresenter>())
         {
            action(simulationSelectionPresenter.StartSelection(_parameterIdentification.AllSimulations, allowMultipleSelections));
         }
      }

      private void addSimulationToView(ISimulation simulation)
      {
         _lazyLoadTask.Load(simulation);
         var simulationNode = _treeNodeFactory.CreateFor(simulation);
         _view.AddNode(simulationNode);
      }

      public void Refresh()
      {
         EditParameterIdentification(_parameterIdentification);
      }

      public void RemoveSimulation(ISimulation simulation)
      {
         _lazyLoadTask.Load(simulation);
         if (_parameterIdentification.AnyOutputOfSimulationMapped(simulation))
         {
            var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ParameterIdentification.ReallyDeleteSimulationUsedInParameterIdentification(simulation.Name));
            if (viewResult == ViewResult.No)
               return;
         }

         _parameterIdentification.RemoveSimulation(simulation);
         _view.DestroyNode(simulation.Id);
         SimulationRemoved(this, new SimulationEventArgs(simulation));
      }

      public void ShowContextMenu(IReadOnlyList<ITreeNode> treeNodes, Point popupLocation)
      {
         var contextMenu = _multipleTreeNodeContextMenuFactory.CreateFor(treeNodes, this);
         contextMenu.Show(View, popupLocation);
      }

      public void ShowContextMenu(ITreeNode node, Point popupLocation)
      {
         var contextMenu = _treeNodeContextMenuFactory.CreateFor(node, this);
         contextMenu.Show(View, popupLocation);
      }
   }
}