using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace SuperGra.Model
{
public class MyItem : INotifyPropertyChanged, IMovable
{
    public string Description { get; set; }
    public string Nick { get; set; }

    string _ImageUri;
    public string ImageUri
    {
        get
        {
            return _ImageUri;
        }
        set
        {
            if (_ImageUri != value)
            {
                _ImageUri = value;
                TheImage = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
            }
        }
    }

    [XmlIgnore]
    public BitmapImage TheImage { get; private set; }

    double _X;
    public double X
    {
        get
        {
            return _X;
        }
        set
        {
            if (_X != value)
            {
                _X = value;
                RaisePropertyChanged("X");
            }
        }
    }

    double _Y;
    public double Y
    {
        get
        {
            return _Y;
        }
        set
        {
            if (_Y != value)
            {
                _Y = value;
                RaisePropertyChanged("Y");
            }
        }
    }

    [XmlIgnore]
    public FrameworkElement Parent { get; set; }

    void RaisePropertyChanged(string prop)
    {
        if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
    }
    public event PropertyChangedEventHandler PropertyChanged;

}
}
