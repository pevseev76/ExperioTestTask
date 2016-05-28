using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ThinClient
{
    public class RibViewModel : INotifyPropertyChanged
    {
        private double beginX;
        private double beginY;
        private double endX;
        private double endY;

        private Color lineColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public string BeginId { get; set; }
        public string EndId { get; set; }

        public double BeginX
        {
            get { return beginX; }
            
            set
            {
                beginX = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("BeginX"));
            }
        }

        public double BeginY
        {
            get { return beginY; }

            set
            {
                beginY = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("BeginY"));
            }
        }

        public double EndX
        {
            get { return endX; }

            set
            {
                endX = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("EndX"));
            }
        }

        public double EndY
        {
            get { return endY; }

            set
            {
                endY = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("EndY"));
            }
        }

        public Color LineColor
        {
            get { return lineColor; }

            set
            {
                lineColor = value;

                var eh = PropertyChanged;

                if (eh != null)
                    eh(this, new PropertyChangedEventArgs("LineColor"));
            }
        }
    }
}
