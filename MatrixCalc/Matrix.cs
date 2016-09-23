using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

/// <summary>
/// Представляет собой класс матриц, 
/// используемых матричным калькулятором.
/// </summary>
public class Matrix
{
    /// <summary>
    /// Двуразмерный массив с ячейками типа double. 
    /// Хранит данные матрицы.
    /// </summary>
    private double[,] matrix;
    
    /// <summary>
    /// Инициализирует объект матрицы.
    /// </summary>
    /// <param name="rowCount">Количество строк.</param>
    /// <param name="columnCount">Количество столбцов.</param>
	public Matrix(int rowCount, int columnCount)
	{
        this.matrix = new double[rowCount, columnCount];
	}

    /// <summary>
    /// Получает элемент матрицы по двум индексам.
    /// </summary>
    /// <param name="x">Первый индекс.</param>
    /// <param name="y">Второй индекс.</param>
    /// <returns>Элемент матрицы.</returns>
    public double this[int x, int y]
    {
        get { return matrix[x, y]; }
        set { matrix[x, y] = value; }
    }

    /// <summary>
    /// Получает количество столбцов матрицы.
    /// </summary>
    /// <returns>Количество столбцов.</returns>
    public int GetWidth()
    {
        return matrix.GetLength(0);
    }

    /// <summary>
    /// Получает количество строк матрицы.
    /// </summary>
    /// <returns>Количество строк.</returns>
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
        Matrix result = new Matrix(this.GetWidth(), this.GetHeight());

        // implement logic...
        
        return result;
    }

    /// <summary>
    /// Возвращает определитель (детерминант) матрицы.
    /// </summary>
    /// <returns>Определитель матрицы</returns>
    public double GetDeterminant()
    {
        return GetDet(this, 1);
    }

    /// <summary>
    /// Рекурсивный хелпер для получения определителя матрицы.
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="col">Номер колонки разложения</param>
    /// <returns>Определитель матрицы</returns>
    private double GetDet(Matrix matrix, int col)
    {
        double determinant = 0;

        int width = matrix.GetWidth();
        int height = matrix.GetHeight();

        for (int x = 0; x < width; x++)
        {
            double det = matrix[x, 0] * Math.Pow(-1, col);

            Matrix temp = matrix;
            temp = RemoveRow(temp, 0);
            temp = RemoveColumn(temp, x);

            double det2 = GetDet(temp, col += 1);
            if (det2 != 0) det *= det2;
            determinant += det;
        }

        (new MessageDialog(determinant.ToString())).ShowAsync();
        return determinant;
    }

    /// <summary>
    /// Удаляет колонку из матрицы.
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="index">Индекс удаляемой строки</param>
    /// <returns>Новая матрица</returns>
    public Matrix RemoveRow(Matrix matrix, int index)
    {
        Matrix result = new Matrix(matrix.GetWidth(), matrix.GetHeight());
        for (int x = 0; x < matrix.GetHeight() - 1; x++)
        {
            for (int y = 0; y < matrix.GetWidth(); y++)
            {
                result[x, y] = matrix[x < index ? x : x + 1, y];
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
    public Matrix RemoveColumn(Matrix matrix, int index)
    {
        Matrix result = new Matrix(matrix.GetWidth(), matrix.GetHeight());   
        for (int col = 0; col < result.GetWidth(); col++)
        {
            for (int row = 0; row < result.GetHeight(); row++)
            {
                    result[col, row] = matrix[col, row];
                
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