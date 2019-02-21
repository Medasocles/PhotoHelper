using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PhotoHelper.Core.Command
{
    /// <summary>
    /// Represents a custom implementation of TriggerAction you can use for Event-To-Command-Binding
    /// that includes forwarding the event args to your command.
    /// The standard implementation InvokeCommandAction does not provide you with the event args.
    /// <example>
    /// <i:Interaction.Triggers>
    ///      <i:EventTrigger EventName="Closing" >
    ///         <i:CustomInvokeCommandAction Command="{Binding ACommandProperty}" />
    ///     </i:EventTrigger>
    /// </i:Interaction.Triggers>
    /// </example>
    /// </summary>
    public sealed class CustomInvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            nameof(CommandParameter), typeof(object), typeof(CustomInvokeCommandAction), null);

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(CustomInvokeCommandAction), null);

        public static readonly DependencyProperty DynamicCommandParameterProperty = DependencyProperty.Register(
            nameof(DynamicCommandParameter), typeof(DynamicCommandParameters), typeof(CustomInvokeCommandAction),
            new PropertyMetadata(default(DynamicCommandParameters)));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public DynamicCommandParameters DynamicCommandParameter
        {
            get => (DynamicCommandParameters) GetValue(DynamicCommandParameterProperty);
            set => SetValue(DynamicCommandParameterProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (AssociatedObject != null)
            {
                var command = Command;
                if (command != null)
                {
                    Tuple<DynamicCommandParameters, object> tuple;
                    switch (DynamicCommandParameter)
                    {
                        case DynamicCommandParameters.EventArgs:
                            tuple = new Tuple<DynamicCommandParameters, object>(DynamicCommandParameters.EventArgs,
                                parameter);
                            break;
                        case DynamicCommandParameters.MousePosition:
                            tuple = new Tuple<DynamicCommandParameters, object>(DynamicCommandParameters.MousePosition,
                                Mouse.GetPosition((IInputElement) AssociatedObject));
                            break;
                        default:
                            tuple = new Tuple<DynamicCommandParameters, object>(DynamicCommandParameters.None,
                                CommandParameter);
                            break;
                    }

                    if (command.CanExecute(tuple))
                        command.Execute(tuple);
                }
            }
        }
    }

    public enum DynamicCommandParameters
    {
        None,
        EventArgs,
        MousePosition
    }
}
