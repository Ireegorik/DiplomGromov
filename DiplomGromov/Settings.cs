using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DiplomGromov.Models;
using RiakClient;

namespace DiplomGromov
{
    class Settings
    {
        public static Frame MainContent;
        public static IRiakClient client;
        public static Users user;
        public static int lastSolvetTaskID = -1;
        public static MainWindow mw;
    }
}
