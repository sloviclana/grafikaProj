using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RGrafikaPZ1.Model
{
    public class PowerEntity
    {

        private long id;
        private string name;
        private double x;
        private double y;
        private Brush boja;
        private string tooltip;

        public PowerEntity()
        {

        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return tooltip;
            }
            set
            {
                tooltip = value;
            }
        }

        public Brush Boja
        {
            get
            {
                return boja;
            }
            set
            {
                boja = value;
            }
        }
    }
}
