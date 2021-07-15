using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RfFondPortfolio.Common.Interfaces;

namespace RfBondManagement.WinForm.Controls
{
    public partial class DisplayPaperListUC : UserControl
    {
        protected IPaperRepository _paperRepository;

        public DisplayPaperListUC()
        {
            InitializeComponent();
        }

        public void InitDI(IPaperRepository paperRepository)
        {
            _paperRepository = paperRepository;
        }


    }
}
