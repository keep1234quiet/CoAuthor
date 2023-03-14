namespace CoAuthor
{
    partial class RibbonChatGPT : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonChatGPT()
            : base(Globals.Factory.GetRibbonFactory()) {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.tab_title = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.toggleButton_autowrite = this.Factory.CreateRibbonToggleButton();
            this.button_refactor = this.Factory.CreateRibbonButton();
            this.button_custom_command_1 = this.Factory.CreateRibbonButton();
            this.button_custom_command_2 = this.Factory.CreateRibbonButton();
            this.button_custom_command_3 = this.Factory.CreateRibbonButton();
            this.button_setting = this.Factory.CreateRibbonButton();
            this.button_help = this.Factory.CreateRibbonButton();
            this.tab_title.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_title
            // 
            this.tab_title.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab_title.Groups.Add(this.group1);
            this.tab_title.Label = "CoAuthor";
            this.tab_title.Name = "tab_title";
            // 
            // group1
            // 
            this.group1.Items.Add(this.toggleButton_autowrite);
            this.group1.Items.Add(this.button_refactor);
            this.group1.Items.Add(this.button_custom_command_1);
            this.group1.Items.Add(this.button_custom_command_2);
            this.group1.Items.Add(this.button_custom_command_3);
            this.group1.Items.Add(this.button_setting);
            this.group1.Items.Add(this.button_help);
            this.group1.Label = "ChatGPT";
            this.group1.Name = "group1";
            // 
            // toggleButton_autowrite
            // 
            this.toggleButton_autowrite.Checked = true;
            this.toggleButton_autowrite.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.toggleButton_autowrite.Image = global::CoAuthor.Properties.Resources.pen_write_blue;
            this.toggleButton_autowrite.Label = "自动续写";
            this.toggleButton_autowrite.Name = "toggleButton_autowrite";
            this.toggleButton_autowrite.ShowImage = true;
            this.toggleButton_autowrite.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.toggleButton_autowrite_Click);
            // 
            // button_refactor
            // 
            this.button_refactor.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_refactor.Image = global::CoAuthor.Properties.Resources.refresh;
            this.button_refactor.Label = "重构内容";
            this.button_refactor.Name = "button_refactor";
            this.button_refactor.ShowImage = true;
            this.button_refactor.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_refactor_Click);
            // 
            // button_custom_command_1
            // 
            this.button_custom_command_1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_custom_command_1.Image = global::CoAuthor.Properties.Resources.command;
            this.button_custom_command_1.Label = "自定义指令1";
            this.button_custom_command_1.Name = "button_custom_command_1";
            this.button_custom_command_1.ShowImage = true;
            this.button_custom_command_1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_custom_command_1_Click);
            // 
            // button_custom_command_2
            // 
            this.button_custom_command_2.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_custom_command_2.Image = global::CoAuthor.Properties.Resources.command;
            this.button_custom_command_2.Label = "自定义指令2";
            this.button_custom_command_2.Name = "button_custom_command_2";
            this.button_custom_command_2.ShowImage = true;
            this.button_custom_command_2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_custom_command_2_Click);
            // 
            // button_custom_command_3
            // 
            this.button_custom_command_3.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_custom_command_3.Image = global::CoAuthor.Properties.Resources.command;
            this.button_custom_command_3.Label = "自定义指令3";
            this.button_custom_command_3.Name = "button_custom_command_3";
            this.button_custom_command_3.ShowImage = true;
            this.button_custom_command_3.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_custom_command_3_Click);
            // 
            // button_setting
            // 
            this.button_setting.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_setting.Image = global::CoAuthor.Properties.Resources.setting;
            this.button_setting.Label = "设置";
            this.button_setting.Name = "button_setting";
            this.button_setting.ShowImage = true;
            this.button_setting.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_setting_Click);
            // 
            // button_help
            // 
            this.button_help.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button_help.Image = global::CoAuthor.Properties.Resources.help;
            this.button_help.Label = "使用帮助";
            this.button_help.Name = "button_help";
            this.button_help.ShowImage = true;
            this.button_help.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button_help_Click);
            // 
            // RibbonChatGPT
            // 
            this.Name = "RibbonChatGPT";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab_title);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab_title.ResumeLayout(false);
            this.tab_title.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_help;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_refactor;
        protected internal Microsoft.Office.Tools.Ribbon.RibbonTab tab_title;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_custom_command_1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_setting;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton_autowrite;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_custom_command_2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton button_custom_command_3;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonChatGPT Ribbon1 {
            get { return this.GetRibbon<RibbonChatGPT>(); }
        }
    }
}
