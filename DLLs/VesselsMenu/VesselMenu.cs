﻿using MenuInterface;
using PresenterContext;
using System;

namespace VesselsMenu
{
	public class VesselMenu : MenuBase
	{
		public VesselMenu(MenuAccess menuAccess) : base(menuAccess)
		{ }

		private ElementsListFormExample CreateForm()
		{
			VesselsListFormPresenter listFormPresenter = new VesselsListFormPresenter();
			ElementsListFormExample form = listFormPresenter.Form as ElementsListFormExample;

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
