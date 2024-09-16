using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Menus
{
	public class MenuChoiceFormPresenter : ChoiceFormPresenter
	{
		public (int, string) SelectedMenu { get; private set; }

		private SystemDataManager _systemDataManager;
		private List<MenuData> _menuItems;

		public MenuChoiceFormPresenter() : base()
		{
			_systemDataManager = new SystemDataManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Menu", "Меню", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			var selectedData = _menuItems.Find(m => m.MenuId == SelectedId);
			SelectedMenu = (selectedData.MenuId, selectedData.Name);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);

			var menu = _systemDataManager.GetMenuList();
			_menuItems = menu;
			var table = Tables[MainTableKey];

			foreach (var menuItem in menu)
			{
				table.Rows.Add(menuItem.MenuId, menuItem.Name);
			}
			
		}
	}
}
