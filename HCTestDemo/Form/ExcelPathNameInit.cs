using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCTestDemo
{
    public delegate void SaveMsgInitDelegate(string filepath, int time, List<int> list, bool cmd);
    public partial class ExcelPathNameInit : Form
    {
        SaveMsgInitDelegate _saveMsgInit;
        string path;

        public ExcelPathNameInit(SaveMsgInitDelegate saveMsgInit,string path)
        {
            InitializeComponent();
            this._saveMsgInit = saveMsgInit;
            this.path = path;
        }
        public ExcelPathNameInit()
        {
            InitializeComponent();
            
        }


        private void ExcelPathNameInit_Load(object sender, EventArgs e)
        {

            fileSaveName.Text = DateTime.Now.ToString("HHmmss");
            textBox1.Text =System.IO.Path.Combine(path, (DateTime.Now.ToString("HHmmss") + ".xlsx")); 
            SelectFiles.SelectAll();

        }

        private void bt_Confirm_Click(object sender, EventArgs e)
        {
            List<int> selectIndex = new List<int>(); ;
            selectIndex.AddRange(SelectFiles.SelectedIndexes);
            textBox1.Text = System.IO.Path.Combine(path, (fileSaveName.Text + ".xlsx"));
            int saveInterval;
            if (int.TryParse(SaveInterval.Text, out saveInterval))
            {
                if (SelectFiles.SelectedIndexes.Count != 0)
                {
                    _saveMsgInit(textBox1.Text, saveInterval, selectIndex, true);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("未选择保存项目！");

                }

            }
            else
            {
                MessageBox.Show("保存间隔格式不对！");
            }

        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            List<int> selectIndex = new List<int>(); ;
            selectIndex.AddRange(SelectFiles.SelectedIndexes);
            _saveMsgInit(textBox1.Text, 100, selectIndex, false);
            this.Close();

        }




    }
}
