using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace PresenterContext
{
	public static class FormsCopy
	{
		public static void FormContentCopy(Form form, Form formContent, string[] excElements, Point offset = new Point())
		{

			for (int i = form.Controls.Count - 1; i >= 0; i--)
			{
				var element = form.Controls[i];

				if(!excElements.Contains(element.Name))
					form.Controls.Remove(element);
			}

			foreach (Control control in formContent.Controls)
			{
				Control newControl = (Control)Activator.CreateInstance(control.GetType());
				newControl.Location = new Point(control.Location.X + offset.X, control.Location.Y + offset.Y);

				newControl.Size = control.Size;
				newControl.Text = control.Text;

				form.Controls.Add(newControl);
			}

		}
	}
}
