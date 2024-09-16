using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class PortMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public PortMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetPortList, AddPort, EditPort, RemovePort, "Порты");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetPortList()
		{
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetPortsList();

			return list;
		}

		private void AddPort(string name) => _referencesManager.AddPort(name);

		private void EditPort(int portId, string name) => _referencesManager.EditPort(portId, name);

		private void RemovePort(int portId) => _referencesManager.RemovePort(portId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}
	}
}
