
using System.Linq;
using Godot;

public partial class MultiPath: Path
{
    [Export]
    private Path2D[] paths;

    [Export]
    private int currentPath = 0;

    public void addPath(Path2D newPath) {
        paths.Append(newPath);
    }

    public override Path2D getPath()
    {
        currentPath = (currentPath + 1) % paths.Length;
        return paths[currentPath];
    }
}