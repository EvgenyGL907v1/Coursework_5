using MenuInterface;
using PresenterContext;
using System;

namespace AboutProgram
{
	public class AboutProgramMenu : MenuBase
	{
		private string _message = "Данная программа является фрагментом информационной системы судоходной компании.\r\nАвтор: Глазырин Евгений Алексеевич, 2023 г.\r\nЯзык и технология пользовательского интерфейса: C#, Windows Forms.\r\nСреда разработки: Microsoft Visual Studio 2022.\r\nРеляционная система управления базами данных – SQLite.\r\n";

		public AboutProgramMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			Title = "О программе";
		}

		public override void OpenMenu(object obj, EventArgs args)
		{
			base.OpenMenu(obj, args);
			MessageFormExample messageForm = new MessageFormExample(_message);
			MessageFormPresenter messageFormPresenter = new MessageFormPresenter(_message);

			messageForm.Presenter = messageFormPresenter;
			messageForm.Text = Title;
			messageForm.Show();
		}

		public override void OpenMenuContent(object obj, EventArgs args)
		{
			base.OpenMenuContent(obj, args);
			MessageFormExample messageForm = new MessageFormExample(_message);
			MessageFormPresenter messageFormPresenter = new MessageFormPresenter(_message);
			messageForm.Presenter = messageFormPresenter;
			messageForm.Text = Title;

			MessageCaller.LoadFormContent(messageForm);
		}
	}
}
