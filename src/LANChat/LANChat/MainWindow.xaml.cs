using LANChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LANChat.Common;
using System.ComponentModel;
using LANChat.Views;

namespace LANChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get { return (MainViewModel)DataContext; } }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel.MessageArrived += ViewModel_MessageArrived;
        }

        private void ViewModel_MessageArrived(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
                FlashWindow.Flash(this, 5);
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var be = TxtMessage.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();

                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                {
                    var selectionIndex = TxtMessage.SelectionStart;
                    TxtMessage.Text = TxtMessage.Text.Insert(selectionIndex, Environment.NewLine);
                    TxtMessage.SelectionStart = selectionIndex + Environment.NewLine.Length;
                    return;
                }

                if (ViewModel.SendMessageCommand.CanExecute(null))
                    ViewModel.SendMessageCommand.Execute(null);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ViewModel.DisconnectFromServerCommand.CanExecute(null))
                ViewModel.DisconnectFromServerCommand.Execute(null);
        }

        private void BtnOpenServerWindow_Click(object sender, RoutedEventArgs e)
        {
            var serverWindow = new ServerWindow();
            serverWindow.Show();
        }
    }
}
