using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionPersonal.Utiles
{
    internal class ExceptionManager
    {
        public static void Execute(Exception e, String msg)
        {
            MessageBox.Show(msg + e.Message + "\n" + e.StackTrace);
        }
    }
}
