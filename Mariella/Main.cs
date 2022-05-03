using System;
using System.Windows.Forms;
using System.Drawing;

namespace Mariella {
    public partial class Main : Form {
        private const string status_initial = "Stanley dragged and dropped a file onto the window, or used the \"file > Open\" menu.";
        private TSPSave loadedSave;
        ContentEditorMain CEM1 = new ContentEditorMain(ContentEditorMain.ContentType.Data);
        ContentEditorMain CEM2 = new ContentEditorMain(ContentEditorMain.ContentType.KV);
        ContentEditorMain CEM3 = new ContentEditorMain(ContentEditorMain.ContentType.KV);
        ContentEditorMain CEM4 = new ContentEditorMain(ContentEditorMain.ContentType.KV);
        ContentEditorMain CEM5 = new ContentEditorMain(ContentEditorMain.ContentType.KV);
        public Main() {
            InitializeComponent();
            menuStrip1.Renderer = new Mariella.DarkMenuRender();
            this.tabPage1.Controls.Add(CEM1);
            this.tabPage2.Controls.Add(CEM2);
            this.tabPage3.Controls.Add(CEM3);
            this.tabPage4.Controls.Add(CEM4);
            this.tabPage5.Controls.Add(CEM5);
            this.fileStatusLabel.Text = status_initial;
        }

        private void Main_Load(object sender, EventArgs e) {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        void Main_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        void Main_DragDrop(object sender, DragEventArgs e) {
            foreach (string file in (string[])e.Data.GetData(DataFormats.FileDrop)) {
                ImportSaveFile(file);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog OFD = new OpenFileDialog {
                Filter = "Text Files | *.txt|All files (*.*)|*.*"
            };
            if (OFD.ShowDialog() == DialogResult.OK) {
                ImportSaveFile(OFD.FileName);
            }
        }

        private void ImportSaveFile(string path) {
            this.loadedSave = SaveProcessor.ProcessSaveFile(path);
            if (this.loadedSave != null) ProcessSaveData(this.loadedSave);
        }

        private void CreateSaveFile() {
            this.loadedSave = SaveProcessor.CreateNew();
            if (this.loadedSave != null) ProcessSaveData(this.loadedSave);
        }

        private void ProcessSaveData(TSPSave save) {
            CEM1.LoadSave(loadedSave, ContentEditorMain.ContentSection.Save);
            CEM2.LoadSave(loadedSave, ContentEditorMain.ContentSection.KVInteger);
            CEM3.LoadSave(loadedSave, ContentEditorMain.ContentSection.KVFloat);
            CEM4.LoadSave(loadedSave, ContentEditorMain.ContentSection.KVBoolean);
            CEM5.LoadSave(loadedSave, ContentEditorMain.ContentSection.KVString);
            this.fileStatusLabel.Text = "Loaded file : " + ((this.loadedSave.path == "") ? "(New Save)" : this.loadedSave.path);
            UpdateUIState();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFileDialog SFD = new SaveFileDialog {
                Filter = "Text File | *.txt|All files (*.*)|*.*",
                DefaultExt = ".txt",
                FileName = System.IO.Path.GetFileNameWithoutExtension(loadedSave.path)
            };
            if (SFD.ShowDialog() == DialogResult.OK) {
                SaveSession(SFD.FileName);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.loadedSave?.path == null) {
                saveAsToolStripMenuItem.PerformClick();
            } else {
                SaveSession(this.loadedSave.path);
            }
        }

        private void reloadFileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.loadedSave?.path != null) ImportSaveFile(this.loadedSave.path);
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e) {
            DestroySession();
        }

        public void DestroySession() {
            this.loadedSave = null;
            CEM1.DropData();
            CEM2.DropData();
            CEM3.DropData();
            CEM4.DropData();
            CEM5.DropData();
            this.fileStatusLabel.Text = status_initial;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
            UpdateUIState();
        }

        public void SaveSession(string path) {
            CEM1.SaveDataContainer();
            CEM2.SaveDataContainer();
            CEM2.SaveDataContainer();
            CEM4.SaveDataContainer();
            CEM5.SaveDataContainer();
            SaveProcessor.Save(path, this.loadedSave);
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.loadedSave?.path != null) ImportSaveFile(this.loadedSave.path);
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e) {
            DestroySession();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            string savefolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\Crows Crows Crows\\The Stanley Parable_ Ultra Deluxe\\tspud-savedata.txt";
            if (System.IO.File.Exists(savefolder)) {
                ImportSaveFile(savefolder);
            } else {
                MessageBox.Show("Hmmm. I'm sorry stanley, but I'm having trouble locating your save file.\n Could you try opening it yourself?");
            }
        }

        private void UpdateUIState() {
            bool loaded = (this.loadedSave != null);
            this.reloadToolStripMenuItem.Visible = (loaded && this.loadedSave.path != "");
            this.xToolStripMenuItem.Visible = loaded;
            this.saveToolStripMenuItem.Enabled = loaded;
            this.saveAsToolStripMenuItem.Enabled = loaded;
            this.reloadFileToolStripMenuItem.Enabled = (loaded && this.loadedSave.path != "");
            this.closeFileToolStripMenuItem.Enabled = loaded;
            this.tspTabControl1.Visible = loaded;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.loadedSave != null) {
                int Close = (int)MessageBox.Show("Wait! Stanley, you already have a save open. Are you sure you want to discard it?", "Sad Ending", MessageBoxButtons.YesNo);
                if (Close == (int)DialogResult.No) return;
            }
            this.DestroySession();
            this.CreateSaveFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("TSPUD Save Editor :)");
        }
    }
}
