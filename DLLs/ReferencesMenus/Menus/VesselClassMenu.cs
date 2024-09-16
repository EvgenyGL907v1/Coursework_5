using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class VesselClassMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public VesselClassMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetVesselClassList, AddVesselClass, EditVesselClass, RemoveVesselClass, "Классы судов");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetVesselClassList()
		{
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetClassesList();

			return list;
		}

		private void AddVesselClass(string name) => _referencesManager.AddClass(name);

		private void EditVesselClass(int vesselClassId, string name) => _referencesManager.EditClass(vesselClassId, name);

		private void RemoveVesselClass(int vesselClassId) => _referencesManager.RemoveClass(vesselClassId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}
	}
}
