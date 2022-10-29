using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCTestDemo
{
    public partial class LoadFrm : Form
    {
        public LoadFrm()
        {
            InitializeComponent();
        }

        private void LoadFrm_Load(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            foreach (var type in this.GetType().Assembly.GetTypes())
            {
                if (typeof(Form).IsAssignableFrom(type))
                {
                    try
                    {
                        using (Form form = Activator.CreateInstance(type) as Form)
                        {
                            names.Add(form.Name); //names.Add(form.name)
                            lbx_TestForm.Items.Add(form.Name);
                        }
                    }
                    catch
                    {
                        throw new Exception(string.Format("类型为 {0} 的窗体没有无参的构造函数，无法获取其名称", type));
                    }
                }
            }
            //从items里去除一些非项目主界面的窗体名称
            lbx_TestForm.Items.Remove("ExcelPathNameInit");
            lbx_TestForm.Items.Remove("LoadFrm");
            lbx_TestForm.Items.Remove("LinearReg");
            lbx_TestForm.Items.Remove("AnalogKB");
            lbx_TestForm.SelectedIndex = 0;
            lbx_TestForm.Font = new Font(this.Font.FontFamily, 15);//20改成你想要的字体大小;
        }

        private void bt_OpenTest_Click(object sender, EventArgs e)
        {
            //反射调用窗体名打开窗体
            Assembly assembly = Assembly.GetExecutingAssembly();

            //反射调用有参的构造方法
            //将关闭主测试程序命令传给测试项目，也可以将方法，参数等传递
            object[] ObjArray = new object[1];
            ObjArray[0] = new Action(() => { this.Show(); });
            Form form = assembly.CreateInstance("HCTestDemo." + lbx_TestForm.SelectedItem.ToString(), true, BindingFlags.Default, null, ObjArray, null, null) as Form;

            //反射调用无参的构造方法
            //Form form = (Form)assembly.CreateInstance("HCTestDemo." + lbx_TestForm.SelectedItem.ToString());

            //打开选中的项目窗体
            form.Show();
            //隐藏主窗体
            this.Hide();
        }

        private void lbx_TestForm_SelectedIndexChanged(object sender, EventArgs e)
        {

            //反射调用窗体名打开窗体
            Assembly assembly = Assembly.GetExecutingAssembly();
            //反射调用无参的构造方法
            Form form = (Form)assembly.CreateInstance("HCTestDemo." + lbx_TestForm.SelectedItem.ToString());
            label3.Text = form.Text;
        }
    }
}
