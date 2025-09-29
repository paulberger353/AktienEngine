using AktienEngine.ViewModel;
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

namespace AktienEngine.View
{
    /// <summary>
    /// Interaktionslogik für LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : UserControl
    {
        public LauncherWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Methode wird aufgerufen wenn auf ein Bild geklickt wird.
        /// Andere Bilder normal Zustand, das geklickte Bild wird hervorgehoben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_SwitchGame(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image img && DataContext is VMMainWindow vm)
            {
                //Bei klicken auf Image, andere Games normal Zustand, das geklickte Game hervorheben
                int col = Grid.GetColumn(img);
                vm.SelectedImage = col; 
            }
        }
    }
}
