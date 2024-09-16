using PresenterContext;
using System;

namespace MenuInterface
{
    public struct MenuAccess
    { 
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        public static MenuAccess FullAccess => new MenuAccess { Read = true, Write = true, Edit = true, Delete = true };
    }

    public class MenuBase
    {
        protected string Title { get; set; }
        protected MenuAccess MenuAccess { get; private set; }

		public MenuBase(MenuAccess menuAccess)
        { 
            MenuAccess = menuAccess;
        }

        protected void ListFormAccessControl(ElementsListFormExample elementsListForm)
        {
            if (elementsListForm == null)
                return;

            elementsListForm.AddButton.Enabled = (MenuAccess.Write);
            elementsListForm.EditButton.Enabled = (MenuAccess.Edit);
            elementsListForm.DeleteButton.Enabled = (MenuAccess.Delete);
            elementsListForm.ViewButton.Visible = (MenuAccess.Read);
        }

		public virtual void OpenMenu(object obj, EventArgs args)
        { }

        public virtual void OpenMenuContent(object obj, EventArgs args)
        { }
    }
}
