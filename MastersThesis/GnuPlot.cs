using System.Diagnostics;

namespace MastersThesis {
    internal class GnuPlot {

        #region Private Class Variables
        /// <summary>
        /// Процесс для GnuPlot
        /// </summary>
        private readonly Process _process;
        /// <summary>
        /// StreamWriter для GnuPlot
        /// </summary>
        private readonly StreamWriter _streamWriter;
        /// <summary>
        /// Буфер для построения 3D графиков
        /// </summary>
        private readonly List<PlotStore> _splotBuffer;
        /// <summary>
        /// Построение с сохранением предыдущих элементов
        /// </summary>
        private bool _hold;
        #endregion

        #region Constructor & Properties
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path"></param>
        internal GnuPlot(string path) {
            _process = new Process();
            _process.StartInfo.FileName = path;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardInput = true;
            _process.Start();

            _streamWriter = _process.StandardInput;
            _splotBuffer = new List<PlotStore>();
            _hold = true;
        }

        /// <summary>
        /// Включение/выключение сохранения предыдущих построений (при изменении происходит очистка буфера)
        /// </summary>
        internal bool Hold {
            get {
                return _hold;
            }
            set {
                _hold = value;
                _splotBuffer.Clear();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Команда set для установки множества параметров
        /// </summary>
        /// <param name="options">Передаваемые параметры</param>
        internal void Set(params string[] options) {
            for (int j = 0; j < options.Length; j++) {
                _streamWriter.WriteLine($"set {options[j]}");
            }
        }
        /// <summary>
        /// Команда unset для установки по умолчанию множества параметров
        /// </summary>
        /// <param name="options">Передаваемые параметры</param>
        internal void Unset(params string[] options) {
            for (int j = 0; j < options.Length; j++) {
                _streamWriter.WriteLine($"unset {options[j]}");
            }
        }
        /// <summary>
        /// Запись данных в поток с корректировкой вещественных чисел (замена ',' на '.')
        /// </summary>
        /// <param name="x">Массив значений (координата x)</param>
        /// <param name="y">Массив значений (координата y)</param>
        /// <param name="z">Массив значений (координата z)</param>
        private void WriteData(double[] x, double[] y, double[] z) {
            for (int j = 0; j < x.Length; j++) {
                _streamWriter.WriteLine($"{x[j].ToString().Replace(',', '.')} {y[j].ToString().Replace(',', '.')} {z[j].ToString().Replace(',', '.')}");
            }
        }
        /// <summary>
        /// Построение 3D графиков по данным из буфера
        /// </summary>
        /// <param name="splotBuffer">Буфер для построения 3D графиков</param>
        private void SPlot(List<PlotStore> splotBuffer) {
            string plotstring = string.Empty;

            for (int j = 0; j < splotBuffer.Count; j++) {
                PlotStore plotStore = splotBuffer[j];

                string defopts = (plotStore.Options.Length > 0 && (plotStore.Options.Contains(" w") || plotStore.Options[0] == 'w')) ? " " : " with lines ";
                string splot = j == 0 ? "splot" : ",";

                plotstring += $"{splot} {@"""-"""} {defopts}{plotStore.Options}";
            }
            _streamWriter.WriteLine(plotstring);

            for (int j = 0; j < splotBuffer.Count; j++) {
                WriteData(splotBuffer[j].XArray, splotBuffer[j].YArray, splotBuffer[j].ZArray);
                _streamWriter.WriteLine("e");
            }
            _streamWriter.Flush();
        }
        /// <summary>
        /// Построение 3D графиков по набору точек, заданных через массивы значений соответсвующих координат,
        /// с использованием дополнительных параметров построения
        /// </summary>
        /// <param name="x">Массив значений (координата x)</param>
        /// <param name="y">Массив значений (координата y)</param>
        /// <param name="z">Массив значений (координата z)</param>
        /// <param name="options">Передаваемые параметры</param>
        internal void SPlot(double[] x, double[] y, double[] z, string options = "") {
            if (!_hold) {
                _splotBuffer.Clear();
            }
            _splotBuffer.Add(new PlotStore(x, y, z, options));
            SPlot(_splotBuffer);
        }
        #endregion

        #region Class PlotStore
        internal class PlotStore {

            #region Private Class Variables
            /// <summary>
            /// Массив значений (координата x)
            /// </summary>
            private readonly double[] _xArray;
            /// <summary>
            /// Массив значений (координата y)
            /// </summary>
            private readonly double[] _yArray;
            /// <summary>
            /// Массив значений (координата z)
            /// </summary>
            private readonly double[] _zArray;
            /// <summary>
            /// Передаваемые параметры
            /// </summary>
            private readonly string _options;
            #endregion

            #region Constructor & Properties
            /// <summary>
            /// Конструктор для передачи данных (используем PlotType = SplotXYZ) в буфер
            /// </summary>
            /// <param name="x">Массив значений (координата x)</param>
            /// <param name="y">Массив значений (координата y)</param>
            /// <param name="z">Массив значений (координата z)</param>
            /// <param name="options">Передаваемые параметры</param>
            internal PlotStore(double[] x, double[] y, double[] z, string options = "") {
                _xArray = x;
                _yArray = y;
                _zArray = z;
                _options = options;
            }

            /// <summary>
            /// Массив значений (координата x)
            /// </summary>
            internal double[] XArray { get { return _xArray; } }
            /// <summary>
            /// Массив значений (координата y)
            /// </summary>
            internal double[] YArray { get { return _yArray; } }
            /// <summary>
            /// Массив значений (координата z)
            /// </summary>
            internal double[] ZArray { get { return _zArray; } }
            /// <summary>
            /// Передаваемые параметры
            /// </summary>
            internal string Options { get { return _options; } }
            #endregion
        }
        #endregion
    }
}