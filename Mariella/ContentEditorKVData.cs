using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mariella {
    public partial class ContentEditorKVData : UserControl, IContentEditor {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private System.Drawing.Text.PrivateFontCollection fonts = new System.Drawing.Text.PrivateFontCollection();
        Font myFont;

        public KVStruct loadedSave { get; set; }

        public ContentEditorKVData() {
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
            labelName.Font = myFont;
            labelValue.Font = myFont;
        }

        public void LoadContent(DataContainer saveData) {
            if (this.loadedSave != null) this.SaveContent();
            if (saveData != null) {
                KVStruct data = (KVStruct)saveData.data;
                this.loadedSave = data;
                this.RTName.Text = data.Key;
                this.RTValue.Text = data.Represent();
            }
        }

        public void SaveContent(DataContainer target = null) {
            if (target != null) {
                this.loadedSave = (KVStruct)target.data;
            }
            if (this.loadedSave?.Key != null) { //?
                this.loadedSave.Key = this.RTName.Text;
                this.loadedSave.Value = this.RTValue.Text;
            }
        }

        public void DropContent() {
            this.loadedSave = null;
            this.RTName.Clear();
            this.RTValue.Clear();
        }
    }
}
