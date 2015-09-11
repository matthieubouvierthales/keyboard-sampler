using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sampler
{
    public class TaskbarClickCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.MainWindow.WindowState=WindowState.Normal;
        }

        public event EventHandler CanExecuteChanged;
    }
}
