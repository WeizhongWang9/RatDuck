using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static RatDuck.Script.Godot.TokenLabelPreset;

namespace RatDuck.Script.Godot
{
	public class TokenLabel
	{
		public string name="";
		public string description="";
		public TokenLabel(string name, string description)
		{
			this.name = name;
			this.description = description;
		}
	}
	public class TokenLabelPreset
	{
		//List<TokenLabel> placePresets = new();
		List<TokenLabel> policeStationPresets = new();
		List<TokenLabel> prisonPresets = new();
		List<TokenLabel> labPresets = new();
		List<CardInfo> _bodyPresets;
		public List<CardInfo> bodyPresets { get { return _bodyPresets; } }
		
		Random randGen = new(Guid.NewGuid().GetHashCode());

		public TokenLabelPreset()
		{
			init();

		}


		public void initBodyPlaceCardInfos(List<CardInfo> bodyPlaceCardInfos)
		{
			_bodyPresets = bodyPlaceCardInfos;
		}

		public string rollPoliceStationPresets() { return rollTokenLabel(policeStationPresets); }
		public string rollLabPresets() {return rollTokenLabel(labPresets); }
		public string rollPrisonPresets() { return rollTokenLabel(prisonPresets); }
		string rollTokenLabel(List<TokenLabel> list)
		{
			var s = "";
			var randNumber = randGen.Next() % list.Count;
			var ele = list[randNumber];
			list.Remove(ele);
			s = ele.name;

			return s;
		}
		

		void init()
		{
			//initPlacePresets();
			initPolicePresets();
			initPrisonPresets();
			initLabPresets();
		}

		void initPrisonPresets()
		{
			const ulong size = 2;
			string[] names = new string[size];
			string[] description = new string[size];

			names[0] = "QCPrison";
			names[1] = "DQCamp";


			foreach (string name in names)
			{
				var label = new TokenLabel(name, "");
				prisonPresets.Add(label);
			}
		}

		void initLabPresets()
		{
			const ulong size = 2;
			string[] names = new string[size];
			string[] description = new string[size];

			names[0] = "UmBra-Corp";
			names[1] = "SinoBio";


			foreach (string name in names)
			{
				var label = new TokenLabel(name, "");
				labPresets.Add(label);
			}
		}

		void initPolicePresets()
		{
			const ulong size = 2;
			string[] names = new string[size];
			string[] description = new string[size];

			names[0] = "LAPD";
			names[1] = "SHPD";


			foreach (string name in names)
			{
				var label = new TokenLabel(name, "");
				policeStationPresets.Add(label);
			}
		}

		/*void initPlacePresets()
		{
			const ulong size = 12;
			string[] names = new string[size];
			string[] description = new string[size];

			names[0] = "Bar";
			names[1] = "Hotel";
			names[2] = "Pharmacy";
			names[3] = "Dining Place";
			names[4] = "Factory";
			names[5] = "Company";
			names[6] = "Fountain";
			names[7] = "University";
			names[8] = "Neighborhood";
			names[9] = "Church";
			names[10] = "Theater";
			names[11] = "Shopping Mall";

			foreach (string name in names)
			{
				var label = new TokenLabel(name, "");
				placePresets.Add(label);
			}
		}*/
		public class CardInfo: ICardInfo
		{
			public string nice_name;
			public string texture_path;
			public string backface_texture_path;
			public string resource_script_path;
			public string description;

			public string get_back_face_texture_path()
			{
				return backface_texture_path;
			}

			public string get_description()
			{
				return description;
			}

			public string get_name()
			{
				return nice_name;
			}

			public string get_texture_path()
			{
				return texture_path;
			}
		}

		public interface ICardInfo
		{
			string get_name();
			string get_description();
			string get_back_face_texture_path();
			string get_texture_path();

		}

	}
		

}
