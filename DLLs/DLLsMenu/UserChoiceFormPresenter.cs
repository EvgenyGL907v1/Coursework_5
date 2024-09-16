using DatabaseManager;
using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menus
{
	public class UserChoiseFormPresenter : ChoiceFormPresenter
	{
		public (int, string) SelectedUser { get; private set; }

		private SystemDataManager _systemDataManager;
		private List<UserData> _usersList;

		public UserChoiseFormPresenter() : base()
		{
			_systemDataManager = new SystemDataManager();
		}

		public override void TableInit()
		{
			base.TableInit();

			List<TableColumn> columns = new List<TableColumn>()
			{
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Login", "Пользователь", TableColumnType.TEXT)
			};

			GridViewPresenter gridCreate = new GridViewPresenter(columns, Tables[MainTableKey]);
		}

		public override void SelectItem(object obj, EventArgs eventArgs)
		{ 
			base.SelectItem(obj, eventArgs);

			var selectedData = _usersList.Find(m => m.UserId == SelectedId);
			SelectedUser = (selectedData.UserId, selectedData.Login);

			Form.DialogResult = DialogResult.OK;
			Form.Close();
		}

		public override void TableLoad(object obj)
		{ 
			base.TableLoad(obj);

			var userList = _systemDataManager.GetUsersList();
			_usersList = userList;
			var table = Tables[MainTableKey];

			foreach (var user in userList) 
			{
				table.Rows.Add(user.UserId, user.Login);
			}
		}

	}
}
