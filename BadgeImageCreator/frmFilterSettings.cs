using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace BadgeImageCreator
{
    public partial class frmFilterSettings : Form
    {
        public frmFilterSettings()
        {
            InitializeComponent();
        }

        private AForge.Imaging.Filters.BaseFilter _filter;
        public AForge.Imaging.Filters.BaseFilter FilterToModify
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != null)
                {
                    throw new ArgumentException("Cannot assign a filter to a dialog that's already been configured", "FilterToModify");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("Cannot assign a null to the filter configuration", "FilterToModify");
                }

                _filter = value;

                var filterProps = _filter.GetType().GetProperties();
                var baseProps = new HashSet<string>(from x in typeof(AForge.Imaging.Filters.BaseFilter).GetProperties() select x.Name);
                int pos = 0;
                foreach (var prop in filterProps)
                {
                    if (baseProps.Contains(prop.Name))
                    {
                        continue;
                    }

                    Label lblTitle = new Label();
                    lblTitle.Name = "lbl" + prop.Name;
                    lblTitle.AutoSize = true;
                    lblTitle.Text = prop.Name;
                    lblTitle.Left = 15;
                    lblTitle.Top = 15 + pos * lblTitle.Height;
                    grpSettings.Controls.Add(lblTitle);

                    pos++;
                }
            }
        }

    }
}
