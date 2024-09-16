using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Menus
{
	public class PortChoiceFormPresenter : ChoiceFormPresenter
	{
		private ReferencesManager _referencesManager;
		private List<(int, string)> _items;

		public (int, string) SelectedPort { get; set; }

		public PortChoiceFormPresenter() 
		{
			_referencesManager = new ReferencesManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Port", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);

			foreach (DataGridViewColumn column in Tables[MainTableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			var selectedData = _items.Find(m => m.Item1 == SelectedId);
			SelectedPort = (selectedData.Item1, selectedData.Item2);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);
			_items = _referencesManager.GetPortsList();
			var table = Tables[MainTableKey];

			foreach (var item in _items)
			{
				table.Rows.Add(item.Item1, item.Item2);
			}
		}
	}
}
