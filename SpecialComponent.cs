using Grasshopper.Kernel;
using System;

public class SpecialComponent : GH_Component
{
    public SpecialComponent() : base("Special Component", "SpecComp", "Special component showing winforms override", "Special", "Special")
    {
    }

    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
        pManager.AddIntegerParameter("Option", "O", "Option parameter", 0, 1);
        if (base.get_Attributes() == null)
        {
            this.CreateAttributes();
        }
        base.get_Params().get_Input()[0].set_Attributes(new SpecialLinkedAttributes(base.get_Params().get_Input()[0], base.get_Attributes()));
    }

    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
        pManager.AddTextParameter("Output", "O", "Output value", 0);
    }

    protected override void SolveInstance(IGH_DataAccess DA)
    {
        int num = 0;
        if (DA.GetData<int>(0, ref num))
        {
            switch (num)
            {
                case 1:
                    DA.SetData(0, "A");
                    return;

                case 2:
                    DA.SetData(0, "B");
                    return;
            }
            DA.SetData(0, "Unknown");
        }
    }

    public override Guid ComponentGuid
    {
        get
        {
            return new Guid("{DE314B90-AA73-4793-AB1C-051659B486E9}");
        }
    }
}

