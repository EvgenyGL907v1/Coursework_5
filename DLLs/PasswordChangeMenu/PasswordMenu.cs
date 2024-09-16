using MenuInterface;
using PresenterContext;
using System;

namespace PasswordChangeMenu
{
	public class PasswordMenu : MenuBase
	{
		public PasswordMenu(MenuAccess menuAccess) : base(menuAccess)
		{ }

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			ChangePasswordFormPresenter presenter = new ChangePasswordFormPresenter();
			presenter.ShowDialog();
		}
	}
}
