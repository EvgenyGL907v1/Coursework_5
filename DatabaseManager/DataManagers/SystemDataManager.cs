using System;
using System.Collections.Generic;

namespace DatabaseManager
{
	public class SystemDataManager : DataManager, IUserData, IMenuData
	{

		#region USERS

		public void UserRegistration(string login, string password)
		{
			password = DataEncryption.Encrypt(password);

			string checkRequest = $"SELECT * FROM Users WHERE Login = '{login}'";
			var checkResult = Database.GetRequest(checkRequest);

			if (checkResult.Rows.Count > 0)
				throw new Exception("Пользователь с таким логином уже существует");

			string command = $"INSERT INTO Users (Login, Password) VALUES ('{login}', '{password}');";
			Database.ExecuteCommand(command);
		}

		public int UserAuthorization(string login, string password)
		{
			password = DataEncryption.Encrypt(password);

			string request = $"SELECT * FROM Users WHERE Login = '{login}' AND Password = '{password}'";
			var result = Database.GetRequest(request);

			if (result.Rows.Count == 0)
				throw new Exception("Неверный логин или пароль");

			var res = result.Rows[0]["UserId"].ToString();
			return int.Parse(res);
		}

		public void UserPasswordChange(int userId, string login, string password)
		{
			string command = $"UPDATE Users SET Login = '{login}', Password = '{DataEncryption.Encrypt(password)}' WHERE UserId = {userId};";
			Database.ExecuteCommand(command);
		}

		public List<UserData> GetUsersList()
		{
			string request = "SELECT * FROM Users;";
			var result = Database.GetRequest(request);


			List<UserData> users = new List<UserData>();

			for (int i = 0; i < result.Rows.Count; i++)
			{
				TableDataManager tableData = new TableDataManager(result);
				tableData.SetRow(i);

				UserData user = new UserData()
				{ 
					UserId = tableData.GetValue<int>("UserId"),
					Login = tableData.GetValue<string>("Login"),
				};

				users.Add(user);
			}

			return users;
		}

		public (string, string) GetUserInfo(int userId)
		{
			string request = $"SELECT * FROM Users WHERE UserId = {userId};";
			var result = Database.GetRequest(request);

			TableDataManager tableData = new TableDataManager(result);
			return (tableData.GetValue<string>("Login"), DataEncryption.Decrypt(tableData.GetValue<string>("Password")));
		}

		#endregion

		#region MENU_AND_ACCESS

		public List<MenuData> GetMenuList()
		{
			string request = "SELECT * FROM DLLMenuData WHERE IsDefault = 0;";
			var result = Database.GetRequest(request);

			List<MenuData> menuList = new List<MenuData>(0);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				TableDataManager tableData = new TableDataManager(result);
				tableData.SetRow(i);

				MenuData menuData = new MenuData()
				{
					MenuId = tableData.GetValue<int>("MenuId"),
					Name = tableData.GetValue<string>("Name"),
					DLL = tableData.GetValue<string>("DLL"),
					Key = tableData.GetValue<string>("Key"),
					MenuParentId= tableData.GetValue<int>("ParentId"),
					Default = tableData.GetValue<int>("IsDefault") > 0,
				};

				menuList.Add(menuData);
			}

			return menuList;
		}

