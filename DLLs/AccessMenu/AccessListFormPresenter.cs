using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AccessMenu
{
	public class AccessListFormPresenter : ListFormPresenter
	{
		private SystemDataManager _systemDataManager;

		public AccessListFormPresenter() : base()
		{
			_systemDataManager = new SystemDataManager();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("User", "Пользователь", TableColumnType.TEXT),
				new TableColumn("Menu", "Меню", TableColumnType.TEXT),
				new TableColumn("Read", "Чтение", TableColumnType.CHECK_BOX),
				new TableColumn("Write", "Запись", TableColumnType.CHECK_BOX),
				new TableColumn("Edit", "Редактирование", TableColumnType.CHECK_BOX),
				new TableColumn("Delete", "Удаление", TableColumnType.CHECK_BOX),
			};
			GridViewPresenter gridViewPresenter = new GridViewPresenter(columns, Tables[TableKey]);
		}
		protected override void ButtonsInit(ElementsListFormExample formExample)
		{
			base.ButtonsInit(formExample);
			TableInit();
		}

		public override void Add()
		{
			base.Add();
			AccessElementFormPresenter accessElement = new AccessElementFormPresenter(0, ElementFormType.CREATE);
			accessElement.ShowDialog();
			TableUpdate();
		}

		public override void Edit()
		{
			base.Edit();

			if (SelectedId == -1)
				throw new Exception("Не выбрана строка");

			AccessElementFormPresenter accessElement = new AccessElementFormPresenter(SelectedId, ElementFormType.UPDATE);
			accessElement.ShowDialog();
			TableUpdate();
		}

		public override void Delete()
		{
			base.Delete();

			if (SelectedId == -1)
				throw new Exception("Не выбрана строка");

			_systemDataManager.DeleteMenuAccess(SelectedId);
			TableUpdate();
		}

		public override void TableUpdate()
		{
			base.TableUpdate();
			var accessList = _systemDataManager.GetAccessList();

			Tables[TableKey].Rows.Clear();

			foreach (var access in accessList)
				Tables[TableKey].Rows.Add(access.AccessId, access.UserLogin, access.Name, access.Read, access.Write, access.Edit, access.Delete);

		}
	}
}
