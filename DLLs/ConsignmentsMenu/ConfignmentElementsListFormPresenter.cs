using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConsignmentsMenu
{
    public class ConfignmentElementsListFormPresenter : ListFormPresenter
	{
		private RegistersDataManager _registerDataManager;

		public ConfignmentElementsListFormPresenter() : base()
		{ 
			_registerDataManager = new RegistersDataManager();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Sender", "Отправление", TableColumnType.TEXT),
				new TableColumn("Прибытие", "Прибытие", TableColumnType.TEXT),
                new TableColumn("CustomNumber", "Номер", TableColumnType.TEXT),
            };

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[TableKey]);

			foreach (DataGridViewColumn column in Tables[TableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			TableInit();
			base.Load(obj, eventArgs);
		}

		public override void Add()
		{
			try
			{
				base.Add();

				ConfignmentElementFormPresenter configElementForm = new ConfignmentElementFormPresenter(0, ElementFormType.CREATE);
				configElementForm.ShowDialog();
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
					throw new Exception("Маршрут не выбран");

				ConfignmentElementFormPresenter configElementForm = new ConfignmentElementFormPresenter(SelectedId, ElementFormType.UPDATE);
				configElementForm.ShowDialog();
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
                try
                {
                    base.Delete();

                    if (SelectedId == -1)
                        throw new Exception("Маршрут не выбран");

                    _registerDataManager.DeleteCargoConsignment(SelectedId);
                    TableUpdate();

                }
                catch (Exception ex)
                {
                    MessageCaller.CallErrorMessage(ex.Message);
                }
            }    
		}

		public override void TableUpdate()
		{
			base.TableUpdate();

			var confignments = _registerDataManager.GetCargosList();
			Tables[TableKey].Rows.Clear();

			foreach (var item in confignments)
			{
                DateTime DepartDate = DateTime.Parse(item.DepartDate);
                DateTime ArrivalDate = DateTime.Parse(item.ArrivalDate);
                Tables[TableKey].Rows.Add(item.ConsignmentId,    $"{item.SendClientName},    {item.SendPortName},    {DepartDate.ToString("dd.MM.yyyy")}", $"{item.ReceivClientName},    {item.ReceivPortName},    {ArrivalDate.ToString("dd.MM.yyyy")}", item.CustomNumber);
            }
        }
	}
}
