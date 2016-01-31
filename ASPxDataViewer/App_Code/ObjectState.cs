
/// <summary>
/// Внутренний статус объекта. Для поиска только изменных записей
/// </summary>
public enum ObjectState
{
    None,           // не изменялся
    New,            // новый
    Updated,        // изменен
    Deleted         // удален
}