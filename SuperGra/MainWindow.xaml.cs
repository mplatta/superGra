using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using SuperGra.Model;
using SuperGra.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            qr_Generate();
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

        private void qr_Generate()
        {
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode("255.255.255.255", out qrCode);
            WriteableBitmapRenderer wRenderer = new WriteableBitmapRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Colors.Black, Colors.White);
            WriteableBitmap wBitmap = new WriteableBitmap(50, 50, 35, 35, PixelFormats.Gray8, null);
            wRenderer.Draw(wBitmap, qrCode.Matrix);

            QrCodeImage.Source = wBitmap;
        }
    }
}
