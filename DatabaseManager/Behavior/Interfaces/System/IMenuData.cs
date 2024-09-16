using System.Collections.Generic;

namespace DatabaseManager
{
	/// <summary> Информация о меню и доступе к нему пользователя </summary>
	public struct MenuData
	{ 
		public int AccessId { get; set; }

		public int MenuId { get; set; }
		public string Name { get; set; }
		public string DLL { get; set; }
		public string Key { get; set; }
		public int MenuParentId { get; set; }

		public int UserId { get; set; }
		public string UserLogin { get; set; }

		public bool Read { get; set; }
		public bool Write { get; set; }
		public bool Edit { get; set; }
		public bool Delete { get; set; }

		public bool Default { get; set; }

	}

	public interface IMenuData
	{
		List<MenuData> GetMenuList();
		List<MenuData> GetUserMenuList(int userId);

		List<MenuData> GetAccessList();

		void AddMenuAccess(MenuData menuData);
		void EditMenuAccess(MenuData menuData);
		void DeleteMenuAccess(int accessId);
	}
}
