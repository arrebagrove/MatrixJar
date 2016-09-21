using System;

/// <summary>
/// Представляет собой класс матриц, 
/// используемых матричным калькулятором.
/// </summary>
public class Matrix
{
    public double[,] matrix;
    public int ass = 0;

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

    public static Matrix operator *(Matrix A, Matrix B)
    {
        throw new NotImplementedException();
    }

    public Matrix Transpose()
    {
        throw new NotImplementedException();
    }

    public Matrix Inverse()
    {
        throw new NotImplementedException();
    }

    public double GetDeterminant()
    {
        throw new NotImplementedException();
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