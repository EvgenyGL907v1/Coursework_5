using System;
using System.Windows.Forms;

namespace PresenterContext
{
	public class ListFormPresenter : PresenterBase, IListForm, IListFormView
	{
		protected string TableKey = "MainTable";

		protected int SelectedId { get; private set; }

		public ListFormPresenter() : base(new ElementsListFormExample())
		{
			var table = CreateDataGridView(390, TableKey, new System.Drawing.Point(0, 50));
			table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			table.SelectionChanged += new EventHandler(SelectRow);

			ButtonsInit(Form as ElementsListFormExample);
			(Form as ElementsListFormExample).Presenter = this;

			SelectedId = -1;
		}

		protected virtual void ButtonsInit(ElementsListFormExample formExample)
		{
			Buttons.Add("AddButton", formExample.AddButton);
			Buttons.Add("EditButton", formExample.EditButton);
			Buttons.Add("DeleteButton", formExample.DeleteButton);
			Buttons.Add("ViewButton", formExample.ViewButton);

			formExample.AddButton.Click += new EventHandler((e, args) => Add());
			formExample.EditButton.Click += new EventHandler((e, args) => Edit());
			formExample.DeleteButton.Click += new EventHandler((e, args) => Delete());
			formExample.ViewButton.Click += new EventHandler((e, args) => View());
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			TableUpdate();
		}

		public virtual void Add()
		{ }

		public virtual void Delete()
		{ }

		public virtual void Edit()
		{ }

		public virtual void View()
		{ }

		public virtual void TableUpdate()
		{
			foreach (DataGridViewColumn column in Tables[TableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public virtual void SelectRow(object sender, EventArgs args)
		{
			var table = Tables[TableKey];

			if (table.SelectedRows.Count == 0)
			{
				SelectedId = -1;
				return;
			}

			SelectedId = (int)table.SelectedRows[0].Cells[0].Value;
		}
	}
}
