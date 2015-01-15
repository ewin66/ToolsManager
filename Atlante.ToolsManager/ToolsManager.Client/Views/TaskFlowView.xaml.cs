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
using ToolsManager.Client.ViewModels;

namespace ToolsManager.Client.Views
{
    /// <summary>
    /// Interaction logic for TaskFlowView.xaml
    /// </summary>
    public partial class TaskFlowView : UserControl
    {
        public TaskFlowView()
        {
            InitializeComponent();
        }

        private void SuccessTaskSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = this.DataContext as TaskFlowViewModel;
            if (viewModel == null)
                return;

            if (e.AddedItems.Count <= 0)
                return;

            viewModel.ChangeSelectedSuccessBranch(e.AddedItems[0]);
        }

        private void ErrorTaskSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = this.DataContext as TaskFlowViewModel;
            if (viewModel == null)
                return;

            if (e.AddedItems.Count <= 0)
                return;

            viewModel.ChangeSelectedErrorBranch(e.AddedItems[0]);
        }
    }
}
