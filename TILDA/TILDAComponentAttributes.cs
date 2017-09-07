namespace TILDA
{
    using Grasshopper;
    using Grasshopper.GUI;
    using Grasshopper.GUI.Canvas;
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Attributes;
    using System;

    internal class TILDAComponentAttributes : GH_ComponentAttributes
    {
        private TILDA.TILDAComponent MyComponent;
        private TILDA.ProblemBuilder problem;

        public TILDAComponentAttributes(IGH_Component component) : base(component)
        {
            this.MyComponent = (TILDAComponent) component;
        }


        [STAThread]
        public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
        {
            this.problem = new TILDA.ProblemBuilder(this.MyComponent);
            this.problem.Start();
            Grasshopper.Instances.ActiveCanvas.Document.NewSolution(true);
            this.MyComponent.modelCreated = true;
            return base.RespondToMouseDoubleClick(sender, e);
        }
    }
}

