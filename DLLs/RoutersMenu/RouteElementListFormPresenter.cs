using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RoutersMenu
{
	public class RouteElementListFormPresenter : ListFormPresenter
	{
		private RegistersDataManager _register;

		public RouteElementListFormPresenter() : base() 
		{
			_register = new RegistersDataManager();
		}

		private void TableInit()
		{
			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Rout", "Название", TableColumnType.TEXT)
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

				RouteElementFormPresenter routeElementForm = new RouteElementFormPresenter(0, ElementFormType.CREATE);
				routeElementForm.ShowDialog();
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

				RouteElementFormPresenter routeElementForm = new RouteElementFormPresenter(SelectedId, ElementFormType.UPDATE);
				routeElementForm.ShowDialog();
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

                    _register.RemoveRout(SelectedId);
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

			var routs = _register.GetRoutsList();
			Tables[TableKey].Rows.Clear();

			foreach (var item in routs)
				Tables[TableKey].Rows.Add(item.Item1, item.Item2);
		}
	}
}
