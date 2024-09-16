using MenuInterface;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AboutProgram
{

	public class HelpMenu : MenuBase
	{
		private string _message = "Все пункты меню отображаются сверху главного окна.\r\nВ зависимости от доступа, открывается возможность видеть таблицы в выпадающем списке меню, добавлять, редактировать и удалять данные из таблиц базы. \r\nРедактирование таблиц производиться по одной записи за раз.\r\nВ разделе “Документы” можно посмотреть и выписки и экспортировать их в Microsoft Excel.\r\nВ разделе настройки можно сменить логин и пароль.\r\nДля получения прав доступа обратитесь к администратору.\r\n";

		public HelpMenu(MenuAccess menuAccess) : base(menuAccess)
		{
			Title = "Справка";
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
