using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClientsMenu
{
	public class ClientChoiceFormPresenter : ChoiceFormPresenter
	{
		private RegistersDataManager _registeredDataManager;
		private List<ClientData> _clients;

		public (int, string) SelectedClient { get; set; }

		public ClientChoiceFormPresenter()
		{
			_registeredDataManager = new RegistersDataManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Client", "Название", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);

			foreach (DataGridViewColumn column in Tables[MainTableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{
			base.SelectItem(obj, eventArgs);

			var selectedData = _clients.Find(c => c.ClientId == SelectedId);
			SelectedClient = (selectedData.ClientId, selectedData.Name);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{
			base.TableLoad(obj);
			_clients = _registeredDataManager.GetClientsList();

			var table = Tables[MainTableKey];

			foreach (var client in _clients)
			{
				table.Rows.Add(client.ClientId, client.Name);
			}
		}
	}
}
