using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;

namespace MS43_Recalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class InitialAFR
        {

            private string _FileName = @"C:\ms43_recalc\datafiles\init_afr.dat";
            private ObservableCollection<DataObject> _init_afr;
            private ObservableCollection<DataObject> _init_afr_idle;
            public InitialAFR()
            {
                _init_afr = new ObservableCollection<DataObject>();
                _init_afr_idle = new ObservableCollection<DataObject>();
            }
            public void LoadInitAFR()
            {
                using (StreamReader textReader = new StreamReader(_FileName))
                {
//                    var _init_afr = new ObservableCollection<DataObject>();
                    string[] cells;
                    var Delimiter = ';';
                    string line = textReader.ReadLine();
                    string afr_name = "";
                    while (line != null)
                    {
                        { afr_name = line.Split(Delimiter)[0]; line = textReader.ReadLine(); }
                        if (afr_name == "Target AFR Basic")
                        {
                            for (int init_afr_length = 0; init_afr_length < 16; init_afr_length++)
                            {
                                cells = line.Split(Delimiter);
                                _init_afr.Add(new DataObject()
                                {
                                    A = cells[0],
                                    B = cells[1],
                                    C = cells[2],
                                    D = cells[3],
                                    E = cells[4],
                                    F = cells[5],
                                    G = cells[6],
                                    H = cells[7],
                                    I = cells[8],
                                    Y = cells[9],
                                    K = cells[10],
                                    L = cells[11]
                                });
                                line = textReader.ReadLine();
                            }
                        }
                        if (afr_name == "Target AFR Idle")
                        {
                            for (int init_afr_idle_length = 0; init_afr_idle_length < 6; init_afr_idle_length++)
                            {
                                cells = line.Split(Delimiter);
                                _init_afr_idle.Add(new DataObject()
                                {
                                    A = cells[0],
                                    B = cells[1],
                                    C = cells[2],
                                    D = cells[3],
                                    E = cells[4],
                                    F = cells[5],

                                });
                                line = textReader.ReadLine();
                            }
                        }

                    }
                }
            }
            public ObservableCollection<DataObject> init_afr { get { return _init_afr; } }
            public ObservableCollection<DataObject> init_afr_idle { get { return _init_afr_idle; } }
        }

        public
        class ECUData
        {
            private ObservableCollection<DataObject> _list;
            private string _FileName;
            private decimal[,] _IP_TIB_table = new decimal[16, 12];
            private decimal[,] _IP_TI_tco_2_PL_IVVT_x_table = new decimal[16, 12];
            private decimal[,] _IP_TI_tco_1_PL_IVVT_x_table = new decimal[16, 12];
            private decimal[,] _ip_ti_tco_2_is_ivvt_n_maf_table = new decimal[6, 6];
            private decimal[,] _ip_ti_tco_1_is_ivvt_n_maf_table = new decimal[6, 6];
            public void ECUDataRead(string _FileName)
            {
                using (StreamReader textReader = new StreamReader(_FileName))
                {

                    string[] cells;
                    string line = textReader.ReadLine();
                    string map_name = "";
                    int map_length = 0;
                    
                    while (line != null)
                    {                        
                        var Delimiter = ';';
                        if (line.Split(Delimiter).Length == 1)
                        { map_name = line.Split(Delimiter)[0]; line = textReader.ReadLine(); }
                        if (map_name == "IP_TIB" && map_length < 16)
                        {
                            for (int ip_tib_map_length = 0; ip_tib_map_length < 16; ip_tib_map_length++)
                            {
                                cells = line.Split(Delimiter);
                                for (int i = 0; i < 12; i++)
                                {
                                    _IP_TIB_table[ip_tib_map_length, i] = Convert.ToDecimal(cells[i]);
                                }
                                line = textReader.ReadLine();
                            }
                        }

                        if (map_name == "IP_TI_tco_2_PL_IVVT_x" && map_length < 16)
                        {
                            for (int IP_TI_tco_2_PL_IVVT_x_length = 0; IP_TI_tco_2_PL_IVVT_x_length < 16; IP_TI_tco_2_PL_IVVT_x_length++)
                            {
                                cells = line.Split(Delimiter);
                                for (int i = 0; i < 12; i++)
                                {
                                    _IP_TI_tco_2_PL_IVVT_x_table[IP_TI_tco_2_PL_IVVT_x_length, i] = Convert.ToDecimal(cells[i]);
                                }
                                line = textReader.ReadLine();
                            }
                        }
                        if (map_name == "IP_TI_tco_1_PL_IVVT_x" && map_length < 16)
                        {
                            for (int IP_TI_tco_1_PL_IVVT_x_length = 0; IP_TI_tco_1_PL_IVVT_x_length < 16; IP_TI_tco_1_PL_IVVT_x_length++)
                            {
                                cells = line.Split(Delimiter);
                                for (int i = 0; i < 12; i++)
                                {
                                    _IP_TI_tco_1_PL_IVVT_x_table[IP_TI_tco_1_PL_IVVT_x_length, i] = Convert.ToDecimal(cells[i]);
                                }
                                line = textReader.ReadLine();
                            }
                        }
                        if (map_name == "ip_ti_tco_2_is_ivvt_n_maf" && map_length < 6)
                        {
                            for (int ip_ti_tco_2_is_ivvt_n_maf_length = 0; ip_ti_tco_2_is_ivvt_n_maf_length < 6; ip_ti_tco_2_is_ivvt_n_maf_length++)
                            {
                                cells = line.Split(Delimiter);
                                for (int i = 0; i < 6; i++)
                                {
                                    _ip_ti_tco_2_is_ivvt_n_maf_table[ip_ti_tco_2_is_ivvt_n_maf_length, i] = Convert.ToDecimal(cells[i]);
                                }
                                line = textReader.ReadLine();
                            }
                        }
                        if (map_name == "ip_ti_tco_1_is_ivvt_n_maf" && map_length < 6)
                        {
                            for (int ip_ti_tco_1_is_ivvt_n_maf_length = 0; ip_ti_tco_1_is_ivvt_n_maf_length < 6; ip_ti_tco_1_is_ivvt_n_maf_length++)
                            {
                                cells = line.Split(Delimiter);
                                for (int i = 0; i < 6; i++)
                                {
                                    _ip_ti_tco_1_is_ivvt_n_maf_table[ip_ti_tco_1_is_ivvt_n_maf_length, i] = Convert.ToDecimal(cells[i]);
                                }
                                line = textReader.ReadLine();
                            }
                        }





                        line = textReader.ReadLine();
                    }
                }
            }
            public decimal[,] IP_TIB_table { get { return _IP_TIB_table; } }
            public decimal[,] IP_TI_tco_2_PL_IVVT_x_table { get { return _IP_TI_tco_2_PL_IVVT_x_table; } }
            public decimal[,] IP_TI_tco_1_PL_IVVT_x_table { get { return _IP_TI_tco_1_PL_IVVT_x_table; } }
            public decimal[,] ip_ti_tco_2_is_ivvt_n_maf_table { get { return _ip_ti_tco_2_is_ivvt_n_maf_table; } }
            public decimal[,] ip_ti_tco_1_is_ivvt_n_maf_table { get { return _ip_ti_tco_1_is_ivvt_n_maf_table; } }
        }

        class Table16x12
        {
            //private decimal[] _rpm_axis;
            //private decimal[] _engine_loaf_axis;
            private decimal[] _rpm_axis = new decimal[16] { 600, 1000, 1200, 1500, 1600, 2000, 2500, 3000, 3500, 3900, 4100, 4500, 4800, 5500, 6000, 6500 };
            private decimal[] _engine_load_axis = new decimal[12] { 35, 75, 125, 175, 250, 325, 400, 450, 500, 550, 600, 700 };
            
            private decimal[] _rpm_idle_axis = new decimal[6] { 320, 520, 580, 980, 1200, 1600 };
            private decimal[] _engine_load_idle_axis = new decimal[6] { 35, 76, 125, 174, 251, 425 };

            private decimal[,] _table_idle = new decimal[6, 6];
            private decimal[,] _hit_count_table_idle = new decimal[6, 6];
            private int _rpm_idle_value_index;
            private int _engine_load_idle_value_index;

            private decimal[,] _table = new decimal[16, 12];
            private decimal[,] _hit_count_table = new decimal[16, 12];
            private int _rpm_value_index;
            private int _engine_load_value_index;
            public void GenerateTable16x12()
            {

                for (int i = 0; i < 16; i++)
                {
                    for (int f = 0; f < 12; f++)
                    {
                        _table[i, f] = 0;
                    }
                }

            }

            public void IndexOfTableCalculation_idle(ObservableCollection<DataObject> _idle_list)
            {
                decimal rpm_idle = 0;
                decimal afr_idle = 0;
                decimal engine_load_idle = 0;
                foreach (var val in _idle_list)
                {
                    rpm_idle = Convert.ToDecimal(val.A);
                    afr_idle = Convert.ToDecimal(val.B);
                    engine_load_idle = Convert.ToDecimal(val.D);

                    for (int i = 0; i < _rpm_idle_axis.Length - 1; i++)
                    {
                        if (rpm_idle > _rpm_idle_axis[i] && rpm_idle < _rpm_idle_axis[i + 1])
                        {
                            decimal p1 = Math.Abs(rpm_idle - _rpm_idle_axis[i]);
                            decimal p2 = Math.Abs(_rpm_idle_axis[i + 1] - rpm_idle);
                            if (p1 < p2) { _rpm_idle_value_index = i; }
                            else if (p1 > p2) { _rpm_idle_value_index = i + 1; }
                            break;
                        }
                    }
                    for (int i = 0; i < _engine_load_idle_axis.Length - 1; i++)
                    {
                        if (engine_load_idle > _engine_load_idle_axis[i] && engine_load_idle < _engine_load_idle_axis[i + 1])
                        {
                            decimal p1 = Math.Abs(engine_load_idle - _engine_load_idle_axis[i]);
                            decimal p2 = Math.Abs(_engine_load_idle_axis[i + 1] - engine_load_idle);
                            if (p1 < p2) { _engine_load_idle_value_index = i; }
                            else if (p1 > p2) { _engine_load_idle_value_index = i + 1; }
                            break;
                        }
                    }
                    _table_idle[_rpm_idle_value_index, _engine_load_idle_value_index] += afr_idle;
                    _hit_count_table_idle[_rpm_idle_value_index, _engine_load_idle_value_index]++;
                }
                //AFR table generate
                for (int f = 0; f < 6; f++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (_hit_count_table_idle[i, f] > 0)
                        {
                            _table_idle[i, f] = Math.Round(_table_idle[i, f] / _hit_count_table_idle[i, f], 2);
                        }
                    }
                }
            }

            public void IndexOfTableCalculation(ObservableCollection<DataObject> _list)
            {
                decimal rpm = 0;
                decimal afr = 0;
                decimal aim = 0;
                decimal engine_load = 0;
                decimal pwg_angle = 0;
                decimal speed = 0;
                foreach (var val in _list)
                {
                    rpm = Convert.ToDecimal(val.A);
                    afr = Convert.ToDecimal(val.B);
                    aim = Convert.ToDecimal(val.C);
                    engine_load = Convert.ToDecimal(val.D);

                
                
                    for (int i = 0; i < _rpm_axis.Length-1; i++)
                    {
                        if (rpm > _rpm_axis[i] && rpm < _rpm_axis[i + 1])
                        {
                            decimal p1 = Math.Abs(rpm - _rpm_axis[i]);
                            decimal p2 = Math.Abs(_rpm_axis[i + 1] - rpm);
                            if (p1 < p2) { _rpm_value_index = i; }
                            else if (p1 > p2) { _rpm_value_index = i + 1; }
                            break;
                        }
                    }
                    for (int i = 0; i < _engine_load_axis.Length-1; i++)
                    {
                        if (engine_load > _engine_load_axis[i] && engine_load < _engine_load_axis[i + 1])
                        {
                            decimal p1 = Math.Abs(engine_load - _engine_load_axis[i]);
                            decimal p2 = Math.Abs(_engine_load_axis[i + 1] - engine_load);
                            if (p1 < p2) { _engine_load_value_index = i; }
                            else if (p1 > p2) { _engine_load_value_index = i + 1; }
                            break;
                        }
                    }
                    _table[_rpm_value_index, _engine_load_value_index] += afr;
                    _hit_count_table[_rpm_value_index, _engine_load_value_index]++;

                }
                //AFR table generate
                for (int f = 0; f < 12; f++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (_hit_count_table[i, f] > 0)
                        {
                            _table[i, f] = Math.Round(_table[i, f] / _hit_count_table[i, f],2);
                        }
                    }
                }
            }
            public decimal[,] table16x12 { get { return _table; } }
            public decimal[,] table6x6 { get { return _table_idle; } }

            public decimal[] rpm_idle_axis { get { return _rpm_idle_axis; } }
            public decimal[] engine_load_idle_axis { get { return _engine_load_idle_axis; } }

            public decimal[] rpm_axis { get { return _rpm_axis; } }
            public decimal[] engine_load_axis { get { return _engine_load_axis; } }
        }
        class DataFromLog
        {
            private ObservableCollection<DataObject> _list;
            private ObservableCollection<DataObject> _idle_list;
            private string _FileName;
            public DataFromLog(string FileName)
            {
                _list = new ObservableCollection<DataObject>();
                _idle_list = new ObservableCollection<DataObject>();
                _FileName = FileName;
            }

            public void DataProcessing()
            {
                using (StreamReader textReader = new StreamReader(_FileName))
                {
                    //
                    string[] columns;
                    int entriesFound = 0;

                    string line = textReader.ReadLine();
                    int skipCount = 0;
                    int afr_index = 0;
                    int rpm_index = 0;
                    int aim_index = 0;
                    int engine_load_index = 0;
                    int pwg_angle_index = 0;
                    int speed_index = 0;
                    int temp_index = 0;
                    int inj_time_index = 0;
                    while (line != null && skipCount < 1)
                    {


                        var Delimiter = ';';
                        columns = line.Split(Delimiter);
                        
                        for (int i = 0; i < columns.Length; i++)
                        {

                            if (columns[i].Contains("AFR"))
                            {
                                afr_index = i;
                            }
                            if (columns[i].Contains("RPM"))
                            {
                                rpm_index = i;
                            }
                            if (columns[i].Contains("Air mass"))
                            {
                                aim_index = i;
                            }
                            if (columns[i].Contains("MAF_MES"))
                            {
                                engine_load_index = i;
                            }
                            if (columns[i].Contains("Pedal"))
                            {
                                pwg_angle_index = i;
                            }
                            if (columns[i].Contains("Speed"))
                            {
                                speed_index = i;
                            }
                            if (columns[i].Contains("Temp coolant"))
                            {
                                temp_index = i;
                            }
                            if (columns[i].Contains("Inj time"))
                            {
                                inj_time_index = i;
                            }
                        }
                        
                        line = textReader.ReadLine();
                        skipCount++;

                    }
                    //decimal summ_afr = 0;
                    while (line != null)
                    {
                        var Delimiter = ';';
                        columns = line.Split(Delimiter);
                        //if (skipCount == 1)
                        //{ 

                        //    list.Add(new DataObject() { A = columns[0], B = columns[2], C = columns[3] });

                        //}
                        //if (skipCount < 1)
                        //{

                        decimal rpm = Convert.ToDecimal(columns[rpm_index].Replace('.', ','));
                        decimal afr = Convert.ToDecimal(columns[afr_index].Replace('.', ','));
                        decimal aim = Convert.ToDecimal(columns[aim_index].Replace('.', ','));
                        decimal engine_load = Convert.ToDecimal(columns[engine_load_index].Replace('.', ','));
                        decimal pwg_angle = Convert.ToDecimal(columns[pwg_angle_index].Replace('.', ','));
                        decimal speed = Convert.ToDecimal(columns[speed_index].Replace('.', ','));
                        decimal temp = Convert.ToDecimal(columns[temp_index].Replace('.', ','));
                        decimal inj_time = Convert.ToDecimal(columns[inj_time_index].Replace('.', ','));
                        decimal strokes = rpm / 2 * 6 * 60;
                        decimal load = 0;
                        if (strokes > 0) { load = Math.Round(Convert.ToDecimal(aim / strokes * 1000000), 0); }
                        if (afr > 8 && afr < 18 && pwg_angle > 1 && temp >= 85)
                        {
                            _list.Add(new DataObject() { A = rpm.ToString(), B = afr.ToString(), C = aim.ToString(), D = load.ToString(), E = engine_load.ToString(), F = pwg_angle.ToString(), G = speed.ToString(), H = temp.ToString(), I = inj_time.ToString() });
                           // int hit_count = 0;
                           // if (rpm > 500 && rpm < 750 && aim > 35 && aim < 75)
                            //{
                             //   summ_afr = summ_afr + afr;
                               // hit_count++;
                            //}
                        }
                        if (afr > 8 && afr < 18 && pwg_angle < 1 && temp >= 85)
                        {
                            _idle_list.Add(new DataObject() { A = rpm.ToString(), B = afr.ToString(), C = aim.ToString(), D = load.ToString(), E = engine_load.ToString(), F = pwg_angle.ToString() });
                        }
                        //}
                        //skipCount--;
                        entriesFound++;
                        line = textReader.ReadLine();

                    }
                    //AFR.Add(new DataObject() { A = summ_afr.ToString(), B = "3", C = "0" });    
                    







                    //
                    
                    
                    //_list.Add(new DataObject() {A = line, B = line, C = line, D = line });
                    
                }
            }
            public ObservableCollection<DataObject> list { get { return _list; } }
            public ObservableCollection<DataObject> idle_list { get { return _idle_list; } }
        }
        public class DataObject
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string E { get; set; }
            public string F { get; set; }
            public string G { get; set; }
            public string H { get; set; }
            public string I { get; set; }
            public string Y { get; set; }
            public string K { get; set; }
            public string L { get; set; }

        }
        public MainWindow()
        {
            InitializeComponent();
            InitialAFR init_afr = new InitialAFR();
            init_afr.LoadInitAFR();
            this.target_afr.ItemsSource = init_afr.init_afr;
            this.target_afr_idle.ItemsSource = init_afr.init_afr_idle;
            Table16x12 zz = new Table16x12();
            zz.GenerateTable16x12();
            for (int i = 0; i < zz.engine_load_axis.Length; i++)
            {
                this.Afr_from_log.Columns[i].Header = zz.engine_load_axis[i];
                this.IP_TIB_current.Columns[i].Header = zz.engine_load_axis[i];
                this.IP_TI_tco_2_PL_IVVT_x_current.Columns[i].Header = zz.engine_load_axis[i];
                this.target_afr.Columns[i].Header = zz.engine_load_axis[i];
                this.IP_TIB_recalculated.Columns[i].Header = zz.engine_load_axis[i];
                this.IP_TI_tco_2_PL_IVVT_x_recalculated.Columns[i].Header = zz.engine_load_axis[i];
                this.IP_TI_tco_1_PL_IVVT_x_current.Columns[i].Header = zz.engine_load_axis[i];

            }
            for (int i = 0; i < zz.engine_load_idle_axis.Length; i++)
            {
                this.ip_ti_tco_2_is_ivvt_n_maf_current.Columns[i].Header = zz.engine_load_idle_axis[i];
                this.ip_ti_tco_2_is_ivvt_n_maf_recalculated.Columns[i].Header = zz.engine_load_idle_axis[i];
                this.ip_ti_tco_1_is_ivvt_n_maf_current.Columns[i].Header = zz.engine_load_idle_axis[i];
                this.ip_ti_tco_1_is_ivvt_n_maf_recalculated.Columns[i].Header = zz.engine_load_idle_axis[i];
                this.Afr_from_log_idle.Columns[i].Header = zz.engine_load_idle_axis[i];
                this.target_afr_idle.Columns[i].Header = zz.engine_load_idle_axis[i];
            }
            
            
            
            
        }
        private void Grid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Table16x12 rpm_axis = new Table16x12();

            var id = e.Row.GetIndex();
            e.Row.Header = rpm_axis.rpm_axis[id];
        }
        private void Grid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Table16x12 rpm_axis_idle = new Table16x12();

            var id = e.Row.GetIndex();
            e.Row.Header = rpm_axis_idle.rpm_idle_axis[id];
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                textbox1.Text = ofd.FileName;
                DataFromLog xx = new DataFromLog(ofd.FileName);
                xx.DataProcessing();
                this.dataGrid1.ItemsSource = xx.list;

                Table16x12 zz = new Table16x12();
                zz.GenerateTable16x12();
                zz.IndexOfTableCalculation(xx.list);
                zz.IndexOfTableCalculation_idle(xx.idle_list);
                var afr_list = new ObservableCollection<DataObject>();
                var afr_list_idle = new ObservableCollection<DataObject>();
                this.Afr_from_log.ItemsSource = afr_list;
                this.Afr_from_log_idle.ItemsSource = afr_list_idle;



                for (int i = 0; i < 16; i++)
                {
                    afr_list.Add(new DataObject()
                    {
                        A = zz.table16x12[i, 0].ToString(),
                        B = zz.table16x12[i, 1].ToString(),
                        C = zz.table16x12[i, 2].ToString(),
                        D = zz.table16x12[i, 3].ToString(),
                        E = zz.table16x12[i, 4].ToString(),
                        F = zz.table16x12[i, 5].ToString(),
                        G = zz.table16x12[i, 6].ToString(),
                        H = zz.table16x12[i, 7].ToString(),
                        I = zz.table16x12[i, 8].ToString(),
                        Y = zz.table16x12[i, 9].ToString(),
                        K = zz.table16x12[i, 10].ToString(),
                        L = zz.table16x12[i, 11].ToString()
                    });


                }

                for (int i = 0; i < 6; i++)
                {
                    afr_list_idle.Add(new DataObject()
                    {
                        A = zz.table6x6[i, 0].ToString(),
                        B = zz.table6x6[i, 1].ToString(),
                        C = zz.table6x6[i, 2].ToString(),
                        D = zz.table6x6[i, 3].ToString(),
                        E = zz.table6x6[i, 4].ToString(),
                        F = zz.table6x6[i, 5].ToString()
                        
                    });


                }

                var x = xx.list.Max(m => Convert.ToDecimal(m.E));
                textbox2.Text = x.ToString();
                AFR.IsSelected = true;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog read_ecu_setting = new OpenFileDialog();
            if (read_ecu_setting.ShowDialog() == true)
            {
                ECUData ECUDataRead = new ECUData();
                ECUDataRead.ECUDataRead(read_ecu_setting.FileName);
                var ip_tib = new ObservableCollection<DataObject>();
                var IP_TI_tco_2_PL_IVVT_x = new ObservableCollection<DataObject>();
                var IP_TI_tco_1_PL_IVVT_x = new ObservableCollection<DataObject>();
                var ip_ti_tco_2_is_ivvt_n_maf_table = new ObservableCollection<DataObject>();
                var ip_ti_tco_1_is_ivvt_n_maf_table = new ObservableCollection<DataObject>();
                this.IP_TIB_current.ItemsSource = ip_tib;
                this.IP_TI_tco_2_PL_IVVT_x_current.ItemsSource = IP_TI_tco_2_PL_IVVT_x;
                this.IP_TI_tco_1_PL_IVVT_x_current.ItemsSource = IP_TI_tco_1_PL_IVVT_x;
                this.ip_ti_tco_2_is_ivvt_n_maf_current.ItemsSource = ip_ti_tco_2_is_ivvt_n_maf_table;
                this.ip_ti_tco_1_is_ivvt_n_maf_current.ItemsSource = ip_ti_tco_1_is_ivvt_n_maf_table;
                Table16x12 zz = new Table16x12();
                zz.GenerateTable16x12();


                for (int i = 0; i < 16; i++)
                {
                    {
                        ip_tib.Add(new DataObject()
                        {
                            A = ECUDataRead.IP_TIB_table[i, 0].ToString(),
                            B = ECUDataRead.IP_TIB_table[i, 1].ToString(),
                            C = ECUDataRead.IP_TIB_table[i, 2].ToString(),
                            D = ECUDataRead.IP_TIB_table[i, 3].ToString(),
                            E = ECUDataRead.IP_TIB_table[i, 4].ToString(),
                            F = ECUDataRead.IP_TIB_table[i, 5].ToString(),
                            G = ECUDataRead.IP_TIB_table[i, 6].ToString(),
                            H = ECUDataRead.IP_TIB_table[i, 7].ToString(),
                            I = ECUDataRead.IP_TIB_table[i, 8].ToString(),
                            Y = ECUDataRead.IP_TIB_table[i, 9].ToString(),
                            K = ECUDataRead.IP_TIB_table[i, 10].ToString(),
                            L = ECUDataRead.IP_TIB_table[i, 11].ToString()
                        });
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    {
                        IP_TI_tco_2_PL_IVVT_x.Add(new DataObject()
                        {
                            A = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 0].ToString(),
                            B = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 1].ToString(),
                            C = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 2].ToString(),
                            D = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 3].ToString(),
                            E = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 4].ToString(),
                            F = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 5].ToString(),
                            G = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 6].ToString(),
                            H = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 7].ToString(),
                            I = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 8].ToString(),
                            Y = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 9].ToString(),
                            K = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 10].ToString(),
                            L = ECUDataRead.IP_TI_tco_2_PL_IVVT_x_table[i, 11].ToString()
                        });
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    {
                        IP_TI_tco_1_PL_IVVT_x.Add(new DataObject()
                        {
                            A = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 0].ToString(),
                            B = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 1].ToString(),
                            C = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 2].ToString(),
                            D = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 3].ToString(),
                            E = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 4].ToString(),
                            F = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 5].ToString(),
                            G = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 6].ToString(),
                            H = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 7].ToString(),
                            I = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 8].ToString(),
                            Y = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 9].ToString(),
                            K = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 10].ToString(),
                            L = ECUDataRead.IP_TI_tco_1_PL_IVVT_x_table[i, 11].ToString()
                        });
                    }
                }

                for (int i = 0; i < 6; i++)
                {
                    {
                        ip_ti_tco_2_is_ivvt_n_maf_table.Add(new DataObject()
                        {
                            A = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 0].ToString(),
                            B = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 1].ToString(),
                            C = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 2].ToString(),
                            D = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 3].ToString(),
                            E = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 4].ToString(),
                            F = ECUDataRead.ip_ti_tco_2_is_ivvt_n_maf_table[i, 5].ToString(),
                        });
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    {
                        ip_ti_tco_1_is_ivvt_n_maf_table.Add(new DataObject()
                        {
                            A = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 0].ToString(),
                            B = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 1].ToString(),
                            C = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 2].ToString(),
                            D = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 3].ToString(),
                            E = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 4].ToString(),
                            F = ECUDataRead.ip_ti_tco_1_is_ivvt_n_maf_table[i, 5].ToString(),
                        });
                    }
                }


                ECUCurrentSettings.IsSelected = true;
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            var IP_TIB_recalculated = new ObservableCollection<DataObject>();
            this.IP_TIB_recalculated.ItemsSource = IP_TIB_recalculated;
            var IP_TI_tco_2_PL_IVVT_x_recalculated = new ObservableCollection<DataObject>();
            this.IP_TI_tco_2_PL_IVVT_x_recalculated.ItemsSource = IP_TI_tco_2_PL_IVVT_x_recalculated;
            var IP_TI_tco_1_PL_IVVT_x_recalculated = new ObservableCollection<DataObject>();
            this.IP_TI_tco_1_PL_IVVT_x_recalculated.ItemsSource = IP_TI_tco_1_PL_IVVT_x_recalculated;
            var ip_ti_tco_2_is_ivvt_n_maf_recalculated = new ObservableCollection<DataObject>();
            this.ip_ti_tco_2_is_ivvt_n_maf_recalculated.ItemsSource = ip_ti_tco_2_is_ivvt_n_maf_recalculated;
            var ip_ti_tco_1_is_ivvt_n_maf_recalculated = new ObservableCollection<DataObject>();
            this.ip_ti_tco_1_is_ivvt_n_maf_recalculated.ItemsSource = ip_ti_tco_1_is_ivvt_n_maf_recalculated;
            var Afr_from_log = this.Afr_from_log.Items.SourceCollection as ObservableCollection<DataObject>;
            var target_afr = this.target_afr.Items.SourceCollection as ObservableCollection<DataObject>;
            var IP_TIB_current = this.IP_TIB_current.Items.SourceCollection as ObservableCollection<DataObject>;
            var IP_TI_tco_2_PL_IVVT_x_current = this.IP_TI_tco_2_PL_IVVT_x_current.Items.SourceCollection as ObservableCollection<DataObject>;
            var IP_TI_tco_1_PL_IVVT_x_current = this.IP_TI_tco_1_PL_IVVT_x_current.Items.SourceCollection as ObservableCollection<DataObject>;
            var ip_ti_tco_2_is_ivvt_n_maf_current = this.ip_ti_tco_2_is_ivvt_n_maf_current.Items.SourceCollection as ObservableCollection<DataObject>;
            var ip_ti_tco_1_is_ivvt_n_maf_current = this.ip_ti_tco_1_is_ivvt_n_maf_current.Items.SourceCollection as ObservableCollection<DataObject>;
            var Afr_from_log_idle = this.Afr_from_log_idle.Items.SourceCollection as ObservableCollection<DataObject>;
            var target_afr_idle = this.target_afr_idle.Items.SourceCollection as ObservableCollection<DataObject>;
            for (int i = 0; i < Afr_from_log.Count; i++)
            {
                IP_TIB_recalculated.Add(new DataObject()
                {
                    A = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].A) * Convert.ToDecimal(Afr_from_log[i].A) / Convert.ToDecimal(target_afr[i].A), 3)).Replace(',', '.'),
                    B = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].B) * Convert.ToDecimal(Afr_from_log[i].B) / Convert.ToDecimal(target_afr[i].B), 3)).Replace(',', '.'),
                    C = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].C) * Convert.ToDecimal(Afr_from_log[i].C) / Convert.ToDecimal(target_afr[i].C), 3)).Replace(',', '.'),
                    D = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].D) * Convert.ToDecimal(Afr_from_log[i].D) / Convert.ToDecimal(target_afr[i].D), 3)).Replace(',', '.'),
                    E = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].E) * Convert.ToDecimal(Afr_from_log[i].E) / Convert.ToDecimal(target_afr[i].E), 3)).Replace(',', '.'),
                    F = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].F) * Convert.ToDecimal(Afr_from_log[i].F) / Convert.ToDecimal(target_afr[i].F), 3)).Replace(',', '.'),
                    G = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].G) * Convert.ToDecimal(Afr_from_log[i].G) / Convert.ToDecimal(target_afr[i].G), 3)).Replace(',', '.'),
                    H = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].H) * Convert.ToDecimal(Afr_from_log[i].H) / Convert.ToDecimal(target_afr[i].H), 3)).Replace(',', '.'),
                    I = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].I) * Convert.ToDecimal(Afr_from_log[i].I) / Convert.ToDecimal(target_afr[i].I), 3)).Replace(',', '.'),
                    Y = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].Y) * Convert.ToDecimal(Afr_from_log[i].Y) / Convert.ToDecimal(target_afr[i].Y), 3)).Replace(',', '.'),
                    K = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].K) * Convert.ToDecimal(Afr_from_log[i].K) / Convert.ToDecimal(target_afr[i].K), 3)).Replace(',', '.'),
                    L = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TIB_current[i].L) * Convert.ToDecimal(Afr_from_log[i].L) / Convert.ToDecimal(target_afr[i].L), 3)).Replace(',', '.')

                });
            }
            for (int i = 0; i < Afr_from_log.Count; i++)
            {
                IP_TI_tco_2_PL_IVVT_x_recalculated.Add(new DataObject()
                {
                    A = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].A) * Convert.ToDecimal(Afr_from_log[i].A) / Convert.ToDecimal(target_afr[i].A), 3)).Replace(',', '.'),
                    B = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].B) * Convert.ToDecimal(Afr_from_log[i].B) / Convert.ToDecimal(target_afr[i].B), 3)).Replace(',', '.'),
                    C = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].C) * Convert.ToDecimal(Afr_from_log[i].C) / Convert.ToDecimal(target_afr[i].C), 3)).Replace(',', '.'),
                    D = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].D) * Convert.ToDecimal(Afr_from_log[i].D) / Convert.ToDecimal(target_afr[i].D), 3)).Replace(',', '.'),
                    E = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].E) * Convert.ToDecimal(Afr_from_log[i].E) / Convert.ToDecimal(target_afr[i].E), 3)).Replace(',', '.'),
                    F = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].F) * Convert.ToDecimal(Afr_from_log[i].F) / Convert.ToDecimal(target_afr[i].F), 3)).Replace(',', '.'),
                    G = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].G) * Convert.ToDecimal(Afr_from_log[i].G) / Convert.ToDecimal(target_afr[i].G), 2)).Replace(',', '.'),
                    H = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].H) * Convert.ToDecimal(Afr_from_log[i].H) / Convert.ToDecimal(target_afr[i].H), 3)).Replace(',', '.'),
                    I = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].I) * Convert.ToDecimal(Afr_from_log[i].I) / Convert.ToDecimal(target_afr[i].I), 3)).Replace(',', '.'),
                    Y = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].Y) * Convert.ToDecimal(Afr_from_log[i].Y) / Convert.ToDecimal(target_afr[i].Y), 3)).Replace(',', '.'),
                    K = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].K) * Convert.ToDecimal(Afr_from_log[i].K) / Convert.ToDecimal(target_afr[i].K), 3)).Replace(',', '.'),
                    L = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_2_PL_IVVT_x_current[i].L) * Convert.ToDecimal(Afr_from_log[i].L) / Convert.ToDecimal(target_afr[i].L), 3)).Replace(',', '.')

                });
            }
            for (int i = 0; i < Afr_from_log.Count; i++)
            {
                IP_TI_tco_1_PL_IVVT_x_recalculated.Add(new DataObject()
                {
                    A = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].A) * Convert.ToDecimal(Afr_from_log[i].A) / Convert.ToDecimal(target_afr[i].A), 3)).Replace(',', '.'),
                    B = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].B) * Convert.ToDecimal(Afr_from_log[i].B) / Convert.ToDecimal(target_afr[i].B), 3)).Replace(',', '.'),
                    C = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].C) * Convert.ToDecimal(Afr_from_log[i].C) / Convert.ToDecimal(target_afr[i].C), 3)).Replace(',', '.'),
                    D = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].D) * Convert.ToDecimal(Afr_from_log[i].D) / Convert.ToDecimal(target_afr[i].D), 3)).Replace(',', '.'),
                    E = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].E) * Convert.ToDecimal(Afr_from_log[i].E) / Convert.ToDecimal(target_afr[i].E), 3)).Replace(',', '.'),
                    F = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].F) * Convert.ToDecimal(Afr_from_log[i].F) / Convert.ToDecimal(target_afr[i].F), 3)).Replace(',', '.'),
                    G = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].G) * Convert.ToDecimal(Afr_from_log[i].G) / Convert.ToDecimal(target_afr[i].G), 3)).Replace(',', '.'),
                    H = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].H) * Convert.ToDecimal(Afr_from_log[i].H) / Convert.ToDecimal(target_afr[i].H), 3)).Replace(',', '.'),
                    I = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].I) * Convert.ToDecimal(Afr_from_log[i].I) / Convert.ToDecimal(target_afr[i].I), 3)).Replace(',', '.'),
                    Y = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].Y) * Convert.ToDecimal(Afr_from_log[i].Y) / Convert.ToDecimal(target_afr[i].Y), 3)).Replace(',', '.'),
                    K = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].K) * Convert.ToDecimal(Afr_from_log[i].K) / Convert.ToDecimal(target_afr[i].K), 3)).Replace(',', '.'),
                    L = Convert.ToString(Math.Round(Convert.ToDecimal(IP_TI_tco_1_PL_IVVT_x_current[i].L) * Convert.ToDecimal(Afr_from_log[i].L) / Convert.ToDecimal(target_afr[i].L), 3)).Replace(',', '.')

                });
            }
            for (int i = 0; i < Afr_from_log_idle.Count; i++)
            {
                ip_ti_tco_2_is_ivvt_n_maf_recalculated.Add(new DataObject()
                {
                    A = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].A) * Convert.ToDecimal(Afr_from_log_idle[i].A) / Convert.ToDecimal(target_afr_idle[i].A), 2)).Replace(',', '.'),
                    B = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].B) * Convert.ToDecimal(Afr_from_log_idle[i].B) / Convert.ToDecimal(target_afr_idle[i].B), 2)).Replace(',', '.'),
                    C = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].C) * Convert.ToDecimal(Afr_from_log_idle[i].C) / Convert.ToDecimal(target_afr_idle[i].C), 2)).Replace(',', '.'),
                    D = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].D) * Convert.ToDecimal(Afr_from_log_idle[i].D) / Convert.ToDecimal(target_afr_idle[i].D), 2)).Replace(',', '.'),
                    E = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].E) * Convert.ToDecimal(Afr_from_log_idle[i].E) / Convert.ToDecimal(target_afr_idle[i].E), 2)).Replace(',', '.'),
                    F = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_2_is_ivvt_n_maf_current[i].F) * Convert.ToDecimal(Afr_from_log_idle[i].F) / Convert.ToDecimal(target_afr_idle[i].F), 2)).Replace(',', '.')
                    

                });
            }
            for (int i = 0; i < Afr_from_log_idle.Count; i++)
            {
                ip_ti_tco_1_is_ivvt_n_maf_recalculated.Add(new DataObject()
                {
                    A = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].A) * Convert.ToDecimal(Afr_from_log_idle[i].A) / Convert.ToDecimal(target_afr_idle[i].A), 2)).Replace(',', '.'),
                    B = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].B) * Convert.ToDecimal(Afr_from_log_idle[i].B) / Convert.ToDecimal(target_afr_idle[i].B), 2)).Replace(',', '.'),
                    C = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].C) * Convert.ToDecimal(Afr_from_log_idle[i].C) / Convert.ToDecimal(target_afr_idle[i].C), 2)).Replace(',', '.'),
                    D = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].D) * Convert.ToDecimal(Afr_from_log_idle[i].D) / Convert.ToDecimal(target_afr_idle[i].D), 2)).Replace(',', '.'),
                    E = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].E) * Convert.ToDecimal(Afr_from_log_idle[i].E) / Convert.ToDecimal(target_afr_idle[i].E), 2)).Replace(',', '.'),
                    F = Convert.ToString(Math.Round(Convert.ToDecimal(ip_ti_tco_1_is_ivvt_n_maf_current[i].F) * Convert.ToDecimal(Afr_from_log_idle[i].F) / Convert.ToDecimal(target_afr_idle[i].F), 2)).Replace(',', '.')


                });
            }


        }
    }
}

