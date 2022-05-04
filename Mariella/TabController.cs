using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mariella {

    /// <summary>
    /// This is a pretty rudimentary way of handling multiple files.
    /// </summary>
    public class TabController {
        public readonly TSPTabControl host;
        public TSPSave loadedSave { get { return GetCurrentManager()?.loadedSave; } }
        List<EditorManager> editorManagers;

        public TabController(TSPTabControl Host) { 
            this.host = Host;
            host.TabRequestDestroy += new TabRequestDestroyEventHandler(this.TabRequestDestroy);
            this.editorManagers = new List<EditorManager>();
        }

        public EditorManager GetCurrentManager() {
            if (this.editorManagers.Count > 0) {
                return this.editorManagers[this.host.SelectedIndex];
            } else {
                return null;
            }
        }

        public bool ActiveTabs() {
            return this.editorManagers.Count > 0;
        }

        public EditorManager AddTab(string tabName = "" ) {
            if(tabName == "") tabName = "New Save " + (this.editorManagers.Count() + 1);
            TabPage TP = new TabPage(tabName);
            EditorManager em = new EditorManager(this, TP);
            editorManagers.Add(em);
            host.TabPages.Add(TP);
            host.SelectedIndex = host.TabCount - 1;
            return em;
        }

        public void LoadSave(TSPSave save) {
            int tid = host.SelectedIndex;
            this.LoadSave(tid, save);
        }

        public void Reload() {
            this.LoadSave(this.loadedSave);
        }

        public void LoadSave(int tabID, TSPSave save) {
            this.editorManagers[tabID].Load(save);
            Updating.Invoke();
        }

        public void Save(string path) {
            int tid = (host.SelectedIndex + 1 == host.TabPages.Count) ? host.SelectedIndex : -1;
            this.Save(tid, path);
        }

        public void Save(int tabID, string path) {
            this.editorManagers[tabID].Save(path);
        }

        public void CloseTab(int? tabID = null) {
            int tid = tabID ?? ((host.SelectedIndex + 1 == host.TabPages.Count) ? host.SelectedIndex : -1);
            if (tid != -1) {
                DialogResult msg = MessageBox.Show("Are you sure you want to close this file?", "Close?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (msg == DialogResult.Yes) {
                    editorManagers[tid].DestroyTab();
                    editorManagers.RemoveAt(tid);
                    host.TabPages.RemoveAt(tid);
                    Updating.Invoke();
                }
            }
        }

        public void TabRequestDestroy(Object sender, int tabID = -1) {
            CloseTab(tabID);
        }

        [System.ComponentModel.Description("Occurs as controller performs an update.")]
        public event TabControllerUpdatingEventHandler Updating;
        public delegate void TabControllerUpdatingEventHandler();

    }

    /// <summary>
    /// Each EditorManager is essentially like having a new main
    /// </summary>
    public class EditorManager {
        public readonly TabPage hostTabPage;
        public readonly TabController hostTabController;
        public readonly List<TabPage> editorTabs;
        public readonly TSPTabControl editorTabControl; 
        public List<ContentEditorMain> contentEditors;
        public TSPSave loadedSave { get; private set; }

        public EditorManager(TabController Parent, TabPage Target) {
            this.editorTabControl = new TSPTabControl();
            this.hostTabPage = Target;
            this.hostTabController = Parent;
            this.editorTabs = new List<TabPage>();
            this.contentEditors = new List<ContentEditorMain> { //maybe dont look at this bit...
                new ContentEditorMain(ContentEditorMain.ContentType.Data, ContentEditorMain.ContentSection.Save),
                new ContentEditorMain(ContentEditorMain.ContentType.KV, ContentEditorMain.ContentSection.KVInteger),
                new ContentEditorMain(ContentEditorMain.ContentType.KV, ContentEditorMain.ContentSection.KVFloat),
                new ContentEditorMain(ContentEditorMain.ContentType.KV, ContentEditorMain.ContentSection.KVBoolean),
                new ContentEditorMain(ContentEditorMain.ContentType.KV, ContentEditorMain.ContentSection.KVString)
            };
            this.hostTabPage.Controls.Add(this.editorTabControl);
            this.editorTabControl.Dock = DockStyle.Fill;
            List<string> names = new List<string> { "Primary Data", "Int Data", "Float Data", "Bool Data", "String Data" };
            for (int i = 0;i < this.contentEditors.Count();i++) {
                TabPage TP = new TabPage(names[i]);
                editorTabs.Add(TP);
                this.editorTabControl.TabPages.Add(TP);
                TP.Controls.Add(this.contentEditors[i]);
            }
        }

        public void Load(TSPSave save) {
            this.loadedSave = save;
            foreach (ContentEditorMain CEM in contentEditors) {
                CEM.LoadSave(save);
            }
        }
        
        public void Save(string path) {
            foreach (ContentEditorMain CEM in contentEditors) {
                CEM.SaveDataContainer();
            }
            SaveProcessor.Save(path, this.loadedSave);
        }

        public void DestroyTab() {
            this.loadedSave = null;
            foreach (ContentEditorMain CEM in contentEditors) {
                CEM.DropData();
            }
            foreach (TabPage t in this.editorTabs) {
                t.Dispose();
            }
            //this.hostTabController.host.TabPages.Remove(this.hostTabPage);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
            GC.Collect();
        }

    }
}
