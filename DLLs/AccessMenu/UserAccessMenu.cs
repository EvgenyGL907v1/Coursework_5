using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccessMenu
{
	public class UserAccessMenu : MenuBase
	{
		public UserAccessMenu(MenuAccess menuAccess) : base(menuAccess)
		{}

		private ElementsListFormExample CreateForm()
		{
			AccessListFormPresenter presenter = new AccessListFormPresenter();
			ElementsListFormExample form = presenter.Form as ElementsListFormExample;

			ListFormAccessControl(form);
			form.ViewButton.Visible = false;
			return form;
		}

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			ElementsListFormExample form = CreateForm();

			form.Show();
			form.Presenter.Loading();
		}

		public override void OpenMenuContent(object obj, EventArgs args)
		{
			base.OpenMenuContent(obj, args);
			ElementsListFormExample form = CreateForm();

			MessageCaller.LoadFormContent(form);
			form.Presenter.Loading();
		}
	}
}
