﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RGrafikaPZ1.Model
{
    public class SwitchEntity : PowerEntity
    {

        private string status;

        public SwitchEntity()
        {
            Boja = Brushes.MediumPurple;
        }
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

    }
}
