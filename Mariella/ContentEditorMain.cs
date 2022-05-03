using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mariella {
    public partial class ContentEditorMain : UserControl {
        public IContentEditor editorControl = null;
        public TSPSave loadedSave = null;
        public ContentSection section = ContentSection.Save;
        public ContentType type = ContentType.Data;

        public ContentEditorMain() {
            InitializeComponent();
        }

        public ContentEditorMain(ContentType contentType) {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
            this.editorControl = InitEditor(contentType);
            ((Control)this.editorControl).Dock = DockStyle.Fill;
            ((Control)this.editorControl).Visible = false;
            this.splitContainer1.Panel2.Controls.Add((Control)this.editorControl);
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public enum ContentType {
            KV,
            Data
        }
        
        public enum ContentSection {
            Save,
            KVInteger,
            KVFloat,
            KVBoolean,
            KVString
        }

        public IContentEditor InitEditor(ContentType contentType) {
            this.type = contentType;
            switch(contentType) {
                case ContentType.KV:
                    return new ContentEditorKVData();
                case ContentType.Data:
                    return new ContentEditorData();
                default:
                    return null;
            }
        }

        public void LoadDataContainer(DataContainer dc) {
            editorControl.LoadContent(dc);
        }

        public void SaveDataContainer(DataContainer dc = null) {
            editorControl.SaveContent(dc);
        }

        private DataContainer ConstructDCForContent(int index) {
            return DataContainer.Create(ObtainSaveSectionContent()[index]);
        }

        private dynamic ObtainSaveSectionContent() {
            switch (section) {
                case ContentSection.Save:
                    return this.loadedSave.saveData.saveDataCache;
                case ContentSection.KVInteger:
                    return this.loadedSave.fieldData.IntData;
                case ContentSection.KVFloat:
                    return this.loadedSave.fieldData.FloatData;
                case ContentSection.KVBoolean:
                    return this.loadedSave.fieldData.BoolData;
                case ContentSection.KVString:
                    return this.loadedSave.fieldData.StringData;
                default:
                    return this.loadedSave.saveData.saveDataCache;
            }
        }

        private Type ObtainTypeFromSection() {
            switch (section) {
                case ContentSection.Save:
                    return typeof(SaveDataStruct);
                case ContentSection.KVInteger:
                    return typeof(int);
                case ContentSection.KVFloat:
                    return typeof(float);
                case ContentSection.KVBoolean:
                    return typeof(bool);
                case ContentSection.KVString:
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }

        private void ProcessContentList() {
            RefreshContentList();
            UpdateUIState();
        }

        private void RefreshContentList() {
            int index = 0;
            if(this.listBoxContent.Items.Count > 0) {
                index = this.listBoxContent.SelectedIndex;
                this.listBoxContent.Items.Clear();
            }
            if (this.type == ContentType.Data) {
                List<SaveDataStruct> dataStruct = (List<SaveDataStruct>)this.ObtainSaveSectionContent();
                for (int i = 0;i < dataStruct.Count;i++) {
                    this.listBoxContent.Items.Add(((IDataStruct)dataStruct[i]).GetKeyName());
                }
            } else {
                IReadOnlyList<KVStruct> dataStruct = (IReadOnlyList<KVStruct>)this.ObtainSaveSectionContent();
                for (int i = 0;i < dataStruct.Count();i++) {
                    this.listBoxContent.Items.Add(((IDataStruct)dataStruct[i]).GetKeyName());
                }
            }
            if (this.listBoxContent.Items.Count > 0) {
                this.listBoxContent.SelectedIndex = Math.Min(this.listBoxContent.Items.Count -1,index);
            }
        }

        private void EditorReloadContent(int index) {
            this.LoadDataContainer(ConstructDCForContent(index));
        }

        public void LoadSave(TSPSave save, ContentSection Section) {
            this.loadedSave = save;
            this.section = Section;
            ProcessContentList();
        }

        public void DropData() {
            this.loadedSave = null;
            this.editorControl.DropContent();
            this.listBoxContent.Items.Clear();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            EditorReloadContent(listBoxContent.SelectedIndex);
        }

        private void UpdateUIState() {
            bool loaded = (this.listBoxContent.Items.Count > 0);
            ((UserControl)this.editorControl).Visible = loaded;
            contentRemove.Visible = loaded;
            contentDown.Visible = false; //yes these are false, because the havent been implemented yet
            contentUp.Visible = false;
        }

        private void contentAdd_Click(object sender, EventArgs e) {
            if (this.type == ContentType.Data) {
                List<SaveDataStruct> dataStruct = (List<SaveDataStruct>)this.ObtainSaveSectionContent();
                dataStruct.Add(new SaveDataStruct("Data" + this.listBoxContent.Items.Count));
            } else {
                this.loadedSave.AddNewFieldData(ObtainTypeFromSection(), "Data" + this.listBoxContent.Items.Count);
            }
            RefreshContentList();
            UpdateUIState();
            this.listBoxContent.SelectedIndex = this.listBoxContent.Items.Count - 1;
        }

        private void contentRemove_Click(object sender, EventArgs e) {
            if (this.type == ContentType.Data) {
                List<SaveDataStruct> dataStruct = (List<SaveDataStruct>)this.ObtainSaveSectionContent();
                dataStruct.RemoveAt(listBoxContent.SelectedIndex);
            } else {
                this.loadedSave.RemoveFieldData(ObtainTypeFromSection(), listBoxContent.SelectedIndex);
            }
            RefreshContentList();
            UpdateUIState();
        }
    }
}
