using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        private Frame windowFrame;
        public StartPage()
        {
            this.InitializeComponent();
            windowFrame = Window.Current.Content as Frame;
        }

        private void Scanner_Clicked(object sender, RoutedEventArgs e)
        {
            if(windowFrame == null)
            {
                return;
            }
            windowFrame.Navigate(typeof(SResultPage));
        }

        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            if (windowFrame == null)
            {
                return;
            }
            windowFrame.Navigate(typeof(MainPage));
        }
    }
}
