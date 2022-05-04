using System;
using System.Windows.Forms;
using System.Drawing;

namespace Mariella {
    public partial class Main : Form {
        private const string status_initial = "Stanley dragged and dropped a file onto the window, or used the \"file > Open\" menu.";
        private static readonly string savefolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\Crows Crows Crows\\The Stanley Parable_ Ultra Deluxe\\tspud-savedata.txt";

        TSPSave loadedSave { get { return this.tabController.loadedSave; } }

        public TabController tabController;
        public Main() {
            InitializeComponent();
            menuStrip1.Renderer = new Mariella.DarkMenuRender();
            tabController = new TabController(tspTabControl1);
            tabController.Updating += new TabController.TabControllerUpdatingEventHandler(this.UpdateUIState);
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
            
            ProcessSaveData(SaveProcessor.ProcessSaveFile(path));
        }

        private void CreateSaveFile() {
            ProcessSaveData(SaveProcessor.CreateNew());
        }

        private void ProcessSaveData(TSPSave save) {
            this.tabController.AddTab(System.IO.Path.GetFileName(save?.path ?? ""));
            this.tabController.LoadSave(save);
            //UpdateUIState();
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
            Reload();
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e) {
            DestroySession();
        }

        public void Reload() {
            if (this.loadedSave?.path != null) this.tabController.LoadSave(SaveProcessor.ProcessSaveFile(this.loadedSave.path));
        }

        public void DestroySession() {
            this.tabController.CloseTab();
            
            UpdateUIState();
        }

        public void SaveSession(string path) {
            this.tabController.Save(path);
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e) {
            Reload();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e) {
            DestroySession();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (System.IO.File.Exists(savefolder)) {
                ImportSaveFile(savefolder);
            } else {
                MessageBox.Show("Hmmm. I'm sorry Stanley, but I'm having trouble locating your save file.\n Could you try opening it yourself?");
            }
        }

        private void UpdateUIState() {
            bool loaded = (this.tabController.ActiveTabs());
            bool physical = (loaded && this.loadedSave?.path != "");
            this.reloadToolStripMenuItem.Visible = physical;
            this.xToolStripMenuItem.Visible = loaded;
            this.saveToolStripMenuItem.Enabled = loaded;
            this.saveAsToolStripMenuItem.Enabled = loaded;
            this.reloadFileToolStripMenuItem.Enabled = physical;
            this.closeFileToolStripMenuItem.Enabled = loaded;
            this.tspTabControl1.Visible = loaded;
            this.toolStripLabelOpenSave.Visible = physical;
            this.fileStatusLabel.Text = loaded ? "Loaded file : " + ((this.loadedSave?.path == "") ? "(New Save)" : this.loadedSave?.path) : status_initial;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            this.CreateSaveFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            new About().ShowDialog();
        }

        private void openDefaultSaveFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(savefolder));
            } catch (Exception) {
                MessageBox.Show("Hmmm. I'm sorry Stanley, but I'm having trouble locating your save folder.\n Did you break something again?");
            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e) {
            if(!String.IsNullOrWhiteSpace(this.loadedSave?.path)) { System.Diagnostics.Process.Start(this.loadedSave.path); }
        }

        private void tspTabControl1_Selected(object sender, TabControlEventArgs e) {
           // this.loadedSave = GetCurrentSave();
        }
    }
}
