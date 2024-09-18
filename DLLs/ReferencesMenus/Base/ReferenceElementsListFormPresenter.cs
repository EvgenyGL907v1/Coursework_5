using PresenterContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReferencesMenus
{
	public delegate List<(int, string)> TableLoadEvent();

	public delegate void AddElement(string name);
	public delegate void EditElement(int elementId, string name);
	public delegate void DeleteElement(int elementId);

	public class ReferenceElementsListFormPresenter : ListFormPresenter
	{
		private AddElement _addElementEvent;
		private EditElement _editElementEvent;
		private DeleteElement _deleteElementEvent;

		private TableLoadEvent _loadTable;

		private string _elementName;

		private (int, string) _selectedElement;

		public ReferenceElementsListFormPresenter(TableLoadEvent loadTableEvent, AddElement addElementEvent, EditElement editElementEvent, DeleteElement deleteElementEvent, string elementName) : base()
		{ 
			_addElementEvent = addElementEvent;
			_editElementEvent = editElementEvent;
			_deleteElementEvent = deleteElementEvent;

			_loadTable = loadTableEvent;
			_elementName = elementName;
		}

		private void TableInit()
		{ 
			List<TableColumn> tableColumns = new List<TableColumn>()
			{ 
				new TableColumn("Id", "#", TableColumnType.TEXT),
				new TableColumn("Name", "Название", TableColumnType.TEXT),
			};

			GridViewPresenter gridViewPresenter = new GridViewPresenter(tableColumns, Tables[TableKey]);
			Tables[TableKey].ReadOnly = true;
			Tables[TableKey].AllowUserToResizeColumns = false;
		}

		protected override void ButtonsInit(ElementsListFormExample formExample)
		{
			base.ButtonsInit(formExample);
			TableInit();
		}

		public override void SelectRow(object sender, EventArgs args)
		{
			base.SelectRow(sender, args);

			if(Tables[TableKey].SelectedRows.Count != 0)
			{ 
				var row = Tables[TableKey].SelectedRows[0];
				_selectedElement = (int.Parse(row.Cells[0].Value.ToString()), row.Cells[1].Value.ToString());
			}
		}

		public override void Add()
		{
			try
			{
				base.Add();
				ReferenceElementFormPresenter elementForm = new ReferenceElementFormPresenter(_addElementEvent, _elementName + ". Создание");
				elementForm.ShowDialog();
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
				{
					MessageCaller.CallInfomationMessage("Элемент не выбран");
					return;
				}
				
				ReferenceElementFormPresenter elementForm = new ReferenceElementFormPresenter(_editElementEvent, SelectedId, _selectedElement.Item2, _elementName + ". Редактирование");
				elementForm.ShowDialog();
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

                    if (SelectedId == 0 || SelectedId == -1)
                    {
                        MessageCaller.CallInfomationMessage("Элемент не выбран");
                        return;
                    }

                    _deleteElementEvent(SelectedId);
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
			List<(int, string)> elements = _loadTable.Invoke();

			Tables[TableKey].Rows.Clear();

			foreach (var element in elements)
				Tables[TableKey].Rows.Add(element.Item1, element.Item2);

		}
	}
}
