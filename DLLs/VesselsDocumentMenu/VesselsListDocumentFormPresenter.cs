using DatabaseManager;
using DataExport;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VesselsDocumentMenu
{
    public class VesselsListDocumentFormPresenter : PresenterBase
	{
		public VesselsListDocumentFormPresenter() : base(new ElementFormExample())
		{
			Title = "Выписка маршрутов";

			CreateDataGridView(200, "Routes");
			CreateButton("Экспот", Export);
		}


		private void Export(object obj, EventArgs args)
		{
			try
			{
				RegistersDataManager registersData = new RegistersDataManager();
				DocumentCreator.Instance().CreateRoutsDocument(registersData.GetVesselsAndRout());

				MessageCaller.CallInfomationMessage("Документ успешно создан");
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Rout", "#", TableColumnType.TEXT),
				new TableColumn("Rout", "Маршрут", TableColumnType.TEXT),
				new TableColumn("Rout", "Корабль", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables["Routes"]);

			foreach (DataGridViewColumn column in Tables["Routes"].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		private void TableLoad()
		{
			var dataManager = new RegistersDataManager();
			var list = dataManager.GetVesselsAndRout();

			Tables["Routes"].Rows.Clear();

			foreach (var item in list)
				Tables["Routes"].Rows.Add(item.Item1, item.Item2, item.Item4);
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			TableInit();
			TableLoad();
		}
	}
}
