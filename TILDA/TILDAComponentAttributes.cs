namespace TILDA
{
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Attributes;
    using System;

    internal class TILDAComponentAttributes : GH_ComponentAttributes
    {
        private TILDA.TILDAComponent MyComponent;

        public TILDAComponentAttributes(IGH_Component component) : base(component)
        {
            this.MyComponent = (TILDAComponent) component;
        }


        [STAThread]
        public override Grasshopper.GUI.Canvas.GH_ObjectResponse RespondToMouseDoubleClick(Grasshopper.GUI.Canvas.GH_Canvas sender, Grasshopper.GUI.GH_CanvasMouseEvent e)
        {
            this.MyComponent.modelType = "Test";
            this.buildModel();
            this.MyComponent.modelCreated = true;
            Grasshopper.Instances.ActiveCanvas.Document.NewSolution(true);
            return base.RespondToMouseDoubleClick(sender, e);
        }

        private void buildModel()
        {
            ProblemBuilder pb = new ProblemBuilder(this.MyComponent);
            pb.Start();
        }
    }
}

