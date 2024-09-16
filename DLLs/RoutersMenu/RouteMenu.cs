using MenuInterface;
using PresenterContext;
using System;

namespace RoutersMenu
{
	public class RouteMenu : MenuBase
	{
		public RouteMenu(MenuAccess menuAccess) : base(menuAccess) 
		{ }

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);

			RouteElementListFormPresenter routeElementListForm = new RouteElementListFormPresenter();
			ListFormAccessControl(routeElementListForm.Form as ElementsListFormExample);
			routeElementListForm.ShowDialog();
		}
	}
}
