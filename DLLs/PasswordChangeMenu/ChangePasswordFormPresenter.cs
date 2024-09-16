using DatabaseManager;
using PresenterContext;
using System;
using System.Windows.Forms;

namespace PasswordChangeMenu
{
	public class ChangePasswordFormPresenter : PresenterBase
	{
		private SystemDataManager _systemDataManager;
		private int _currentUserId;

		public ChangePasswordFormPresenter() : base(new ElementFormExample())
		{
			_systemDataManager = new SystemDataManager();
			var currentUserData = _systemDataManager.GetUserInfo(MessageCaller.CurrentUser.UserId);
			_currentUserId = MessageCaller.CurrentUser.UserId;

			var loginText = CreateTextBox("Логин");
			loginText.Text = currentUserData.Item1;
			var passwordText = CreateTextBox("Пароль");
			passwordText.Text = currentUserData.Item2;

			CreateButton("Изменить данные", ChangePassword);
		}

		private void ChangePassword(object obj, EventArgs args)
		{
			try
			{
				_systemDataManager.UserPasswordChange(_currentUserId, TextBoxes["Логин"].Text, TextBoxes["Пароль"].Text);
				MessageCaller.CallInfomationMessage("Пароль успешно изменен");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}
	}
}
