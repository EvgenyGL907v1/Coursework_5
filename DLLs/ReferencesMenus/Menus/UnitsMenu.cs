using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class UnitsMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public UnitsMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetUnitsList, AddUnit, EditUnit, RemoveUnit, "Ед. измерения");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetUnitsList()
		{ 
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetUnitsList();

			return list;
		}

		private void AddUnit(string name) => _referencesManager.AddUnit(name);

		private void EditUnit(int unitId, string name) => _referencesManager.EditUnit(unitId, name);

		private void RemoveUnit(int unitId) => _referencesManager.RemoveUnit(unitId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}

	}
}
