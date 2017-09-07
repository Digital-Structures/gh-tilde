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
            this.MyComponent = component;
        }

        [STAThread]
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            this.problem = new TILDA.ProblemBuilder(this.MyComponent);
            this.problem.Start();
            Instances.get_ActiveCanvas().get_Document().NewSolution(true);
            this.MyComponent.modelCreated = true;
            return base.RespondToMouseDoubleClick(sender, e);
        }
    }
}

