// File: src/Ankit/Scripts/Builders/BulletBuilders.cs
using Godot;
using System;

// Standard Bullet Builder
public class StandardBulletBuilder : IBulletBuilder
{
    protected Bullet bullet;

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

    public virtual void SetSpeed(float speed)
    {
        if (bullet != null)
        {
            bullet.Speed = speed;
            GD.Print($"Bullet speed set to {speed}");
        }
    }

    public virtual void SetDamage(int damage)
    {
        if (bullet != null)
        {
            bullet.Damage = damage;
            GD.Print($"Bullet damage set to {damage}");
        }
    }

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

    public virtual Bullet GetResult()
    {
        if (bullet == null)
        {
            GD.PrintErr("Attempting to get null bullet");
        }
        return bullet;
    }
}

// Rapid Bullet Builder
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

// Heavy Bullet Builder
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