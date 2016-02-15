using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResumesApp
{
    public static class FormHelper
    {
        public static void PopulateDataGrid(DataGridView dgv, DataTable gridTable)
        {
            dgv.DataSource = gridTable;
            for (int i = 0; i < dgv.Columns.Count - 6; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
