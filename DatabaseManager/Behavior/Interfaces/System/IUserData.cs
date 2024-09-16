
using System.Collections.Generic;

namespace DatabaseManager
{
	public struct UserData
	{ 
		public int UserId { get; set; }
		public string Login { get; set; }
	}

	public interface IUserData
	{
		/// <summary> Регистрация пользователя </summary>
		void UserRegistration(string login, string password);
		/// <summary> Авторизация пользователя </summary>
		int UserAuthorization(string login, string password);
		/// <summary> Смена пароля пользователя </summary>
		void UserPasswordChange(int userId, string login, string password);
		List<UserData> GetUsersList();
		(string, string) GetUserInfo(int userId);
	}
}
