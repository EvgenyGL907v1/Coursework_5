using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Menus
{
	public class UnitChoiceFormPresenter : ChoiceFormPresenter
	{
		public (int, string) SelectedUnit { get; private set; }

		private ReferencesManager _referenceManager;
		private List<(int, string)> _menuItems;

		public UnitChoiceFormPresenter() : base()
		{
			_referenceManager = new ReferencesManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Menu", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			var selectedData = _menuItems.Find(m => m.Item1 == SelectedId);
			SelectedUnit = (selectedData.Item1, selectedData.Item2);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);

			var menu = _referenceManager.GetUnitsList();
			_menuItems = menu;
			var table = Tables[MainTableKey];

			foreach (var menuItem in menu)
			{
				table.Rows.Add(menuItem.Item1, menuItem.Item2);
			}

		}
	}
}
