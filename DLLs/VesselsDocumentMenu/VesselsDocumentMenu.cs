using MenuInterface;
using PresenterContext;
using System;

namespace VesselsDocumentMenu
{
	public class VesselsDocumentMenu : MenuBase
	{
		public VesselsDocumentMenu(MenuAccess menuAccess) : base(menuAccess)
		{ }

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);

			VesselsListDocumentFormPresenter documentPresent = new VesselsListDocumentFormPresenter();
			ListFormAccessControl(documentPresent.Form as ElementsListFormExample);
			documentPresent.ShowDialog();
		}
	}
}
