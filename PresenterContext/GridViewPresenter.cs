using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterContext
{
	public enum TableColumnType
	{ 
		TEXT,
		CHECK_BOX,
	}

	public struct TableColumn
	{ 
		public string Name { get; private set; }
		public string Header { get; private set; }
		public TableColumnType Type { get; private set; }

		public TableColumn(string name, string header, TableColumnType type)
		{ 
			Name = name;
			Header = header;
			Type = type;
		}
	}

	public class GridViewPresenter
	{
		public DataGridView DataGridView { get; private set; }

        public GridViewPresenter(List<TableColumn> columns, DataGridView dataGridView)
		{
			DataGridView = dataGridView;
			dataGridView.Columns.Clear();

			foreach (var column in columns)
				CreateColumn(column);
		}

		private void CreateColumn(TableColumn column)
		{
			DataGridViewColumn newColumn = column.Type == TableColumnType.CHECK_BOX
				? (DataGridViewColumn)new DataGridViewCheckBoxColumn()
				: new DataGridViewTextBoxColumn();

			newColumn.Name = column.Name;
			newColumn.HeaderText = column.Header;

            DataGridView.Columns.Add(newColumn);
		}


	}
}
