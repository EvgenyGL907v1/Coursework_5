using DatabaseManager;
using System;
using System.Windows.Forms;

namespace PresenterContext
{
	public static class MessageCaller
	{
		public static UserData CurrentUser { get; private set; }

		public static Action<string> OnErrorMessageCall { get; set; }
		public static Action<string> OnInfomationMessageCall { get; set; }
		public static Action<string> OnQuestionMessageCall { get; set; }

		public static Action<UserData> OnUserLogin { get; set; }
		public static Action OnUserLogOut { get; set; }

		public static Action<Form> OnFormContentShow { get; set; }

		public static void CallErrorMessage(string text) => OnErrorMessageCall?.Invoke(text);
		public static void CallInfomationMessage(string text) => OnInfomationMessageCall?.Invoke(text);
		public static void CallQuestionMessage(string text) => OnQuestionMessageCall?.Invoke(text);

		public static void UserLogin(UserData user)
		{
			CurrentUser = user;
			OnUserLogin?.Invoke(user);
		}
		public static void UserLogOut() => OnUserLogOut?.Invoke();

		public static void LoadFormContent(Form formContent) => OnFormContentShow?.Invoke(formContent);
	}
}
