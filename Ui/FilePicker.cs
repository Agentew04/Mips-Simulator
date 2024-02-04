using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MipsSimulator.Ui {
    public static partial class FilePicker {

        public static (string textSgm, string dataSgm) OpenFile() {
            string textSgm = string.Empty;
            string dataSgm = string.Empty;

            OpenFileDialog openTextDialog = new() {
                Filter = "Binary Text Segment Files (*.bin)|*.bin|All files (*.*)|*.*",
                Title = "Open Text Segment File",
            };

            if (openTextDialog.ShowDialog() == DialogResult.OK) {
                textSgm = openTextDialog.FileName;
            }

            OpenFileDialog openDataDialog = new() {
                Filter = "Binary Data Segment Files (*.bin)|*.bin|All files (*.*)|*.*",
                Title = "Open Data Segment File",
            };

            if (openDataDialog.ShowDialog() == DialogResult.OK) {
                dataSgm = openDataDialog.FileName;
            }

            return (textSgm, dataSgm);
        }
    }
}
