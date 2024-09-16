using ClientsMenu;
using DatabaseManager;
using Menus;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace ConsignmentsMenu
{
	public class ConfignmentElementFormPresenter : ElementFormPresenter
	{
		private RegistersDataManager _dataManager;
		private ReferencesManager _referencesManager;

		private List<CargoInfo> _cargoList;
		private CargoInfo _selectedCargo;

		private (int, string) _sender;
		private (int, string) _recipient;

		private (int, string) _senderPort;
		private (int, string) _recipientPort;


        public ConfignmentElementFormPresenter(int elementId, ElementFormType type) : base(elementId, type)
		{
			_dataManager = new RegistersDataManager();
			_referencesManager = new ReferencesManager();

			var table = CreateDataGridView(200, "Cargo");
			TableInit();
			table.SelectionChanged += new EventHandler(SelectPort);

			CreateButton("Добавить груз", AddCargo);
			CreateButton("Удалить груз", DeleteCargo);

			CreateDateTimePicker("Отправка");
			CreateComboBox("Порт отп.", SelectSenderPort);
			CreateComboBox("Отправитель", SelectSender);

			CreateDateTimePicker("Прибытие");
			CreateComboBox("Порт пр.", SelectRecipientPort);
			CreateComboBox("Получатель", SelectRecipient);

			CreateTextBox("Номер");

			if (type == ElementFormType.CREATE)
				CreateButton("Добавить", CreateElementEvent);
			else if (type == ElementFormType.UPDATE)
				CreateButton("Редактировать", EditElementEvent);

			_cargoList = new List<CargoInfo>(0);
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			if(Type == ElementFormType.UPDATE)
				LoadData();
		}

		private void SelectSender(object obj, EventArgs args)
		{
			ClientChoiceFormPresenter clientChoice = new ClientChoiceFormPresenter();
			if (clientChoice.ShowDialog() == DialogResult.OK)
			{
				_sender = clientChoice.SelectedClient;
				var menuBox = ComboBoxes["Отправитель"];

				menuBox.Items.Clear();
				menuBox.Items.Add(_sender.Item2);
				menuBox.SelectedIndex = 0;
			}
		}
		private void SelectRecipient(object obj, EventArgs args)
		{
			ClientChoiceFormPresenter clientChoice = new ClientChoiceFormPresenter();
			if (clientChoice.ShowDialog() == DialogResult.OK)
			{
				_recipient = clientChoice.SelectedClient;
				var menuBox = ComboBoxes["Получатель"];

				menuBox.Items.Clear();
				menuBox.Items.Add(_recipient.Item2);
				menuBox.SelectedIndex = 0;
			}
		}
		private void SelectSenderPort(object obj, EventArgs args)
		{
			PortChoiceFormPresenter portChoice = new PortChoiceFormPresenter();
			if (portChoice.ShowDialog() == DialogResult.OK)
			{
				_senderPort = portChoice.SelectedPort;
				var menuBox = ComboBoxes["Порт отп."];

				menuBox.Items.Clear();
				menuBox.Items.Add(_senderPort.Item2);
				menuBox.SelectedIndex = 0;
			}
		}
		private void SelectRecipientPort(object obj, EventArgs args)
		{
			PortChoiceFormPresenter portChoice = new PortChoiceFormPresenter();
			if (portChoice.ShowDialog() == DialogResult.OK)
			{
				_recipientPort = portChoice.SelectedPort;
				var menuBox = ComboBoxes["Порт пр."];

				menuBox.Items.Clear();
				menuBox.Items.Add(_recipientPort.Item2);
				menuBox.SelectedIndex = 0;
			}
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Cargo", "Груз", TableColumnType.TEXT),
				new TableColumn("Count", "Ед. изм", TableColumnType.TEXT),
				new TableColumn("Count", "Количество", TableColumnType.TEXT),
				new TableColumn("SCount", "Страховка", TableColumnType.TEXT),
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables["Cargo"]);

			foreach (DataGridViewColumn column in Tables["Cargo"].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}
		private void TableUpdate()
		{
			Tables["Cargo"].Rows.Clear();

			foreach (var cargo in _cargoList)
				Tables["Cargo"].Rows.Add(cargo.CargoId, cargo.CargoName, cargo.UnitName,cargo.Count, cargo.InsuredCount);

		}

		private void AddCargo(object obj, EventArgs args)
		{
			CargoInfoCreatorFormPresenter cargoCreate = new CargoInfoCreatorFormPresenter();
			if (cargoCreate.ShowDialog() == DialogResult.OK)
			{
				var cargo = cargoCreate.CargoInfo;
				_cargoList.Add(cargo);
			}
			TableUpdate();
		}
		private void DeleteCargo(object obj, EventArgs args)
		{
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
                if (Tables["Cargo"].SelectedRows.Count == 0)
                {
                    MessageCaller.CallErrorMessage("Груз не выбран");
                    return;
                }

                _cargoList.Remove(_selectedCargo);
                TableUpdate();
            }    
		}

		private void SelectPort(object obj, EventArgs args)
		{
			if (Tables["Cargo"].SelectedRows.Count == 0)
			{
				_selectedCargo = new CargoInfo();
				return;
			}

			_selectedCargo = _cargoList[Tables["Cargo"].SelectedRows[0].Index];

		}

		private void LoadData()
		{
			try 
			{
                var config = _dataManager.GetCargosList().Find(c => c.ConsignmentId == CurrentElementId);

                var portsList = _referencesManager.GetPortsList();
                var clientsList = _dataManager.GetClientsList();

                _senderPort = portsList.Find(c => c.Item1 == config.SendPortId);
                ComboBoxes["Порт отп."].Items.Add(_senderPort.Item2);
                ComboBoxes["Порт отп."].SelectedIndex = 0;

                _recipientPort = portsList.Find(c => c.Item1 == config.ReceivPortId);
                ComboBoxes["Порт пр."].Items.Add(_recipientPort.Item2);
                ComboBoxes["Порт пр."].SelectedIndex = 0;

                var sender = (clientsList.Find(c => c.ClientId == config.SendClientId));
                ComboBoxes["Отправитель"].Items.Add(sender.Name);
                ComboBoxes["Отправитель"].SelectedIndex = 0;
                _sender = (sender.ClientId, sender.Name);

                var recip = clientsList.Find(c => c.ClientId == config.ReceivClientId);
                ComboBoxes["Получатель"].Items.Add(recip.Name);
                ComboBoxes["Получатель"].SelectedIndex = 0;
                _recipient = (recip.ClientId, recip.Name);

                DateTimePickers["Отправка"].Value = DateTime.Parse(config.DepartDate);
                DateTimePickers["Прибытие"].Value = DateTime.Parse(config.ArrivalDate);

                TextBoxes["Номер"].Text = config.CustomNumber;

                _cargoList = _dataManager.GetCargoInfos(config.ConsignmentId);
                TableUpdate();
            }
			catch { }

		}

		public override void CreateElement()
		{
			try
			{
				base.CreateElement();

				if (Tables["Cargo"].RowCount == 0)
					throw new Exception("Нет грузов");

				List<CargoInfo> cargos = new List<CargoInfo>();

				for (int i = 0; i < Tables["Cargo"].Rows.Count; i++)
				{
					var row = Tables["Cargo"].Rows[i];
					var units = new ReferencesManager().GetUnitsList();

                    cargos.Add(new CargoInfo()
					{ 
						CargoId = int.Parse(row.Cells[0].Value.ToString()),
						CargoName = row.Cells[1].Value.ToString(),
						UnitId = units.Find(u => u.Item2 == row.Cells[2].Value.ToString()).Item1,
						Count = int.Parse(row.Cells[3].Value.ToString()),
						InsuredCount = int.Parse(row.Cells[4].Value.ToString()),
                    });
				}

				if (ComboBoxes["Порт отп."].SelectedIndex == -1)
					throw new Exception("Порт отправки не выбран");

				if(ComboBoxes["Порт пр."].SelectedIndex == -1)
					throw new Exception("Порт прибытия не выбран");

				if (ComboBoxes["Отправитель"].SelectedIndex == -1)
					throw new Exception("Отправитель не выбран");

				if(ComboBoxes["Получатель"].SelectedIndex == -1)
					throw new Exception("Получатель не выбран");

				CargoConsignment cargoConsignment = new CargoConsignment()
				{ 
					SendPortId = _senderPort.Item1,
					ReceivPortId = _recipientPort.Item1,
					CustomNumber = TextBoxes["Номер"].Text,

					DepartDate = DateTimePickers["Отправка"].Value.ToString("yyyy-MM-dd"),
					ArrivalDate = DateTimePickers["Прибытие"].Value.ToString("yyyy-MM-dd"),

					SendClientId = _sender.Item1,
					ReceivClientId = _recipient.Item1,
				};

				_dataManager.AddCargoConsignment(cargoConsignment, cargos);
				MessageCaller.CallInfomationMessage("Информации о партии успешно доабвлена");
				Form.Close();
                TableUpdate();
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

                var config = _dataManager.GetCargosList().Find(c => c.ConsignmentId == CurrentElementId);

                if (Tables["Cargo"].RowCount == 0)
					throw new Exception("Нет грузов");

				List<CargoInfo> cargos = new List<CargoInfo>();

                for (int i = 0; i < Tables["Cargo"].Rows.Count; i++)
				{
					var row = Tables["Cargo"].Rows[i];
                    var units = new ReferencesManager().GetUnitsList();

                    cargos.Add(new CargoInfo()
                    {
                        CargoId = int.Parse(row.Cells[0].Value.ToString()),
                        CargoName = row.Cells[1].Value.ToString(),
                        UnitId = units.Find(u => u.Item2 == row.Cells[2].Value.ToString()).Item1,
                        Count = int.Parse(row.Cells[3].Value.ToString()),
                        InsuredCount = int.Parse(row.Cells[4].Value.ToString()),
                    });
                }

				if (ComboBoxes["Порт отп."].SelectedIndex == -1)
					throw new Exception("Порт отправки не выбран");

				if (ComboBoxes["Порт пр."].SelectedIndex == -1)
					throw new Exception("Порт прибытия не выбран");

				if (ComboBoxes["Отправитель"].SelectedIndex == -1)
					throw new Exception("Отправитель не выбран");

				if (ComboBoxes["Получатель"].SelectedIndex == -1)
					throw new Exception("Получатель не выбран");

				CargoConsignment cargoConsignment = new CargoConsignment()
				{
					SendPortId = _senderPort.Item1,
					ReceivPortId = _recipientPort.Item1,
					CustomNumber = TextBoxes["Номер"].Text,

					DepartDate = DateTimePickers["Отправка"].Value.ToString("yyyy-MM-dd"),
					ArrivalDate = DateTimePickers["Прибытие"].Value.ToString("yyyy-MM-dd"),

					SendClientId = _sender.Item1,
					ReceivClientId = _recipient.Item1,

					ConsignmentId = config.ConsignmentId,
                };

				_dataManager.EditCargoConsignment(cargoConsignment, cargos);
				MessageCaller.CallInfomationMessage("Информации о партии успешно изменена");
				Form.Close();
                TableUpdate();
            }
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

	}
}
