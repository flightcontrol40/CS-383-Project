// File: src/Ankit/Scripts/Builders/IBulletBuilder.cs
using Godot;

public interface IBulletBuilder
{
    void CreateBullet(PackedScene bulletScene);
    void SetSpeed(float speed);
    void SetDamage(int damage);
    void SetProperties(Vector2 position, Vector2 direction);
    Bullet GetResult();
}