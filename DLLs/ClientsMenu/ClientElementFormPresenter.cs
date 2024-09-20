using DatabaseManager;
using Menus;
using PresenterContext;
using System;
using System.Windows.Forms;
using System.Linq;

namespace ClientsMenu
{
    public class ClientElementFormPresenter : ElementFormPresenter
	{
		private int _selectedBankId = -1;
		private RegistersDataManager _registersDataManager;

		public ClientElementFormPresenter(int elementId, ElementFormType type) : base(elementId, type)
		{
			CreateTextBox("Название");
			var box = CreateComboBox("Банк", new EventHandler(BankChoise));

			CreateTextBox("ИНН");

			if (type == ElementFormType.CREATE)
				CreateButton("Добавить", CreateElementEvent);
			else if (type == ElementFormType.UPDATE)
				CreateButton("Редактировать", EditElementEvent);

			_registersDataManager = new RegistersDataManager();
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			if (Type == ElementFormType.UPDATE)
				LoadData();
		}

		private void LoadData()
		{
			var client = _registersDataManager.GetClientsList().Find(c => c.ClientId == CurrentElementId);

			ReferencesManager referencesManager = new ReferencesManager();
			_selectedBankId = client.BankId;

			ComboBoxes["Банк"].Items.Add(referencesManager.GetBanksList().Find(b => b.Item1 == client.BankId).Item2);
			ComboBoxes["Банк"].SelectedIndex = 0;

            TextBoxes["Название"].Text = client.Name;
            TextBoxes["ИНН"].Text = client.TaxNumber;
		}

		private void BankChoise(object obj, EventArgs args)
		{
			BankChoiceFormPresenter bankChoice = new BankChoiceFormPresenter();
			if (bankChoice.ShowDialog() == DialogResult.OK)
			{
				var user = bankChoice.SelectedBank.Item2;
				_selectedBankId = bankChoice.SelectedBank.Item1;

				var userBox = ComboBoxes["Банк"];
				userBox.Items.Clear();
				userBox.Items.Add(user);
				userBox.SelectedIndex = 0;
			}
		}

		public override void CreateElement()
		{
			try
			{
				base.CreateElement();

				if (_selectedBankId == -1)
					throw new Exception("Не выбран банк");

				ClientData clientData = new ClientData()
				{
					Name = TextBoxes["Название"].Text,
					TaxNumber = TextBoxes["ИНН"].Text,
					BankId = _selectedBankId,
				};

                if (clientData.Name == "")
                    throw new Exception("Имя не может быть пустым");

                if (clientData.BankId <= 0)
                    throw new Exception("Банк не выбран");

                if (clientData.TaxNumber.Length != 10 || !clientData.TaxNumber.All(char.IsDigit))
                    throw new Exception("ИНН юридического лица должен состоять из 10 цифр");

                _registersDataManager.AddClient(clientData);
				MessageCaller.CallInfomationMessage("Добавлен новый клиент");
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

				if (_selectedBankId == -1)
					throw new Exception("Не выбран банк");

				ClientData clientData = new ClientData()
				{
					ClientId = CurrentElementId,
					Name = TextBoxes["Название"].Text,
					TaxNumber = TextBoxes["ИНН"].Text,
					BankId = _selectedBankId,
				};

                if (clientData.Name == "")
                    throw new Exception("Имя не может быть пустым");

                if (clientData.BankId <= 0)
                    throw new Exception("Банк не выбран");

                if (clientData.TaxNumber.Length != 10 || !clientData.TaxNumber.All(char.IsDigit))
                    throw new Exception("ИНН юридического лица должен состоять из 10 цифр");

                _registersDataManager.EditClient(clientData);
                MessageCaller.CallInfomationMessage("Информация изменена");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

	}
}
