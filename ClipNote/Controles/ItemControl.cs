using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eClipx.Controles
{
    internal partial class ItemControl : UserControl
    {
        private static Color ODD_COLOR = System.Drawing.Color.AliceBlue;
        private static Color PAIR_COLOR = System.Drawing.Color.White;
        private static Color SELECTED_COLOR = System.Drawing.SystemColors.ActiveCaption;


        private int _ItemId;
        private int _Index;
        private Action<int> _OnItemSelected;
        private Action<int, int> _OnItemClicked;

        private bool _IsOdd;
        public bool IsOdd
        {
            get
            {
                return _IsOdd;
            }
            set
            {
                _IsOdd = value;
                SetBackColor();
            }
        }
                
        public string Text {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }

        private bool _IsInCloud;
        public bool IsInCloud {
            get
            {
                return _IsInCloud;
            }
            set
            {
                _IsInCloud = value;
                if (_IsInCloud)
                {
                    iconPicture.Image = eClipx.Properties.Resources.evernote_upload;
                }
                else
                {
                    iconPicture.Image = eClipx.Properties.Resources.evernote;
                }
            }
        }

        public int ItemId { get { return _ItemId; } }

        public ItemControl()
        {
            InitializeComponent();
        }

        public ItemControl(int index, bool isOdd, ClipBoardElement item, Action<int> onItemSelected, Action<int, int> onItemClicked): this()
        {
            IsOdd = isOdd;
            Text = item.Content.ToString();
            IsInCloud = item.IsInCloud;

            _Index = index;
            _ItemId = item.Id;
            _OnItemSelected = onItemSelected;
            _OnItemClicked = onItemClicked;

        }

        public void Select()
        {
            rootPanel.BackColor = SELECTED_COLOR;
            Refresh();
        }

        public void UnSelect()
        {
            SetBackColor();
            Refresh();
        }

        private void SetBackColor()
        {
            if (_IsOdd)
            {
                rootPanel.BackColor = ODD_COLOR;
            }
            else
            {
                rootPanel.BackColor = PAIR_COLOR;
            }
        }

        private void lblText_MouseClick(object sender, MouseEventArgs e)
        {
            _OnItemClicked(_Index, _ItemId);
        }

        private void controlMouseEnter(object sender, EventArgs e)
        {
            _OnItemSelected(_Index);
            Select();
        }

        private void controlMouseLeave(object sender, EventArgs e)
        {
            UnSelect();
        }

    }
}
