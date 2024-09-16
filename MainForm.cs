using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShippingCompanyManager
{
	public partial class MainForm : Form
	{
		public static MainForm CurrentMainForm { get; set; }

		public MainForm(UserData userData)
		{
			InitializeComponent();

			MenuLoader menuLoader = new MenuLoader();

			SystemDataManager dataManager = new SystemDataManager();
			var menuList = dataManager.GetUserMenuList(userData.UserId);
			var menu = menuLoader.MenuCreate(menuList);

			foreach (var item in menu)
				menuStrip1.Items.Add(item);

			CurrentMainForm = this;
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			MessageCaller.UserLogOut();
		}

	}
}
