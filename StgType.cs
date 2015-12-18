namespace Stg
{
    /// <summary>
    /// Флаги типов используемые при сериализации в Stg
    /// </summary>
    public enum StgType: byte
    {
        /// <summary>
        /// Базовый элемент
        /// </summary>
        Node = 0,
        /// <summary>
        /// Массив
        /// </summary>
        Array = 1,
        /// <summary>
        /// System.Boolean
        /// </summary>
        Boolean = 2,
        /// <summary>
        /// System.Byte
        /// </summary>
        Byte = 3,
        /// <summary>
        /// System.Char
        /// </summary>
        Char = 4,
        /// <summary>
        /// System.Int16
        /// </summary>
        Int16 = 5,
        /// <summary>
        /// System.Int32
        /// </summary>
        Int32 = 6,
        /// <summary>
        /// System.Int64
        /// </summary>
        Int64 = 7,
        /// <summary>
        /// System.Single
        /// </summary>
        Single = 8,
        /// <summary>
        /// System.Double
        /// </summary>
        Double = 9,
        /// <summary>
        /// System.String
        /// </summary>
        String = 10,
    }
}