		public List<MenuData> GetUserMenuList(int userId)
		{
			string request = $"SELECT m.MenuId,m.Name,m.IsDefault, m.DLL,m.Key,m.ParentId,a.Read,a.Write,a.Edit,a.\"Delete\" FROM DLLMenuData m LEFT JOIN MenuAccess a ON a.MenuId = m.MenuId WHERE a.UserId = {userId} OR m.IsDefault = 1 OR m.DLL = '';";
			var result = Database.GetRequest(request);

			List<MenuData> menuList = new List<MenuData>(0);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

				bool isDefault = tableData.GetValue<int>("IsDefault") > 0;

				MenuData menuData = new MenuData()
				{
					MenuId = tableData.GetValue<int>("MenuId"),
					Name = tableData.GetValue<string>("Name"),
					DLL = tableData.GetValue<string>("DLL"),
					Key = tableData.GetValue<string>("Key"),
					MenuParentId = tableData.GetValue<int>("ParentId"),

					Read = tableData.GetValue<int>("Read") > 0 || isDefault,
					Write = tableData.GetValue<int>("Write") > 0 || isDefault,
					Edit = tableData.GetValue<int>("Edit") > 0 || isDefault,
					Delete = tableData.GetValue<int>("Delete") > 0 || isDefault

				};

				menuList.Add(menuData);
			}

			return menuList;
		}

		public void AddMenuAccess(MenuData menuData)
		{
			string checkRequest = $"SELECT * FROM MenuAccess WHERE UserId = {menuData.UserId} AND MenuId = {menuData.MenuId};";
			var result = Database.GetRequest(checkRequest);

			if (result.Rows.Count > 0)
				throw new Exception("Данный доступ уже открыть");


			string command = $"INSERT INTO MenuAccess (UserId, MenuId, Read, Write, Edit, [Delete]) VALUES ({menuData.UserId}, {menuData.MenuId}, {menuData.Read}, {menuData.Write}, {menuData.Edit}, {menuData.Delete});";
			Database.ExecuteCommand(command);
		}

		public void EditMenuAccess(MenuData menuData)
		{
			string checkRequest = $"SELECT * FROM MenuAccess WHERE UserId = {menuData.UserId} AND MenuId = {menuData.MenuId} And Id != {menuData.MenuId};";
			var result = Database.GetRequest(checkRequest);

			if (result.Rows.Count > 0)
				throw new Exception("Данный доступ уже открыть");

			string command = $"UPDATE MenuAccess " +
				$"SET UserId = {menuData.UserId}, " +
				$"MenuId = {menuData.MenuId}, " +
				$"Read = {(menuData.Read ? 1 : 0)}," +
				$"Write = {(menuData.Write ? 1 : 0)}," +
				$"Edit = {(menuData.Edit ? 1 : 0)}," +
				$"[Delete] = {(menuData.Delete ? 1 : 0)} " +
				$"WHERE Id = {menuData.AccessId};";

			Database.ExecuteCommand(command);
		}

		public void DeleteMenuAccess(int accessId)
		{
			string command = $"DELETE FROM MenuAccess WHERE Id = {accessId};";
			Database.ExecuteCommand(command);
		}

		public List<MenuData> GetAccessList()
		{
			string request = $"SELECT * FROM MenuAccess access JOIN Users users ON access.UserId = users.UserId JOIN DLLMenuData menu ON menu.MenuId = access.MenuId";
			var result = Database.GetRequest(request);

			List<MenuData> menuList = new List<MenuData>(0);

			TableDataManager tableData = new TableDataManager(result);

			for (int i = 0; i < result.Rows.Count; i++)
			{
				tableData.SetRow(i);

				MenuData menuData = new MenuData()
				{
					AccessId = tableData.GetValue<int>("Id"),
					MenuId = tableData.GetValue<int>("MenuId"),
					Name = tableData.GetValue<string>("Name"),
					DLL = tableData.GetValue<string>("DLL"),
					Key = tableData.GetValue<string>("Key"),
					MenuParentId = tableData.GetValue<int>("ParentId"),

					UserId = tableData.GetValue<int>("UserId"),
					UserLogin = tableData.GetValue<string>("Login"),

					Read = tableData.GetValue<int>("Read") > 0,
					Write = tableData.GetValue<int>("Write") > 0,
					Edit = tableData.GetValue<int>("Edit") > 0,
					Delete = tableData.GetValue<int>("Delete") > 0

				};

				menuList.Add(menuData);
			}

			return menuList;


		}

		#endregion

	}
}
