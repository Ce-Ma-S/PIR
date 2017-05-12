using Common.Views;
using Pir.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Pir.Views
{
    /// <summary>
    /// Main page.
    /// </summary>
    public sealed partial class Main :
        Page,
        IViewOf<ViewModels.Main>
    {
        public Main()
        {
            InitializeComponent();
            Initialize();
        }

        #region Progress

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(Main), null);

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            nameof(Message), typeof(string), typeof(Main), null);

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        #endregion

        public ViewModels.Main Model
        {
            get { return (ViewModels.Main)DataContext; }
            private set { DataContext = value; }
        }

        private async Task Initialize()
        {
            try
            {
                IsBusy = true;
                Message = "Initializing...";
                Model = new ViewModels.Main();
                await Model.Initialize();
            }
            finally
            {
                Message = null;
                IsBusy = false;
            }
        }
    }
}
