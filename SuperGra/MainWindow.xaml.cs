using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.Win32;
using Newtonsoft.Json;
using SuperGra.Dialogs;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SuperGra
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
	/// 
    public partial class MainWindow : Window, IDialogWindow
    {
		// TODO: Zrobić z tego jakiś ładny MVC kiedyś xD

		private static readonly string url             = "http://localhost:34450/api/";
		private static readonly string apiGetCharacter = "character";

		private ImageBrush imagePhoto { get; set; }
		private PostService ps;

		MainViewModel vm = new MainViewModel();
        Data.Repository repository = new Data.Repository();
        
        public void getEvent(Object sender, EventString e)
        {
			Debug.WriteLine(e.JsonString);
            dynamic jsonResult = JsonConvert.DeserializeObject(e.JsonString);

			int    action = jsonResult.Action;
			string id     = jsonResult.Id;

			Character character;

			switch (action)
			{
				case 1:	// someone connect from mobile apk
					// TODO: powiadomienie że ktoś się połączył
					;
					break;
				case 2: // someone create new character
					// TODO: niezabezpieczone jak null albo zerwie połaczenie
					character = _get_character_from_url(url + apiGetCharacter + "/" + jsonResult.CharacterId.ToString());
					if (character != null) AddNewWidget(character);
					break;
				case 3: // someone update charater
					// TODO: niezabezpieczone jak null albo zerwie połaczenie
					character = _get_character_from_url(url + apiGetCharacter + "/" + jsonResult.CharacterId.ToString());
					if (character != null) UpdateWidget(character);
					break;
				case 4: // dice roll
					;
					break;
				case 5: // message
					;
					break;
				case 6: // avatar img
					;
					break;
			}
		}

		#region Buttons

		private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var myCanvas = FindVisualChildren<ListView>(mylist);

            foreach (var i in myCanvas)
            {
                if(i.Name == "lvStats")
                {
                    Debug.WriteLine(i.Items.Count);
                }
            }
        }

        private void bSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            repository.Save<ObservableCollection<MyItem>>(vm.AllItems);
        }

        private void bLoad_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            vm.AllItems = repository.Load<ObservableCollection<MyItem>>();
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

		#endregion

		#region help functions

		private void AddNewWidget(Character ch)
		{
			Dispatcher.Invoke(new Action(() => { vm.AllItems.Add(new MyItem { ImageUri = "Media/squirtle.png", CharacterCard = ch }); }));
		}

		private void UpdateWidget(Character ch)
		{
			foreach(MyItem mi in vm.AllItems)
			{
                if(mi.CharacterCard.Id != null)
                {
				    if (mi.CharacterCard.Equals(ch))
				    {
					    mi.CharacterCard.Update(ch);
				    }
                }
            }
		}

		private Character _get_character_from_url(string _url)
		{
			string json = ps.SendGet(_url);

			

			Character ch;

			if (json == null) return null;
			
			ch =  JsonConvert.DeserializeObject<Character>(ps.SendGet(_url));

			dynamic ttt = JsonConvert.DeserializeObject(json);
			int id = ttt.CharacterId;

			Debug.WriteLine("GGG" + id.ToString());
			Debug.WriteLine("AAA" + json);
			Debug.WriteLine("BBB" + ch.getJSONString());

			ch.IdCharacter = id;

			return ch;
		}

		private string _get_local_ip()
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

		private List<String> _get_local_ip2()
		{
			List<String> ips = new List<String>();
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					ips.Add(ip.ToString());
				}
			}

			return ips;
		}

		private void Qr_Generate()
		{
			//string ip = _get_local_ip();
			List<String> arrIp = _get_local_ip2();

			string ip = arrIp[arrIp.Count - 1];

			txb_ip.Text = "IP: " + ip;

			QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.M);
			QrCode qrCode;
			encoder.TryEncode(ip, out qrCode);
			WriteableBitmapRenderer wRenderer = new WriteableBitmapRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Colors.Black, Colors.White);
			WriteableBitmap wBitmap = new WriteableBitmap(50, 50, 35, 35, PixelFormats.Gray8, null);
			wRenderer.Draw(wBitmap, qrCode.Matrix);

			QrCodeImage.Source = wBitmap;
		}

		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
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

		#endregion

		#region Constructors/Destructors

		public MainWindow()
		{
			ps = new PostService();
			InitializeComponent();
			DataContext = vm;

			vm.AllItems = new ObservableCollection<MyItem>();

			//#region TestRegion
			////TEST
			//Character myCharacter = new Character();
			//myCharacter.Id = "TEST";
			//myCharacter.Name = "Squirtle";
			//myCharacter.Class = "Pokemon";
			//myCharacter.Description = "Typ Wodny";
			//Stat testParam = new Stat { Name = "Strength", Value = 100 };
			//Stat testParam2 = new Stat { Name = "Agility", Value = 100 };
			//Stat testParam3 = new Stat { Name = "Luck", Value = 100 };
			//Stat testParam4 = new Stat { Name = "Power", Value = 100 };
			//ObservableCollection<Stat> testList = new ObservableCollection<Stat>();
			//testList.Add(testParam);
			//testList.Add(testParam2);
			//testList.Add(testParam3);
			//testList.Add(testParam4);
			//myCharacter.Stats = testList;
			//ObservableCollection<string> testEQList = new ObservableCollection<string>();
			//testEQList.Add("sword");
			//testEQList.Add("dupa");
			//myCharacter.Equipment = testEQList;
			//MyItem testItem = new MyItem { ImageUri = "Media/squirtle.png", CharacterCard = myCharacter };
			//vm.AllItems.Add(testItem);
			//#endregion
			Qr_Generate();

            ps.es += getEvent;
            ps.Start();
        }

		~MainWindow()
        {
            ps.Stop();
        }

		#endregion
	}
}
