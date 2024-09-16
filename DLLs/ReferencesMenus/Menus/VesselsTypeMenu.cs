using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class VesselTypeMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public VesselTypeMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetVesselTypeList, AddVesselType, EditVesselType, RemoveVesselType, "Типы судов");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetVesselTypeList()
		{
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetTypesList();

			return list;
		}

		private void AddVesselType(string name) => _referencesManager.AddType(name);

		private void EditVesselType(int vesselTypeId, string name) => _referencesManager.EditType(vesselTypeId, name);

		private void RemoveVesselType(int vesselTypeId) => _referencesManager.RemoveType(vesselTypeId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}
	}
}
