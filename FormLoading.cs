using System.Drawing;
using System.Windows.Forms;

namespace CoAuthor
{
    public partial class FormLoading : Form
    {
        public delegate void DelegateTest(string tips);
        //使用delegate关键字,声明委托类型,参数和返回值都有要求,见下面说明
        public DelegateTest delegateTest;

        public static FormLoading form_loading;

        public FormLoading() {
            InitializeComponent();
            form_loading = this;

            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);

            this.ShowInTaskbar = false;//取消任务栏图标

            label_tips.Text = "";
        }


        /// <summary>
        /// 让程序不显示在alt+Tab视图窗体中
        /// </summary>
        protected override CreateParams CreateParams {
            get {
                const int WS_EX_APPWINDOW = 0x40000;
                const int WS_EX_TOOLWINDOW = 0x80;
                CreateParams cp = base.CreateParams;
                cp.ExStyle &= (~WS_EX_APPWINDOW);    // 不显示在TaskBar
                cp.ExStyle |= WS_EX_TOOLWINDOW;      // 不显示在Alt+Tab
                return cp;
            }
        }


        public void Loading() {
            form_loading.pictureBox_loading.Visible = true;
            form_loading.label_tips.Text = "loading";
        }
        public void HideLoading() {
            form_loading.pictureBox_loading.Visible = false;
            form_loading.label_tips.Text = "";
        }
    }
}
