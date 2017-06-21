using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Container;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Starter.Tasks;
using MyContext = OSPSuite.Starter.Tasks.MyContext;

namespace OSPSuite.Starter.Views
{
   public partial class FormMain : Form
   {
      private readonly IHistoryManager _historyManager;
      private readonly ScreenBinder<Parameter> _screenBinder;
      private readonly Parameter _parameter;

      public FormMain()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<Parameter>();
         _historyManager = IoC.Resolve<IHistoryManager>();

         InitializeBinding();
         _parameter = new Parameter();
         _screenBinder.BindToSource(_parameter);
      }

      private void InitializeBinding()
      {
         _screenBinder.BindingMode = BindingMode.OneWay;
         _screenBinder.Bind(param => param.Value)
            .To(tbValue)
            .OnValueUpdating += ParameterValueUpdating;
      }

      private void ParameterValueUpdating(Parameter parameter, PropertyValueSetEventArgs<double> e)
      {
         ICommand<MyContext> command = new ParameterValueSetCommand(parameter, e.NewValue);
         command.Execute(new MyContext());
         _historyManager.AddToHistory(command);
      }
   }
}