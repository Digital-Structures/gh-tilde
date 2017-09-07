using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using System;
using System.Windows.Forms;

public class SpecialLinkedAttributes : GH_LinkedParamAttributes
{
    public SpecialLinkedAttributes(IGH_Param param, IGH_Attributes parent) : base(param, parent)
    {
    }

    private void OptionAClick(object sender, EventArgs e)
    {
        (sender as Control).FindForm().DialogResult = DialogResult.OK;
        Param_Integer integer = base.get_Owner() as Param_Integer;
        integer.RecordUndoEvent("Set Option");
        integer.get_PersistentData().Clear();
        integer.get_PersistentData().Append(new GH_Integer(1));
        integer.ExpireSolution(true);
    }

    private void OptionBClick(object sender, EventArgs e)
    {
        (sender as Control).FindForm().DialogResult = DialogResult.OK;
        Param_Integer integer = base.get_Owner() as Param_Integer;
        integer.RecordUndoEvent("Set Option");
        integer.get_PersistentData().Clear();
        integer.get_PersistentData().Append(new GH_Integer(2));
        integer.ExpireSolution(true);
    }

    public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
    {
        if (e.get_Button() == MouseButtons.Left)
        {
            Form form = new Form {
                Width = 400,
                Height = 400,
                StartPosition = FormStartPosition.Manual
            };
            GH_WindowsFormUtil.CenterFormOnCursor(form, true);
            Button button = new Button();
            Button button2 = new Button();
            button.Click += new EventHandler(this.OptionAClick);
            button2.Click += new EventHandler(this.OptionBClick);
            button.Text = "Option A";
            button2.Text = "Option B";
            button.Width = 300;
            button2.Width = 300;
            button.Height = 0x20;
            button2.Height = 0x20;
            button.Left = 50;
            button2.Left = 50;
            button.Top = 50;
            button2.Top = (form.ClientSize.Height - button2.Height) - 50;
            form.Controls.Add(button);
            form.Controls.Add(button2);
            form.ShowDialog(sender.FindForm());
            return 3;
        }
        return base.RespondToMouseDoubleClick(sender, e);
    }
}

