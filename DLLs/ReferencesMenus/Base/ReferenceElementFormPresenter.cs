using PresenterContext;
using System;

namespace ReferencesMenus
{
	public class ReferenceElementFormPresenter : ElementFormPresenter
	{
		private AddElement _addEvent;
		private EditElement _editEvent;

		public ReferenceElementFormPresenter(AddElement addEvent, string title) : base(0, ElementFormType.CREATE)
		{
			_addEvent = addEvent;
			Form.Text = title;

			CreateTextBox("Название");
			CreateButton("Добавить", CreateElementEvent);
		}

		public ReferenceElementFormPresenter(EditElement editEvent, int elementId, string elementName ,string title) : base(elementId, ElementFormType.UPDATE)
		{
			_editEvent = editEvent;
			Form.Text = title;

			var textBox = CreateTextBox("Название");
			textBox.Text = elementName;
			CreateButton("Изменить", EditElementEvent);
		}

		public override void CreateElement()//!!
		{
			try
			{
				base.CreateElement();

				string enterText = TextBoxes["Название"].Text;
				if(enterText == "")
                    throw new Exception("Название не может быть пустым");

                _addEvent.Invoke(enterText);
				MessageCaller.CallInfomationMessage("Успешно добавлено");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		public override void EditElement()
		{
			try
			{
				base.EditElement();

                string enterText = TextBoxes["Название"].Text;
                if (enterText == "")
                    throw new Exception("Название не может быть пустым");

                _editEvent.Invoke(CurrentElementId, enterText);
				MessageCaller.CallInfomationMessage("Редактирование выполнено успешно");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}
	}
}
