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
	public partial class ElementsListFormExample : Form
	{
		public ListFormPresenter Presenter { get; set; }

		public Button AddButton => button1;
		public Button EditButton => button2;
		public Button DeleteButton => button3;
		public Button ViewButton => button4;

		public ElementsListFormExample()
		{
			InitializeComponent();
        }

	}
}
