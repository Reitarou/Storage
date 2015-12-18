namespace Stg
{
    /// <summary>
    /// Инферфейс <c>IStgSerializable</c> преодставляет методы для загрузки и сохранения объекта в элемент <see cref="Stg.StgNode"/>
    /// <note type="important">
    /// Структуры не должны реализовывать этот интерфейс
    /// </note>
    /// </summary>
    /// <example> Пример реализации на C#
    /// <code>
    /// class TestClass : IStgSerializable
    /// {
    /// 
    ///        #region IStgSerializable Members
    ///
    ///        public void SaveToStg(StgElementNode stgNode)
    ///        {
    ///            // DoSave
    ///        }
    ///
    ///        public void LoadFromStg(StgElementNode stgNode)
    ///        {
    ///            // DoLoad
    ///        }
    ///
    ///        #endregion
    /// }
    ///
    /// public struct TestStruct
    /// {
    ///        public void SaveToStg(StgElementNode stgNode)
    ///        {
    ///            // DoSave
    ///        }
    ///
    ///        public static TestStruct LoadFromStg(StgElementNode stgNode)
    ///        {
    ///             TestStruct result = new TestStruct();
    ///             // DoLoad
    ///             return result;
    ///        }
    /// }
    /// </code>
    /// </example>
    public interface IStgSerializable
    {
        /// <summary>
        /// Сохранение объекта в элемент <see cref="Stg.StgNode"/>
        /// </summary>
        /// <param name="node">Элемент в который необходимо сохранить объект</param>
        /// <seealso cref="Stg.StgNode"/>
        void SaveToStg(StgNode node);

        /// <summary>
        /// Загрузка объекта из элемента <see cref="Stg.StgNode"/>
        /// </summary>
        /// <param name="node">Элемент из которого необходимо выполнить чтение</param>
        /// <para>Подробная информация <seealso cref="Stg.StgNode"/></para>
        void LoadFromStg(StgNode node);
    }
}
