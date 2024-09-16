using DatabaseManager;
using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShippingCompanyManager
{
	public class MenuLoader
	{
		public MenuLoader()
		{ }

		private ToolStripMenuItem CreateToolStripMenu(MenuData menuData)
		{
			ToolStripMenuItem menuItem = new ToolStripMenuItem();
			menuItem.Text = menuData.Name;

			if (menuData.DLL != "")
				menuItem = ToolStripMenuInit(menuItem, menuData);

			return menuItem;
		}

		private ToolStripMenuItem ToolStripMenuInit(ToolStripMenuItem menuItem, MenuData menuData)
		{
			MenuAccess menuAccess = new MenuAccess()
			{
				Read = menuData.Read,
				Write = menuData.Write,
				Edit = menuData.Edit,
				Delete = menuData.Delete,
			};

			var menu = FileLoader.LoadFormFromDLL(menuData.DLL, menuData.Key, menuAccess);
			menuItem.Click += new EventHandler(menu.OpenMenu);

			if (!menuData.Read)
				menuItem.Enabled = false;

			return menuItem;
		}

		public List<ToolStripMenuItem> MenuCreate(List<MenuData> menus)
		{
			List<ToolStripMenuItem> menu = new List<ToolStripMenuItem>();
			Dictionary<int, ToolStripMenuItem> menuTools = new Dictionary<int, ToolStripMenuItem>();

			List <ToolStripMenuItem> parentsMenu = new List<ToolStripMenuItem>();

			foreach (MenuData menuData in menus.Where(m => m.MenuParentId == 0))
			{
				try
				{
					ToolStripMenuItem menuItem = CreateToolStripMenu(menuData);
					menu.Add(menuItem);
					parentsMenu.Add(menuItem);
					menuTools.Add(menuData.MenuId, menuItem);
				}
				catch (Exception ex)
				{
					MessageCaller.CallErrorMessage(ex.Message);
				}
			}

			var otherMenu = menus.OrderBy(m => m.MenuParentId);

			foreach (MenuData menuData in menus.Where(m => m.MenuParentId != 0).OrderBy(m => m.MenuParentId))
			{
				try
				{
					ToolStripMenuItem menuItem = CreateToolStripMenu(menuData);
					menu.Add(menuItem);
					menuTools[menuData.MenuParentId].DropDownItems.Add(menuItem);
					menuTools.Add(menuData.MenuId, menuItem);
				}
				catch (Exception ex)
				{
					MessageCaller.CallErrorMessage(ex.Message);
				}
			}

			return parentsMenu;
		}
	}
}
