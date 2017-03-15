using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public static class ParameterIdentificationItems
   {
      public static readonly ParameterIdentificationItem<IParameterIdentificationDataSelectionPresenter> Data = new ParameterIdentificationItem<IParameterIdentificationDataSelectionPresenter>();
      public static readonly ParameterIdentificationItem<IParameterIdentificationParameterSelectionPresenter> Parameters = new ParameterIdentificationItem<IParameterIdentificationParameterSelectionPresenter>();
      public static readonly ParameterIdentificationItem<IParameterIdentificationConfigurationPresenter> Configuration = new ParameterIdentificationItem<IParameterIdentificationConfigurationPresenter>();
      public static readonly ParameterIdentificationItem<IParameterIdentificationResultsPresenter> Results = new ParameterIdentificationItem<IParameterIdentificationResultsPresenter>();
      public static readonly IReadOnlyList<ISubPresenterItem> All = new List<ISubPresenterItem> { Data, Parameters, Configuration, Results };
   }

   public class ParameterIdentificationItem<TSubPresenter> : SubPresenterItem<TSubPresenter> where TSubPresenter : IParameterIdentificationItemPresenter
   {
   }
}