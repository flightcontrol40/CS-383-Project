// File: src/Ankit/Scripts/Builders/IBulletBuilder.cs
using Godot;

// Interface defining methods for constructing and configuring a Bullet.

public interface IBulletBuilder
{
    // Initializes a Bullet with the given scene.
    void CreateBullet(PackedScene bulletScene);

    // Sets bullet speed.
    void SetSpeed(float speed);

    // Sets bullet damage.
    void SetDamage(int damage);

    // Sets bullet position and direction.
    void SetProperties(Vector2 position, Vector2 direction);

    // Returns the fully configured Bullet.
    Bullet GetResult();
}
