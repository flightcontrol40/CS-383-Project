// File: src/Ankit/Scripts/Builders/BulletBuilders.cs
using Godot;
using System;

// Standard Bullet Builder: Implements IBulletBuilder to create and configure a standard bullet.
public class StandardBulletBuilder : IBulletBuilder
{
    protected Bullet bullet;

    // Creates a bullet instance from a PackedScene and adds it to the "Projectile" group.
    public virtual void CreateBullet(PackedScene bulletScene)
    {
        try
        {
            bullet = bulletScene.Instantiate<Bullet>();
            if (bullet != null)
            {
                GD.Print("Bullet instantiated successfully");
                bullet.AddToGroup("Projectile");
            }
            else
            {
                GD.PrintErr("Failed to instantiate bullet");
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error creating bullet: {e.Message}");
            bullet = null;
        }
    }

    // Sets the bullet's speed.
    public virtual void SetSpeed(float speed)
    {
        if (bullet != null)
        {
            bullet.Speed = speed;
            GD.Print($"Bullet speed set to {speed}");
        }
    }

    // Sets the bullet's damage value.
    public virtual void SetDamage(int damage)
    {
        if (bullet != null)
        {
            bullet.Damage = damage;
            GD.Print($"Bullet damage set to {damage}");
        }
    }

    // Configures the bullet's initial position, direction, and rotation.
    public virtual void SetProperties(Vector2 position, Vector2 direction)
    {
        if (bullet != null)
        {
            bullet.GlobalPosition = position;
            bullet.Direction = direction;
            bullet.Rotation = direction.Angle();
            GD.Print($"Bullet properties set - Position: {position}, Direction: {direction}");
        }
    }

    // Returns the fully configured bullet instance.
    public virtual Bullet GetResult()
    {
        if (bullet == null)
        {
            GD.PrintErr("Attempting to get null bullet");
        }
        return bullet;
    }
}

// Rapid Bullet Builder: Increases bullet speed by 20% for a faster bullet.
public class RapidBulletBuilder : StandardBulletBuilder
{
    public override void SetProperties(Vector2 position, Vector2 direction)
    {
        base.SetProperties(position, direction);
        if (bullet != null)
        {
            bullet.Speed *= 1.2f; // 20% faster
        }
    }
}

// Heavy Bullet Builder: Doubles bullet damage for a more powerful bullet.
public class HeavyBulletBuilder : StandardBulletBuilder
{
    public override void SetProperties(Vector2 position, Vector2 direction)
    {
        base.SetProperties(position, direction);
        if (bullet != null)
        {
            bullet.Damage *= 2; // Double damage
        }
    }
}
