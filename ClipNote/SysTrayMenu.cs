using eClipx.Controles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eClipx
{
    internal class ClipBoardElement
    {
        public enum EnumFormat { Text, Image };

        public EnumFormat ContenteFormat { get; set; }
        public object Content { get; set; }
        public TextDataFormat ClipboardTextFormat { get; set; }

        public bool IsInCloud { get; set; }
        public bool MustBeInCloud { get; set; }

        public int Id
        {
            get
            {
                int hash = 0;
                if (this.ContenteFormat == EnumFormat.Text)
                    hash = ((string)Content).GetHashCode();
                else
                {
                    hash = ((System.Drawing.Image)Content).GetHashCode();
                }
                return hash;
            }
        }
    }


    public class SysTrayMenu : Form
    {
        private const int CACHE_SIZE = 100;
        private const int OUT_COOR = -4000;
        private const int FORM_WIDTH = 600;
        private const char KEY_X = 'x';
        private const string SEARCH_SHORT_CUT = "Ctrl + S";

        private NotifyIcon trayIcon;
        private Panel rootPanel;
        private Panel topPanel;
        private PictureBox pictureBox1;
        private TextBox textBox1;
        private Label separatorLabel;
        private FlowLayoutPanel listViewRootPanel;
        private ContextMenu trayMenu;

        private int selectedIndex = -1;
        private MruList<int, ClipBoardElement> clipboardItems;
        private List<ItemControl> controlItems;

        private IntPtr nextClipboardViewer;

        public SysTrayMenu()
        {
            InitializeComponent();

            this.ClientSize = new System.Drawing.Size(FORM_WIDTH, Screen.PrimaryScreen.WorkingArea.Height);
            this.LostFocus += (object sender, EventArgs e) => MoveOutScreen();

            MoveOutScreen();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayMenu.MenuItems.Add("Show", OnShow);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "eClipx";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            KeyboardHook.CallWhen(true, true, Keys.X, GlobalHookKeyPress);

            Common.MyHandle = Handle;
            CurrentApp.StartWatcher();

            clipboardItems = new MruList<int, ClipBoardElement>(CACHE_SIZE);
            controlItems = new List<ItemControl>(CACHE_SIZE);

            nextClipboardViewer = (IntPtr)User32.SetClipboardViewer((int)this.Handle);
        }

        private void BindList()
        {
            Action<int> currentIndexCallback = index => UpdateSelectedIndex(index);
            Action<int, int> itemClickedCallback = (index, itemId) => {
                UpdateSelectedIndex(index);
                OnPaste(itemId);
            };

            listViewRootPanel.SuspendLayout();
            controlItems.Clear();
            listViewRootPanel.Controls.Clear();
            selectedIndex = -1;
            int i = 0;
            foreach(var sortedItem in clipboardItems)
            {
                var item = sortedItem;
                var controlItem = new ItemControl(i, i % 2 != 0, item, currentIndexCallback, itemClickedCallback);
                listViewRootPanel.Controls.Add(controlItem);
                controlItems.Add(controlItem);
                i++;
            }
            listViewRootPanel.ResumeLayout();
        }

        private void GlobalHookKeyPress()
        {
            MoveInScreen();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnShow(object sender, EventArgs e)
        {
            Common.ForegroundWindow = User32.GetForegroundWindow();
            MoveInScreen();
        }

        private void OnPaste(int itemId)
        {
            var item = clipboardItems.GetItem(itemId, null);
            MoveOutScreen();
            CurrentApp.SendKeys(item.Content.ToString());
        }

        private void UpdateSelectedIndex(int nextIndex)
        {
            if (controlItems.Count > 0)
            {
                if (nextIndex >= controlItems.Count)
                {
                    nextIndex = 0;
                }

                if (nextIndex < 0)
                {
                    nextIndex = controlItems.Count - 1;
                }

                if (selectedIndex >= 0)
                {
                    controlItems[selectedIndex].UnSelect();
                }

                controlItems[nextIndex].Select();
                selectedIndex = nextIndex;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool processed = false;
            if (keyData == Keys.Escape)
            {
                MoveOutScreen();
                processed = true;
            }
            else if (keyData == Keys.Down)
            {
                UpdateSelectedIndex(selectedIndex + 1);
                processed = true;
            }
            else if (keyData == Keys.Up)
            {
                UpdateSelectedIndex(selectedIndex - 1);
                processed = true;
            }
            else if (keyData == Keys.Enter)
            {
                if (selectedIndex >= 0)
                {
                    OnPaste(controlItems[selectedIndex].ItemId);
                }
                processed = true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                if (textBox1.Text == SEARCH_SHORT_CUT)
                {
                    textBox1.Text = "";
                    textBox1.ForeColor = Color.Black;
                }
                textBox1.Focus();
                processed = true;
            }
            return processed || base.ProcessCmdKey(ref msg, keyData);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void MoveOutScreen()
        {
            Location = new Point(OUT_COOR, OUT_COOR);
            TopMost = false;
            User32.ShowWindow(Common.MyHandle, User32.WindowShowStyle.Hide);
            User32.SetForegroundWindow(CurrentApp.Handler);
        }

        private void MoveInScreen()
        {
            BindList();
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width / 2) - (FORM_WIDTH / 2), 0);
            User32.ShowWindow(Common.MyHandle, User32.WindowShowStyle.Show);
            User32.SetForegroundWindow(Common.MyHandle);
            TopMost = true;
        }

        private void SysTrayMenu_Deactivate(object sender, EventArgs e)
        {
            MoveOutScreen();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    OnClipboardChange();
                    User32.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        User32.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void OnClipboardChange()
        {
            try
            {
                TextDataFormat textDataFormat = TextDataFormat.Text;
                ClipBoardElement.EnumFormat elementFormat = ClipBoardElement.EnumFormat.Text;
                object data = Clipboard.GetText();

                if (data != null && data.ToString().Trim() != "")
                {
                    var item = new ClipBoardElement()
                    {
                        ClipboardTextFormat = textDataFormat,
                        ContenteFormat = elementFormat,
                        Content = data,
                        IsInCloud = false
                    };

                    clipboardItems.GetItem(item.Id, item);
                }

            }
            catch (Exception e)
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trayIcon.Dispose();
                CurrentApp.StopWatcher();
                KeyboardHook.Unbind();
                User32.ChangeClipboardChain(this.Handle, nextClipboardViewer);
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.rootPanel = new System.Windows.Forms.Panel();
            this.listViewRootPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.separatorLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rootPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // rootPanel
            // 
            this.rootPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rootPanel.Controls.Add(this.listViewRootPanel);
            this.rootPanel.Controls.Add(this.topPanel);
            this.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootPanel.Location = new System.Drawing.Point(0, 0);
            this.rootPanel.Name = "rootPanel";
            this.rootPanel.Size = new System.Drawing.Size(600, 262);
            this.rootPanel.TabIndex = 0;
            // 
            // listViewRootPanel
            // 
            this.listViewRootPanel.BackColor = System.Drawing.Color.White;
            this.listViewRootPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewRootPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.listViewRootPanel.Location = new System.Drawing.Point(0, 35);
            this.listViewRootPanel.Margin = new System.Windows.Forms.Padding(0);
            this.listViewRootPanel.Name = "listViewRootPanel";
            this.listViewRootPanel.Size = new System.Drawing.Size(598, 225);
            this.listViewRootPanel.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.separatorLabel);
            this.topPanel.Controls.Add(this.pictureBox1);
            this.topPanel.Controls.Add(this.textBox1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(598, 35);
            this.topPanel.TabIndex = 1;
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separatorLabel.Location = new System.Drawing.Point(0, 33);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(598, 2);
            this.separatorLabel.TabIndex = 2;
            this.separatorLabel.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(555, 26);
            this.textBox1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::eClipx.Properties.Resources.Ok;
            this.pictureBox1.InitialImage = global::eClipx.Properties.Resources.snipper;
            this.pictureBox1.Location = new System.Drawing.Point(564, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 28);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // SysTrayMenu
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 262);
            this.Controls.Add(this.rootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SysTrayMenu";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "eClipx";
            this.Deactivate += new System.EventHandler(this.SysTrayMenu_Deactivate);
            this.rootPanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
