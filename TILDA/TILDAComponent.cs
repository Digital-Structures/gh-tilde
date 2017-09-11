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
    using DSECommon;

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
        public List<DSEVariable> VarsList;
        public List<GH_NumberSlider> SlidersList;
        public SurrogateModelBuilder model;
        public bool modelCreated;
        public int numVariables;
        private ProgressBar pb;
        public double ratio;
        public RegressionReport rr;
        public List<GH_NumberSlider> slidersListFeatures;
        public string modelType;
        public double modelParam;

        public TILDAComponent() : base("Tilde", "Tilde", "Surrogate Modeling tool for approximating objective functions", "DSE", "Simplify")
        {
            this.slidersListFeatures = new List<GH_NumberSlider>();
            this.VarsList = new List<DSEVariable>();
            this.SlidersList = new List<GH_NumberSlider>();
            this.modelCreated = false;
            this.ratio = 0.5;
            this.pb = new ProgressBar();
        }

        public override void CreateAttributes()
        {
            base.m_attributes = new TILDA.TILDAComponentAttributes(this);
        }

        private void readSlidersList()
        {
            this.VarsList.Clear();
            this.SlidersList = new List<GH_NumberSlider>();
            int nVars = this.Params.Input[0].Sources.Count;
            for (int i = 0; i < nVars; i++)
            {
                this.SlidersList.Add(this.Params.Input[0].Sources[i] as GH_NumberSlider);
            }

      
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            List<bool> list = new List<bool> { 
                true,
                true,
                true
            };
            pManager.AddNumberParameter("Design Map", "DM", "Collection of values of features in a design space", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Number of Variables", "N", "Number of Variables on Design Map", GH_ParamAccess.item);
            pManager.AddNumberParameter("Training/Validation Ratio", "Ratio", "Ratio between Training and Validation data from Design Map data", 0, 0.5);
            pManager.AddNumberParameter("Predict", "P", "Features set to predict a solution", GH_ParamAccess.list);
        }



        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Output", "Output", "Predicted value", GH_ParamAccess.item);
            pManager.AddTextParameter("Model Type", "Mod", "Model type selected for surrogate model", GH_ParamAccess.item);
            pManager.AddNumberParameter("Nuisance Parameter", "Param", "Parameter for selected surrogate model", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.designs = new GH_Structure<GH_Number>();
            this.fitness = 0.0;
            this.numVariables = 0;
            this.listPredict = new List<double>();
            if ((DA.GetDataTree<GH_Number>(0, out this.designs) && DA.GetData<int>(1, ref this.numVariables)) && (DA.GetData<double>(2, ref this.ratio) && DA.GetDataList<double>(3, this.listPredict)))
            {
                int num;
                this.pb.Width = 200;
                this.pb.Height = 50;
                this.pb.Show();
                this.designMap = new List<List<double>>();
                for (num = 0; num < this.designs.Branches.Count; num++)
                {
                    List<double> item = new List<double>();
                    for (int i = 0; i < this.designs.Branches[0].Count; i++)
                    {
                        GH_Path path = this.designs.get_Path(num);
                        double num3 = this.designs.get_DataItem(path, i).Value;
                        item.Add(num3);
                    }
                    this.designMap.Add(item);
                }
                if (this.designs.Branches.Count < 1)
                {
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel) 20, "Insuficient data provided");
                }
                if ((this.numVariables > this.designs.Branches[0].Count) || (this.numVariables < 1))
                {
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel) 20, "Inconsistent number of variables");
                }
                else if (!this.modelCreated)
                {
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel) 20, "Model Not created, please doubleclick");
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
                    DA.SetData(1, this.modelType);
                    DA.SetData(2, this.modelParam);
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


        protected override Bitmap Icon
        {
            get
            {
                return TILDA.Properties.Resources.ttilda;
            }
        }
    }
}

