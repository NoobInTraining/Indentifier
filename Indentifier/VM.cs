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
		private bool isNotCopied = true;

		public Viewmodel()
		{
			Tabify = new EigeneKlassen.WPF.Commands.GeneralCommand(
				o => !string.IsNullOrWhiteSpace(ToIndent) && !string.IsNullOrEmpty(Identifiert),
				o =>
				{
					//split the input
					var lines = Regex.Split(toIndet, Environment.NewLine);
					//Find the highest index
					int index = 0;
					Array.ForEach(lines, l => index = (l.IndexOf(identi) > index) ? l.IndexOf(identi) : index);

					//intterate thorugh each line
					for (int i = 0; i < lines.Length; i++)
					{
						if (!Regex.IsMatch(lines[i], identi, caseSensi? RegexOptions.None : RegexOptions.IgnoreCase))
							continue;

						//split by the identifier thus removing it
						var splitted = Regex.Split(lines[i], identi, caseSensi ? RegexOptions.None : RegexOptions.IgnoreCase);
						lines[i] = splitted[0].PadRight(index) + identi + splitted[1].TrimStart();

						//add any other lines that might exist
						for (int j = 2; j < splitted.Length; j++)						
							lines[j] += identi + splitted[j];
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

		public EigeneKlassen.WPF.Commands.GeneralCommand Tabify { get; private set; }

		public EigeneKlassen.WPF.Commands.GeneralCommand CopyToClipboard { get; private set; }


	}
}
