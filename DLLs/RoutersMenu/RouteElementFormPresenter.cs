using DatabaseManager;
using Menus;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RoutersMenu
{
	public class RouteElementFormPresenter : ElementFormPresenter
	{
		private RegistersDataManager _dataManager;
		private List<(int, string)> _ports;
		private (int, string) _selectedPort;

		private VesselData _vesselData = new VesselData() { VesselId = 0 };

		public RouteElementFormPresenter(int elementId, ElementFormType type) : base(elementId, type)
		{
			_dataManager = new RegistersDataManager();
			CreateTextBox("Название");

			var table = CreateDataGridView(200, "Ports");
			TableInit();
			table.SelectionChanged += new EventHandler(SelectPort);

			CreateButton("Добавить порт", AddPort);
			CreateButton("Удалить порт", DeletePort);

			CreateComboBox("Корабль", SelectVessel);

			if(type == ElementFormType.CREATE)
				CreateButton("Добавить", CreateElementEvent);
			else if (type == ElementFormType.UPDATE)
				CreateButton("Редактировать", EditElementEvent);

			_ports = new List<(int, string)>(0);
			_selectedPort = (0, "");
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);

			if (Type == ElementFormType.UPDATE)
				LoadData();
		}

		private void LoadData()
		{
			var currentElement = _dataManager.GetRoutsList().Find(r => r.Item1 == CurrentElementId);

			TextBoxes["Название"].Text = currentElement.Item2;
			_ports = _dataManager.GetRoutPorts(currentElement.Item1);

			var vesselInfo = _dataManager.GetVesselsAndRout().Find(vr => vr.Item1 == currentElement.Item1);

			if (vesselInfo.Item3 != 0)
			{
				_vesselData = new VesselData() { VesselId = vesselInfo.Item3, Name = vesselInfo.Item4 };
				ComboBoxes["Корабль"].Text = _vesselData.Name;
			}
			
			TableUpdate();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Port", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables["Ports"]);

			foreach (DataGridViewColumn column in Tables["Ports"].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		private void SelectVessel(object obj, EventArgs args)
		{
			var vesselChoice = new VesselsChoiseFormPresenter();
			if (vesselChoice.ShowDialog() == DialogResult.OK)
			{
				_vesselData = vesselChoice.SelectedVessel;
				ComboBoxes["Корабль"].Items.Clear();
				ComboBoxes["Корабль"].Items.Add(_vesselData.Name);
				ComboBoxes["Корабль"].SelectedIndex = 0;
			}
			TableUpdate();
		}

		private void SelectPort(object obj, EventArgs args)
		{
			if (Tables["Ports"].SelectedRows.Count == 0)
			{
				_selectedPort = (0, "");
				return;
			}

			var row = Tables["Ports"].SelectedRows[0];
			_selectedPort = (int.Parse(row.Cells[0].Value.ToString()), row.Cells[1].Value.ToString());

		}

		private void TableUpdate()
		{
			Tables["Ports"].Rows.Clear();

			foreach (var port in _ports)
				Tables["Ports"].Rows.Add(port.Item1, port.Item2);

		}

		private void AddPort(object obj, EventArgs args)
		{
			var portChoice = new PortChoiceFormPresenter();
			if (portChoice.ShowDialog() == DialogResult.OK)
			{
				var port = portChoice.SelectedPort;
				_ports.Add(port);
			}
			TableUpdate();
		}

		private void DeletePort(object obj, EventArgs args)
		{
            if (Tables["Ports"].SelectedRows.Count == 0)
            {
                MessageCaller.CallErrorMessage("Порт не выбран");
                return;
            }

            _ports.Remove(_selectedPort);
            TableUpdate();
        }

		public override void CreateElement()
		{
			try
			{
				base.CreateElement();
				_dataManager.AddRout(TextBoxes["Название"].Text, _ports.Select(p => p.Item1).ToList(), _vesselData);
				MessageCaller.CallInfomationMessage("Маршрут успешно добавлен");

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
				_dataManager.EditRout(TextBoxes["Название"].Text, CurrentElementId, _ports.Select(p => p.Item1).ToList(), _vesselData);

				MessageCaller.CallInfomationMessage("Маршрут успешно изменен");
				Form.Close();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
			
		}

	}
}
