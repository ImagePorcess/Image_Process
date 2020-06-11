using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using STOM.Control;
namespace STOM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Work.InitWork();
            MotionControl.InitMotionCard();
            CommucateControl.ConnectCognexSockets();
            CommucateControl.ConnectPDCASocket();
            Application.Run(new Main());
            
        }
    }
}
