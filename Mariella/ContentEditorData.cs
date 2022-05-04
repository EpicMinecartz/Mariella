using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mariella {
    public partial class ContentEditorData : UserControl, IContentEditor {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private System.Drawing.Text.PrivateFontCollection fonts = new System.Drawing.Text.PrivateFontCollection();
        private Font myFont;

        public SaveDataStruct loadedSave { get; set; }
        public ContentEditorMain parent { get; set; }

        public ContentEditorData(ContentEditorMain parent) {
            InitializeComponent();
            byte[] fontData = Properties.Resources.LeagueGothic_Regular;
            int fontlength = fontData.Length;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, fontlength);
            AddFontMemResourceEx(fontPtr, (uint)fontlength, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);

            myFont = new Font(fonts.Families[0], 16.0F);
            foreach (Control control in this.splitContainer1.Panel2.Controls) {
                control.Font = myFont;
            }
            foreach (Control control in this.splitContainer1.Panel1.Controls) {
                control.Font = myFont;
            }
            this.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            this.parent = parent;
        }

        public void LoadContent(DataContainer saveData) {
            if (this.loadedSave != null) this.SaveContent();
            if (saveData != null) {
                SaveDataStruct data = (SaveDataStruct)saveData.data;
                this.loadedSave = data;
                this.comboType.SelectedIndex = data.configureableType;
                this.RTName.Text = data.key;
                this.RTInteger.Text = data.IntValue.ToString();
                this.RTFloat.Text = data.FloatValue.ToString();
                this.checkState.Checked = data.BooleanValue;
                this.RTString.Text = data.StringValue;
            }
        }

        public void SaveContent(DataContainer target = null) {
            if(target != null) {
                if(this.loadedSave == null) throw new Exception("Stanley! You've broken my application!");
                this.loadedSave = (SaveDataStruct)target.data;
            }
            this.loadedSave.configureableType = this.comboType.SelectedIndex;
            this.loadedSave.key = this.RTName.Text;
            float rtf = 0.0f;
            int rti = 0;
            Single.TryParse(this.RTFloat.Text, out rtf);
            this.loadedSave.FloatValue = rtf;
            Int32.TryParse(this.RTInteger.Text, out rti);
            this.loadedSave.IntValue = rti;
            this.loadedSave.BooleanValue = this.checkState.Checked;
            this.loadedSave.StringValue = this.RTString.Text;
        }

        private void UpdateTypeState(int type) {
            Control ctrl = null;
            Control ctrl2 = null;
            switch (type) {
                case 0:
                    ctrl = RTInteger;
                    ctrl2 = labelInteger;
                    break;
                case 1:
                    ctrl = RTFloat;
                    ctrl2 = labelFloat;
                    break;
                case 2:
                    ctrl = checkState;
                    ctrl2 = labelState;
                    break;
                case 3:
                    ctrl = RTString;
                    ctrl2 = labelString;
                    break;
            }
            foreach (Control control in this.splitContainer1.Panel2.Controls) {
                if (control == ctrl || control == ctrl2) {
                    control.ForeColor = Color.FromArgb(240, 240, 240);
                    if (control is RichTextBox) control.BackColor = Color.FromArgb(40, 40, 40);
                } else {
                    control.ForeColor = Color.FromArgb(64, 64, 64);
                    if (control is RichTextBox) control.BackColor = Color.FromArgb(35, 35, 35);
                }
            }
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateTypeState(comboType.SelectedIndex);
        }

        public void DropContent() {
            this.loadedSave = null;
            this.RTName.Clear();
            this.RTFloat.Clear();
            this.checkState.Checked = false;
            this.RTString.Clear();
        }

        private void checkState_CheckedChanged(object sender, EventArgs e) {
            checkState.Text = (checkState.Checked) ? "true" : "false";
        }

        private void RTName_TextChanged(object sender, EventArgs e) {
            this.parent.RecieveContentUpdate(ContentEditorMain.UpdateState.Name, RTName.Text);
        }
    }
}
