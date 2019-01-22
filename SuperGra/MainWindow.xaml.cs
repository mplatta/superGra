using SuperGra.Model;
using SuperGra.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SuperGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
        MainViewModel vm = new MainViewModel();
        Data.Repository repository = new Data.Repository();

        public MainWindow()
		{
			InitializeComponent();
            DataContext = vm;

            vm.AllItems = new ObservableCollection<MyItem>();
        }

        private void bAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.AllItems.Add(new MyItem { ImageUri = "Media/squirtle.png", Description = DateTime.Now.ToString() });
        }

        private void bSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            repository.Save<ObservableCollection<MyItem>>(vm.AllItems);
        }

        private void bLoad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.AllItems = repository.Load<ObservableCollection<MyItem>>();
        }
    }
}
