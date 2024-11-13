using INI_file_study.Utilities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using System.Xml;


namespace INI_file_study
{
    public partial class Form1 : Form
    {
        string szIniFilePath = "Data/data.ini";
        string szXmlFilePath = "Data/data.xml";
        string szJsonFilePath = "Data/data.json";

        private XmlDocument _doc = new XmlDocument();
        private jsonFile.Root _data;
        
        private Dictionary<TextBox, (string, string)> _arrIniT;
        private Dictionary<TextBox, string> _arrXmlT;
        private Dictionary<TextBox, (Func<double> getter, Action<double> setter)> _arrJsonT;

        private uint _sel = 0; //0:ini file, 1:xml file, 2:json file
        public Form1()
        {
            InitializeComponent();

            initTable();
            switch (this._sel)
            {
                case 0:
                    readIniFile();
                    break;
                case 1:
                    readXmlFile();
                    break;
                case 2:
                    readJsonFile();
                    break;
            }
        }

        private void initTable()
        {
            // For ini file
            this._arrIniT = new Dictionary<TextBox, (string, string)>
            {
                {txtR1Min, ("TestPoint1", "ImpedanceMin")},
                {txtR1Max, ("TestPoint1", "ImpedanceMax")},
                {txtV1Min, ("TestPoint1", "VoltageMin")},
                {txtV1Max, ("TestPoint1", "VoltageMax")},
                {txtI1Min, ("TestPoint1", "CurrentMin")},
                {txtI1Max, ("TestPoint1", "CurrentMax")},
                {txtP1Min, ("TestPoint1", "PowerMin")},
                {txtP1Max, ("TestPoint1", "PowerMax")},
                {txtR2Min, ("TestPoint2", "ImpedanceMin")},
                {txtR2Max, ("TestPoint2", "ImpedanceMax")},
                {txtV2Min, ("TestPoint2", "VoltageMin")},
                {txtV2Max, ("TestPoint2", "VoltageMax")},
            };

            // For xml file
            this._arrXmlT = new Dictionary<TextBox, string> 
            {
                {txtR1Min, "/root/TestPoint1/Impedance/Min"},
                {txtR1Max, "/root/TestPoint1/Impedance/Max"},
                {txtV1Min, "/root/TestPoint1/Voltage/Min"},
                {txtV1Max, "/root/TestPoint1/Voltage/Max"},
                {txtI1Min, "/root/TestPoint1/Current/Min"},
                {txtI1Max, "/root/TestPoint1/Current/Max"},
                {txtP1Min, "/root/TestPoint1/Power/Min"},
                {txtP1Max, "/root/TestPoint1/Power/Max"},
                {txtR2Min, "/root/TestPoint2/Impedance/Min"},
                {txtR2Max, "/root/TestPoint2/Impedance/Max"},
                {txtV2Min, "/root/TestPoint2/Voltage/Min"},
                {txtV2Max, "/root/TestPoint2/Voltage/Max"},
            };

            // For json file
            this._arrJsonT = new Dictionary<TextBox, (Func<double> getter, Action<double> setter)>
            {
                {txtR1Min, (() => _data.TestPoint1.Impedance.Min, value => _data.TestPoint1.Impedance.Min = value) },
                {txtR1Max, (() => _data.TestPoint1.Impedance.Max, value => _data.TestPoint1.Impedance.Max = value) },
                {txtV1Min, (() => _data.TestPoint1.Voltage.Min, value => _data.TestPoint1.Voltage.Min = value) },
                {txtV1Max, (() => _data.TestPoint1.Voltage.Max, value => _data.TestPoint1.Voltage.Max = value) },
                {txtI1Min, (() => _data.TestPoint1.Current.Min, value => _data.TestPoint1.Current.Min = value) },
                {txtI1Max, (() => _data.TestPoint1.Current.Max, value => _data.TestPoint1.Current.Max = value) },
                {txtP1Min, (() => _data.TestPoint1.Power.Min, value => _data.TestPoint1.Power.Min = value) },
                {txtP1Max, (() => _data.TestPoint1.Power.Max, value => _data.TestPoint1.Power.Max = value) },
                {txtR2Min, (() => _data.TestPoint2.Impedance.Min, value => _data.TestPoint2.Impedance.Min = value) },
                {txtR2Max, (() => _data.TestPoint2.Impedance.Max, value => _data.TestPoint2.Impedance.Max = value) },
                {txtV2Min, (() => _data.TestPoint2.Voltage.Min, value => _data.TestPoint2.Voltage.Min = value) },
                {txtV2Max, (() => _data.TestPoint2.Voltage.Max, value => _data.TestPoint2.Voltage.Max = value) },
            };
        }

        private void textChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && _arrJsonT.TryGetValue(textBox, out var binding))
            {
                if (double.TryParse(textBox.Text, out double newValue))
                {
                    // Update the corresponding JSON property
                    binding.setter(newValue);
                }
            }
        }

        private void readIniFile()
        {
            var iniFile = new IniFile(szIniFilePath);

            foreach (var item in this._arrIniT)
            {
                item.Key.Text = iniFile.Read(item.Value.Item1, item.Value.Item2);
            }
        }

        private void writeIniFile()
        {
            var iniFile = new IniFile(szIniFilePath);

            foreach (var item in this._arrIniT)
            {
                iniFile.Write(item.Value.Item1, item.Value.Item2, item.Key.Text);
            }
        }

        private void readJsonFile()
        {
            string jsonString = File.ReadAllText(this.szJsonFilePath);
            this._data = JsonSerializer.Deserialize<jsonFile.Root>(jsonString);

            foreach (var textBox in this._arrJsonT.Keys)
            {
                textBox.Text = this._arrJsonT[textBox].getter().ToString();
                textBox.TextChanged += textChanged;
            }
        }
        private void writeJsonFile()
        {
            var option = new JsonSerializerOptions { WriteIndented = true };
            string szJsonStr = JsonSerializer.Serialize(this._data, option);
            File.WriteAllText(this.szJsonFilePath, szJsonStr);
        }

        private void readXmlFile()
        {
            this._doc.Load(szXmlFilePath);

            foreach (var entry in this._arrXmlT)
            {
                XmlNode node = this._doc.SelectSingleNode(entry.Value);
                if (null != node) 
                { 
                    entry.Key.Text = node.InnerText; 
                }
            }
        }
        private void writeXmlFile()
        {
            this._doc.Load(szXmlFilePath);

            foreach (var entry in this._arrXmlT)
            {
                XmlNode node = this._doc.SelectSingleNode(entry.Value);
                if (null != node)
                {
                    node.InnerText = entry.Key.Text;
                }
            }
            this._doc.Save(szXmlFilePath);
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            switch (this._sel)
            {
                case 0:
                    readIniFile();
                    break;
                case 1:
                    readXmlFile();
                    break;
                case 2:
                    readJsonFile();
                    break;
            }

        }
        private void btnWrite_Click(object sender, EventArgs e)
        {
            switch (this._sel)
            {
                case 0:
                    writeIniFile();
                    break;
                case 1:
                    writeXmlFile();
                    break;
                case 2:
                    writeJsonFile();
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear();
                }
            }
        }

    }
}
