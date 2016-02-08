using System;

/// <summary>
/// Заказ
/// </summary>
[Serializable]
public class Order
{
    /// <summary>
    /// Код
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Количество
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Цена
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public ObjectState State { get; set; }
}