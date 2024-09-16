using System;

namespace PresenterContext
{
	public interface IChoiseForm
	{
		void SelectItem(object obj, EventArgs eventArgs);
		void OnRowSelect(object obj, EventArgs eventArgs);
		void TableLoad(object obj);
		void TableInit();
	}
}
