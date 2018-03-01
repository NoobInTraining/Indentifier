using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Indentifier
{
	class Viewmodel : EigeneKlassen.WPF.BaseNotifyPropertyChanged
	{
		private bool isNotCopied = false;

		public Viewmodel()
		{
			Tabify = new EigeneKlassen.WPF.Commands.GeneralCommand(
				o => !string.IsNullOrWhiteSpace(ToIndent) && !string.IsNullOrEmpty(Identifiert),
				o =>
				{
					//generate teh pattern
					Regex pattern;
					if (singleWord)
						pattern = new Regex(@"\b" + identi + @"\b", caseSensi ? RegexOptions.None : RegexOptions.IgnoreCase);
					else
						pattern = new Regex(identi, caseSensi ? RegexOptions.None : RegexOptions.IgnoreCase);

					//split the input
					var lines = Regex.Split(toIndet, Environment.NewLine);
					//Find the highest index
					int index = 0;					
					foreach (var line in lines)
					{
						var tmpIdx = pattern.Match(line).Index;
						if (tmpIdx > index)
							index = tmpIdx;
					}

					//intterate thorugh each line
					for (int i = 0; i < lines.Length; i++)
					{
						
						if (!pattern.IsMatch(lines[i]))
							continue;

						var tmpMatch = pattern.Match(lines[i]);

						//split by the identifier thus removing it
						var splitted = pattern.Split(lines[i]);
						lines[i] = splitted[0].PadRight(index) + tmpMatch.Value + splitted[1];

						//add any other lines that might exist
						for (int j = 2; j < splitted.Length; j++)						
							lines[j] += tmpMatch.Value + splitted[j];
					}

					ToIndent = string.Join(Environment.NewLine, lines);
					isNotCopied = true;
				}
			);

			CopyToClipboard = new EigeneKlassen.WPF.Commands.GeneralCommand(
				o => isNotCopied && !string.IsNullOrWhiteSpace(ToIndent) ,
				o => { System.Windows.Clipboard.SetText(ToIndent); isNotCopied = false; }
			);
		}

		#region [ Identifiert ]

		string identi;

		public string Identifiert
		{
			get
			{
				return identi;
			}
			set
			{
				if (identi != value)
				{
					identi = value;
					RaisePropertyChanged(nameof(Identifiert));
				}
			}
		}

		#endregion [ Identifiert ] 

		#region [ ToIndent ]

		string toIndet;

		public string ToIndent
		{
			get
			{
				return toIndet;
			}
			set
			{
				if (toIndet != value)
				{
					toIndet = value;
					RaisePropertyChanged(nameof(ToIndent));
				}
			}
		}

		#endregion [ ToIndent ]		

		#region [ CaseSensitive ]

		bool caseSensi = false;

		public bool CaseSensitive
		{
			get
			{
				return caseSensi;
			}
			set
			{
				if (caseSensi != value)
				{
					caseSensi = value;
					RaisePropertyChanged(nameof(CaseSensitive));
				}
			}
		}

		#endregion [ CaseSensitive ] 

		#region [ SingleWord ]

		bool singleWord = true;

		public bool SingleWord
		{
			get
			{
				return singleWord;
			}
			set
			{
				if (singleWord != value)
				{
					singleWord = value;
					RaisePropertyChanged(nameof(SingleWord));
				}
			}
		}

		#endregion [ SingleWord ] 

		public EigeneKlassen.WPF.Commands.GeneralCommand Tabify { get; private set; }

		public EigeneKlassen.WPF.Commands.GeneralCommand CopyToClipboard { get; private set; }


	}
}
