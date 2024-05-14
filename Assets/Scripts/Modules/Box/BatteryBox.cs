using Module.Neighbors;

public class BatteryBox : ModuleBox
{
    private protected override void FindNeighbors()
    {
        Neighbors.Add(Directions.Up, _moduleManager.ModuleBoxes[1]);
        Neighbors.Add(Directions.Down, _moduleManager.ModuleBoxes[5]);
    }

    private void OnValidate()
    {
        ModuleLane = Lane.Beginning;
        Number = 0;
    }
}
