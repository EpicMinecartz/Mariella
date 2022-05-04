using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mariella {
    public interface IContentEditor {
        ContentEditorMain parent { get; set; }
        void LoadContent(DataContainer data);
        void SaveContent(DataContainer data);
        void DropContent();
    }
}
