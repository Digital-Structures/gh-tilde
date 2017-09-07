namespace TILDA
{
    using Grasshopper.Kernel;
    using Grasshopper.Kernel.Data;
    using Grasshopper.Kernel.Special;
    using StructureEngine.MachineLearning;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using TILDA.Properties;


    using System.Linq;
    using Grasshopper;
    using Grasshopper.Kernel.Parameters;
    using Grasshopper.Kernel.Types;


    public class TILDAComponent : GH_Component
    {
        public List<List<double>> designMap;
        public GH_Structure<GH_Number> designs;
        public double fitness;
        public List<double> listPredict;
        public SurrogateModelBuilder model;
        public bool modelCreated;
        public int numVariables;
        private ProgressBar pb;
        public double ratio;
        public RegressionReport rr;
        public List<GH_NumberSlider> slidersListFeatures;

        public TILDAComponent() : base("TILDA", "TILDA", "Surrogate Model", "MIT", "Surrogate Modeling")
        {
            this.slidersListFeatures = new List<GH_NumberSlider>();
            this.modelCreated = false;
            this.ratio = 0.5;
            this.pb = new ProgressBar();
        }

        public override void CreateAttributes()
        {
            base.m_attributes = new TILDA.TILDAComponentAttributes(this);
        }

        public List<GH_NumberSlider> readSlidersList()
        {
            this.slidersListFeatures.Clear();
            foreach (IGH_Param param in base.get_Params().get_Input()[3].get_Sources())
            {
                GH_NumberSlider item = param as GH_NumberSlider;
                this.slidersListFeatures.Add(item);
            }
            return this.slidersListFeatures;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            List<bool> list = new List<bool> { 
                true,
                true,
                true
            };
            pManager.AddNumberParameter("Design Map", "DM", "Collection of values of features in a design space", 2);
            pManager.AddIntegerParameter("Number of Variables", "N", "Number of Variables on Design Map", 0);
            pManager.AddNumberParameter("Training/Validation Ratio", "Ratio", "Ratio between Training and Validation data from Design Map data", 0, 0.5);
            pManager.AddNumberParameter("Predict", "P", "Features set to predict a solution", 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Output", "Output", "Predicted value", 0);
            pManager.AddNumberParameter("Model Type", "Mod", "Model type selected for surrogate model", 0);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.designs = new GH_Structure<GH_Number>();
            this.fitness = 0.0;
            this.numVariables = 0;
            this.listPredict = new List<double>();
            if ((DA.GetDataTree<GH_Number>(0, ref this.designs) && DA.GetData<int>(1, ref this.numVariables)) && (DA.GetData<double>(2, ref this.ratio) && DA.GetDataList<double>(3, this.listPredict)))
            {
                int num;
                this.pb.Width = 200;
                this.pb.Height = 50;
                this.pb.Show();
                this.designMap = new List<List<double>>();
                for (num = 0; num < this.designs.get_Branches().Count; num++)
                {
                    List<double> item = new List<double>();
                    for (int i = 0; i < this.designs.get_Branches()[0].Count; i++)
                    {
                        GH_Path path = this.designs.get_Path(num);
                        double num3 = this.designs.get_DataItem(path, i).get_Value();
                        item.Add(num3);
                    }
                    this.designMap.Add(item);
                }
                if (this.designs.get_Branches().Count < 1)
                {
                    this.AddRuntimeMessage(20, "Insuficient data provided");
                }
                if ((this.numVariables > this.designs.get_Branches()[0].Count) || (this.numVariables < 1))
                {
                    this.AddRuntimeMessage(20, "Inconsistent number of variables");
                }
                else if (!this.modelCreated)
                {
                    this.AddRuntimeMessage(20, "Model Not created");
                }
                else
                {
                    List<double> features = new List<double>();
                    for (num = 0; num < this.listPredict.Count; num++)
                    {
                        features.Add(this.listPredict[num]);
                    }
                    Observation test = new Observation(features, 0.0);
                    double num4 = this.rr.Model.Predict(test);
                    DA.SetData(0, num4);
                }
            }
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("{edb5567c-c83c-40b0-adec-b9385e386653}");
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return 2;
            }
        }

        protected override Bitmap Icon
        {
            get
            {
                return TILDA.Properties.Resources.ttilda;
            }
        }
    }
}

