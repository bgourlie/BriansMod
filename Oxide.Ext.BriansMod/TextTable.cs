namespace Oxide.Ext.BriansMod
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class TextTable
	{
		private readonly string[] _columns;
		private readonly List<List<string>> _rows = new List<List<string>>();

		public TextTable(params string[] columns)
		{
			_columns = columns;
		}

		public void AddRow(params object[] values)
		{
			if (values.Length > _columns.Length)
			{
				throw new Exception("Row has too many columns.");
			}

			_rows.Add(new List<string>(values.Select(v => v.ToString())));
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			var colWidths = _columns.Select(c => c.Length).ToArray();
			for(var i = 0; i < _rows.Count; i++)
			{
				for (var j = 0; j < _rows[i].Count; j++)
				{
					if (_rows[i][j].Length > colWidths[j])
					{
						colWidths[j] = _rows[i][j].Length;
					}
				}
			}

			var totalWidth = colWidths.Aggregate((total, width) => total + width);
			for(var i = 0; i < _columns.Length; i++)
			{
				sb.AppendFormat("| {0}{1} ", _columns[i], new string(' ', colWidths[i] - _columns[i].Length));
			}
			sb.AppendLine();
			sb.AppendLine(new string('-', totalWidth + (_columns.Length * 3)));

			foreach (var row in _rows)
			{
				for (var i = 0; i < row.Count; i++)
				{
					sb.AppendFormat("| {0}{1} ", row[i], new string(' ', colWidths[i] - row[i].Length));
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
