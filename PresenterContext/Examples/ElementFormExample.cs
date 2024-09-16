using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresenterContext
{
	public partial class ElementFormExample : Form
	{
		public ElementFormPresenter Presenter { get; set; }

		public ElementFormExample()
		{
			InitializeComponent();
		}
	}
}
