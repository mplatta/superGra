using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SuperGra.Model
{
    public class Character : INotifyPropertyChanged
	{
		private string _id;
		private int _id_character;
		private string _name;
		private string _description;
		private string _class;
        private ObservableCollection<Stat> _stats;
        //private List<string> _equipment;
        private ObservableCollection<string> _equipment;

        public event PropertyChangedEventHandler PropertyChanged;

		#region Getters/Setters

		public string Id
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
				OnPropertyChanged(nameof(Id));
			}
		}

		public int IdCharacter
		{
			get
			{
				return _id_character;
			}

			set
			{
				_id_character = value;
				OnPropertyChanged(nameof(IdCharacter));
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
				OnPropertyChanged(nameof(Description));
			}
		}

		public string Class
		{
			get
			{
				return _class;
			}

			set
			{
				_class = value;
				OnPropertyChanged(nameof(Class));
			}
		}

		public ObservableCollection<Stat> Stats
		{
			get
			{
				return _stats;
			}

			set
			{
				_stats = value;
				OnPropertyChanged(nameof(Stats));
			}
		}

		public ObservableCollection<string> Equipment
		{
			get
			{
				return _equipment;
			}

			set
			{
				_equipment = value;
				OnPropertyChanged(nameof(Equipment));
			}
		}

		#endregion

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		public void Update(Character ch)
		{
			Name        = ch.Name;
			Description = ch.Description;
			Class       = ch.Class;
			Stats       = ch.Stats;
			Equipment   = ch.Equipment;
		}

		public override bool Equals(object obj)
		{
			return Id.Equals(((Character)obj).Id);
		}
	}
}