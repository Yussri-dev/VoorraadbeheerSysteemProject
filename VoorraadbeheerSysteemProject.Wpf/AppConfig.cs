using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf
{
    public static class AppConfig
    {
        public static string ApiUrl => ConfigurationManager.AppSettings["NGrokApiUri"];
        //public static string ApiUrl => ConfigurationManager.AppSettings["LocalHostUri"];

    }
}
