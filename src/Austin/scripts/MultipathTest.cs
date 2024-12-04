// This file was used to make sure that the multipath scene worked as expected. It did (or at least it used to until the chickens stopped working)
// Don't worry about this file it is not neeed anymore
using Godot;
using Chicken;

public partial class MultipathTest : Node2D
{
    public override void _Ready()
    {

        // Create a chicken instance using the factory
        BaseChicken chicken = ChickenFactory.MakeKFC(Chicken.Cost.ChickenR1);
        BaseChicken chicken2 = ChickenFactory.MakeKFC(Chicken.Cost.ChickenR1);
        //BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();

        // Get the path for the chicken to follow
        Path2D path = GetNode<Path>("Map/Path").getPath();
        Path2D path2 = GetNode<Path>("Map/Path").getPath();

        // Start the chicken on the path
        chicken.Start(path);
        chicken2.Start(path2);
    }
}
