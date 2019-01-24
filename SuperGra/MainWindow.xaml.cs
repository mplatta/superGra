using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.Win32;
using Newtonsoft.Json;
using SuperGra.Model;
using SuperGra.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        private ImageBrush imagePhoto { get; set; }
        private PostService ps;

        public void getEvent(Object sender, EventString e)
        {
            dynamic jsonResult = JsonConvert.DeserializeObject(e.JsonString);
            string id = jsonResult.Id;
            Character test = new Character { Id = id, Class = "FDF" };

            Dispatcher.Invoke(new Action(() => { vm.AllItems.Add(new MyItem { ImageUri = "Media/squirtle.png", CharacterCard = test }); }));
        }

        public MainWindow()
        {
            ps = new PostService();
            InitializeComponent();
            DataContext = vm;

            vm.AllItems = new ObservableCollection<MyItem>();

            #region TestRegion
            //TEST
            //Character myCharacter = new Character();
            //myCharacter.Name = "Squirtle";
            //myCharacter.Class = "Pokemon";
            //myCharacter.Description = "Typ Wodny";
            //Stat testParam = new Stat { Name = "Strength", Value = 100 };
            //Stat testParam2 = new Stat { Name = "Agility", Value = 100 };
            //Stat testParam3 = new Stat { Name = "Luck", Value = 100 };
            //Stat testParam4 = new Stat { Name = "Power", Value = 100 };
            //List<Stat> testList = new List<Stat>();
            //testList.Add(testParam);
            //testList.Add(testParam2);
            //testList.Add(testParam3);
            //testList.Add(testParam4);
            //myCharacter.Stats = testList;
            //MyItem testItem = new MyItem { ImageUri = "Media/squirtle.png", CharacterCard = myCharacter };
            //vm.AllItems.Add(testItem);
            #endregion

            Qr_Generate();

            ps.es += getEvent;
            ps.Start();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

            //vm.AllItems.Add(new MyItem { ImageUri = "Media/squirtle.png", Description = DateTime.Now.ToString(), Nick = "Squirtle" });
            //Debug.WriteLine(ps.SendNews("{'Id':'gggg', 'Action':1}").ToString());
            //         Character test = new Character { Id = "22", Class = "FDF" };

            //         Dispatcher.Invoke(new Action(() => { vm.AllItems.Add(new MyItem { ImageUri = "Media/squirtle.png", CharacterCard = test }); }));
        }

        private void bSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            repository.Save<ObservableCollection<MyItem>>(vm.AllItems);
        }

        private void bLoad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.AllItems = repository.Load<ObservableCollection<MyItem>>();
        }

        private string _getLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void Qr_Generate()
        {
            string ip = _getLocalIp();

            txb_ip.Text = "IP: " + ip;

            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode(ip, out qrCode);
            WriteableBitmapRenderer wRenderer = new WriteableBitmapRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Colors.Black, Colors.White);
            WriteableBitmap wBitmap = new WriteableBitmap(50, 50, 35, 35, PixelFormats.Gray8, null);
            wRenderer.Draw(wBitmap, qrCode.Matrix);

            QrCodeImage.Source = wBitmap;
        }

        private void BtnLoadMap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                var myCanvas = FindVisualChildren<Canvas>(mylist);

                foreach (var i in myCanvas)
                {
                    if (i.Name == "myCanvas")
                    {
                        imagePhoto = new ImageBrush();
                        imagePhoto.ImageSource = new BitmapImage(new Uri(op.FileName, UriKind.Absolute));
                        i.Background = imagePhoto;
                    }
                }
            }
        }

        private IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }
                else
                {
                    var childOfChild = FindVisualChildren<T>(child);
                    if (childOfChild != null)
                    {
                        foreach (var subchild in childOfChild)
                        {
                            yield return subchild;
                        }
                    }
                }
            }
        }

        ~MainWindow()
        {
            ps.Stop();
        }
    }
}
