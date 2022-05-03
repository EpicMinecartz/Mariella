using System;
using System.Windows.Forms;

namespace Mariella {
    /// <summary>
    /// This whole thing is bursting with awful code, please avert your eyes if you dont want to experience eternal pain and suffering 
    /// </summary>
    internal static class Program {

        [System.Runtime.InteropServices.DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private static void DWMDark(IntPtr handle, bool enabled) {
            int useImmersiveDarkMode = enabled ? 1 : 0;
            if (DwmSetWindowAttribute(handle, 19, ref useImmersiveDarkMode, 4) != 0)
                DwmSetWindowAttribute(handle, 20, ref useImmersiveDarkMode, 4);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Main main = new Main();
            DWMDark(main.Handle, true);
            Application.Run(main);
        }
    }
}
