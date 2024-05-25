
public class GameManager : Singleton<GameManager>
{
    private void OnEnable()
    {
        EventSystem<OnPlaceableDataSelected>.Subscribe(OnPlaceableDataSelected);
    }

    private void OnPlaceableDataSelected(OnPlaceableDataSelected obj)
    {
        
    }
}
