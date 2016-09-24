using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace myMatrix
{

    /// <summary>
    /// Представляет класс матриц, используемых 
    /// матричным калькулятором.
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Двумерный массив с ячейками типа double. 
        /// Хранит данные матрицы.
        /// </summary>
        private double[,] matrix;

        /// <summary>
        /// Инициализирует объект матрицы.
        /// </summary>
        /// <param name="rowCount">Количество строк</param>
        /// <param name="columnCount">Количество столбцов</param>
        public Matrix(int rowCount, int columnCount)
        {
            this.matrix = new double[rowCount, columnCount];
        }

        /// <summary>
        /// Получает элемент матрицы по двум индексам.
        /// </summary>
        /// <param name="x">Столбец</param>
        /// <param name="y">Строка</param>
        /// <returns>Элемент матрицы.</returns>
        public double this[int x, int y]
        {
            get { return matrix[x, y]; }
            set { matrix[x, y] = value; }
        }

        /// <summary>
        /// Получает количество столбцов матрицы.
        /// </summary>
        /// <returns>Количество столбцов</returns>
        public int GetWidth()
        {
            return matrix.GetLength(0);
        }

        /// <summary>
        /// Получает количество строк матрицы.
        /// </summary>
        /// <returns>Количество строк</returns>
        public int GetHeight()
        {
            return matrix.GetLength(1);
        }

        /// <summary>
        /// Возвращает сумму двух матриц.
        /// </summary>
        /// <param name="A">Матрица A</param>
        /// <param name="B">Матрица B</param>
        /// <returns>Сумма матриц A + B</returns>
        public static Matrix operator +(Matrix A, Matrix B)
        {
            if ((A.GetWidth() != B.GetWidth()) || (A.GetHeight() != B.GetHeight()))
                throw new MatrixSizeException("Размеры матриц не совпадают!");

            int width = A.GetWidth();
            int height = A.GetHeight();
            Matrix result = new Matrix(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result[x, y] = A[x, y] + B[x, y];
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает разность двух матриц.
        /// </summary>
        /// <param name="A">Матрица A</param>
        /// <param name="B">Матрица B</param>
        /// <returns>Разность матриц A - B</returns>
        public static Matrix operator -(Matrix A, Matrix B)
        {
            if ((A.GetWidth() != B.GetWidth()) || (A.GetHeight() != B.GetHeight()))
                throw new MatrixSizeException("Размеры матриц не совпадают!");

            int width = A.GetWidth();
            int height = A.GetHeight();
            Matrix result = new Matrix(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result[x, y] = A[x, y] - B[x, y];
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает транспонированную матрицу.
        /// </summary>
        /// <returns>Транспонированная матрица</returns>
        public Matrix Transpose()
        {
            Matrix result = new Matrix(this.GetHeight(), this.GetWidth());
            for (int x = 0; x < this.GetWidth(); x++)
            {
                for (int y = 0; y < this.GetHeight(); y++)
                {
                    result[y, x] = this.matrix[x, y];
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает произведение двух матриц.
        /// </summary>
        /// <param name="A">Матрица А</param>
        /// <param name="B">Матрица В</param>
        /// <returns>Произведение А * В</returns>
        public static Matrix operator *(Matrix A, Matrix B)
        {
            if (A.GetHeight() != B.GetWidth())
                throw new MatrixSizeException("Количество столбцов А не равно количеству строк В. Операция невозможна.");

            Matrix result = new Matrix(A.GetWidth(), B.GetHeight());
            for (int x = 0; x < result.GetWidth(); x++)
            {
                for (int y = 0; y < result.GetHeight(); y++)
                {
                    for (int i = 0; i < A.GetHeight(); i++)
                    {
                        result[x, y] += A[x, i] * B[i, y];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает обратную матрицу.
        /// </summary>
        /// <returns>Обратная матрица</returns>
        public Matrix Inverse()
        {
            int width = this.GetWidth();
            int height = this.GetHeight();
            if (width != height)
                throw new MatrixSizeException("Количество строк не равно количеству столбцов!");

            double determinant = this.GetDeterminant();

            Matrix result = new Matrix(width, height);

            if ((width <= 2) && (height <= 2))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        result[width - 1 - x, height - 1 - y] = this[x, y] / determinant * Math.Pow(-1, x + y);
                    }
                }

                return result.Transpose();
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double minor = Math.Pow(-1, x + y);

                    Matrix temp = this;
                    temp = RemoveRow(temp, x);
                    temp = RemoveColumn(temp, y);
                    minor *= GetDet(temp);

                    result[x, y] = minor / determinant;
                }
            }

            return result.Transpose();
        }

        /// <summary>
        /// Возвращает определитель (детерминант) матрицы.
        /// </summary>
        /// <returns>Определитель матрицы</returns>
        public double GetDeterminant()
        {
            return GetDet(this);
        }

        /// <summary>
        /// Рекурсивный хелпер для получения определителя матрицы.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="col">Номер колонки разложения</param>
        /// <returns>Определитель матрицы</returns>
        private double GetDet(Matrix matrix)
        {
            double determinant = 0;
            int width = matrix.GetWidth();
            int height = matrix.GetHeight();
            if (width != height)
                throw new MatrixSizeException("Количество строк не равно количеству столбцов!");

            if ((width > 2) && (height > 2))
            {
                for (int x = 0; x < width; x++)
                {
                    double det = matrix[0, x] * Math.Pow(-1, x);

                    Matrix temp = matrix;
                    temp = RemoveRow(temp, 0);
                    temp = RemoveColumn(temp, x);
                    det *= GetDet(temp);

                    determinant += det;
                }
            }
            else
            {
                determinant = (matrix[0, 0] * matrix[1, 1]) - 
                    (matrix[0, 1] * matrix[1, 0]);
            } 
            
            return determinant;
        }

        /// <summary>
        /// Удаляет колонку из матрицы.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="index">Индекс удаляемой строки</param>
        /// <returns>Новая матрица</returns>
        private Matrix RemoveRow(Matrix matrix, int index)
        {
            int rowCount = matrix.GetHeight();
            int columnCount = matrix.GetWidth() - 1;

            Matrix result = new Matrix(columnCount, rowCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    result[col, row] = matrix[col < index ? col : col + 1, row];
                }
            }

            return result;
        }

        /// <summary>
        /// Удаляет столбец из матрицы.
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="index">Индекс удаляемой колонки</param>
        /// <returns>Новая матрица</returns>
        private Matrix RemoveColumn(Matrix matrix, int index)
        {
            int rowCount = matrix.GetHeight() - 1;
            int columnCount = matrix.GetWidth();

            Matrix result = new Matrix(columnCount, rowCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    result[col, row] = matrix[col, row < index ? row : row + 1];
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Исключение, вызываемое, если матрицы имеют разный размер.
    /// </summary>
    public class MatrixSizeException : Exception
    {
        public MatrixSizeException() { }
        public MatrixSizeException(string message) : base(message) { }
    }

    /// <summary>
    /// Исключение, вызываемое, если ввод матрицы осуществлен некорректно.
    /// </summary>
    public class MatrixInputInvalidException : Exception
    {
        public MatrixInputInvalidException() { }
        public MatrixInputInvalidException(string message) : base(message) { }
    }
}