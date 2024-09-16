using PresenterContext;
using System;
using System.Linq;
using System.Windows.Forms;
using Menus;
using DatabaseManager;

namespace AccessMenu
{
	public class AccessElementFormPresenter : ElementFormPresenter
	{
		private SystemDataManager _systemDataManager;

		private int _selectedMenu;
		private int _selectedUser;

		public AccessElementFormPresenter(int elementId, ElementFormType type) : base(elementId, type)
		{
			_systemDataManager = new SystemDataManager();

			var userBox = CreateComboBox("Пользователь");
			userBox.Click += new EventHandler(UserChoise);
			var menuBox = CreateComboBox("Меню");
			menuBox.Click += new EventHandler(MenuCoise);

			CreateCheckBox("Чтение");
			CreateCheckBox("Создание");
			CreateCheckBox("Редактирование");
			CreateCheckBox("Удаление");

			if (type == ElementFormType.CREATE)
				CreateButton("Открыть доступ", CreateElementEvent);
			else if (type == ElementFormType.UPDATE)
				CreateButton("Редактировать доступ", EditElementEvent);
		}

		private void UserChoise(object obj, EventArgs args)
		{
			UserChoiseFormPresenter userChoise = new UserChoiseFormPresenter();
			if (userChoise.ShowDialog() == DialogResult.OK)
			{
				var user = userChoise.SelectedUser.Item2;
				_selectedUser = userChoise.SelectedUser.Item1;

				var userBox = ComboBoxes["Пользователь"];
				userBox.Items.Clear();
				userBox.Items.Add(user);
				userBox.SelectedIndex = 0;
			}
		}
		private void MenuCoise(object obj, EventArgs args)
		{ 
			MenuChoiceFormPresenter menuChoise = new MenuChoiceFormPresenter();
			if (menuChoise.ShowDialog() == DialogResult.OK)
			{ 
				var menu = menuChoise.SelectedMenu.Item2;
				_selectedMenu = menuChoise.SelectedMenu.Item1;
				var menuBox = ComboBoxes["Меню"];

				menuBox.Items.Clear();
				menuBox.Items.Add(menu);
				menuBox.SelectedIndex = 0;
			}
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);

			if (Type == ElementFormType.CREATE)
				return;

			var accessList = _systemDataManager.GetAccessList();

			if (!accessList.Any(a => a.AccessId == CurrentElementId))
				throw new Exception($"Элемента с id {CurrentElementId} - не существует");

			var access = _systemDataManager.GetAccessList().Find(a => a.AccessId == CurrentElementId);

			_selectedUser = access.UserId;
			ComboBoxes["Пользователь"].Items.Add(access.UserLogin);
			ComboBoxes["Пользователь"].SelectedIndex = 0;

			_selectedMenu = access.MenuId;
			ComboBoxes["Меню"].Items.Add(access.Name);
			ComboBoxes["Меню"].SelectedIndex = 0;

			CheckBoxes["Чтение"].Checked = access.Read;
			CheckBoxes["Создание"].Checked = access.Write;
			CheckBoxes["Редактирование"].Checked = access.Edit;
			CheckBoxes["Удаление"].Checked = access.Delete;
		}

		public override void CreateElement()
		{
			try
			{
				base.CreateElement();

				if (ComboBoxes["Меню"].SelectedItem == null)
					throw new Exception("Не выбрано меню");

				if (ComboBoxes["Пользователь"].SelectedItem == null)
					throw new Exception("Не выбран пользователь");

				MenuData menuData = new MenuData()
				{
					MenuId = _selectedMenu,
					UserId = _selectedUser,

					Read = CheckBoxes["Чтение"].Checked,
					Write = CheckBoxes["Создание"].Checked,
					Edit = CheckBoxes["Редактирование"].Checked,
					Delete = CheckBoxes["Удаление"].Checked,

				};

				_systemDataManager.AddMenuAccess(menuData);
				MessageCaller.CallInfomationMessage($"Доступ успешно к {ComboBoxes["Меню"].SelectedItem} успешно открыт");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		public override void EditElement()
		{
			try
			{
				base.CreateElement();

				if (ComboBoxes["Меню"].SelectedItem == null)
					throw new Exception("Не выбрано меню");

				if (ComboBoxes["Пользователь"].SelectedItem == null)
					throw new Exception("Не выбран пользователь");

				MenuData menuData = new MenuData()
				{
					AccessId = CurrentElementId,

					MenuId = _selectedMenu,
					UserId = _selectedUser,

					Read = CheckBoxes["Чтение"].Checked,
					Write = CheckBoxes["Создание"].Checked,
					Edit = CheckBoxes["Редактирование"].Checked,
					Delete = CheckBoxes["Удаление"].Checked,

				};

				_systemDataManager.EditMenuAccess(menuData);
				MessageCaller.CallInfomationMessage($"Доступ успешно к {ComboBoxes["Меню"].SelectedItem} изменен");
				Form.Close();

			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

	}
}
