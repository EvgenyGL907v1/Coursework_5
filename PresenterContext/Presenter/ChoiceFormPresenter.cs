using System;
using System.Windows.Forms;

namespace PresenterContext
{
	public class ChoiceFormPresenter : PresenterBase, IChoiseForm
	{
		protected const string MainTableKey = "MainTable";

		public int SelectedId { get; private set; }

		public ChoiceFormPresenter() : base(new ChoiceFormExample())
		{
			SelectedId = -1;

			var table = CreateDataGridView(300, MainTableKey);
			table.SelectionChanged += new EventHandler(OnRowSelect);
			table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			CreateButton("Выбрать", SelectItem);
			(Form as ChoiceFormExample).Presenter = this;
			TableInit();
		}

		public virtual void TableInit()
		{
			Tables[MainTableKey].ReadOnly = true;
		}

		public virtual void OnRowSelect(object obj, EventArgs eventArgs)
		{
			var table = Tables[MainTableKey];

			if (table.SelectedRows.Count == 0)
				return;

			var cellValue = table.SelectedRows[0].Cells[0].Value;
			SelectedId = (int)cellValue;
		}

		public virtual void TableLoad(object obj)
		{
			foreach (DataGridViewColumn column in Tables[MainTableKey].Columns)
				column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		public virtual void SelectItem(object obj, EventArgs eventArgs)
		{
			Form.DialogResult = (SelectedId != -1) ? DialogResult.OK : DialogResult.Abort;
			Form.Close();
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);
			TableLoad(null);
		}

	}
}
