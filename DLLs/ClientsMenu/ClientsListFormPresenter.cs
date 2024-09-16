using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientsMenu
{
	public class ClientsListFormPresenter : ListFormPresenter
	{
		private RegistersDataManager _registersDataManager;

		public ClientsListFormPresenter() : base()
		{
			_registersDataManager = new RegistersDataManager();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Name", "Название", TableColumnType.TEXT),
				new TableColumn("BankName", "Банк", TableColumnType.TEXT),
				new TableColumn("TaxNumber", "ИНН", TableColumnType.TEXT),
			};
			GridViewPresenter gridViewPresenter = new GridViewPresenter(columns, Tables[TableKey]);
		}

		protected override void ButtonsInit(ElementsListFormExample formExample)
		{
			base.ButtonsInit(formExample);
			TableInit();
		}

		public override void Add()
		{
			try
			{
				base.Add();

				ClientElementFormPresenter accessElement = new ClientElementFormPresenter(0, ElementFormType.CREATE);
				accessElement.ShowDialog();
				TableUpdate();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		public override void Edit()
		{
			try
			{
				base.Edit();

				if (SelectedId == -1)
					throw new Exception("Не выбрана строка");

				ClientElementFormPresenter accessElement = new ClientElementFormPresenter(SelectedId, ElementFormType.UPDATE);
				accessElement.ShowDialog();
				TableUpdate();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		public override void Delete()
		{
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
                base.Delete();

				if (SelectedId == -1)
					throw new Exception("Не выбрана строка");

				_registersDataManager.DeleteClient(SelectedId);
				TableUpdate();
			}

		}

		public override void TableUpdate()
		{
			base.TableUpdate();
			var clients = _registersDataManager.GetClientsList();

			Tables[TableKey].Rows.Clear();

			foreach (var client in clients)
				Tables[TableKey].Rows.Add(client.ClientId, client.Name, client.BankName, client.TaxNumber);
		}
	}
}
