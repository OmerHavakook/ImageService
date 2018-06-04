using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    interface IMainWindowModel : INotifyPropertyChanged
    {
        bool Connect { get; set; }
    }
}
