﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace biodanza
{
    public partial class MyLabel :  Label
    {
        public MyLabel() 
        {

           // InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x0084;
            const int HTTRANSPARENT = (-1);

            if (!DesignMode && m.Msg == WM_NCHITTEST )
            {
                
                
                    m.Result = (IntPtr)HTTRANSPARENT;
                
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
