using DatabaseManager;
using PresenterContext;
using System;
using System.Windows.Forms;

namespace ShippingCompanyManager
{
	internal static class Program
	{
		private static UserData s_currentUser { get; set; }

		private static void InformationMessage(string text) => MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		private static void ErrorMessage(string text) => MessageBox.Show(text, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

		private static void OpenMainForm()
		{
			MainForm mainForm = new MainForm(s_currentUser);
			mainForm.Show();
		}
		private static void OpenLoginForm()
		{
			LoginForm loginForm = new LoginForm();
			loginForm.Show();
		}
		private static void OnUserLogin(UserData user)
		{
			s_currentUser = user;
			OpenMainForm();
		}

		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MessageCaller.OnInfomationMessageCall += InformationMessage;
			MessageCaller.OnErrorMessageCall += ErrorMessage;

			MessageCaller.OnUserLogin += OnUserLogin;
			MessageCaller.OnUserLogOut += OpenLoginForm;

			Application.Run(new LoginForm());
		}
	}
}
