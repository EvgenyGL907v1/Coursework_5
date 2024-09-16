using MenuInterface;
using PresenterContext;
using System;

namespace CargoDocumentMenu
{
	public class CargoDocumentsMenu : MenuBase
	{
		public CargoDocumentsMenu(MenuAccess menuAccess) : base(menuAccess)
		{ }

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);

			CargoDocumentsListFormPresenter documentPresent = new CargoDocumentsListFormPresenter();
			ListFormAccessControl(documentPresent.Form as ElementsListFormExample);
			documentPresent.ShowDialog();
		}
	}
}
