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
using System.ComponentModel;

namespace VG_List
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VGameList vglist;
        Visibility[] colVisibility;
        ICollectionView vglistView;

        public MainWindow()
        {
            InitializeComponent();
            vglist = new VGameList();
            vglist.DeserializeVGList();
            colVisibility = new Visibility[DGProd.Columns.Count];
            colShow();

            vglistView = CollectionViewSource.GetDefaultView(vglist.VGames);
            //FilterSelector();

            BindProd();

            rbAll.IsChecked = true;
        }

        private void loadFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
                return;
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            vglist.SerializeVGList();
        }

        private void BindProd()
        {
            if (vglist.VGames != null)
            {
                //DGProd.ItemsSource = vglist.VGames;
                DGProd.ItemsSource = vglistView;
                DGProd.AutoGenerateColumns = false;
            }
        }

        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            string txt = InputTextBox.Text;
            //var count = txt.Where(x => x == '&').Count();
            var query = from item in txt.Split('#')
                        let parts = item.Split('|')
                        let t = parts[0]
                        let p = parts[1]
                        select new VGame(t, p, VGStatus.Играю);

            foreach (var vg in query) vglist.VGames.Add(vg);
            //foreach (var vg in query) vglist.VGames.AddNewItem(vg);
            DGProd.Items.Refresh();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) vglist.SerializeVGList();
            else Application.Current.Shutdown();
        }

        private void chooseColumns_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void colVisArrFill()
        {
            for (int i = 0; i < colVisibility.Length; i++)
            {
                CheckBox cb = (CheckBox)chooseColumns.ContextMenu.Items[i];
                if ((bool)cb.IsChecked)
                    colVisibility[i] = Visibility.Visible;
                else
                    colVisibility[i] = Visibility.Collapsed;
            }
        }

        private void colShow()
        {
            colVisArrFill();
            for (int i = 0; i < colVisibility.Length; i++)
            {
                DGProd.Columns[i].Visibility = colVisibility[i];
            }
        }

        private void FilterSelector()
        {
            VGStatus s = VGStatus.Пройдена;
            if (rbAll.IsChecked == true)
            {
                //s = VGStatus.Играю;
                vglistView.Filter = new Predicate<object>(item => ((VGame)item).Title != null);
            }
            else
            {
                if (rbPlaying.IsChecked == true)
                    s = VGStatus.Играю;
                if (rbBeaten.IsChecked == true)
                    s = VGStatus.Пройдена;
                if (rbOnHold.IsChecked == true)
                    s = VGStatus.Отложена;
                if (rbDropped.IsChecked == true)
                    s = VGStatus.Заброшена;
                if (rbToPlay.IsChecked == true)
                    s = VGStatus.Поиграть;
                vglistView.Filter =
                    new Predicate<object>(item => ((VGame)item).Status.Equals(s));
            }
            DGProd.Items.Refresh();
        }

        private void cBDateAdded_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBDateFinished_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBStatus_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBPlatform_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBTitle_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBScore_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void cBCategory_Click(object sender, RoutedEventArgs e)
        {
            colShow();
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }

        private void rbPlaying_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }

        private void rbBeaten_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }

        private void rbOnHold_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }

        private void rbDropped_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }

        private void rbToPlay_Checked(object sender, RoutedEventArgs e)
        {
            FilterSelector();
        }
    }
}
