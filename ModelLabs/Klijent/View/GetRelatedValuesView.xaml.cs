using FTN.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Klijent.View
{
    /// <summary>
    /// Interaction logic for GetRelatedValuesView.xaml
    /// </summary>
    public partial class GetRelatedValuesView : Window, INotifyPropertyChanged
    {
        private List<long> comboBoxRelatedValues = new List<long>();
        private long gidRelatedValues;
        private List<ModelCode> refIdsRelatedValues = new List<ModelCode>();

        private ModelCode refIDGRelatedValues;
        private List<ModelCode> types = new List<ModelCode>();
        private ModelCode type;

        private List<ModelCode> propertiesRelatedValues = new List<ModelCode>();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<long> ComboBoxRelatedValues
        {
            get { return comboBoxRelatedValues; }
            set { comboBoxRelatedValues = value; OnPropertyChanged("ComboBoxRelatedValues"); }
        }

        public long GidRelatedValues
        {
            get { return gidRelatedValues; }
            set
            {
                gidRelatedValues = value; OnPropertyChanged("GidRelatedValues");
                OnPropertyChanged("RefIdsRelatedValues"); OnPropertyChanged("Types");
                OnPropertyChanged("PropertiesRelatedValues");
                OnPropertyChanged("RefIds");
            }
        }

        public List<ModelCode> RefIds
        {
            get
            {
                if (gidRelatedValues != 0)
                {
                    return PomocneMetode.GetRefIds(gidRelatedValues);
                }
                return null;
            }
            set
            {
                refIdsRelatedValues = value; OnPropertyChanged("RefIds");
                OnPropertyChanged("Types");
            }
        }

        public ModelCode RefIDGRelatedValues
        {
            get { return refIDGRelatedValues; }
            set
            {
                refIDGRelatedValues = value; OnPropertyChanged("RefIDGRelatedValues");
                GetTypes(refIDGRelatedValues); types.Add((long)0x000); OnPropertyChanged("Types");

            }
        }

        public List<ModelCode> Types
        {
            get { return types; }
            set { types = value; OnPropertyChanged("Types"); OnPropertyChanged("PropertiesRelatedValues"); }
        }

        public List<ModelCode> PropertiesRelatedValues
        {
            get
            {
                if (type != 0)
                {
                    return GetProperties3(type, false);
                }
                else
                {
                    return GetProperties3(type, true);
                }
            }
            set
            {
                propertiesRelatedValues = value; OnPropertyChanged("PropertiesRelatedValues");
                
            }
        }

        public ModelCode Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged("Type"); OnPropertyChanged("PropertiesRelatedValues"); }
        }

        public GetRelatedValuesView()
        {
            InitializeComponent();
            GDAProxy gDAProxy = new GDAProxy();
            ComboBoxRelatedValues = gDAProxy.GetAllGids();
            DataContext = this;
        }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private List<ModelCode> GetTypes(ModelCode kodProp)
        {
            ModelResourcesDesc modResDes = new ModelResourcesDesc();


            string[] props = (kodProp.ToString()).Split('_');
            props[1] = props[1].TrimEnd('S');

            DMSType propertyCode = ModelResourcesDesc.GetTypeFromModelCode(kodProp);


            ModelCode mc;
            ModelCodeHelper.GetModelCodeFromString(propertyCode.ToString(), out mc);

            foreach (ModelCode modelCode in Enum.GetValues(typeof(ModelCode)))
            {
                if (String.Compare(props[1], modelCode.ToString()) == 0)
                {
                    DMSType type = ModelCodeHelper.GetTypeFromModelCode(modelCode);
                    if (type == 0)
                    {
                        types = new List<ModelCode>();
                        List<DMSType> r = modResDes.GetLeaves(modelCode);
                        foreach (DMSType ff in r)
                        {
                            types.Add(modResDes.GetModelCodeFromType(ff));
                        }
                    }
                    else
                    {
                        types = new List<ModelCode>();
                        types.Add(modelCode);
                    }

                }
            }


            return new List<ModelCode>();
        }

        public  List<ModelCode> GetProperties3(ModelCode mode, bool indikatorNule)
        {
            ModelResourcesDesc mr = new ModelResourcesDesc();
            List<ModelCode> list = new List<ModelCode>();
            if (indikatorNule == false)
            {
                list = mr.GetAllPropertyIds(mode);
            }
            else
            {
                List<ModelCode> DmsTipovi = new List<ModelCode>();
                DmsTipovi = Types;
                foreach (var item in DmsTipovi)
                {
                    if(item.ToString() == "0")
                    {
                        continue;
                    }
                    List<ModelCode> pomocnaLista = new List<ModelCode>();
                    pomocnaLista = mr.GetAllPropertyIds(item);
                    foreach (var item1 in pomocnaLista)
                    {
                        if (list.Contains(item1))
                        {
                            continue;
                        }
                        list.Add(item1);
                    }
                }
            }

            return list;
        }


        private void GetExValButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox3Properties.SelectedItems == null || RefIDGRelatedValues == 0 || GidRelatedValues == 0 )
            {
                MessageBox.Show("Izaberite atribut");
                return;
            }

            List<ModelCode> l = new List<ModelCode>();
            foreach (var v in listBox3Properties.SelectedItems)
            {
                l.Add((ModelCode)v);
            }

            Association association = new Association();
            association.PropertyId = RefIDGRelatedValues;
            association.Type = Type;

            string ss = "";

            if(Type.ToString() != "0")
            {
                RelatedValuesRezultat.Text = new GDAProxy().GetRelatedValues(GidRelatedValues, association, l);
            }
            else
            {
                foreach (var item in Types)
                {
                    if(item.ToString() == "0")
                    {
                        continue;
                    }
                    association.Type = item;
                    List<ModelCode> retVal = new List<ModelCode>();
                    retVal = VratiPosebneMC(l, GetProperties3(item, false));
                    ss += new GDAProxy().GetRelatedValues(GidRelatedValues, association, retVal);
                }
                RelatedValuesRezultat.Text = ss;

            }


            //RelatedValuesRezultat.Text = new GDAProxy().GetRelatedValues(GidRelatedValues, association, l);

            if (RelatedValuesRezultat.Text == "")
            {
                RelatedValuesRezultat.Text = "Nastala je greska prilikom ispisa atributa!";
            }                              
        }

        private List<ModelCode> VratiPosebneMC(List<ModelCode> selektovani, List<ModelCode>postoje)
        {
            List<ModelCode> retVal = new List<ModelCode>();

            foreach (var item in postoje)
            {
                if(selektovani.Contains(item))
                {
                    retVal.Add(item);
                }
            }

            return retVal;
        }

        private void GetExtentValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetExtentValuesView getExtentValuesView = new GetExtentValuesView();
            getExtentValuesView.Show();
            this.Close();
        }

        private void GetValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetValuesView getValuesView = new GetValuesView();
            getValuesView.Show();

            this.Close();
        }
    }
}
