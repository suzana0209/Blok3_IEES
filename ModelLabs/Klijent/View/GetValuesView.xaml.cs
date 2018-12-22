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
    /// Interaction logic for GetValuesView.xaml
    /// </summary>
    public partial class GetValuesView : Window, INotifyPropertyChanged
    {
        private List<long> comboBoxGetValues = new List<long>();
        private long gidGetValues;
        private List<ModelCode> propertiesGetValues = new List<ModelCode>();

        public List<long> ComboBoxGetValues
        {
            get { return comboBoxGetValues; }
            set { comboBoxGetValues = value; OnPropertyChanged("ComboBoxGetValuesPath"); }
        }

        public long GidGetValues
        {
            get { return gidGetValues; }
            set { gidGetValues = value; OnPropertyChanged("GidGetValues");  OnPropertyChanged("PropertiesGetValues"); }
        }

        public List<ModelCode> PropertiesGetValues
        {
           get
            {
                if (gidGetValues != 0)
                    return PomocneMetode.GetProperties(gidGetValues);
                else
                    return null;
            }
            set { propertiesGetValues = value; OnPropertyChanged("PropertiesGetValues"); OnPropertyChanged("GidGetValues"); }
        }
        

        public GetValuesView()
        {
            InitializeComponent();

            GDAProxy gdaProxy = new GDAProxy();
            comboBoxGetValues = gdaProxy.GetAllGids();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void GetValuesViewRezButton_Click(object sender, RoutedEventArgs e)
        {
            if(listBoxGetValues.SelectedItems == null || GidGetValues == 0)
            {
                MessageBox.Show("Izaberite atribut(e)!");
                return;
            }

            List<ModelCode> pomocnaLista = new List<ModelCode>();
            foreach (var item in listBoxGetValues.SelectedItems)
            {
                pomocnaLista.Add((ModelCode)item);
            }
            GVListBoxRezultat.Text = new GDAProxy().GetValues(GidGetValues,pomocnaLista);
        }

        private void GetExtentValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetExtentValuesView getExValWin = new GetExtentValuesView();
            getExValWin.Show();
            this.Close();
        }

        private void GetRelatedValuesButton_Click(object sender, RoutedEventArgs e)
        {
            GetRelatedValuesView getRelatedValuesView = new GetRelatedValuesView();
            getRelatedValuesView.Show();
            this.Close();
            
        }

    }
}
