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
    /// Interaction logic for GetExtentValuesView.xaml
    /// </summary>
    public partial class GetExtentValuesView : Window, INotifyPropertyChanged
    {
        private List<DMSType> comboBoxGetExtentValues = new List<DMSType>();
        private List<ModelCode> properitiesGetExtentValues = new List<ModelCode>();
        private DMSType modelCodeExValues;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DMSType> ComboBoxGetExtentValues
        {
            get { return comboBoxGetExtentValues; }
            set { comboBoxGetExtentValues = value; OnPropertyChanged("ComboBoxGetExtentValues"); }
        }

        public DMSType ModelCodeExValues
        {
            get { return modelCodeExValues; }
            set { modelCodeExValues = value; OnPropertyChanged("ModelCodeExValues"); OnPropertyChanged("ProperitiesGetExtentValues"); }
        }

        public List<ModelCode> ProperitiesGetExtentValues
        {
            get
            {
                if (modelCodeExValues != 0)
                {
                    return PomocneMetode.GetProperties2(modelCodeExValues);
                }
                return null;
            }
            
            set { properitiesGetExtentValues = value; OnPropertyChanged("ProperitiesGetExtentValues"); OnPropertyChanged("ModelCodeExValues"); }
        }

       
        public GetExtentValuesView()
        {
            InitializeComponent();
            GDAProxy gdaProxy = new GDAProxy();
            comboBoxGetExtentValues = Enum.GetValues(typeof(DMSType)).Cast<DMSType>().ToList().FindAll(t => t != DMSType.MASK_TYPE);
            DataContext = this;
        }
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void GetExtentValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetExtentValuesView getExValWin = new GetExtentValuesView();
            getExValWin.Show();
        }

        private void GetValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetValuesView getValuesView = new GetValuesView();
            getValuesView.Show();
            this.Close();
        }

        private void GetRelatedValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetRelatedValuesView getRelatedValuesView = new GetRelatedValuesView();
            getRelatedValuesView.Show();
            this.Close();
        }

        private void GetExValButton_Click(object sender, RoutedEventArgs e)
        {
            if(listBoxGetExtentValues.SelectedItems == null || ModelCodeExValues == 0)
            {
                MessageBox.Show("Izaberite atribut");
            }
            List<ModelCode> retVal = new List<ModelCode>();
            foreach (var item in listBoxGetExtentValues.SelectedItems)
            {
                retVal.Add((ModelCode)item);
            }
            GEVListBoxRezultat.Text = new GDAProxy().GetExtentValues(ModelCodeExValues, retVal);
        }
    }
}
