using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PresenterContext
{
	public enum ElementFormType
	{ 
		CREATE,
		UPDATE,
		VIEW
	}


	/// <summary> Базовое поведение Presenter окна элемента </summary>
	public class ElementFormPresenter : PresenterBase, IElementForm
	{
		protected ElementFormType Type { get; private set; }
		protected int CurrentElementId { get; set; }

		public ElementFormPresenter(int elementId, ElementFormType type) : base(new ElementFormExample())
		{
			Type = type;
			CurrentElementId = elementId;

			(Form as ElementFormExample).Presenter = this;
		}

		protected override void Load(object obj, EventArgs eventArgs)
		{
			base.Load(obj, eventArgs);

			foreach (Button button in Buttons.Values)
				button.Visible = (Type != ElementFormType.VIEW);

			foreach (TextBox textBox in TextBoxes.Values)
				textBox.ReadOnly = (Type == ElementFormType.VIEW);

			foreach (CheckBox checkBox in CheckBoxes.Values)
				checkBox.Enabled = (Type != ElementFormType.VIEW);

			foreach (DataGridView table in Tables.Values)
				table.ReadOnly = (Type == ElementFormType.VIEW);

		}

		protected void CreateElementEvent(object obj, EventArgs eventArgs) => CreateElement();
		protected void EditElementEvent(object obj, EventArgs eventArgs) => EditElement();

		public virtual void CreateElement()
		{ }

		public virtual void EditElement()
		{ }
		
	}
}
