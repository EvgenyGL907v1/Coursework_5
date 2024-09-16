using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class CargoMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public CargoMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetCargoList, AddCargo, EditCargo, RemoveCargo, "Грузы");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetCargoList()
		{
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetCargoList();

			return list;
		}

		private void AddCargo(string name) => _referencesManager.AddCargo(name);

		private void EditCargo(int cargoId, string name) => _referencesManager.EditCargo(cargoId, name);

		private void RemoveCargo(int cargoId) => _referencesManager.DeleteCargo(cargoId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}
	}
}
