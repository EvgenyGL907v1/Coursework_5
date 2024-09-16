using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;

namespace ReferencesMenus
{
	public class BankMenu : MenuBase
	{
		private ReferenceElementsListFormPresenter _presenter;
		private ReferencesManager _referencesManager;

		public BankMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			_referencesManager = new ReferencesManager();
			_presenter = new ReferenceElementsListFormPresenter(GetBankList, AddBank, EditBank, RemoveBank, "Банки");
			ListFormAccessControl(_presenter.Form as ElementsListFormExample);
		}

		private List<(int, string)> GetBankList()
		{
			ReferencesManager referencesManager = new ReferencesManager();
			var list = referencesManager.GetBanksList();

			return list;
		}

		private void AddBank(string name) => _referencesManager.AddBank(name);

		private void EditBank(int bankId, string name) => _referencesManager.EditBank(bankId, name);

		private void RemoveBank(int bankId) => _referencesManager.RemoveBank(bankId);

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			_presenter.ShowDialog();
		}
	}

}
